using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected != null)
            {
                Button button = selected.GetComponent<Button>();

                if (button != null)
                {
                    button.onClick.Invoke();
                }
            }
        }
    }
}