using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public void Title()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Title"); // メインゲームシーンに戻る
    }

    public void Retry()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); // メインゲームシーンに戻る
    }

    public void Quit()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        Application.Quit(); // ゲームを終了する
    }
}
