using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    void Update()
    {
        Debug.Log("RetryButton Update running");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Z detected");
            Retry();
        }

        if (Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("Pad detected");
            Retry();
        }
    }

    public void Retry()
    {
        Debug.Log("Retry clicked!");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}