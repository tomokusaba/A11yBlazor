namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// アクセシビリティ契約の違反を表す例外です。
/// </summary>
public sealed class AccessibilityValidationException : InvalidOperationException
{
    /// <summary>
    /// 検証結果から例外を初期化します。
    /// </summary>
    /// <param name="result">失敗した検証結果です。</param>
    public AccessibilityValidationException(AccessibilityValidationResult result)
        : base(CreateMessage(result))
    {
        Result = result;
    }

    /// <summary>
    /// 失敗した検証結果を取得します。
    /// </summary>
    public AccessibilityValidationResult Result { get; }

    /// <summary>
    /// 検証結果を例外メッセージへ変換します。
    /// </summary>
    /// <param name="result">変換対象の検証結果です。</param>
    /// <returns>利用者が修正箇所を特定できるメッセージです。</returns>
    private static string CreateMessage(AccessibilityValidationResult result)
    {
        var details = string.Join(
            Environment.NewLine,
            result.Violations.Select(
                violation => $"- [{violation.RuleId}] {violation.Message} (WCAG {violation.WcagCriteria})"));

        return $"{result.ComponentName} のアクセシビリティ契約に違反しています。{Environment.NewLine}{details}";
    }
}
