using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClearMenuController : MonoBehaviour
{
    public Button defaultButton; // 初期選択したいボタン

    public float inputDelay = 3f; // 操作を受け付けない秒数

    private bool inputEnabled = false; // 入力を受け付けるかどうか

    void Start()
    {   
        // 全ボタンを一時的に無効化
        foreach (Button btn in FindObjectsOfType<Button>())
        {
            btn.interactable = false;
        }

        // 一定時間後に有効化
        Invoke(nameof(EnableInput), inputDelay);

        // 初期ボタンを明示的に選択（見た目のフォーカス用）
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
    }

    void EnableInput()
    {
        // 全ボタンを再び有効化
        foreach (Button btn in FindObjectsOfType<Button>())
        {
            btn.interactable = true;
        }
    }

    void Update()
    {
        // 選択が解除されていたら初期ボタンを選択
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }

        // キーボード/ゲームパッドで決定
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                Button btn = selected.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.Invoke();
                }
            }
        }
    }
}
