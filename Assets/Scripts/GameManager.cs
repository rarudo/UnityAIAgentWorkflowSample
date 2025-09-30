using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    // ゲーム状態の定義
    public enum GameState
    {
        Start,
        Playing,
        GameOver,
        Clear
    }

    // プライベートフィールド
    private GameState currentState;
    private int score;
    private int remainingBlocks;

    // UI参照
    public Text scoreText;
    public Button startButton;
    public Button retryButton;
    public GameObject gameOverPanel;
    public GameObject clearPanel;

    // ボールとブロック配置データ
    public GameObject ballPrefab;
    public GameObject blockPrefab;
    public Transform blocksParent;

    // ブロック配置パターン（1=ブロックあり、0=なし）
    private int[,] blockPattern = new int[,]
    {
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1}
    };

    // 列ごとのネオンカラー配列
    private static readonly Color[] columnColors = new Color[]
    {
        new Color(1.0f, 0.1f, 0.1f) * 2.0f,  // Red
        new Color(1.0f, 0.5f, 0.0f) * 2.0f,  // Orange
        new Color(1.0f, 1.0f, 0.2f) * 2.0f,  // Yellow
        new Color(0.2f, 1.0f, 0.2f) * 2.0f,  // Green
        new Color(0.0f, 1.0f, 1.0f) * 2.0f,  // Cyan
        new Color(0.2f, 0.3f, 1.0f) * 2.0f,  // Blue
        new Color(0.8f, 0.2f, 1.0f) * 2.0f,  // Purple
        new Color(1.0f, 0.2f, 0.8f) * 2.0f   // Pink
    };

    private void Awake()
    {
        // シングルトン処理
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // 初期状態設定
        currentState = GameState.Start;

        // ボタンイベント登録
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryGame);
        }

        // UI初期化
        UpdateUI();
    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        score = 0;

        // ブロック生成
        GenerateBlocks();

        // ボール発射
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            BallController ballController = ball.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.Launch();
            }
        }

        UpdateUI();
    }

    private void GenerateBlocks()
    {
        remainingBlocks = 0;

        float startX = -4f;
        float startY = 3f;
        float xSpacing = 1f;
        float ySpacing = 0.5f;

        for (int row = 0; row < blockPattern.GetLength(0); row++)
        {
            for (int col = 0; col < blockPattern.GetLength(1); col++)
            {
                if (blockPattern[row, col] == 1)
                {
                    Vector3 position = new Vector3(
                        startX + (col * xSpacing),
                        startY - (row * ySpacing),
                        0
                    );

                    GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
                    if (blocksParent != null)
                    {
                        block.transform.SetParent(blocksParent);
                    }

                    // 列ごとの色設定を追加
                    BlockController blockController = block.GetComponent<BlockController>();
                    if (blockController != null && col < columnColors.Length)
                    {
                        blockController.SetBlockColor(columnColors[col]);
                    }

                    remainingBlocks++;
                }
            }
        }
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;

        // ボール停止
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        UpdateUI();
    }

    public void GameClear()
    {
        currentState = GameState.Clear;

        // ボール停止
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;
        remainingBlocks--;

        // スコア更新
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        // クリア判定
        CheckGameClear();
    }

    private void CheckGameClear()
    {
        if (remainingBlocks <= 0 && currentState == GameState.Playing)
        {
            GameClear();
        }
    }

    public void RetryGame()
    {
        // シーンリロード
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateUI()
    {
        // スコア表示更新
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        // 状態に応じたUI表示
        if (startButton != null)
        {
            startButton.gameObject.SetActive(currentState == GameState.Start);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(currentState == GameState.GameOver);
        }

        if (clearPanel != null)
        {
            clearPanel.SetActive(currentState == GameState.Clear);
        }

        if (retryButton != null)
        {
            retryButton.gameObject.SetActive(
                currentState == GameState.GameOver || currentState == GameState.Clear
            );
        }
    }

    // ゲーム状態取得
    public GameState GetCurrentState()
    {
        return currentState;
    }
}