using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuCursor : MonoBehaviour
{
    public Image image;
    public Sprite spriteA;
    public Sprite spriteB;

    void Start()
    {
        StartCoroutine(CursorAnim());
    }

    IEnumerator CursorAnim()
    {
        while (true)
        {
            image.sprite = spriteA;
            yield return new WaitForSeconds(0.1f);

            image.sprite = spriteB;
            yield return new WaitForSeconds(0.1f);
        }
    }
}