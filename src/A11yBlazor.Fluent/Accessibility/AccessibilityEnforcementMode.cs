namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// アクセシビリティ契約違反の処理方法を表します。
/// </summary>
public enum AccessibilityEnforcementMode
{
    /// <summary>
    /// 契約違反を検出時に例外として通知します。
    /// </summary>
    Throw = 0,

    /// <summary>
    /// 契約違反を警告ログとして通知し、描画を継続します。
    /// </summary>
    Log = 1,

    /// <summary>
    /// 契約違反を検出せず、通常の描画を継続します。
    /// </summary>
    Ignore = 2,
}
