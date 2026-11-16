using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// Fluent UI コンポーネントへアクセシビリティ属性を追加します。
/// </summary>
public static class AccessibilityAttributeHelper
{
    /// <summary>
    /// コンポーネントの公開パラメーターからアクセシビリティ属性を構築して既存属性へマージします。
    /// </summary>
    /// <param name="component">マージ対象のコンポーネントです。</param>
    /// <returns>追加属性を格納した新しい読み取り専用辞書です。</returns>
    public static IReadOnlyDictionary<string, object> Merge(IComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);

        var componentName = GetFluentComponentName(component.GetType());
        return MergeCore(
            GetValue<IReadOnlyDictionary<string, object>>(component, "AdditionalAttributes"),
            GetValue<string>(component, "AccessibleName"),
            GetValue<string>(component, "AccessibleDescription"),
            GetValue<bool>(component, "Decorative"),
            GetValue<string>(component, "LoadingLabel"),
            GetValue(component, "Loading") is bool and true,
            GetValue<string>(component, "NewWindowDescription"),
            GetValue(component, "Target")?.ToString() == "Blank",
            GetValue<EventCallback<KeyboardEventArgs>>(component, "OnKeyboardActivate"),
            componentName is "FluentAvatar" or "FluentCard" or "FluentIcon" or "FluentEmoji",
            GetValue<EventCallback<KeyboardEventArgs>>(component, "OnKeyboardAlternative"),
            componentName is "FluentDataGrid" or "FluentDragContainer" or
                "FluentDropZone" or "FluentSortableList");
    }

    /// <summary>
    /// 互換性のため、個別引数を設定オブジェクトへまとめて既存属性へマージします。
    /// </summary>
    /// <param name="attributes">既存の追加属性です。</param>
    /// <param name="accessibleName">利用者へ公開する名前です。</param>
    /// <param name="accessibleDescription">利用者へ公開する説明です。</param>
    /// <param name="decorativeIcon">アイコンを装飾として支援技術から隠すかどうかです。</param>
    /// <param name="loadingLabel">読み込み中に公開する状態名です。</param>
    /// <param name="loading">読み込み中かどうかです。</param>
    /// <param name="newWindowDescription">新しい閲覧コンテキストを説明するテキストです。</param>
    /// <param name="opensNewWindow">リンクが新しい閲覧コンテキストを開くかどうかです。</param>
    /// <param name="keyboardActivation">クリック操作のキーボード代替処理です。</param>
    /// <param name="interactiveRoot">ルート要素へ操作可能な意味を付けるかどうかです。</param>
    /// <param name="keyboardAlternative">ドラッグ操作のキーボード代替処理です。</param>
    /// <param name="draggableRoot">ルート要素がドラッグ操作の起点になるかどうかです。</param>
    /// <returns>追加属性を格納した新しい読み取り専用辞書です。</returns>
    [Obsolete("AccessibilityAttributeHelper.Merge(IComponent) を使用してください。")]
    public static IReadOnlyDictionary<string, object> Merge(
        IReadOnlyDictionary<string, object>? attributes,
        string? accessibleName,
        string? accessibleDescription,
        bool decorativeIcon,
        string? loadingLabel,
        bool loading,
        string? newWindowDescription = null,
        bool opensNewWindow = false,
        EventCallback<KeyboardEventArgs> keyboardActivation = default,
        bool interactiveRoot = false,
        EventCallback<KeyboardEventArgs> keyboardAlternative = default,
        bool draggableRoot = false)
    {
        return MergeCore(
            attributes,
            accessibleName,
            accessibleDescription,
            decorativeIcon,
            loadingLabel,
            loading,
            newWindowDescription,
            opensNewWindow,
            keyboardActivation,
            interactiveRoot,
            keyboardAlternative,
            draggableRoot);
    }

    /// <summary>
    /// 解決済みのアクセシビリティ値を既存属性へマージします。
    /// </summary>
    /// <param name="attributes">既存の追加属性です。</param>
    /// <param name="accessibleName">利用者へ公開する名前です。</param>
    /// <param name="accessibleDescription">利用者へ公開する説明です。</param>
    /// <param name="decorativeIcon">アイコンを装飾として隠すかどうかです。</param>
    /// <param name="loadingLabel">読み込み中に公開する状態名です。</param>
    /// <param name="loading">読み込み中かどうかです。</param>
    /// <param name="newWindowDescription">新しい閲覧コンテキストを説明するテキストです。</param>
    /// <param name="opensNewWindow">新しい閲覧コンテキストを開くかどうかです。</param>
    /// <param name="keyboardActivation">クリック操作のキーボード代替です。</param>
    /// <param name="interactiveRoot">ルート要素を操作可能にするかどうかです。</param>
    /// <param name="keyboardAlternative">ドラッグ操作のキーボード代替です。</param>
    /// <param name="draggableRoot">ルート要素がドラッグ起点かどうかです。</param>
    /// <returns>追加属性を格納した新しい読み取り専用辞書です。</returns>
    private static IReadOnlyDictionary<string, object> MergeCore(
        IReadOnlyDictionary<string, object>? attributes,
        string? accessibleName,
        string? accessibleDescription,
        bool decorativeIcon,
        string? loadingLabel,
        bool loading,
        string? newWindowDescription,
        bool opensNewWindow,
        EventCallback<KeyboardEventArgs> keyboardActivation,
        bool interactiveRoot,
        EventCallback<KeyboardEventArgs> keyboardAlternative,
        bool draggableRoot)
    {
        var result = attributes is null
            ? new Dictionary<string, object>(StringComparer.Ordinal)
            : new Dictionary<string, object>(attributes, StringComparer.Ordinal);

        if (loading && !string.IsNullOrWhiteSpace(loadingLabel))
        {
            result["aria-label"] = loadingLabel;
            result["aria-busy"] = "true";
        }
        else if (!string.IsNullOrWhiteSpace(accessibleName))
        {
            result["aria-label"] = accessibleName;
        }

        var descriptions = new[]
        {
            accessibleDescription,
            opensNewWindow ? newWindowDescription : null,
        }
        .Where(description => !string.IsNullOrWhiteSpace(description))
        .ToArray();
        if (descriptions.Length > 0)
        {
            result["aria-description"] = string.Join(" ", descriptions);
        }

        if (decorativeIcon && !keyboardActivation.HasDelegate)
        {
            result["aria-hidden"] = "true";
            result["focusable"] = "false";
        }

        if (interactiveRoot && keyboardActivation.HasDelegate)
        {
            result.TryAdd("role", "button");
            result.TryAdd("tabindex", 0);
            result["onkeydown"] = keyboardActivation;
        }
        else if (draggableRoot && keyboardAlternative.HasDelegate)
        {
            result.TryAdd("tabindex", 0);
            result["onkeydown"] = keyboardAlternative;
        }

        return result;
    }

    /// <summary>
    /// ラッパーの継承元にある Fluent UI コンポーネント名を取得します。
    /// </summary>
    /// <param name="componentType">対象コンポーネントの型です。</param>
    /// <returns>Fluent UI コンポーネント名です。</returns>
    private static string GetFluentComponentName(Type componentType)
    {
        var current = componentType;
        while (current.BaseType is not null)
        {
            current = current.BaseType;
            if (current.Namespace == "Microsoft.FluentUI.AspNetCore.Components")
            {
                return current.Name.Split('`')[0];
            }
        }

        return componentType.Name;
    }

    /// <summary>
    /// コンポーネントの指定した公開プロパティから値を取得します。
    /// </summary>
    /// <typeparam name="T">取得する値の型です。</typeparam>
    /// <param name="component">値を取得するコンポーネントです。</param>
    /// <param name="propertyName">プロパティ名です。</param>
    /// <returns>取得した値、または型に応じた既定値です。</returns>
    private static T? GetValue<T>(IComponent component, string propertyName)
    {
        var value = GetValue(component, propertyName);
        return value is T typed ? typed : default;
    }

    /// <summary>
    /// コンポーネントの指定した公開プロパティから値を取得します。
    /// </summary>
    /// <param name="component">値を取得するコンポーネントです。</param>
    /// <param name="propertyName">プロパティ名です。</param>
    /// <returns>取得した値、またはプロパティがなければ <see langword="null" /> です。</returns>
    private static object? GetValue(IComponent component, string propertyName)
    {
        return component.GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)
            ?.GetValue(component);
    }
}
