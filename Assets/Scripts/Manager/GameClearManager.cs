using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameClearManager : MonoBehaviour
{
    [SerializeField] private Text clearScoreText; // �X�R�A�\���p��Text�R���|�[�l���g

    private void Start()
    {
        // �Q�[���N���A���ɃX�R�A��\��
        DisplayScore();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SEManager.Instance.PlaySE("Clear");
        SceneManager.LoadScene("GameClear");
    }

    private void DisplayScore()
    {
        if (clearScoreText != null)
        {
            int score = GameManager.Instance.GetScore();
            clearScoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("ClearScoreText is not assigned in the inspector.");
        }
    }
}
