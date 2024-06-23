using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameClearManager : MonoBehaviour
{
    public Button nextButton; // 次のシーンに進むボタン
    public Text scoretext;
    public Text lasttext;

    private void Start()
    {
        // 最初に選択されるボタンを設定
        EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
        HighlightButton(nextButton);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            nextButton.gameObject.SetActive(true);
            lasttext.gameObject.SetActive(true);
            lasttext.text=scoretext.text;
            SEManager.Instance.PlaySE("Clear");
        }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("GameClear");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // 現在選択されているボタンを押す
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }

    private void HighlightButton(Button button)
    {
        // 全てのボタンのハイライトを解除
        Button[] allButtons = FindObjectsOfType<Button>();
        foreach (Button btn in allButtons)
        {
            ColorBlock cb = btn.colors;
            cb.normalColor = Color.white; // デフォルトの色に戻す
            btn.colors = cb;
        }

        // 選択されたボタンをハイライト
        ColorBlock selectedCb = button.colors;
        selectedCb.normalColor = selectedCb.highlightedColor; // ハイライト色を設定
        button.colors = selectedCb;
    }
}
