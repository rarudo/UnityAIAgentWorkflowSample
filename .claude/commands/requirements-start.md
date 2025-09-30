<requirements-gathering-command>

<metadata>
  <timestamp>!`date +%Y-%m-%d-%H%M`</timestamp>
  <command_input>$ARGUMENTS</command_input>
</metadata>

<instructions>
  <overview>
    熟考して要件収集を開始: $ARGUMENTS
  </overview>

  <workflow>
    <instruction>
      以下のフェーズをTodoWriteを使ってTODO化してください。
      あなたの最終目的はユーザーの要求を元に調査を行い<implementation-specification>を完成させることです。
    </instruction>

    <phase id="1" name="初期セットアップと並列コンテキスト収集">
      <steps>
        <step>
          初期設定:
          - タイムスタンプベースのフォルダを作成: requirements/YYYY-MM-DD-HHMM-[slug]
          - $ARGUMENTSからスラグを抽出 (例: "ユーザープロフィールを追加" → "user-profile")
          - 初期ファイルを作成:
            - 00-initial-request.md にユーザーのリクエストを記録
            - metadata.json にステータス追跡情報を記録
          - requirements/.current-requirement をフォルダ名で更新
        </step>
        
        <step>
          ユーザー入力の即座の分析と初期並列調査:
          - 言及された具体的なファイル名、クラス名、機能名を抽出
          - 言及されたファイルが存在する場合は即座に読み込み
          - requirements-researcher エージェントを使って関連するコードベースの依存関係を把握して、調査の効率を上げる
            - 関連するディレクトリ構造を把握（LS, Globツール使用）
          - <parallel-investigation>に必要な事前情報の把握
        </step>
        
        <step>
          並列調査の実施（最大10個のSubAgentを**同時**使用）:
          
          <parallel-investigation>
            以下の調査を1度のTaskツールで複数ツールの呼び出しを同時に行い、並列に調査:
            (requirements-researcher および unity-reference-researcher エージェントを適切に使い分ける)
            
            1. **直接関連ファイルの詳細調査**
               - ユーザーが言及した具体的要素の完全な理解
               - クラス構造、メソッドシグネチャ、プロパティの確認
            
            2. **アーキテクチャパターンの理解**
               - MVPパターン、サービス層、モデル層の構造
               - プロジェクト全体の設計思想
            
            3. **過去の類似要件検索**
               - 類似機能の実装例と設計判断
            
            4. **Git履歴分析**
               - 関連ファイルの変更履歴と意図
            
            5. **ドキュメント検索**
               - Documents/フォルダ内の関連文書
               - CODING_STYLE.md、ARCHITECTURE.mdの確認
            
            6. **依存関係マッピング**
               - 使用するクラス、インターフェース、サービス
               - 外部ライブラリ（UniTask、R3、ZString等）
            
            7. **ライブラリ使用パターン調査**
               - 既存コードでのライブラリ使用例
               - ベストプラクティスの特定
            
            8. **テスト構造の理解**
               - テストフレームワークと実行方法
               - 既存テストパターンの分析
            
            9. **ビルド設定の確認**
               - ビルドプロファイル、環境設定
               - プラットフォーム固有の考慮事項
            
            10. **関連サービス/APIの調査**
                - 内部API、外部連携
                - データフローと通信パターン
          </parallel-investigation>
        </step>
        
        <step>
          調査結果の統合と追加調査計画:
          - 調査結果をカテゴリ別に整理
          - 調査済み項目のキャッシュ（重複回避）
          - 不足情報の特定と優先順位付け
          - 追加調査タスクの生成（必要な場合）
        </step>
      </steps>
    </phase>

    <phase id="2" name="コンテキスト発見質問">
      <steps>
        <step>
          問題空間を理解するための最も重要な5つのyes/no質問を生成:
          - コードベース構造に基づいた質問
          - ユーザーインタラクションとワークフローに関する質問
          - ユーザーが現在使用している類似機能に関する質問
          - 扱うデータ/コンテンツに関する質問
          - 外部統合やサードパーティサービスに関する質問
          - パフォーマンスやスケールの期待に関する質問
          - すべての質問をスマートデフォルト付きで 01-discovery-questions.md に記録
          - スマートデフォルトオプションを提案しながら、5つまとめて質問
          - すべての質問が回答された時にのみ、受け取った回答を 02-discovery-answers.md に記録し、metadata.json を更新
        </step>
      </steps>
    </phase>

    <phase id="3" name="ターゲット指向の深堀り調査">
      <instruction>
        Phase 1の並列調査結果と発見質問の回答を基に、より詳細な調査を実施
      </instruction>
      <steps>
        <step>
          深堀り調査の並列実行:
          
          <deep-investigation>
            Phase 1で収集した情報を基に、以下を並列実行:
            
            1. **API調査と実装パターン検証（必須）**
               - 実際のソースコードを読んで、パブリックメソッド、プロパティ、コンストラクタを完全に理解
               - パラメータ型、戻り値型、例外を含む正確なメソッドシグネチャを検証
               - 存在が不確かなメソッドやプロパティは計画に含めない
            
            2. **既存の実装パターン調査**
               - プロジェクト内で同じクラス/APIの既存使用例を少なくとも3つ見つける
               - 実際の呼び出しパターン、初期化シーケンス、エラーハンドリングを研究
               - 類似機能を分析してプロジェクトの規約を理解
            
            3. **Unityの内部実装調査（必要に応じて）**
               - unity-reference-researcher エージェントを使用
               - Unity固有のAPIや動作の詳細理解
            
            4. **追加のコンテキスト収集**
               - Phase 1で特定された不足情報の収集
               - 優先度に基づいた追加調査
          </deep-investigation>
        </step>
        
        <step>
          03-context-findings.md に調査結果を文書化:
          - 修正が必要な具体的なファイル
          - 従うべき正確なパターン
          - 詳細に分析された類似機能
          - 技術的制約と考慮事項
          - 特定された統合ポイント
          - 不明または不確かなAPI（検証できなかったAPIのリスト）
          - **APIシグネチャと使用パターン**（使用例付きの正確なシグネチャ）
          - **検証済みメソッドの可用性**（存在が確認されたメソッドのみ）
        </step>
      </steps>
    </phase>

    <phase id="4" name="エキスパート要件質問">
      <steps>
        <step>
          コードベースを知るシニア開発者として質問:
          - 最も差し迫った未回答の詳細なyes/no質問トップ5を 04-detail-questions.md に記録
          - コードを知らないプロダクトマネージャーに話すような質問にする
          - これらの質問は、コードを深く理解した今、期待されるシステム動作を明確にするためのもの
          - コードベースパターンに基づくスマートデフォルトを含める
          - **Phase 3で発見された不明なAPIや実装アプローチに関する質問を含める**
          - 一度に5つ全て質問する
          - すべての質問に回答が行われた時にのみ、受け取った回答を 05-detail-answers.md に記録
        </step>
      </steps>
    </phase>

    <phase id="5" name="要件文書化">
      <steps>
        <step>
          06-requirements-spec.md に構造化された実装仕様をMarkdown形式で生成:
          
          <implementation-specification>の構造で構造化された仕様書を作成します：

          <implementation-specification>
          
          # 実装仕様書

          ## 1. 実装概要
          ### 1.1 問題の定義と背景
          - 解決すべき問題の詳細な説明
          - 現状の課題と制約事項
          - ビジネス要求の背景

          ### 1.2 提案ソリューションの概要
          - ソリューションの全体像
          - 期待される効果
          - 技術的アプローチ

          ### 1.3 機能要件
          - 必須機能の一覧
          - オプション機能の一覧
          - 非機能要件（パフォーマンス、セキュリティなど）

          ## 2. 実装手順
          | 順序 | 作業内容 | 詳細手順 | 確認方法 |
          |------|----------|----------|----------|
          | 1 | 基盤準備 | 必要なパッケージのインストール、環境設定 | ビルドが成功すること |
          | 2 | モデル実装 | データモデルの作成、検証ロジックの実装 | 単体テストの通過 |
          | 3 | ビジネスロジック | サービス層の実装、ドメインロジック | 統合テストの通過 |
          | 4 | UI実装 | View, Presenter, ViewModelの実装 | UIテストの通過 |
          | 5 | 統合 | 各コンポーネントの結合、動作確認 | E2Eテストの通過 |

          ## 3. ファイル構造
          ### 3.1 新規作成ファイル
          ```
          Assets//
          ├── [Module名]/
          │   ├── Models/
          │   │   └── [Model名].cs
          │   ├── Presenters/
          │   │   └── [Presenter名].cs
          │   ├── Views/
          │   │   └── [View名].cs
          │   └── Services/
          │       └── [Service名].cs
          ```

          | ファイルパス | 目的 | 主要な責務 |
          |-------------|------|------------|
          | パス1 | 目的1 | 責務1 |
          | パス2 | 目的2 | 責務2 |

          ### 3.2 変更対象ファイル
          | ファイルパス | 変更内容 | 影響範囲 |
          |-------------|----------|----------|
          | パス1 | 変更1 | 影響1 |
          | パス2 | 変更2 | 影響2 |

          ## 4. 実装対象のクラス・メソッド定義
          ### 4.1 クラス定義

          #### [クラス名]
          - **パス**: `Assets//[パス]`
          - **概要**: クラスの目的と責務
          - **継承**: 基底クラス名
          - **インターフェース**: 実装するインターフェース

          **メソッド一覧**:
          | メソッドシグネチャ | 概要 | パラメータ | 戻り値 | 例外 |
          |-------------------|------|------------|--------|------|
          | `public async UniTask<T> MethodName(Type param)` | メソッドの目的 | param: 説明 | T: 説明 | InvalidtionException |

          **プロパティ一覧**:
          | プロパティ名 | 型 | アクセスレベル | 概要 |
          |-------------|-----|---------------|------|
          | PropertyName | Type | public get; private set; | プロパティの用途 |

          **サンプルコード**:
          ```csharp
          // 基本的な使用例
          var instance = new ClassName();
          var result = await instance.MethodName(param);
          ```

          ### 4.2 クラス構造図（Mermaid）
          
          **実装クラスの全体構造**:
          ```mermaid
          classDiagram
            %% インターフェース定義
            class IPresenter {
              <<interface>>
              +Initialize() void
              +Dispose() void
            }
            
            class IView {
              <<interface>>
              +SetPresenter(presenter IPresenter) void
              +Show() void
              +Hide() void
            }
            
            %% 基底クラス
            class UIBehaviour {
              <<abstract>>
              #CompositeDisposable _disposables
              +Awake() void
              +OnDestroy() void
              #OnAwakeInternal()* void
            }
            
            class BasePresenter {
              <<abstract>>
              #CompositeDisposable _disposables
              +Initialize()* void
              +Dispose() void
            }
            
            %% 実装クラス
            class ExamplePresenter {
              -ExampleView _view
              -ExampleModel _model
              -ExampleService _service
              +Initialize() void
              +OnButtonClicked() UniTask
              -LoadDataAsync() UniTask~Data~
            }
            
            class ExampleView {
              -Button _button
              -TextMeshProUGUI _textDisplay
              -ExamplePresenter _presenter
              +SetPresenter(presenter IPresenter) void
              +UpdateDisplay(text string) void
              #OnAwakeInternal() void
            }
            
            class ExampleModel {
              +int Id
              +string Name
              +ReactiveProperty~Data~ Data
              +Validate() bool
            }
            
            class ExampleService {
              -IApiClient _apiClient
              +GetDataAsync(id int) UniTask~Data~
              +SaveDataAsync(data Data) UniTask~bool~
            }
            
            %% 関係性の定義
            BasePresenter ..|> IPresenter : implements
            ExamplePresenter --|> BasePresenter : extends
            ExampleView ..|> IView : implements
            ExampleView --|> UIBehaviour : extends
            
            ExamplePresenter --> ExampleView : uses
            ExamplePresenter --> ExampleModel : manages
            ExamplePresenter --> ExampleService : calls
            ExampleView --> ExamplePresenter : notifies
            
            %% 注記
            note for ExamplePresenter "MVPパターンのPresenter\nビジネスロジックを管理"
            note for ExampleView "Unity UI管理\nユーザー入力を処理"
          ```
          
          **クラス間の関係性凡例**:
          - `--|>` : 継承（extends）
          - `..|>` : 実装（implements）
          - `-->` : 依存・使用（uses/depends on）
          - `--*` : コンポジション（強い所有関係）
          - `--o` : 集約（弱い所有関係）

          ## 5. 依存関係
          ### 5.1 継承クラス
          | 基底クラス | パス | 継承目的 | オーバーライドメソッド |
          |-----------|------|---------|---------------------|
          | BaseClass | パス | 目的 | Method1, Method2 |

          ### 5.2 使用クラス
          | クラス名 | パス | 使用目的 | 使用方法 |
          |---------|------|---------|---------|
          | Class1 | パス1 | 目的1 | 使用方法1 |

          ### 5.3 依存関係図
          ```
          [Component A]
               ↓ 依存
          [Component B] ← 使用 ← [Component C]
               ↓ 実装
          [Interface X]
          ```

          ## 6. コーディング規約
          ### 6.1 命名規則
          | 対象 | 規則 | 良い例 | 悪い例 |
          |------|------|--------|--------|
          | クラス | PascalCase | UserProfile | user_profile |
          | メソッド | PascalCase | GetUserName | getUserName |
          | プライベートフィールド | _camelCase | _userName | userName |
          | パブリックプロパティ | PascalCase | UserName | userName |

          ### 6.2 コーディングパターン
          #### Disposeパターン
          ```csharp
          public class ExampleClass : IDisposable
          {
              private CompositeDisposable _disposables = new();
              
              public void Dispose()
              {
                  _disposables?.Dispose();
              }
          }
          ```

          #### 非同期パターン
          ```csharp
          public async UniTask<Result> ProcessAsync(CancellationToken cancellationToken = default)
          {
              await UniTask.Delay(100, cancellationToken: cancellationToken);
              return new Result();
          }
          ```

          ## 7. ライブラリ使用仕様
          ### 7.1 UniTask
          | API | シグネチャ | 使用例 | 既存使用箇所 |
          |-----|-----------|--------|-------------|
          | UniTask.Delay | `UniTask Delay(int millisecondsDelay, CancellationToken cancellationToken = default)` | `await UniTask.Delay(1000);` | Battle/BattlePresenter.cs |
          | UniTask.WhenAll | `UniTask WhenAll(params UniTask[] tasks)` | `await UniTask.WhenAll(task1, task2);` | Common/CommonPresenter.cs |

          ### 7.2 R3
          | API | シグネチャ | 使用例 | 既存使用箇所 |
          |-----|-----------|--------|-------------|
          | Observable.Timer | `Observable<long> Timer(TimeSpan dueTime)` | `Observable.Timer(TimeSpan.FromSeconds(1))` | OutGame/TimerPresenter.cs |

          ### 7.3 ZString
          | API | シグネチャ | 使用例 | 既存使用箇所 |
          |-----|-----------|--------|-------------|
          | ZString.Format | `string Format<T1, T2>(string format, T1 arg1, T2 arg2)` | `ZString.Format("Score: {0}", score)` | UI/ScoreView.cs |

          ## 8. 参照ドキュメント
          | ドキュメント | パス | 関連セクション | 参照目的 |
          |-------------|------|---------------|----------|
          | コーディング規約 | Documents/CODING_STYLE.md | 命名規則セクション | 命名規則の確認 |
          | アーキテクチャガイド | Documents/ARCHITECTURE.md | MVPパターン | 設計パターンの理解 |

          ## 9. 類似実装の参考
          | 実装 | パス | 概要 | 適用可能なパターン |
          |------|------|------|------------------|
          | UserProfilePresenter | Assets//OutGame/UserProfile/UserProfilePresenter.cs | ユーザープロファイル表示 | MVP実装パターン、非同期処理 |
          | ItemListView | Assets/Project/OutGame/Item/ItemListView.cs | リスト表示UI | UIレイアウト、スクロール処理 |

          ## 10. アーキテクチャとフロー
          ### 10.1 処理フロー
          ```
          1. ユーザー操作
               ↓
          2. View がイベントを検知
               ↓
          3. Presenter が処理を実行
               ↓
          4. Model/Service を呼び出し
               ↓
          5. データ処理・API通信
               ↓
          6. Presenter が結果を受け取り
               ↓
          7. View を更新
          ```

          ### 10.2 シーケンス図（Mermaid）
          ```mermaid
          sequenceDiagram
            participant View
            participant Presenter
            participant Service
            participant API
            
            View->>Presenter: イベント
            Presenter->>Service: 処理依頼
            Service->>API: リクエスト
            API-->>Service: レスポンス
            Service-->>Presenter: 結果
            Presenter-->>View: 表示更新
          ```

          ## 11. 影響範囲
          | 影響を受ける機能 | 影響の内容 | 影響レベル | 軽減策 |
          |----------------|-----------|-----------|--------|
          | 機能1 | 影響内容1 | 高/中/低 | 対策1 |
          | 機能2 | 影響内容2 | 高/中/低 | 対策2 |

          ## 12. 注意事項
          | カテゴリ | 注意点 | 詳細説明 | 防止策 |
          |---------|--------|---------|--------|
          | パフォーマンス | メモリリーク | Disposeの実装漏れ | IDisposableの実装とusingの使用 |
          | セキュリティ | API認証 | 認証トークンの管理 | SecureStorageの使用 |
          | 互換性 | Unityバージョン | 2023.2以降が必要 | バージョンチェックの実装 |

          ## 13. 受け入れ条件
          | 条件 | 詳細 | 検証方法 | 期待結果 |
          |------|------|---------|---------|
          | 機能動作 | 全機能が正常動作 | 手動テスト実施 | エラーなく完了 |
          | パフォーマンス | 60FPS維持 | Profilerで計測 | フレームレート60以上 |
          | コード品質 | 規約準拠 | Analyzerチェック | 警告0件 |
          | テスト | 全テスト通過 | Unity Test Runner | 全テストグリーン |

          ## 14. 不明な事項
          | 優先度 | タイトル | 詳細説明 | リスクレベル | 提案アプローチ |
          |--------|---------|---------|-------------|---------------|
          | 高 | 項目1 | 説明1 | high/medium/low | アプローチ1 |
          | 中 | 項目2 | 説明2 | high/medium/low | アプローチ2 |
          | 低 | 項目3 | 説明3 | high/medium/low | アプローチ3 |

          ## 15. 仮定事項
          | 仮定内容 | 根拠 | 検証方法 | 代替案 |
          |---------|------|---------|--------|
          | 仮定1 | 根拠1 | 検証方法1 | 代替案1 |
          | 仮定2 | 根拠2 | 検証方法2 | 代替案2 |
          
          </implementation-specification>
        </step>
      </steps>
    </phase>
    
    <phase id="6" name="未解決事項の解決">
      <steps>
        <step>
          06-requirements-spec.mdの<unresolved-items>と<assumptions>を分析:
          - 未解決事項をrisk-level（high/medium/low）で優先順位付け
          - 仮定事項を検証可能性で分類
          - 解決戦略を策定
        </step>
        <step>
          高優先度の未解決事項から順に解決を実施:
          - SubAgentを使用した並列調査で解決可能な項目を調査
          - コードベース、ドキュメント、外部リソースからの情報収集
          - 技術的制約や実装パターンの詳細調査
        </step>
        <step>
          仮定事項の検証:
          - 既存実装パターンとの整合性確認
          - ライブラリやフレームワークの制約確認
          - パフォーマンスや互換性の検証
        </step>
        <step>
          調査で解決できない項目のユーザー質問化:
          - 具体的で回答しやすい質問形式で整理
          - 技術的背景と選択肢を含めて提示
          - ビジネス要件や設計判断が必要な項目を特定
        </step>
        <step>
          すべての未解決事項が解決または明確化されるまで反復:
          - ユーザーからの回答を受けて追加調査を実施
          - 新たに発見された依存関係や制約を調査
          - 解決内容の影響範囲を評価
        </step>
        <step>
          解決プロセスの記録:
          - 07-resolution-log.mdに解決過程と結果を詳細記録
          - 調査方法、発見内容、決定根拠を文書化
          - 残存リスクや今後の検討事項を明記
        </step>
        <step>
          最終仕様書の更新:
          - 06-requirements-spec.mdから<unresolved-items>と<assumptions>を削除
          - 解決された内容を適切なセクションに反映
          - 実装に必要な全ての情報が含まれていることを確認
        </step>
      </steps>
    </phase>
    
    <phase id="7" name="完了">
      <steps>
        <step>
            ユーザーに要件の文章化が完了したことを通知します
            ユーザーが明示的に実装の開始を依頼するまで、実装は禁止です
        </step>
      </steps>
    </phase>
  </workflow>

  <investigation-cache>
    <description>
      調査済み項目をキャッシュして重複を防ぐ
    </description>
    <cache-structure>
      - investigated_files: []
      - investigated_patterns: []
      - investigated_apis: []
      - known_dependencies: {}
    </cache-structure>
  </investigation-cache>

  <investigation-priority>
    <high>
      - ユーザーが明示的に言及した要素
      - 直接的な実装対象
      - クリティカルな依存関係
    </high>
    <medium>
      - 関連する既存実装
      - 間接的な依存関係
      - テストとビルド設定
    </medium>
    <low>
      - ベストプラクティス
      - オプション機能
      - 将来の拡張性
    </low>
  </investigation-priority>

  <question-formats>
    <discovery-questions>
      <format>
        ## Q1: このユーザーはこの機能をビジュアルインターフェースを通じて操作しますか？
        **不明な場合のデフォルト:** はい（ほとんどの機能にはUIコンポーネントがある）
        **依存関係:** UIが必要な場合、フロントエンドフレームワークの選択に影響
        **次のステップへの影響:** UIフレームワークとコンポーネント設計の決定

        ## Q2: この機能はモバイルデバイスで動作する必要がありますか？
        **不明な場合のデフォルト:** はい（モバイルファーストが標準的な実践）
        **依存関係:** レスポンシブデザインとタッチインタラクションの考慮
        **代替パス:** デスクトップ専用の場合、より複雑なインタラクションが可能
      </format>
    </discovery-questions>

    <expert-questions>
      <format>
        ## Q7: 既存の UserService（services/UserService.ts）を拡張すべきですか？
        **不明な場合のデフォルト:** はい（アーキテクチャの一貫性を維持）
        **実装への影響:** 既存のサービスパターンとの統合
        **リスク:** 既存機能への影響を慎重にテストする必要がある

        ## Q8: これにはdb/migrations/に新しいデータベースマイグレーションが必要ですか？
        **不明な場合のデフォルト:** いいえ（類似機能がスキーマ変更を必要としなかったことに基づく）
        **検証方法:** 既存のデータモデルで要件を満たせるか確認
      </format>
    </expert-questions>
  </question-formats>

  <metadata-structure>
    <json-format>
      {
        "id": "feature-slug",
        "started": "ISO-8601-timestamp",
        "lastUpdated": "ISO-8601-timestamp",
        "status": "active",
        "phase": "discovery|context|detail|complete|iteration",
        "planning_methodology": {
          "decomposition_complete": false,
          "dependencies_mapped": false,
          "branches_explored": [],
          "current_branch": null,
          "iterations": 0,
          "completeness_verified": false
        },
        "progress": {
          "discovery": { "answered": 0, "total": 5 },
          "detail": { "answered": 0, "total": 0 }
        },
        "contextFiles": ["分析したファイルのパス"],
        "relatedFeatures": ["見つかった類似機能"],
        "continuation_id": "requirements-thread-id",
        "revision_history": [],
        "investigation_cache": {
          "investigated_files": [],
          "investigated_patterns": [],
          "investigated_apis": [],
          "known_dependencies": {}
        }
      }
    </json-format>
  </metadata-structure>

  <file-requirement-response>
    <json-format>
      {
        "status": "files_required_to_continue",
        "mandatory_instructions": "UserServiceの完全な実装を理解するために必要",
        "files_needed": ["services/UserService.ts", "services/BaseService.ts"]
      }
    </json-format>
  </file-requirement-response>

  <rules>
    <rule>yes/no質問のみ、スマートデフォルト付き</rule>
    <rule>一度にまとめて質問</rule>
    <rule>質問する前にすべての質問をファイルに記録</rule>
    <rule>要件に焦点を当てる（実装ではない）</rule>
    <rule>詳細フェーズでは実際のファイルパスとコンポーネント名を使用</rule>
    <rule>各デフォルトがなぜ理にかなっているかを文書化</rule>
    <rule>推奨ツールが利用できない場合は利用可能なツールを使用</rule>
    <rule>ユーザーが明示的に指示しない限り、実装は行わない</rule>
    <rule>**絵文字を使用しない**（ASCII文字とシンボルのみ）</rule>
    <rule>**時間見積もりやコストは明示的に要求されない限り言及しない**</rule>
    <rule>**Phase 1で可能な限り多くの情報を並列収集する**</rule>
    <rule>**調査の重複を避けるためキャッシュを活用する**</rule>
  </rules>

  <phase-transitions>
    <transition>各フェーズ後にアナウンス: 「フェーズ完了。[次のフェーズ]を開始します...」</transition>
    <transition>次のフェーズに移る前にすべての作業を保存</transition>
  </phase-transitions>

  <research-methodology>
    <overview>
      Claudeはライブラリやプロジェクトのソースコードを探索して、実装に必要なコンテキストを調査します。
      調査を行う場合は <parallel-research-method> を忠実に実行してください。
      **重要**: Phase 1で可能な限り多くの情報を並列収集し、早期に全体像を把握します。
    </overview>

    <parallel-research-method>
      <description>
        初期段階から大規模な並列調査を実施し、調査時間を最小化します。
        TodoWriteツールとTaskツールを最大限活用して効率的にタスクを管理します。
      </description>
      
      <steps>
        <step>
          ユーザー入力から直接特定できる要素を即座に抽出:
          - 具体的なファイル名、クラス名、機能名
          - 言及されたAPIやライブラリ
          - 関連する業務領域やモジュール
        </step>
        
        <step>
          TodoWriteツールを使用して全調査タスクを作成:
          - ユーザー入力から直接特定できる調査タスク
          - 標準的な調査タスク（アーキテクチャ、パターン、ドキュメント等）
          - プロジェクト固有の調査タスク
        </step>
        
        <step>
          最大10個のSubAgentを使い**1つのメッセージで並列調査**:
          - 各SubAgentに十分なコンテキストを提供
          - 調査結果の形式を統一
          - 重複調査を避けるため範囲を明確に定義
        </step>
        
        <step>
          調査結果の統合と分析:
          - キャッシュに調査済み項目を記録
          - 新たに発見された調査対象をTodoWriteで追加
          - 優先度に基づいて追加調査を計画
        </step>
        
        <step>
          必要に応じて追加の詳細調査:
          - 高優先度の未解決項目に集中
          - SubAgentを使わずに深堀り調査
          - 最終的な不明点の整理
        </step>
      </steps>

      <parallel-execution-template>
        <description>
          Phase 1での並列調査の実行例
        </description>
        <code>
          <function_calls>
          <invoke name="Task">
            <parameter name="description">直接関連ファイルの詳細調査</parameter>
            <parameter name="subagent_type">requirements-researcher</parameter>
            <parameter name="prompt">
          ユーザー要求: [完全な原文]
          調査対象ファイル: [具体的なファイルリスト]
          調査目標:
          - クラス構造とメソッドシグネチャの完全な理解
          - 依存関係とインターフェースの特定
          - 使用パターンとベストプラクティスの抽出
          期待される成果物:
          - 各クラスの詳細な構造情報
          - メソッドの正確なシグネチャ
          - 実際の使用例
          注意: コードの編集は行わず、調査と分析のみ実施
            </parameter>
          </invoke>
          
          <invoke name="Task">
            <parameter name="description">アーキテクチャパターン調査</parameter>
            <parameter name="subagent_type">requirements-researcher</parameter>
            <parameter name="prompt">
          ユーザー要求: [完全な原文]
          調査目標: プロジェクト全体のアーキテクチャパターンを理解
          調査範囲:
          - MVPパターンの実装方法
          - サービス層の構造
          - モジュール間の依存関係
          期待される成果物:
          - アーキテクチャの概要図
          - 主要パターンの説明
          - 実装例の参照
          注意: コードの編集は行わず、調査と分析のみ実施
            </parameter>
          </invoke>
          
          <invoke name="Task">
            <parameter name="description">過去の類似要件検索</parameter>
            <parameter name="subagent_type">requirements-researcher</parameter>
            <parameter name="prompt">
          ユーザー要求: [完全な原文]
          検索キーワード: [関連キーワードリスト]
          調査目標: 類似機能の過去の要件定義と設計を発見
          期待される成果物:
          - 関連する要件文書のリスト
          - 設計判断の背景
          - 実装時の注意点
         注意: コードの編集は行わず、調査と分析のみ実施
            </parameter>
          </invoke>
          
          <!-- 他の7つのSubAgentも同様に定義 -->
          <!-- Unity固有の調査にはsubagent_type: unity-reference-researcherを指定 -->
          </function_calls>
        </code>
      </parallel-execution-template>
    </parallel-research-method>

    <subagent-prompt-template>
      <description>
        SubAgentに提供する標準的なプロンプトテンプレート
      </description>
      
      ## 調査タスク: {task_name}
      
      ### ユーザー要求（原文）
      {user_request}
      
      ### 抽出された要素
      - ファイル: {file_list}
      - クラス: {class_list}
      - 機能: {feature_list}
      
      ### 既知のコンテキスト
      ```
      {known_context}
      ```
      
      ### 既に調査済みの項目（重複回避）
      ```
      {investigated_items}
      ```
      
      ### 調査目標
      {investigation_goal}
      
      ### 期待される成果物
      1. {expected_output_1}
      2. {expected_output_2}
      3. {expected_output_3}
      
      ### 回答形式
      以下の形式で構造化して回答してください：
      
      #### 調査結果
      - 発見事項の詳細
      - クラス/メソッドの正確な情報
      - 依存関係と使用パターン
      
      #### 不明点
      - 調査できなかった項目
      - 追加調査が必要な領域
      
      #### 推奨事項
      - ベストプラクティス
      - 注意すべき点
      
      ### 制約事項
      - **コードの編集や修正は絶対に行わない**
      - 推測を避け、確認できた事実のみ報告
      - 調査時間の目安: {time_estimate}
    </subagent-prompt-template>

    <subagent-usage>
      <required-context>
        <description>
          SubAgentを使用する際は以下の情報を必ず含めてください。
        </description>
        <context-item>ユーザーの入力（完全な原文）</context-item>
        <context-item>ユーザー入力から抽出した具体的な要素</context-item>
        <context-item>既に読み込んだ関連ファイルの内容要約</context-item>
        <context-item>調査済み項目のリスト（重複回避用）</context-item>
        <context-item>現在の不明点</context-item>
        <context-item>既に理解している情報の詳細</context-item>
        <context-item>SubAgentに求める具体的なゴール</context-item>
        <context-item>**注意事項**: コードの編集や修正は絶対に行わず、調査と分析のみを実施</context-item>
      </required-context>

      <response-format>
        <description>SubAgentが回答する際の標準形式</description>
        <response-item>
          詳細な調査結果:
          - クラスの構成、メソッドとその引数の構造
          - 依存しているクラスの構造と依存関係
          - 依存しているライブラリの構造と依存関係
          - クラスの使用方法と実装例
          - 参考になるファイルとその理由
        </response-item>
        <response-item>
          調査の結果不明だった内容:
          - アクセスできなかったファイル
          - 確認できなかったAPI
          - 不明確な実装詳細
        </response-item>
        <response-item>
          さらなる調査の必要性:
          - 追加調査が必要なライブラリやクラス
          - 深堀りが必要な技術領域
          - 確認が必要な設計判断
        </response-item>
      </response-format>
    </subagent-usage>
  </research-methodology>

  <completeness-checklist>
    <item>すべての発見質問に回答またはデフォルトが設定されている</item>
    <item>コンテキスト調査で関連ファイルがすべて特定されている</item>
    <item>APIと実装パターンが検証されている</item>
    <item>すべての詳細質問に回答またはデフォルトが設定されている</item>
    <item>要件仕様に視覚的要素が含まれている</item>
    <item>実装リスクと不明点が文書化されている</item>
    <item>受け入れ基準が明確で測定可能である</item>
    <item>調査の重複がキャッシュにより防止されている</item>
    <item>優先度に基づいた調査が実施されている</item>
  </completeness-checklist>
</instructions>

</requirements-gathering-command>
