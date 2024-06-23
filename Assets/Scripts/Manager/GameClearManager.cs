using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextScene();
        }
    }
    */

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            LoadNextScene();
        }

    }

    void LoadNextScene()
    {
        SEManager.Instance.PlaySE("Clear");
        SceneManager.LoadScene("GameClear");
    }
}
