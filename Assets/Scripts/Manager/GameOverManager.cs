using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    void Update()
    {
        // Tキーが押されたときにタイトルシーンに戻る
        if (Input.GetKeyDown(KeyCode.T))
        {
            Title();
        }

        // Rキーが押されたときにタイトルシーンに戻る
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
    }

    public void Title()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // 戻る
    }

    public void Retry()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); //戻る
    }

    public void Quit()
    {
        GameManager.Instance.InitializeItems();
        Application.Quit(); // ゲームを終了する
    }
}
