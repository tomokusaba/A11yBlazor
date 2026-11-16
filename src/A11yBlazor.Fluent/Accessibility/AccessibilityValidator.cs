using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// WCAG 2.1 AA に関連するコンポーネント契約を検証します。
/// </summary>
public sealed class AccessibilityValidator : IAccessibilityValidator
{
    /// <summary>
    /// 可視ラベルを必要とするフォームコンポーネントの基底名です。
    /// </summary>
    private static readonly HashSet<string> LabelledControls =
    [
        "FluentAutocomplete",
        "FluentCalendar",
        "FluentCheckbox",
        "FluentColorPickerInput",
        "FluentCombobox",
        "FluentDatePicker",
        "FluentListbox",
        "FluentNumberInput",
        "FluentRadioGroup",
        "FluentRadio",
        "FluentSelect",
        "FluentSlider",
        "FluentSwitch",
        "FluentTextArea",
        "FluentTextInput",
        "FluentTimePicker",
    ];

    /// <summary>
    /// ボタンとして機能するコンポーネントの基底名です。
    /// </summary>
    private static readonly HashSet<string> ButtonControls =
    [
        "FluentAnchorButton",
        "FluentButton",
        "FluentCompoundButton",
        "FluentMenuButton",
        "FluentSplitButton",
        "FluentToggleButton",
    ];

    /// <summary>
    /// 子コンテンツのない場合にアクセシブル名を必要とするコンポーネントです。
    /// </summary>
    private static readonly HashSet<string> NamedStandaloneControls =
    [
        "FluentInputFile",
        "FluentProgress",
        "FluentProgressBar",
        "FluentProgressRing",
        "FluentSortableList",
        "FluentSpinner",
        "FluentTabs",
    ];

    /// <summary>
    /// 見出しまたは専用テンプレートで名前を付けるコンポーネントです。
    /// </summary>
    private static readonly Dictionary<string, (string Primary, string Template)> NamedHeaders =
        new(StringComparer.Ordinal)
        {
            ["FluentAccordionItem"] = ("Header", "HeaderTemplate"),
            ["FluentTab"] = ("Header", "HeaderTemplate"),
            ["FluentWizardStep"] = ("Label", "LabelTemplate"),
        };

    /// <summary>
    /// 検証オプションを表します。
    /// </summary>
    private readonly AccessibilityOptions _options;

    /// <summary>
    /// 診断出力に使用するロガーです。
    /// </summary>
    private readonly ILogger<AccessibilityValidator> _logger;

    /// <summary>
    /// 検証器を初期化します。
    /// </summary>
    /// <param name="options">検証動作のオプションです。</param>
    /// <param name="logger">診断出力に使用するロガーです。</param>
    public AccessibilityValidator(
        IOptions<AccessibilityOptions> options,
        ILogger<AccessibilityValidator> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public void Validate(IComponent component)
    {
        if (!_options.Enabled || _options.EnforcementMode == AccessibilityEnforcementMode.Ignore)
        {
            return;
        }

        var result = Inspect(component);
        if (result.IsValid)
        {
            return;
        }

        if (_options.EnforcementMode == AccessibilityEnforcementMode.Throw)
        {
            throw new AccessibilityValidationException(result);
        }

        foreach (var violation in result.Violations)
        {
            _logger.LogWarning(
                "A11y contract {RuleId} failed for {ComponentName}: {Message} (WCAG {Criteria})",
                violation.RuleId,
                result.ComponentName,
                violation.Message,
                violation.WcagCriteria);
        }
    }

    /// <inheritdoc />
    public AccessibilityValidationResult Inspect(IComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);

        var type = GetFluentComponentType(component.GetType());
        var componentName = type.Name.Split('`')[0];
        var violations = new List<AccessibilityViolation>();

        ValidateVisibleLabel(component, componentName, violations);
        ValidateNamedControl(component, componentName, violations);
        ValidateStandaloneName(component, componentName, violations);
        ValidateIcon(component, componentName, violations);
        ValidateConditionalInstructions(component, componentName, violations);
        ValidateNewWindow(component, componentName, violations);
        ValidateKeyboardActivation(component, componentName, violations);
        ValidateKeyboardAlternative(component, componentName, violations);
        ValidateConditionalContentNames(component, componentName, violations);
        ValidateImageAlternative(component, componentName, violations);
        ValidateErrorMessage(component, violations);
        ValidateLoadingStatus(component, violations);
        ValidateHeader(component, componentName, violations);
        ValidateLinkDestination(component, componentName, violations);
        ValidateDataGridName(component, componentName, violations);
        ValidateDialogName(component, componentName, violations);
        ValidateMenuAndTreeItems(component, componentName, violations);
        ValidateTooltip(component, componentName, violations);

        return new AccessibilityValidationResult(component.GetType().Name, violations);
    }

    /// <summary>
    /// Fluent UI コンポーネント側の型を特定します。
    /// </summary>
    /// <param name="wrapperType">ラッパー型です。</param>
    /// <returns>継承元の Fluent UI 型です。</returns>
    private static Type GetFluentComponentType(Type wrapperType)
    {
        var current = wrapperType;
        while (current.BaseType is not null)
        {
            current = current.BaseType;
            if (current.Namespace == "Microsoft.FluentUI.AspNetCore.Components")
            {
                return current;
            }
        }

        return wrapperType;
    }

    /// <summary>
    /// フォームコントロールに可視ラベルがあることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateVisibleLabel(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (!LabelledControls.Contains(componentName))
        {
            return;
        }

        var label = GetValue<string>(component, "Label");
        if (string.IsNullOrWhiteSpace(label))
        {
            Add(violations, "A11Y-LABEL", "入力欄に可視の Label を指定してください。", "1.3.1, 3.3.2, 4.1.2");
        }
    }

    /// <summary>
    /// ボタンやアイコンだけの操作にアクセシブルな名前があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateNamedControl(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (!ButtonControls.Contains(componentName))
        {
            return;
        }

        var iconOnly = GetValue<bool>(component, "IconOnly");
        var content = GetValue(component, "ChildContent");
        if (!iconOnly && content is not null)
        {
            return;
        }

        if (HasText(component, "AccessibleName") ||
            HasText(component, "Label") ||
            HasText(component, "Title") ||
            HasText(component, "Tooltip"))
        {
            return;
        }

        Add(violations, "A11Y-NAME", "アイコンのみ、または子コンテンツのない操作には AccessibleName を指定してください。", "2.5.3, 4.1.2");
    }

    /// <summary>
    /// ファイル入力や読み込み表示などにアクセシブル名があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateStandaloneName(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (NamedStandaloneControls.Contains(componentName) && !HasAccessibleName(component))
        {
            Add(violations, "A11Y-STANDALONE-NAME", $"{componentName} の目的または状態を示す AccessibleName を指定してください。", "1.3.1, 4.1.2");
        }
    }

    /// <summary>
    /// アイコンと絵文字の装飾指定またはアクセシブル名を検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateIcon(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName is not ("FluentIcon" or "FluentEmoji"))
        {
            return;
        }

        var decorative = GetValue<bool>(component, "Decorative");
        var interactive = HasDelegate(component, "OnClick");
        if (interactive && decorative)
        {
            Add(violations, "A11Y-INTERACTIVE-ICON", "OnClick を持つアイコンは Decorative=false とし、キーボード操作を提供してください。", "2.1.1, 4.1.2");
        }

        if (!decorative &&
            !HasAccessibleName(component) &&
            !HasText(component, "Title") &&
            !HasText(component, "Tooltip"))
        {
            Add(violations, "A11Y-ICON-NAME", "意味を持つアイコンには AccessibleName、Title、または Tooltip を指定してください。", "1.1.1, 4.1.2");
        }
    }

    /// <summary>
    /// 画像の代替テキストまたは装飾指定があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateImageAlternative(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName != "FluentImage")
        {
            return;
        }

        var decorative = GetValue<bool>(component, "Decorative");
        if (!decorative && !HasText(component, "AlternateText"))
        {
            Add(violations, "A11Y-IMAGE-ALT", "装飾画像でなければ AlternateText を、装飾画像なら Decorative=true を指定してください。", "1.1.1");
        }

        if (decorative)
        {
            var alternateText = GetValue<string>(component, "AlternateText");
            if (!string.IsNullOrEmpty(alternateText))
            {
                Add(violations, "A11Y-DECORATIVE-ALT", "装飾画像には AlternateText を指定せず、空の代替テキストを使用してください。", "1.1.1");
            }
        }
    }

    /// <summary>
    /// エラー状態に説明メッセージがあることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateErrorMessage(
        IComponent component,
        List<AccessibilityViolation> violations)
    {
        var state = GetValue(component, "MessageState");
        if (state?.ToString() != "Error")
        {
            return;
        }

        if (!HasText(component, "Message") && GetValue(component, "MessageTemplate") is null)
        {
            Add(violations, "A11Y-ERROR", "MessageState=Error の場合は Message または MessageTemplate で問題と修正方法を示してください。", "3.3.1");
        }
    }

    /// <summary>
    /// 読み込み状態に状態を示す名前があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateLoadingStatus(
        IComponent component,
        List<AccessibilityViolation> violations)
    {
        var loading = GetValue<bool>(component, "Loading");
        if (loading && !HasText(component, "LoadingLabel"))
        {
            Add(violations, "A11Y-LOADING", "Loading=true の場合は読み込み中であることを示す LoadingLabel を指定してください。", "4.1.3");
        }
    }

    /// <summary>
    /// 展開項目などに識別可能な見出しがあることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateHeader(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (!NamedHeaders.TryGetValue(componentName, out var names))
        {
            return;
        }

        if (!HasText(component, names.Primary) && GetValue(component, names.Template) is null)
        {
            Add(violations, "A11Y-HEADER", $"{names.Primary} または {names.Template} でコンテンツ領域の名前を指定してください。", "1.3.1, 4.1.2");
        }
    }

    /// <summary>
    /// リンク型コンポーネントに遷移先または操作があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateLinkDestination(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName is not ("FluentLink" or "FluentAnchorButton" or "FluentNavItem" or "FluentAppBarItem"))
        {
            return;
        }

        if (!HasText(component, "Href"))
        {
            if (!HasDelegate(component, "OnClick"))
            {
                Add(violations, "A11Y-LINK-TARGET", "リンクには Href、または明示的な OnClick 操作を指定してください。", "2.1.1, 4.1.2");
            }
        }
    }

    /// <summary>
    /// データグリッドに識別名があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateDataGridName(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName == "FluentDataGrid" && !HasText(component, "AccessibleName"))
        {
            Add(violations, "A11Y-GRID-NAME", "データグリッドの目的を説明する AccessibleName を指定してください。", "4.1.2");
        }
    }

    /// <summary>
    /// 入力形式や複数選択・ファイル制限に操作説明があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateConditionalInstructions(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        var constrainedFormat =
            HasText(component, "MaskPattern") ||
            HasText(component, "Pattern") ||
            GetValue(component, "Min") is not null ||
            GetValue(component, "Max") is not null ||
            GetPositiveNumber(component, "MinLength") > 0 ||
            GetPositiveNumber(component, "MaxLength") > 0 ||
            GetPositiveNumber(component, "MinDate") > 0 ||
            GetPositiveNumber(component, "MaxDate") > 0 ||
            GetValue<bool>(component, "Multiple") ||
            componentName == "FluentInputFile" &&
                (HasText(component, "Accept") ||
                 GetPositiveNumber(component, "MaximumFileCount") > 0 ||
                 GetPositiveNumber(component, "MaximumFileSize") > 0);

        if (constrainedFormat && !HasText(component, "AccessibleDescription"))
        {
            Add(
                violations,
                "A11Y-CONDITIONAL-INSTRUCTIONS",
                "入力形式、範囲、複数選択、またはファイル制限が設定されている場合は AccessibleDescription で操作方法と制約を説明してください。",
                "3.3.2, 4.1.2");
        }
    }

    /// <summary>
    /// 新しい閲覧コンテキストを開くリンクに事前説明があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateNewWindow(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        var target = GetValue(component, "Target");
        if (target?.ToString() == "Blank" && !HasText(component, "NewWindowDescription"))
        {
            Add(
                violations,
                "A11Y-NEW-WINDOW",
                $"{componentName} が新しいウィンドウまたはタブを開く場合は NewWindowDescription で事前に説明してください。",
                "2.4.4, 3.2.5");
        }
    }

    /// <summary>
    /// 非ボタンのクリック可能な要素に名前とキーボード操作があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateKeyboardActivation(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName is not ("FluentAvatar" or "FluentCard" or "FluentIcon" or "FluentEmoji") ||
            !HasDelegate(component, "OnClick"))
        {
            return;
        }

        if (!HasText(component, "AccessibleName") && !HasText(component, "Name"))
        {
            Add(violations, "A11Y-INTERACTIVE-NAME", "クリック可能な非ボタン要素には AccessibleName を指定してください。", "2.5.3, 4.1.2");
        }

        if (!HasDelegate(component, "OnKeyboardActivate"))
        {
            Add(
                violations,
                "A11Y-KEYBOARD-ACTIVATE",
                "クリック操作に対応する OnKeyboardActivate で Enter または Space 操作を提供してください。",
                "2.1.1");
        }
    }

    /// <summary>
    /// ドラッグ、ドロップ、列並べ替えにキーボード代替があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateKeyboardAlternative(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        var usesDrag =
            GetValue<bool>(component, "Draggable") ||
            GetValue<bool>(component, "Droppable") ||
            GetValue<bool>(component, "ReorderableColumns") ||
            HasDelegate(component, "OnCellClick") ||
            HasDelegate(component, "OnRowClick") ||
            HasDelegate(component, "OnRowDoubleClick") ||
            HasDelegate(component, "OnDragStart") ||
            HasDelegate(component, "OnDropEnd") ||
            componentName == "FluentSortableList" && HasDelegate(component, "OnUpdate");

        if (usesDrag && !HasDelegate(component, "OnKeyboardAlternative"))
        {
            Add(
                violations,
                "A11Y-KEYBOARD-ALTERNATIVE",
                "ドラッグまたは並べ替え操作には OnKeyboardAlternative で同等のキーボード操作を提供してください。",
                "2.1.1");
        }
    }

    /// <summary>
    /// 画像、アイコン、バッジなどの状態に応じて必要な名前を検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateConditionalContentNames(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName == "FluentAvatar" && HasText(component, "Image") && !HasText(component, "Name"))
        {
            Add(violations, "A11Y-AVATAR-NAME", "画像を表示する Avatar には利用者が表す対象を示す Name を指定してください。", "1.1.1, 4.1.2");
        }

        if (componentName is "FluentBadge" or "FluentCounterBadge" &&
            (GetValue(component, "IconStart") is not null || GetValue(component, "IconEnd") is not null) &&
            !HasText(component, "Content") &&
            !HasText(component, "IconLabel") &&
            !HasAccessibleName(component) &&
            GetValue(component, "ChildContent") is null)
        {
            Add(violations, "A11Y-BADGE-NAME", "アイコンのみのバッジには IconLabel または AccessibleName を指定してください。", "1.1.1, 4.1.2");
        }

        if (componentName == "FluentColorPicker" && !HasAccessibleName(component))
        {
            Add(violations, "A11Y-COLOR-PICKER-NAME", "色選択コンポーネントには選択目的を示す AccessibleName を指定してください。", "1.3.1, 4.1.2");
        }

        if (componentName == "FluentOption" &&
            GetValue(component, "ChildContent") is null &&
            !HasText(component, "Text") &&
            !HasAccessibleName(component))
        {
            Add(violations, "A11Y-OPTION-NAME", "選択肢には表示テキスト、ChildContent、または AccessibleName を指定してください。", "4.1.2");
        }

        if (GetValue<bool>(component, "DisabledFocusable") &&
            !HasText(component, "AccessibleDescription"))
        {
            Add(
                violations,
                "A11Y-DISABLED-REASON",
                "フォーカス可能な無効コントロールには AccessibleDescription で現在利用できない理由を説明してください。",
                "3.3.2, 4.1.2");
        }

        if (GetValue<bool>(component, "RepresentsPersonalData"))
        {
            var autoComplete = GetValue<string>(component, "AutoComplete");
            if (string.IsNullOrWhiteSpace(autoComplete) ||
                string.Equals(autoComplete.Trim(), "off", StringComparison.OrdinalIgnoreCase))
            {
                Add(
                    violations,
                    "A11Y-AUTOCOMPLETE-PURPOSE",
                    "RepresentsPersonalData=true の場合は利用目的に対応する AutoComplete を指定してください。",
                    "1.3.5");
            }
        }
    }

    /// <summary>
    /// ダイアログに名前があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateDialogName(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName == "FluentDialogBody" &&
            !HasText(component, "AccessibleName") &&
            GetValue(component, "TitleTemplate") is null)
        {
            Add(violations, "A11Y-DIALOG-NAME", "ダイアログの目的を示す TitleTemplate または AccessibleName を指定してください。", "2.4.6, 4.1.2");
        }
    }

    /// <summary>
    /// メニュー、ナビゲーション、ツリー項目に利用者へ分かる名前があることを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateMenuAndTreeItems(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        var valid = componentName switch
        {
            "FluentMenuItem" => HasText(component, "Label") ||
                HasText(component, "AccessibleName") ||
                GetValue(component, "ChildContent") is not null,
            "FluentAppBarItem" => HasText(component, "Text") ||
                HasText(component, "AccessibleName") ||
                GetValue(component, "ChildContent") is not null,
            "FluentNavItem" or "FluentTreeItem" => GetValue(component, "ChildContent") is not null ||
                HasText(component, "AccessibleName") ||
                HasText(component, "Text"),
            _ => true,
        };

        if (!valid)
        {
            Add(violations, "A11Y-ITEM-NAME", "メニュー、ナビゲーション、またはツリー項目には可視テキストまたは AccessibleName を指定してください。", "2.4.6, 4.1.2");
        }
    }

    /// <summary>
    /// ツールチップが対象要素と説明内容を持つことを検証します。
    /// </summary>
    /// <param name="component">検証対象です。</param>
    /// <param name="componentName">Fluent UI コンポーネント名です。</param>
    /// <param name="violations">違反を追加する一覧です。</param>
    private static void ValidateTooltip(
        IComponent component,
        string componentName,
        List<AccessibilityViolation> violations)
    {
        if (componentName != "FluentTooltip")
        {
            return;
        }

        if (!HasText(component, "Anchor"))
        {
            Add(violations, "A11Y-TOOLTIP-ANCHOR", "ツールチップに説明対象の Anchor を指定してください。", "1.3.1, 4.1.2");
        }

        if (GetValue(component, "ChildContent") is null)
        {
            Add(violations, "A11Y-TOOLTIP-CONTENT", "ツールチップの説明内容を指定してください。", "1.3.1");
        }
    }

    /// <summary>
    /// 違反を一覧へ追加します。
    /// </summary>
    /// <param name="violations">違反の追加先です。</param>
    /// <param name="ruleId">規則識別子です。</param>
    /// <param name="message">修正方法を含むメッセージです。</param>
    /// <param name="criteria">関連する WCAG 達成基準です。</param>
    private static void Add(
        List<AccessibilityViolation> violations,
        string ruleId,
        string message,
        string criteria)
    {
        violations.Add(new AccessibilityViolation(ruleId, message, criteria));
    }

    /// <summary>
    /// 指定プロパティに有効な文字列があるかを判定します。
    /// </summary>
    /// <param name="component">値を取得するコンポーネントです。</param>
    /// <param name="propertyName">プロパティ名です。</param>
    /// <returns>空白ではない文字列なら <see langword="true" /> です。</returns>
    private static bool HasText(IComponent component, string propertyName)
    {
        return !string.IsNullOrWhiteSpace(GetValue<string>(component, propertyName));
    }

    /// <summary>
    /// コンポーネントがアクセシブルな名前の指定方法を持つかを判定します。
    /// </summary>
    /// <param name="component">名前を持つコンポーネントです。</param>
    /// <returns>AccessibleName または AriaLabel があれば <see langword="true" /> です。</returns>
    private static bool HasAccessibleName(IComponent component)
    {
        return HasText(component, "AccessibleName") || HasText(component, "AriaLabel");
    }

    /// <summary>
    /// 数値プロパティが正の値を持つかを判定します。
    /// </summary>
    /// <param name="component">値を取得するコンポーネントです。</param>
    /// <param name="propertyName">プロパティ名です。</param>
    /// <returns>正の数値ならその値、存在しない場合はゼロです。</returns>
    private static decimal GetPositiveNumber(IComponent component, string propertyName)
    {
        var value = GetValue(component, propertyName);
        if (value is null)
        {
            return 0;
        }

        try
        {
            return Convert.ToDecimal(value);
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException)
        {
            return 0;
        }
    }

    /// <summary>
    /// 指定した EventCallback に利用者処理が登録されているかを判定します。
    /// </summary>
    /// <param name="component">イベントを持つコンポーネントです。</param>
    /// <param name="propertyName">EventCallback のプロパティ名です。</param>
    /// <returns>デリゲートが登録されている場合は <see langword="true" /> です。</returns>
    private static bool HasDelegate(IComponent component, string propertyName)
    {
        var callback = GetValue(component, propertyName);
        return (bool)(callback?.GetType().GetProperty("HasDelegate")?.GetValue(callback) ?? false);
    }

    /// <summary>
    /// 指定した型のプロパティ値を取得します。
    /// </summary>
    /// <typeparam name="T">取得する値の型です。</typeparam>
    /// <param name="component">値を取得するコンポーネントです。</param>
    /// <param name="propertyName">プロパティ名です。</param>
    /// <returns>存在する場合は値、存在しないか型が一致しない場合は既定値です。</returns>
    private static T? GetValue<T>(IComponent component, string propertyName)
    {
        var value = GetValue(component, propertyName);
        return value is T typed ? typed : default;
    }

    /// <summary>
    /// コンポーネントの公開プロパティ値を取得します。
    /// </summary>
    /// <param name="component">値を取得するコンポーネントです。</param>
    /// <param name="propertyName">プロパティ名です。</param>
    /// <returns>プロパティがなければ <see langword="null" /> です。</returns>
    private static object? GetValue(IComponent component, string propertyName)
    {
        return component.GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
            ?.GetValue(component);
    }
}
