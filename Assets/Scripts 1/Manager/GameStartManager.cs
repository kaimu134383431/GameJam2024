using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Stage 1"); // ステージに移行する
    }

}
