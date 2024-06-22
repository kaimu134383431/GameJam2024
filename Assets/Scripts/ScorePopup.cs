using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;

    private Text scoreText;
    private Color originalColor;

    void Awake()
    {
        // 子オブジェクトからTextコンポーネントを取得
        scoreText = GetComponentInChildren<Text>();
        if (scoreText != null)
        {
            originalColor = scoreText.color;
        }
        else
        {
            Debug.LogError("Text component not found in children of ScorePopup");
        }
    }

    void Start()
    {
        StartCoroutine(FadeAndMove());
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "+" + score.ToString();
        }
        else
        {
            Debug.LogError("SetScore called but scoreText is not initialized");
        }
    }

    IEnumerator FadeAndMove()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // テキストを上に移動させる
            transform.position = startPosition + new Vector3(0, moveSpeed * t, 0);

            // テキストをフェードアウトさせる
            if (scoreText != null)
            {
                scoreText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
