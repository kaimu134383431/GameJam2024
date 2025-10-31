using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;
    public float displayDuration = 0f; // 表示時間

    [SerializeField] private Sprite[] itemSprites; // スプライトの配列
    [SerializeField] private int healAmount = 1;   // 回復量

    private Text scoreText;
    private Image scoreImage;
    private Color originalColor;

    void Awake()
    {
        // 子オブジェクトからTextコンポーネントとImageコンポーネントを取得
        //scoreText = GetComponentInChildren<Text>();
        scoreImage = GetComponentInChildren<Image>();

        /*if (scoreText != null)
        {
            originalColor = scoreText.color;
        }
        else
        {
            Debug.LogError("Text component not found in children of ScorePopup");
        }*/

        if (scoreImage == null)
        {
            Debug.LogError("Image component not found in children of ScorePopup");
        }
    }

    void Start()
    {
        // プレイヤーのHPを回復
        PlayerManager player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        if (player != null)
        {
            player.Heal(0.1f);
        }

        StartCoroutine(DisplayAndFade());
    }

    public void SetScore(int score, int spriteIndex)
    {
        /*if (scoreText != null)
        {
            scoreText.text = "+" + score.ToString();
        }
        else
        {
            Debug.LogError("SetScore called but scoreText is not initialized");
        }*/

        if (scoreImage != null && spriteIndex >= 0 && spriteIndex < itemSprites.Length)
        {
            scoreImage.sprite = itemSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("SetScore called but scoreImage is not initialized or spriteIndex is out of range");
        }

        // スプライトの表示位置を調整する
        if (scoreText != null && scoreImage != null)
        {
            scoreImage.rectTransform.anchoredPosition = new Vector2(-50f, scoreText.rectTransform.rect.height + 20f);
        }
    }

    IEnumerator DisplayAndFade()
    {
        yield return new WaitForSeconds(displayDuration);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // テキストを上に移動させる
            transform.position = startPosition + new Vector3(0, moveSpeed * t, 0);

            /* テキストをフェードアウトさせる
            if (scoreText != null)
            {
                scoreText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);
            }*/

            // スプライトのフェードアウト
            if (scoreImage != null)
            {
                scoreImage.color = new Color(scoreImage.color.r, scoreImage.color.g, scoreImage.color.b, 1 - t);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
