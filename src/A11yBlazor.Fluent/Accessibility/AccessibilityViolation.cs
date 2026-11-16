namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// 検出されたアクセシビリティ契約違反を表します。
/// </summary>
public sealed class AccessibilityViolation
{
    /// <summary>
    /// 検出された違反を初期化します。
    /// </summary>
    /// <param name="ruleId">規則の識別子です。</param>
    /// <param name="message">利用者向けの違反内容です。</param>
    /// <param name="wcagCriteria">関連する WCAG 2.1 の達成基準です。</param>
    public AccessibilityViolation(string ruleId, string message, string wcagCriteria)
    {
        RuleId = ruleId;
        Message = message;
        WcagCriteria = wcagCriteria;
    }

    /// <summary>
    /// 規則の識別子を取得します。
    /// </summary>
    public string RuleId { get; }

    /// <summary>
    /// 利用者向けの違反内容を取得します。
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// 関連する WCAG 2.1 の達成基準を取得します。
    /// </summary>
    public string WcagCriteria { get; }
}
