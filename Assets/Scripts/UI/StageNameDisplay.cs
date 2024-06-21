using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class StageNameDisplay : MonoBehaviour
{
    [SerializeField] Text stageNameText;
    [SerializeField] float displayDuration = 0.5f;

    void Start()
    {
        StartCoroutine(DisplayStageName());
    }

    IEnumerator DisplayStageName()
    {
        stageNameText.enabled = true;
        yield return new WaitForSeconds(displayDuration);
        stageNameText.enabled = false;
    }
}
