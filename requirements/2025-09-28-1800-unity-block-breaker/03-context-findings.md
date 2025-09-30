# コンテキスト調査結果

## 1. 修正が必要な具体的なファイル

### 新規作成が必要なファイル
- `Assets/Scenes/Block.unity` - メインゲームシーン
- `Assets/Prefabs/BlockPrefab.prefab` - ブロックPrefab
- `Assets/Prefabs/PaddlePrefab.prefab` - パドルPrefab
- `Assets/Prefabs/BallPrefab.prefab` - ボールPrefab
- `Assets/Scripts/GameManager.cs` - ゲーム管理
- `Assets/Scripts/PaddleController.cs` - パドル制御
- `Assets/Scripts/BallController.cs` - ボール制御
- `Assets/Scripts/BlockController.cs` - ブロック制御

### 変更が必要な既存ファイル
- なし（新規プロジェクトのため）

## 2. 従うべき正確なパターン

### Unity APIシグネチャと使用パターン

#### GameObject生成
```csharp
// シグネチャ: GameObject(string name, params Type[] components)
new GameObject("Paddle", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Rigidbody2D));
```

#### Rigidbody2D制御
```csharp
// シグネチャ: extern public Vector2 linearVelocity { get; set; }
rigidbody2D.linearVelocity = new Vector2(horizontalInput * speed, 0);

// シグネチャ: extern public void MovePosition(Vector2 position)
rigidbody2D.MovePosition(position);
```

#### 入力取得
```csharp
// シグネチャ: public static float GetAxisRaw(string axisName)
float horizontal = Input.GetAxisRaw("Horizontal"); // -1, 0, 1の値
```

#### 衝突検出
```csharp
void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ball"))
    {
        // 処理
    }
}
```

## 3. 詳細に分析された類似機能

### GameManagerシングルトンパターン
```csharp
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
```

### 状態管理パターン
```csharp
public enum GameState { Start, Playing, GameOver }
private GameState currentState;

public void ChangeState(GameState newState)
{
    currentState = newState;
    UpdateUI();
}
```

## 4. 技術的制約と考慮事項

### uLoopMCP制約
- ファイルI/O操作不可（System.IO.*使用禁止）
- AssetDatabase.CreateFolder使用不可
- .cs/.asmdefファイル作成不可（ターミナル/IDE使用必須）
- Editor自動化のみ（スクリプトファイル作成は外部で実行）

### Physics2D設定
- 重力: (0, -9.81) - ボールのgravityScaleは0に設定
- VelocityIterations: 8
- PositionIterations: 3

### 入力システム
- Legacy Input System使用（"Horizontal"軸設定済み）
- New Input Systemも利用可能（1.14.1インストール済み）

## 5. 特定された統合ポイント

### uLoopMCP execute-dynamic-code統合
```csharp
// シーン作成
SceneManager.CreateScene("Block");

// GameObject生成と配置
new GameObject("name", typeof(Component));

// Prefab作成（#if UNITY_EDITOR必須）
PrefabUtility.SaveAsPrefabAsset(gameObject, "path");
```

### UI統合（UGUI）
- Canvas（ScreenSpaceOverlay）
- Button.onClick.AddListener()
- Text.text更新

## 6. 不明または不確かなAPI

### 検証が必要なAPI
- なし（すべてのAPIはunity-cs-referenceで確認済み）

## 7. APIシグネチャと使用パターン

### 検証済みメソッドの可用性

#### GameObject API
- `CreatePrimitive(PrimitiveType type)` - 利用可能
- `AddComponent(Type componentType)` - 利用可能
- `GetComponent<T>()` - 利用可能
- `SetActive(bool value)` - 利用可能

#### Physics2D API
- `Rigidbody2D.linearVelocity` - 利用可能
- `Rigidbody2D.MovePosition(Vector2)` - 利用可能
- `Collider2D.OnCollisionEnter2D` - 利用可能

#### Input API
- `Input.GetAxis(string)` - 利用可能
- `Input.GetAxisRaw(string)` - 利用可能
- `Input.GetKey(KeyCode)` - 利用可能

#### UI API
- `Button.onClick` - 利用可能
- `Text.text` - 利用可能
- `Canvas` - 利用可能

## 8. 推奨実装値

### ゲームフィールド
- カメラOrthographicSize: 5.0
- 画面境界: X=±8.5, Y=±5.0
- パドル初期位置: (0, -4, 0)
- ボール初期位置: (0, -3, 0)

### ブロック配置
- グリッド: 8列×5行
- ブロック間隔: X=1.0, Y=0.5
- 開始位置: (-4, 3, 0)

### 物理パラメータ
- ボール初速: 5.0f
- パドル移動速度: 10.0f
- ボール最大速度: 15.0f
- PhysicsMaterial2D: friction=0, bounciness=1

### ゲーム設定
- 初期ライフ: 3
- ブロック得点: 10点
- ブロック耐久度: 1（1回で破壊）