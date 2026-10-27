using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitleButton : MonoBehaviour
{
    public void BackToTitle()
    {
        Debug.Log("Back to Title!");

        SceneManager.LoadScene("Title");
    }
}