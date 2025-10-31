using UnityEngine;


[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    private GameObject target;
    private Camera cam;

    [Header("アスペクト比設定")]
    public float targetAspect = 16f / 9f;

    void Start()
    {
        cam = GetComponent<Camera>();
        ApplyAspect();
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, 0, transform.position.z);
        }
    }

    void ApplyAspect() 
    {
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // 縦長 → 上下に黒帯（letterbox）
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // 横長 → 左右に黒帯（pillarbox）
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
