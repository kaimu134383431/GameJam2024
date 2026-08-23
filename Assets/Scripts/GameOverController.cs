using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    public MonoBehaviour scrolling;

    public GameObject enemiesParent;

    public Image fadeImage;

    public GameObject gameOverMenu;

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

        // ÉXÉNÉçÅ[Éãí‚é~
        scrolling.enabled = false;

        // ìGí‚é~
        /*foreach (var e in enemiesParent.GetComponentsInChildren<MonoBehaviour>())
        {
            e.enabled = false;
        }*/

        yield return new WaitForSeconds(0.2f);

        Debug.Log("Fade start");

        // à√ì]
        yield return StartCoroutine(FadeToBlack());

        Debug.Log("Fade finished");

        // ÉÅÉjÉÖÅ[ï\é¶
        gameOverMenu.SetActive(true);

        Debug.Log("GameOverMenu active");
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