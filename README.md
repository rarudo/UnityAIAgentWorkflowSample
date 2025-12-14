# Unity AI Agent Workflow Sample

Unityでのゲーム開発にAIエージェント(Claude Code)を活用するサンプルプロジェクトです。要件収集から実装までのワークフローが定義されており、AIとの協業で開発を進められます。

## ディレクトリ構成

```
.
├── .claude/
│   ├── commands/        # スラッシュコマンド定義
│   └── agents/          # カスタムエージェント定義
├── requirements/        # 要件定義ドキュメント
├── Assets/              # Unityプロジェクト
├── unity-cs-reference/  # Unity C# Reference (submodule)
└── CLAUDE.md           # Claude Code向けプロジェクト指示
```

## スラッシュコマンド

`.claude/commands/`に定義されたコマンドは、Claude Code上で`/コマンド名`として実行できます。

### /requirements-start

要件収集を開始するコマンドです。曖昧な要求から構造化された仕様書を作っていきます。

```
/requirements-start ブロック崩しを作りたい
```

実行すると以下のフェーズで進んでいきます：

1. **初期セットアップ**: `requirements/YYYY-MM-DD-HHMM-[slug]/`フォルダを作成し、初期リクエストを記録します
2. **並列コンテキスト収集**: 複数のエージェントが関連コードやドキュメントを調査します
3. **発見質問**: Yes/No形式で5つの質問が提示され、要件の方向性を確認していきます
4. **深堀り調査**: 回答に基づいて詳細な技術調査が走ります
5. **エキスパート質問**: 実装に関わる技術的な確認を5つ質問されます
6. **仕様書生成**: 調査結果をもとに`06-requirements-spec.md`が生成されます
7. **未解決事項の解決**: 仕様書内の不明点を潰していきます

生成される仕様書には、クラス設計、メソッドシグネチャ、依存関係図、処理フロー、受け入れ条件などが含まれています。

### /imprement-start

仕様書に基づいて実装を開始するコマンドです。

```
/imprement-start requirements/2025-09-28-1800-unity-block-breaker
```

以下のフェーズで進みます：
1. 仕様書の読み込みと理解
2. コーディング規約・アーキテクチャの把握
3. 実装タスクの分解とTODO作成、実装
4. 仕様書との整合性チェック
5. コンパイル・Lintの検証
6. 完了レポート

## カスタムエージェント

`.claude/agents/`には、特定の調査タスクに特化したエージェントが定義されています。

### requirements-researcher

プロジェクト内のコードやドキュメントを調査する読み取り専用エージェントです。ファイル編集は行わず、調査結果だけを返してくれます。

主な用途：
- 既存実装パターンの分析
- 依存関係の調査
- 類似機能の検索
- Git履歴の分析

### unity-reference-researcher

`unity-cs-reference`サブモジュールからUnity内部実装を調査するエージェントです。

主な用途：
- Unity APIの内部動作調査
- パフォーマンス影響の分析
- 未ドキュメント機能の調査

## requirementsフォルダ

要件収集で生成されるドキュメントは以下の構造になっています：

```
requirements/2025-09-28-1800-unity-block-breaker/
├── 00-initial-request.md    # ユーザーの初期リクエスト
├── 01-discovery-questions.md # 発見フェーズの質問
├── 02-discovery-answers.md   # 発見フェーズの回答
├── 03-context-findings.md    # コンテキスト調査結果
├── 04-detail-questions.md    # 詳細質問
├── 05-detail-answers.md      # 詳細回答
├── 06-requirements-spec.md   # 最終仕様書
└── metadata.json             # 進行状況のメタデータ
```

`.current-requirement`ファイルで現在作業中の要件を追跡できます。

## 使い方

### 新機能の要件定義

1. Claude Codeを起動します
2. `/requirements-start [やりたいこと]`を実行します
3. AIからの質問に回答していきます（Yes/Noまたはデフォルト値の承認）
4. 生成された仕様書を確認・修正します

### 実装

1. 仕様書の内容を確認します
2. `/imprement-start [仕様書フォルダ]`を実行します
3. AIが仕様書に従って実装を進めてくれます
4. 完了後、動作確認します

### Unity API調査

会話中に`@unity-reference-researcher`を呼び出すか、必要に応じてAIが自動的にエージェントを起動してUnityの内部実装を調査してくれます。

## セットアップ

```bash
# リポジトリのクローン
git clone --recursive [repository-url]

# submoduleの初期化（クローン後に実行する場合）
git submodule update --init
```

Unity 2022.3以降で開いてください。

## 関連ファイル

- `CLAUDE.md`: Claude Codeへのプロジェクト固有の指示
- `.mcp.json`: MCPサーバー設定（uLoopMCPなど）
