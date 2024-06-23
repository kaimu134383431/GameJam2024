using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameClearManager : MonoBehaviour
{
    public Button nextButton; // ���̃V�[���ɐi�ރ{�^��
    public Text scoretext;
    public Text lasttext;

    private void Start()
    {
        // �ŏ��ɑI�������{�^����ݒ�
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
            // ���ݑI������Ă���{�^��������
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }

    private void HighlightButton(Button button)
    {
        // �S�Ẵ{�^���̃n�C���C�g������
        Button[] allButtons = FindObjectsOfType<Button>();
        foreach (Button btn in allButtons)
        {
            ColorBlock cb = btn.colors;
            cb.normalColor = Color.white; // �f�t�H���g�̐F�ɖ߂�
            btn.colors = cb;
        }

        // �I�����ꂽ�{�^�����n�C���C�g
        ColorBlock selectedCb = button.colors;
        selectedCb.normalColor = selectedCb.highlightedColor; // �n�C���C�g�F��ݒ�
        button.colors = selectedCb;
    }
}
