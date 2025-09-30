# Phase 3: ターゲット指向の深堀り調査結果

## 調査結果のまとめ

### 1. 修正が必要なファイル

#### 主要修正対象
1. **Assets/Scenes/Block.unity**
   - Main Cameraの背景色設定変更
   - Global Volume GameObjectの追加

2. **Assets/Scripts/BlockController.cs**
   - SetBlockColorメソッドの修正（SpriteRenderer → MeshRenderer対応）

3. **Assets/Scripts/GameManager.cs**
   - 色配列の追加
   - GenerateBlocks()メソッドでの色設定ロジック追加

#### 新規作成が必要なアセット
1. **Assets/Materials/BlockEmissiveMaterial.mat**
   - URP/Litシェーダーを使用した発光マテリアル

### 2. 技術的実装詳細

#### A. Camera背景設定
```csharp
// 現在の設定
m_ClearFlags: 1 (Skybox)
m_BackGroundColor: {r: 0.15, g: 0.15, b: 0.25, a: 1}

// 変更後の設定
m_ClearFlags: 2 (SolidColor)
m_BackGroundColor: {r: 0, g: 0, b: 0, a: 1}
```

#### B. 発光マテリアル設定（URP/Lit）
```csharp
// 必須プロパティ
_EmissionColor: HDR色値（Color * intensity）
_EMISSION: キーワードを有効化

// パフォーマンス最適化
MaterialPropertyBlock使用
PropertyIDの事前キャッシュ
```

#### C. 中品質Bloom設定
```yaml
threshold: 1.0
intensity: 0.5
scatter: 0.6
skipIterations: 1
maxIterations: 5
highQualityFiltering: false
downscale: Quarter
clamp: 65472
```

### 3. 実装パターン

#### 色パレット定義（8列用）
```csharp
private static readonly Color[] neonColors = {
    new Color(1.0f, 0.1f, 0.1f) * 2.0f,  // Red
    new Color(1.0f, 0.5f, 0.0f) * 2.0f,  // Orange
    new Color(1.0f, 1.0f, 0.2f) * 2.0f,  // Yellow
    new Color(0.2f, 1.0f, 0.2f) * 2.0f,  // Green
    new Color(0.0f, 1.0f, 1.0f) * 2.0f,  // Cyan
    new Color(0.2f, 0.3f, 1.0f) * 2.0f,  // Blue
    new Color(0.8f, 0.2f, 1.0f) * 2.0f,  // Purple
    new Color(1.0f, 0.2f, 0.8f) * 2.0f   // Pink
};
```

#### BlockController修正
```csharp
public void SetBlockColor(Color color)
{
    MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
    if (meshRenderer != null)
    {
        // MaterialPropertyBlockでパフォーマンス最適化
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(props);
        props.SetColor("_EmissionColor", color);
        meshRenderer.SetPropertyBlock(props);
    }
}
```

### 4. 技術的制約と考慮事項

#### パフォーマンス
- MaterialPropertyBlockを使用してドローコール削減
- Bloom設定を中品質に制限（モバイル対応）
- ダウンサンプリングでGPU負荷軽減

#### 互換性
- URP 12.x以降が必要（現在のプロジェクトは対応済み）
- HDRサポート有効（PC_RPAsset.assetで確認済み）

### 5. 統合ポイント

#### GameManager.GenerateBlocks()での統合
```csharp
GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
BlockController blockController = block.GetComponent<BlockController>();
if (blockController != null && col < neonColors.Length)
{
    blockController.SetBlockColor(neonColors[col]);
}
```

### 6. 検証済みメソッドの可用性

#### 確認済みAPI
- Camera.clearFlags（設定可能）
- Camera.backgroundColor（設定可能）
- Material.EnableKeyword()（利用可能）
- Material.SetColor()（利用可能）
- MaterialPropertyBlock.SetColor()（利用可能）
- Shader.PropertyToID()（利用可能）

### 7. 不明または不確かなAPI

なし（全ての必要なAPIは検証済み）