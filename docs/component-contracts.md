# コンポーネント別アクセシビリティ契約

この表は Fluent UI Blazor の公開型メタデータから自動生成されます。
Razor の `EditorRequired`、実行時検証、継承元が公開する状態属性を区別して記載します。

| A11y コンポーネント | 利用時に必須 | 状態別の契約 |
|---|---|---|
| `A11yColumnReorderOptions` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yColumnResizeOptions` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yColumnSortMenuItems` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yColumnSortOptions` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentAccordion` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentAccordionItem` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示<br>展開／折りたたみ: 基底の `aria-expanded` を保持 |
| `A11yFluentAnchorButton` | なし（Fluent UI の既定契約を継承） | アイコンのみ／子なし: `AccessibleName`<br>新しいタブ／ウィンドウ: `NewWindowDescription` |
| `A11yFluentAppBar` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentAppBarItem` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentAutocomplete` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>複数選択・ファイル制限: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentAvatar` | なし（Fluent UI の既定契約を継承） | `OnClick` 使用: `AccessibleName` と `OnKeyboardActivate`<br>画像表示: `Name` |
| `A11yFluentBadge` | なし（Fluent UI の既定契約を継承） | アイコンのみ: `IconLabel` または `AccessibleName` |
| `A11yFluentButton` | なし（Fluent UI の既定契約を継承） | アイコンのみ／子なし: `AccessibleName`<br>フォーカス可能な無効状態: 無効理由を `AccessibleDescription` で説明<br>読み込み中: `LoadingLabel`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentCalendar` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>書式・範囲制約: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentCard` | なし（Fluent UI の既定契約を継承） | `OnClick` 使用: `AccessibleName` と `OnKeyboardActivate` |
| `A11yFluentCheckbox` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持<br>選択: 基底の選択状態と変更通知を保持 |
| `A11yFluentColorPicker` | `AccessibleName` | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentColorPickerInput` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentCombobox` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>複数選択・ファイル制限: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentCompoundButton` | なし（Fluent UI の既定契約を継承） | アイコンのみ／子なし: `AccessibleName`<br>フォーカス可能な無効状態: 無効理由を `AccessibleDescription` で説明<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentCounterBadge` | なし（Fluent UI の既定契約を継承） | アイコンのみ: `IconLabel` または `AccessibleName` |
| `A11yFluentDataGrid` | `AccessibleName` | ドラッグ・並べ替え: `OnKeyboardAlternative`<br>読み込み中: `LoadingLabel` |
| `A11yFluentDataGridCell` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentDataGridRow` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentDatePicker` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>書式・範囲制約: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentDialog` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentDialogBody` | `TitleTemplate` または `AccessibleName` | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentDialogProvider` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentDivider` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentDragContainer` | なし（Fluent UI の既定契約を継承） | ドラッグ・並べ替え: `OnKeyboardAlternative` |
| `A11yFluentDropZone` | なし（Fluent UI の既定契約を継承） | ドラッグ・並べ替え: `OnKeyboardAlternative` |
| `A11yFluentEmoji` | なし（Fluent UI の既定契約を継承） | 意味を持つアイコン: `Decorative=false` と `AccessibleName`<br>クリック可能: 装飾不可、キーボード代替が必要<br>`OnClick` 使用: `AccessibleName` と `OnKeyboardActivate` |
| `A11yFluentErrorBoundary` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentField` | なし（Fluent UI の既定契約を継承） | エラー: `Message` または `MessageTemplate`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentGrid` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentGridItem` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentHighlighter` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentIcon` | なし（Fluent UI の既定契約を継承） | 意味を持つアイコン: `Decorative=false` と `AccessibleName`<br>クリック可能: 装飾不可、キーボード代替が必要<br>`OnClick` 使用: `AccessibleName` と `OnKeyboardActivate` |
| `A11yFluentImage` | 情報画像は `AlternateText`、装飾画像は `Decorative=true` | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentInputFile` | `AccessibleName` | 複数選択・ファイル制限: `AccessibleDescription`<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentKeyCode` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentKeyCodeProvider` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentLabel` | なし（Fluent UI の既定契約を継承） | 必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentLabelInfo` | なし（Fluent UI の既定契約を継承） | 必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentLayout` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentLayoutHamburger` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentLayoutItem` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentLink` | なし（Fluent UI の既定契約を継承） | 新しいタブ／ウィンドウ: `NewWindowDescription` |
| `A11yFluentListbox` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>複数選択・ファイル制限: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentMenu` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentMenuButton` | なし（Fluent UI の既定契約を継承） | アイコンのみ／子なし: `AccessibleName`<br>フォーカス可能な無効状態: 無効理由を `AccessibleDescription` で説明<br>読み込み中: `LoadingLabel`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentMenuItem` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示<br>選択: 基底の選択状態と変更通知を保持 |
| `A11yFluentMenuList` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentMessageBar` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentMessageBarProvider` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentMultiSplitter` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentMultiSplitterPane` | なし（Fluent UI の既定契約を継承） | 書式・範囲制約: `AccessibleDescription` |
| `A11yFluentNav` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentNavCategory` | なし（Fluent UI の既定契約を継承） | 展開／折りたたみ: 基底の `aria-expanded` を保持 |
| `A11yFluentNavItem` | なし（Fluent UI の既定契約を継承） | 新しいタブ／ウィンドウ: `NewWindowDescription`<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentNavSectionHeader` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentNumberInput` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>書式・範囲制約: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentOption` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentOptionString` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentOverflow` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentOverlay` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentPaginator` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentPopover` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentPresenceBadge` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentProgress` | `AccessibleName` | 書式・範囲制約: `AccessibleDescription` |
| `A11yFluentProgressBar` | `AccessibleName` | 書式・範囲制約: `AccessibleDescription` |
| `A11yFluentProgressRing` | `AccessibleName` | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentProviders` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentPullToRefresh` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentRadio` | `Label`（EditorRequired） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentRadioGroup` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentRatingDisplay` | なし（Fluent UI の既定契約を継承） | 書式・範囲制約: `AccessibleDescription` |
| `A11yFluentSelect` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>複数選択・ファイル制限: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentSkeleton` | なし（Fluent UI の既定契約を継承） | 書式・範囲制約: `AccessibleDescription` |
| `A11yFluentSlider` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>書式・範囲制約: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentSortableList` | なし（Fluent UI の既定契約を継承） | ドラッグ・並べ替え: `OnKeyboardAlternative` |
| `A11yFluentSpacer` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentSpinner` | `AccessibleName` | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentSplitButton` | なし（Fluent UI の既定契約を継承） | アイコンのみ／子なし: `AccessibleName` |
| `A11yFluentStack` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentSwitch` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentTab` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentTabs` | `AccessibleName` | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentText` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentTextArea` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>本人データ: `RepresentsPersonalData=true` と対応する `AutoComplete`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>書式・範囲制約: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentTextInput` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>本人データ: `RepresentsPersonalData=true` と対応する `AutoComplete`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>書式・範囲制約: `AccessibleDescription`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentTimePicker` | `Label`（EditorRequired） | エラー: `Message` または `MessageTemplate`<br>自動フォーカス: 画面遷移時の目的を画面単位で確認<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示<br>読み取り専用: 値を読め、操作できないことを保持 |
| `A11yFluentToast` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentToastProvider` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentToggleButton` | なし（Fluent UI の既定契約を継承） | アイコンのみ／子なし: `AccessibleName`<br>フォーカス可能な無効状態: 無効理由を `AccessibleDescription` で説明<br>読み込み中: `LoadingLabel`<br>必須: 基底コンポーネントの必須属性を保持<br>無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentTooltip` | `Anchor` と説明内容 | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentTooltipProvider` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentTreeItem` | なし（Fluent UI の既定契約を継承） | 展開／折りたたみ: 基底の `aria-expanded` を保持 |
| `A11yFluentTreeView` | なし（Fluent UI の既定契約を継承） | 選択: 基底の選択状態と変更通知を保持 |
| `A11yFluentValidationMessage` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentValidationSummary` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentWizard` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFluentWizardStep` | なし（Fluent UI の既定契約を継承） | 無効: 操作停止と利用可能理由を画面文脈で提示 |
| `A11yFluentWizardStepValidator` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yFreeOptionOutput` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yHierarchicalSelectColumn` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yPropertyColumn` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11ySelectColumn` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
| `A11yTemplateColumn` | なし（Fluent UI の既定契約を継承） | 基底コンポーネントの名前・役割・値とキーボード操作を画面単位で確認 |
