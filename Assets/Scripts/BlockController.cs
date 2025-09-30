using UnityEngine;

public class BlockController : MonoBehaviour
{
    // 公開フィールド
    public int pointValue = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ボールと衝突した場合
        if (collision.gameObject.CompareTag("Ball"))
        {
            // スコア加算
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(pointValue);
            }

            // ブロック破壊
            Destroy(gameObject);
        }
    }

    // エディタでブロックの色を設定
    public void SetBlockColor(Color color)
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(props);
            props.SetColor("_BaseColor", color);
            props.SetColor("_EmissionColor", color);
            meshRenderer.SetPropertyBlock(props);
        }
    }

    // 得点設定
    public void SetPointValue(int points)
    {
        pointValue = points;
    }
}