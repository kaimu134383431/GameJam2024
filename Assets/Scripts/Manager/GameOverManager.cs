using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public void Title()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // ���C���Q�[���V�[���ɖ߂�
    }

    public void Retry()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); // ���C���Q�[���V�[���ɖ߂�
    }

    public void Quit()
    {
        GameManager.Instance.InitializeItems();
        Application.Quit(); // �Q�[�����I������
    }
}
