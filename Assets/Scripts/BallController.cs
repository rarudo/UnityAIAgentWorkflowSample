using UnityEngine;

public class BallController : MonoBehaviour
{
    // 公開フィールド
    public float initialSpeed = 5f;
    public float maxSpeed = 15f;

    // プライベートフィールド
    private Rigidbody2D rb2d;
    private bool isMoving;
    private float bottomBoundary = -5.5f;

    private void Start()
    {
        // Rigidbody2Dコンポーネント取得
        rb2d = GetComponent<Rigidbody2D>();

        // Rigidbody2Dの設定
        if (rb2d != null)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.gravityScale = 0f; // 重力を無効化
            rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // 初期状態では停止
        isMoving = false;
    }

    private void Update()
    {
        // 落下チェック
        if (isMoving && transform.position.y < bottomBoundary)
        {
            // ゲームオーバー通知
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            isMoving = false;
        }

        // 速度制限チェック
        if (isMoving && rb2d != null)
        {
            if (rb2d.linearVelocity.magnitude > maxSpeed)
            {
                rb2d.linearVelocity = rb2d.linearVelocity.normalized * maxSpeed;
            }
        }
    }

    public void Launch()
    {
        if (rb2d != null && !isMoving)
        {
            // ランダムな角度（45度〜135度）で発射
            float angle = Random.Range(45f, 135f);
            float radians = angle * Mathf.Deg2Rad;

            // 初期速度ベクトル計算
            Vector2 direction = new Vector2(
                Mathf.Cos(radians),
                Mathf.Sin(radians)
            ).normalized;

            // 速度設定
            rb2d.linearVelocity = direction * initialSpeed;
            isMoving = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMoving) return;

        // パドルに衝突した場合の特殊処理
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // パドルの位置に応じて反射角度を調整
            float hitPoint = transform.position.x - collision.transform.position.x;
            float paddleWidth = collision.collider.bounds.size.x;
            float normalizedHitPoint = hitPoint / (paddleWidth / 2f);

            // 新しい反射角度を計算（-60度〜60度）
            float bounceAngle = normalizedHitPoint * 60f;

            // 上向きの速度を設定
            float speed = rb2d.linearVelocity.magnitude;
            float radians = (90f - bounceAngle) * Mathf.Deg2Rad;

            Vector2 newDirection = new Vector2(
                Mathf.Cos(radians),
                Mathf.Sin(radians)
            ).normalized;

            rb2d.linearVelocity = newDirection * speed;
        }

        // 速度が遅くなりすぎないよう最小速度を保証
        if (rb2d.linearVelocity.magnitude < initialSpeed * 0.5f)
        {
            rb2d.linearVelocity = rb2d.linearVelocity.normalized * initialSpeed;
        }
    }

    // ボール停止
    public void StopBall()
    {
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            isMoving = false;
        }
    }

    // ボール再配置
    public void ResetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        StopBall();
    }

    // 移動状態取得
    public bool IsMoving()
    {
        return isMoving;
    }
}