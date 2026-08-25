using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class GameOverController : MonoBehaviour
{
    public MonoBehaviour scrolling;

    public GameObject enemiesParent;

    public Image fadeImage;

    public GameObject gameOverMenu;

    public GameObject retryButton;

    public float fadeSpeed;

    bool gameOver = false;

    public void StartGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        Debug.Log("GameOverSequence start");

        // スクロール停止
        scrolling.enabled = false;

        // 敵停止
        /*foreach (var e in enemiesParent.GetComponentsInChildren<MonoBehaviour>())
        {
            e.enabled = false;
        }*/

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Fade start");

        // 暗転
        yield return StartCoroutine(FadeToBlack());

        Debug.Log("Fade finished");

        // メニュー表示
        gameOverMenu.SetActive(true);

        // リトライボタンにカーソルを合わせる
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    IEnumerator FadeToBlack()
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}