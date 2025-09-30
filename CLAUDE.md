# CLAUDE.md

このファイルは、Claude Codeがこのリポジトリで作業する際のガイダンスを提供します。

**重要**: Claude Codeは**必ず日本語で回答**してください。コメント、ドキュメント、説明など、すべての出力は日本語で行う必要があります。


### Unityの理解
このプロジェクトは、Unityで実装されたプロジェクトです。
UnityのC#実装はsubmoduleのunity-cs-referenceにコードが存在します。
UnityのAPIについて調査する場合はsubmoduleの探索、または@unity-reference-resarchエージェントを使用してください。

## ライブラリの実装
Unityは以下のライブラリに依存しています。
必要な場合はライブラリを参照して、実装を確認してください
- unity-cs-reference: UnityのEditor実装
- Library/PackageCache: Unityが依存しているパッケージの実装

