using System.Reflection;
using A11yBlazor.Fluent.Accessibility;
using A11yBlazor.Fluent.Components;
using A11yBlazor.Fluent.DependencyInjection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

#pragma warning disable BL0005

namespace A11yBlazor.Fluent.Tests;

/// <summary>
/// パッケージのアクセシビリティ契約を検証するテストです。
/// </summary>
public sealed class AccessibilityContractTests
{
    /// <summary>
    /// Generic Host で検証器と Fluent UI の依存関係を解決できることを確認します。
    /// </summary>
    [Fact]
    public void AddA11yBlazorFluent_RegistersRequiredServices()
    {
        using var host = CreateHost();

        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var configuration = host.Services.GetRequiredService<LibraryConfiguration>();

        Assert.IsType<AccessibilityValidator>(validator);
        Assert.NotNull(configuration);
    }

    /// <summary>
    /// テキスト入力が必須の可視ラベルを要求することを確認します。
    /// </summary>
    [Fact]
    public void TextInput_RequiresVisibleLabel()
    {
        using var host = CreateHost();
        var input = CreateComponent<A11yFluentTextInput>(host.Services);
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();

        var invalid = validator.Inspect(input);
        input.Label = "氏名";
        var valid = validator.Inspect(input);

        Assert.Contains(invalid.Violations, violation => violation.RuleId == "A11Y-LABEL");
        Assert.DoesNotContain(valid.Violations, violation => violation.RuleId == "A11Y-LABEL");
    }

    /// <summary>
    /// テキスト入力のラベルが Razor の必須パラメーターとして公開されることを確認します。
    /// </summary>
    [Fact]
    public void TextInput_ExposesEditorRequiredLabel()
    {
        var label = typeof(A11yFluentTextInput).GetProperty(
            nameof(A11yFluentTextInput.Label),
            BindingFlags.Instance | BindingFlags.Public);

        Assert.NotNull(label);
        Assert.NotNull(label.GetCustomAttribute<EditorRequiredAttribute>());
    }

    /// <summary>
    /// 情報画像には代替テキスト、装飾画像には明示指定が必要なことを確認します。
    /// </summary>
    [Fact]
    public void Image_RequiresAlternativeTextUnlessDecorative()
    {
        using var host = CreateHost();
        var image = CreateComponent<A11yFluentImage>(host.Services);
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();

        var missingAlternative = validator.Inspect(image);
        image.AlternateText = "製品の全景";
        var informative = validator.Inspect(image);
        image.AlternateText = null;
        image.Decorative = true;
        var decorative = validator.Inspect(image);

        Assert.Contains(missingAlternative.Violations, violation => violation.RuleId == "A11Y-IMAGE-ALT");
        Assert.True(informative.IsValid);
        Assert.True(decorative.IsValid);
    }

    /// <summary>
    /// アイコンだけの操作と読み込み状態に状態固有の名前が必要なことを確認します。
    /// </summary>
    [Fact]
    public void Button_RequiresNamesForIconOnlyAndLoadingStates()
    {
        using var host = CreateHost();
        var button = CreateComponent<A11yFluentButton>(host.Services);
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        button.IconOnly = true;
        button.Loading = true;

        var invalid = validator.Inspect(button);
        button.AccessibleName = "再読み込み";
        button.LoadingLabel = "再読み込み中";
        var valid = validator.Inspect(button);

        Assert.Contains(invalid.Violations, violation => violation.RuleId == "A11Y-NAME");
        Assert.Contains(invalid.Violations, violation => violation.RuleId == "A11Y-LOADING");
        Assert.True(valid.IsValid);
    }

    /// <summary>
    /// ファイル入力がラベルではなくアクセシブル名を要求することを確認します。
    /// </summary>
    [Fact]
    public void InputFile_RequiresAccessibleName()
    {
        using var host = CreateHost();
        var inputFile = CreateComponent<A11yFluentInputFile>(host.Services);
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();

        var invalid = validator.Inspect(inputFile);
        inputFile.AccessibleName = "添付ファイルを選択";
        var valid = validator.Inspect(inputFile);

        Assert.Contains(invalid.Violations, violation => violation.RuleId == "A11Y-STANDALONE-NAME");
        Assert.DoesNotContain(valid.Violations, violation => violation.RuleId == "A11Y-STANDALONE-NAME");
    }

    /// <summary>
    /// 入力形式や選択・ファイル制約に追加説明が必要なことを確認します。
    /// </summary>
    [Fact]
    public void ConditionalInputConstraints_RequireInstructions()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var input = CreateComponent<A11yFluentTextInput>(host.Services);
        input.Label = "電話番号";
        input.MaskPattern = "+81-000-000-000";

        var missingInstructions = validator.Inspect(input);
        input.AccessibleDescription = "国番号を含む日本の電話番号を数字で入力してください。";
        var described = validator.Inspect(input);

        Assert.Contains(
            missingInstructions.Violations,
            violation => violation.RuleId == "A11Y-CONDITIONAL-INSTRUCTIONS");
        Assert.True(described.IsValid);
    }

    /// <summary>
    /// 複数選択には複数選択方法の説明が必要なことを確認します。
    /// </summary>
    [Fact]
    public void MultipleSelection_RequiresSelectionInstructions()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var select = CreateComponent<A11yFluentSelect<string, string>>(host.Services);
        select.Label = "担当者";
        select.Multiple = true;

        var missingInstructions = validator.Inspect(select);
        select.AccessibleDescription = "複数項目を選択できます。";
        var described = validator.Inspect(select);

        Assert.Contains(
            missingInstructions.Violations,
            violation => violation.RuleId == "A11Y-CONDITIONAL-INSTRUCTIONS");
        Assert.True(described.IsValid);
    }

    /// <summary>
    /// 新しいタブを開くリンクには事前説明が必要なことを確認します。
    /// </summary>
    [Fact]
    public void LinkWithBlankTarget_RequiresNewWindowDescription()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var link = CreateComponent<A11yFluentLink>(host.Services);
        link.Href = "https://example.com";
        link.Target = LinkTarget.Blank;
        link.AccessibleName = "ヘルプ";

        var missingDescription = validator.Inspect(link);
        link.NewWindowDescription = "新しいタブで開きます。";
        var described = validator.Inspect(link);

        Assert.Contains(
            missingDescription.Violations,
            violation => violation.RuleId == "A11Y-NEW-WINDOW");
        Assert.True(described.IsValid);
    }

    /// <summary>
    /// クリック可能な非ボタン要素にキーボード操作が必要なことを確認します。
    /// </summary>
    [Fact]
    public void ClickableCard_RequiresKeyboardActivation()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var card = CreateComponent<A11yFluentCard>(host.Services);
        card.AccessibleName = "詳細を開く";
        card.OnClick = EventCallback.Factory.Create<MouseEventArgs>(this, static () => { });

        var missingKeyboard = validator.Inspect(card);
        card.OnKeyboardActivate = EventCallback.Factory.Create<KeyboardEventArgs>(
            this,
            static (KeyboardEventArgs _) => { });
        var keyboardEnabled = validator.Inspect(card);

        Assert.Contains(
            missingKeyboard.Violations,
            violation => violation.RuleId == "A11Y-KEYBOARD-ACTIVATE");
        Assert.True(keyboardEnabled.IsValid);
    }

    /// <summary>
    /// ドラッグ操作に同等のキーボード代替が必要なことを確認します。
    /// </summary>
    [Fact]
    public void DraggableDropZone_RequiresKeyboardAlternative()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var zone = CreateComponent<A11yFluentDropZone<string>>(host.Services);
        zone.Draggable = true;

        var missingAlternative = validator.Inspect(zone);
        zone.OnKeyboardAlternative = EventCallback.Factory.Create<KeyboardEventArgs>(
            this,
            static (KeyboardEventArgs _) => { });
        var keyboardEnabled = validator.Inspect(zone);

        Assert.Contains(
            missingAlternative.Violations,
            violation => violation.RuleId == "A11Y-KEYBOARD-ALTERNATIVE");
        Assert.True(keyboardEnabled.IsValid);
    }

    /// <summary>
    /// 本人データとして明示した入力に自動入力目的が必要なことを確認します。
    /// </summary>
    [Fact]
    public void PersonalDataInput_RequiresAutocompletePurpose()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var input = CreateComponent<A11yFluentTextInput>(host.Services);
        input.Label = "氏名";
        input.RepresentsPersonalData = true;

        var missingPurpose = validator.Inspect(input);
        input.AutoComplete = "name";
        var identified = validator.Inspect(input);

        Assert.Contains(
            missingPurpose.Violations,
            violation => violation.RuleId == "A11Y-AUTOCOMPLETE-PURPOSE");
        Assert.True(identified.IsValid);
    }

    /// <summary>
    /// フォーカス可能な無効操作には理由の説明が必要なことを確認します。
    /// </summary>
    [Fact]
    public void FocusableDisabledButton_RequiresReason()
    {
        using var host = CreateHost();
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        var button = CreateComponent<A11yFluentButton>(host.Services);
        button.ChildContent = builder => builder.AddContent(0, "削除");
        button.DisabledFocusable = true;

        var missingReason = validator.Inspect(button);
        button.AccessibleDescription = "権限がないため現在削除できません。";
        var explained = validator.Inspect(button);

        Assert.Contains(
            missingReason.Violations,
            violation => violation.RuleId == "A11Y-DISABLED-REASON");
        Assert.True(explained.IsValid);
    }

    /// <summary>
    /// クリック可能なアイコンが装飾として隠されないことを確認します。
    /// </summary>
    [Fact]
    public void Icon_RejectsDecorativeInteractiveState()
    {
        using var host = CreateHost();
        var icon = ActivatorUtilities.CreateInstance<A11yFluentIcon<Icon>>(host.Services);
        var validator = host.Services.GetRequiredService<IAccessibilityValidator>();
        icon.OnClick = EventCallback.Factory.Create<MouseEventArgs>(this, static () => { });

        var result = validator.Inspect(icon);

        Assert.Contains(result.Violations, violation => violation.RuleId == "A11Y-INTERACTIVE-ICON");
    }

    /// <summary>
    /// 公開 Fluent UI コンポーネントすべてに対応する継承ラッパーがあることを確認します。
    /// </summary>
    [Fact]
    public void AllPublicFluentComponents_HaveGeneratedWrappers()
    {
        var fluentTypes = typeof(FluentButton).Assembly
            .GetExportedTypes()
            .Where(type => type.Namespace == "Microsoft.FluentUI.AspNetCore.Components")
            .Where(type => typeof(IComponent).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract && !type.IsInterface && type.IsClass)
            .Where(type => !type.IsNested)
            .ToArray();

        var wrapperAssembly = typeof(A11yFluentButton).Assembly;
        var missing = fluentTypes
            .Where(type => wrapperAssembly.GetType($"A11yBlazor.Fluent.Components.A11y{type.FullName!.Split('.')[^1]}") is null)
            .Select(type => type.Name)
            .ToArray();

        Assert.NotEmpty(fluentTypes);
        Assert.Empty(missing);
    }

    /// <summary>
    /// テスト用の Generic Host を作成します。
    /// </summary>
    /// <returns>サービスを登録済みのホストです。</returns>
    private static IHost CreateHost()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddA11yBlazorFluent();
        return builder.Build();
    }

    /// <summary>
    /// Generic Host の依存性注入から指定したコンポーネントを作成します。
    /// </summary>
    /// <typeparam name="TComponent">作成するコンポーネント型です。</typeparam>
    /// <param name="services">依存性注入コンテナーです。</param>
    /// <returns>依存関係を解決したコンポーネントです。</returns>
    private static TComponent CreateComponent<TComponent>(IServiceProvider services)
        where TComponent : IComponent
    {
        return ActivatorUtilities.CreateInstance<TComponent>(services);
    }
}
