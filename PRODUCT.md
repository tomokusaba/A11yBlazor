# Product

<!-- impeccable:product-schema 1 -->

## Platform

web

## Users

Blazor アプリケーションを開発し、アクセシビリティを機能の追加ではなく
コンポーネント利用時の既定動作にしたい開発者。

## Product Purpose

Fluent UI Blazor 5 の公開 UI コンポーネントを継承し、WCAG 2.1 AA に配慮した
必須パラメーターと状態依存の契約を提供する再利用可能な NuGet パッケージ。

## Positioning

Fluent UI の見た目と API を維持しつつ、ラベル、代替テキスト、キーボード操作、
状態通知、動的メッセージに関するアクセシビリティ要件を利用時に明示・検証する。

## Operating Context

Blazor Server、Blazor Web App、Blazor WebAssembly、および Generic Host の
依存性注入と同一の .NET コンポーネントコードから利用される。

## Capabilities and Constraints

- Fluent UI Blazor 5 の公開 UI コンポーネントを対象とする。
- 内部向けコンポーネント、イベント引数、モデル、サービス型はラッパー対象外とする。
- WCAG 適合はコンポーネント単体で保証できず、画面文脈・コンテンツ・最終 HTML の
  手動および自動検査も必要とする。
- ラベルや代替テキストの意味的妥当性は開発者が担保する。

## Brand Commitments

- Fluent UI Blazor の API と視覚表現を尊重する。
- C# の公開・非公開を問わず、すべてのメンバーに日本語の XML コメントを付ける。

## Evidence on Hand

Fluent UI Blazor 5 のコンポーネント API と WCAG 2.1 AA の達成基準を実装要件として
参照する。実アプリケーション固有のコンテンツや支援技術試験結果は未提供。

## Product Principles

- 基盤コンポーネントの見た目と操作モデルを不必要に変更しない。
- 要件を利用側で推測させず、パラメーターと診断で明示する。
- 状態固有の要件を条件付き必須項目としてモデル化する。
- コンポーネントの自動検証を最終的な支援技術試験の代替としない。

## Accessibility & Inclusion

WCAG 2.1 AA を設計基準とし、特に名前・役割・値、非テキストコンテンツ、見た目と
順序、キーボード操作、フォーカス、エラー識別、状態変更、拡大・リフローに配慮する。
