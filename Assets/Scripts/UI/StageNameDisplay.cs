using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageNameDisplay : MonoBehaviour
{
    [SerializeField] Text stageNameText;
    [SerializeField] float waitTime = 0f;
    [SerializeField] float displayDuration = 2.0f;
    
    void Start()
    {
        // 開始直後
        ShowStageName("Stage 1", 0f, 2f);
    }

    // 外部から呼び出せるようにメソッド化
    public void ShowStageName(string name, float wait, float duration)
    {
        StopAllCoroutines(); // すでに表示中ならキャンセル
        StartCoroutine(DisplayStageName(name,wait,duration));
    }

    private IEnumerator DisplayStageName(string name, float wait, float duration)
    {
        yield return new WaitForSeconds(wait);
        stageNameText.text = name;
        stageNameText.enabled = true;
        yield return new WaitForSeconds(duration);
        stageNameText.enabled = false;
    }
}
