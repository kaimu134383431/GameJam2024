using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScorePopup : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fadeDuration = 1f;
    public float displayDuration = 2f; // �\������

    [SerializeField] private Sprite[] itemSprites; // �X�v���C�g�̔z��

    private Text scoreText;
    private Image scoreImage;
    private Color originalColor;

    void Awake()
    {
        // �q�I�u�W�F�N�g����Text�R���|�[�l���g��Image�R���|�[�l���g���擾
        scoreText = GetComponentInChildren<Text>();
        scoreImage = GetComponentInChildren<Image>();

        if (scoreText != null)
        {
            originalColor = scoreText.color;
        }
        else
        {
            Debug.LogError("Text component not found in children of ScorePopup");
        }

        if (scoreImage == null)
        {
            Debug.LogError("Image component not found in children of ScorePopup");
        }
    }

    void Start()
    {
        StartCoroutine(DisplayAndFade());
    }

    public void SetScore(int score, int spriteIndex)
    {
        if (scoreText != null)
        {
            scoreText.text = "+" + score.ToString();
        }
        else
        {
            Debug.LogError("SetScore called but scoreText is not initialized");
        }

        if (scoreImage != null && spriteIndex >= 0 && spriteIndex < itemSprites.Length)
        {
            scoreImage.sprite = itemSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("SetScore called but scoreImage is not initialized or spriteIndex is out of range");
        }

        // �X�v���C�g�̕\���ʒu�𒲐�����
        if (scoreText != null && scoreImage != null)
        {
            scoreImage.rectTransform.anchoredPosition = new Vector2(-50f, scoreText.rectTransform.rect.height + 30f);
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

            // �e�L�X�g����Ɉړ�������
            transform.position = startPosition + new Vector3(0, moveSpeed * t, 0);

            // �e�L�X�g���t�F�[�h�A�E�g������
            if (scoreText != null)
            {
                scoreText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);
            }

            // �X�v���C�g�̃t�F�[�h�A�E�g
            if (scoreImage != null)
            {
                scoreImage.color = new Color(scoreImage.color.r, scoreImage.color.g, scoreImage.color.b, 1 - t);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
