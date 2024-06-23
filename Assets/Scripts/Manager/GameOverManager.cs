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
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // 戻る
    }

    public void Retry()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); //戻る
    }

    public void Quit()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        Application.Quit(); // ゲームを終了する
    }
}
