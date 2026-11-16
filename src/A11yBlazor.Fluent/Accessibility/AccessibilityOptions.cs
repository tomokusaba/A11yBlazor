namespace A11yBlazor.Fluent.Accessibility;

/// <summary>
/// アクセシビリティ契約の検証動作を構成します。
/// </summary>
public sealed class AccessibilityOptions
{
    /// <summary>
    /// 契約違反時の処理方法を取得または設定します。
    /// </summary>
    public AccessibilityEnforcementMode EnforcementMode { get; set; } = AccessibilityEnforcementMode.Throw;

    /// <summary>
    /// 契約検証を有効にするかどうかを取得または設定します。
    /// </summary>
    public bool Enabled { get; set; } = true;
}
