namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// コンポーネントに対するアクセシビリティ契約の検証結果を表します。
/// </summary>
public sealed class AccessibilityValidationResult
{
    /// <summary>
    /// 検証結果を初期化します。
    /// </summary>
    /// <param name="componentName">検証したコンポーネント名です。</param>
    /// <param name="violations">検出された違反の一覧です。</param>
    public AccessibilityValidationResult(
        string componentName,
        IReadOnlyList<AccessibilityViolation> violations)
    {
        ComponentName = componentName;
        Violations = violations;
    }

    /// <summary>
    /// 検証したコンポーネント名を取得します。
    /// </summary>
    public string ComponentName { get; }

    /// <summary>
    /// 検出された違反の一覧を取得します。
    /// </summary>
    public IReadOnlyList<AccessibilityViolation> Violations { get; }

    /// <summary>
    /// 違反がない場合は <see langword="true" /> を返します。
    /// </summary>
    public bool IsValid => Violations.Count == 0;
}
