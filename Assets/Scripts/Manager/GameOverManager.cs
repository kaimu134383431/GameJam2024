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
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // �߂�
    }

    public void Retry()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); //�߂�
    }

    public void Quit()
    {
        GameManager.Instance.InitializeItems();
        Application.Quit(); // �Q�[�����I������
    }
}
