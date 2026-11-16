using A11yBlazor.Fluent.Accessibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace A11yBlazor.Fluent.DependencyInjection;

/// <summary>
/// A11yBlazor.Fluent の依存性注入登録を提供します。
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// A11yBlazor.Fluent の検証サービスを依存性注入コンテナーへ登録します。
    /// </summary>
    /// <param name="services">サービス登録先です。</param>
    /// <param name="configure">検証動作を構成するコールバックです。</param>
    /// <returns>チェーン登録用のサービスコレクションです。</returns>
    public static IServiceCollection AddA11yBlazorFluent(
        this IServiceCollection services,
        Action<AccessibilityOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<AccessibilityOptions>();
        if (configure is not null)
        {
            services.Configure(configure);
        }

        services.AddFluentUIComponents();
        services.AddSingleton<IAccessibilityValidator, AccessibilityValidator>();
        return services;
    }
}
