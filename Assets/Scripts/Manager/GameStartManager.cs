using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public void GameStart()
    {
        SEManager.Instance.PlaySE("Button");
        SceneManager.LoadScene("Stage 1"); // �X�e�[�W�Ɉڍs����
    }

}
