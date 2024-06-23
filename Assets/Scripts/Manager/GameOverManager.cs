using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Text clearScoreText; // �X�R�A�\���p��Text�R���|�[�l���g

    private void Start()
    {
        // �Q�[���I�[�o�[���ɃX�R�A��\��
        DisplayScore();
    }

    void DisplayScore()
    {
        if (clearScoreText != null)
        {
            int score = GameManager.Instance.GetScore();
            clearScoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("clearScoreText is not assigned in the inspector.");
        }
    }

    public void Title()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // �^�C�g���V�[���ɖ߂�
    }

    public void Retry()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); // �X�e�[�W1�ɖ߂�
    }

    public void Quit()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        Application.Quit(); // �Q�[�����I������
    }
}
