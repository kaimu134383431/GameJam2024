using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public void Title()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // ���C���Q�[���V�[���ɖ߂�
    }

    public void Retry()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); // ���C���Q�[���V�[���ɖ߂�
    }

    public void Quit()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        Application.Quit(); // �Q�[�����I������
    }
}
