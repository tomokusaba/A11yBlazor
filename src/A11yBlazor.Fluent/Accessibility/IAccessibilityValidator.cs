using Microsoft.AspNetCore.Components;

namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// コンポーネントのアクセシビリティ契約を検証する機能を提供します。
/// </summary>
public interface IAccessibilityValidator
{
    /// <summary>
    /// コンポーネントの契約を検証し、構成された処理方法で違反を通知します。
    /// </summary>
    /// <param name="component">検証対象のコンポーネントです。</param>
    void Validate(IComponent component);

    /// <summary>
    /// コンポーネントの契約を検証し、例外またはログを出さず結果を返します。
    /// </summary>
    /// <param name="component">検証対象のコンポーネントです。</param>
    /// <returns>検証結果です。</returns>
    AccessibilityValidationResult Inspect(IComponent component);
}
