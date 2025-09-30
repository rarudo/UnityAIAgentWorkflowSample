using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // 公開フィールド
    public float moveSpeed = 10f;
    public float leftBoundary = -8f;
    public float rightBoundary = 8f;

    // プライベートフィールド
    private Rigidbody2D rb2d;
    private float horizontalInput;
    private float paddleHalfWidth;

    private void Start()
    {
        // Rigidbody2Dコンポーネント取得
        rb2d = GetComponent<Rigidbody2D>();

        // パドルの半幅を計算
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            paddleHalfWidth = spriteRenderer.bounds.size.x / 2f;
        }
        else
        {
            // SpriteRendererがない場合、デフォルト値を使用
            paddleHalfWidth = 0.5f;
        }

        // Rigidbody2Dの設定
        if (rb2d != null)
        {
            rb2d.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Update()
    {
        // ゲーム中のみ入力を受け付ける
        if (GameManager.Instance != null &&
            GameManager.Instance.GetCurrentState() == GameManager.GameState.Playing)
        {
            // 入力取得（-1, 0, 1の値）
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            horizontalInput = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (rb2d != null)
        {
            // 新しい位置を計算
            Vector2 newPosition = rb2d.position;
            newPosition.x += horizontalInput * moveSpeed * Time.fixedDeltaTime;

            // 位置制限
            newPosition = ClampPosition(newPosition);

            // 物理演算で移動
            rb2d.MovePosition(newPosition);
        }
    }

    private Vector2 ClampPosition(Vector2 position)
    {
        // パドルの幅を考慮して画面内に収める
        position.x = Mathf.Clamp(
            position.x,
            leftBoundary + paddleHalfWidth,
            rightBoundary - paddleHalfWidth
        );

        return position;
    }

    // パドル境界設定（エディタから調整可能）
    public void SetBoundaries(float left, float right)
    {
        leftBoundary = left;
        rightBoundary = right;
    }

    // デバッグ用：境界線を表示
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftLine = new Vector3(leftBoundary + paddleHalfWidth, transform.position.y, 0);
        Vector3 rightLine = new Vector3(rightBoundary - paddleHalfWidth, transform.position.y, 0);

        Gizmos.DrawLine(
            leftLine - Vector3.up * 0.5f,
            leftLine + Vector3.up * 0.5f
        );

        Gizmos.DrawLine(
            rightLine - Vector3.up * 0.5f,
            rightLine + Vector3.up * 0.5f
        );
    }
}