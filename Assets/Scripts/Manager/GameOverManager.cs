using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Text clearScoreText; // スコア表示用のTextコンポーネント

    private void Start()
    {
        // ゲームオーバー時にスコアを表示
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
        SceneManager.LoadScene("Title"); // タイトルシーンに戻る
    }

    public void Retry()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        SceneManager.LoadScene("Stage 1"); // ステージ1に戻る
    }

    public void Quit()
    {
        SEManager.Instance.PlaySE("Button");
        GameManager.Instance.InitializeItems();
        Application.Quit(); // ゲームを終了する
    }
}
