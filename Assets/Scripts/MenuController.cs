using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button defaultButton; // 初期選択したいボタン

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
