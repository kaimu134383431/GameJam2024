using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    void Update()
    {
        // T�L�[�������ꂽ�Ƃ��Ƀ^�C�g���V�[���ɖ߂�
        if (Input.GetKeyDown(KeyCode.T))
        {
            Title();
        }

        // R�L�[�������ꂽ�Ƃ��Ƀ^�C�g���V�[���ɖ߂�
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
    }

    public void Title()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // �߂�
    }

    public void Retry()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); //�߂�
    }

    public void Quit()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        Application.Quit(); // �Q�[�����I������
    }
}
