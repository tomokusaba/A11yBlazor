# A11yBlazor.Fluent

`A11yBlazor.Fluent` は Microsoft 非公式のアクセシビリティ拡張パッケージであり、
Fluent UI Blazor 5 の公開 UI コンポーネントを継承し、WCAG 2.1 AA を意識した
必須パラメーターと状態依存の契約を提供する Razor コンポーネントパッケージです。
対応フレームワークは .NET 8、.NET 9、.NET 10、.NET 11 です。

## 特徴

- Fluent UI Blazor 5 の公開具体コンポーネント 109 種類を継承したラッパー
- フォーム入力に必須の可視 `Label`
- 情報画像に必須の `AlternateText`
- `Decorative="true"` を明示した装飾画像では代替テキスト不要
- アイコンのみの操作、読み込み状態、エラー状態、グリッド、ダイアログの名前と通知
- `AccessibleName`、`AccessibleDescription`、`LoadingLabel` の共通パラメーター
- 条件付きの `NewWindowDescription`、`OnKeyboardActivate`、
  `OnKeyboardAlternative`、`RepresentsPersonalData`
- Generic Host を含む依存性注入コンテナーへの登録
- 構成可能な `Throw`、`Log`、`Ignore` の違反処理

## インストール

```xml
<PackageReference Include="A11yBlazor.Fluent" Version="0.1.0" />
```

Fluent UI Blazor 5 のスタイルとスクリプトは通常どおりアプリに組み込んでください。

## DI 登録

`AddA11yBlazorFluent` は Fluent UI 本体のサービスも登録します。

```csharp
using A11yBlazor.Fluent.DependencyInjection;
```

```csharp
services.AddA11yBlazorFluent();
```

既定では契約違反が `AccessibilityValidationException` になります。既存アプリへ
段階的に導入するときはログモードへ切り替えられます。

```csharp
services.AddA11yBlazorFluent(options =>
{
    options.EnforcementMode = AccessibilityEnforcementMode.Log;
});
```

## Razor での利用

`_Imports.razor` に名前空間を追加します。

```razor
@using A11yBlazor.Fluent.Components
```

テキスト入力にはラベルが必須です。

```razor
<A11yFluentTextInput Label="氏名"
                     Required="true"
                     @bind-Value="Name" />
```

情報を持つ画像は代替テキストが必要です。

```razor
<A11yFluentImage Source="/images/product.png"
                 AlternateText="青い外装の製品を正面から写した写真" />
```

純粋な装飾画像は明示的に指定します。

```razor
<A11yFluentImage Source="/images/separator.png"
                 Decorative="true" />
```

アイコンだけの操作や非同期処理には状態固有の名前を指定します。

```razor
<A11yFluentButton IconOnly="true"
                  IconStart="@(new Icons.Regular.Size20.ArrowClockwise())"
                  AccessibleName="一覧を再読み込み"
                  Loading="@isLoading"
                  LoadingLabel="一覧を読み込み中"
                  OnClick="ReloadAsync" />
```

エラー状態では問題と修正方法を利用者へ示してください。

```razor
<A11yFluentTextInput Label="メールアドレス"
                     MessageState="MessageState.Error"
                     Message="メールアドレスの @ を含む形式で入力してください。"
                     @bind-Value="Email" />
```

## 追加パラメーターに応じた契約

同じコンポーネントでも、次の追加パラメーターを使う場合は対応する入力が必要です。

| 追加状態 | 必要な入力 |
|---|---|
| アイコンのみ、画像のみ、または子コンテンツのない操作 | `AccessibleName` |
| `MaskPattern`、`Pattern`、入力範囲、複数選択、ファイル制限 | 制約と操作方法を示す `AccessibleDescription` |
| `Target="Blank"` | 事前に新規タブまたはウィンドウを説明する `NewWindowDescription` |
| Card、Avatar、Icon、Emoji の `OnClick` | 同じ操作を行う `OnKeyboardActivate` |
| ドラッグ、ドロップ、列や項目の並べ替え | 同等の操作を行う `OnKeyboardAlternative` |
| `Loading="true"` | 進行状態を示す `LoadingLabel` |
| `DisabledFocusable="true"` | 利用できない理由を示す `AccessibleDescription` |
| 本人に関する入力を表すフィールド | `RepresentsPersonalData="true"` と対応する `AutoComplete` |

`OnKeyboardActivate` は Enter または Space を判定して元の操作を実行し、
`OnKeyboardAlternative` は矢印キー、ボタン、フォームなどで同等の並べ替えを実現します。
説明文字列だけでキーボード操作が実装された扱いにはなりません。

新しいタブを開く例:

```razor
<A11yFluentLink Href="/help"
                Target="LinkTarget.Blank"
                AccessibleName="ヘルプ"
                NewWindowDescription="ヘルプは新しいタブで開きます。">
    ヘルプ
</A11yFluentLink>
```

## Fluent UI 本体への追随

このリポジトリでは利用中の Fluent UI バージョンと公開型メタデータを 1 箇所で管理します。
新しい本体を取り込むときは次の手順を使います。

1. `Directory.Build.props` の `FluentUIBlazorVersion` を更新する。
2. 次のコマンドで継承ラッパーを再生成する。

```console
dotnet run --project .tools\ComponentInventory\ComponentInventory.csproj
```

3. `dotnet test A11yBlazor.Fluent.slnx` で全コンポーネントの対応と契約を確認する。
4. `dotnet pack src\A11yBlazor.Fluent\A11yBlazor.Fluent.csproj -c Release` でパッケージを作成する。

生成器は公開された具象 `IComponent` を列挙し、基底型、ジェネリック制約、
コンストラクター、公開パラメーターを反映します。生成済みラッパーを直接編集せず、
新しいアクセシビリティ規則は検証器側へ追加してください。生成物は1クラス1ファイルで
`Components/Generated` に出力されます。109種類の必須入力と状態別契約の一覧は
`docs/component-contracts.md` に自動生成されます。

## WCAG 2.1 AA の適用範囲

このパッケージは、コンポーネント単体で機械的に検出できる契約を支援します。
次の項目を含む最終適合はアプリケーション固有の確認が必要です。

- ラベル、代替テキスト、エラー説明の意味的な妥当性
- ページタイトル、見出し階層、読む順序、複数経路
- キーボード操作、フォーカス順序、フォーカス可視性、ドラッグ操作の代替
- 文字・非テキストコントラスト、200% 拡大、320 CSS px のリフロー
- NVDA、JAWS、VoiceOver など実際のスクリーンリーダーとの相互作用
- 時間制限、点滅、自動更新、音声・動画コンテンツ

したがって、このパッケージだけを使うことで WCAG 2.1 AA への適合が保証されるわけでは
ありません。画面単位の手動試験と支援技術試験をリリース工程へ組み込んでください。
