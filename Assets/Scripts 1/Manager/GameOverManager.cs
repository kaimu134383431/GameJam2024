using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public void Title()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // メインゲームシーンに戻る
    }

    public void Retry()
    {
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); // メインゲームシーンに戻る
    }

    public void Quit()
    {
        GameManager.Instance.InitializeItems();
        Application.Quit(); // ゲームを終了する
    }
}
