using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Retry()
    {
        Debug.Log("Retry clicked!");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}