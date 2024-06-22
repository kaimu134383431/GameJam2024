using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;

    private float elapsedTime = 0.0f;
    private bool isMoving = false;

    void Start()
    {
        // スクロールを開始する
        StartScroll();
    }

    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveTime); // 0から1の範囲で補間率を計算

            // 開始位置から終了位置まで移動する
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // 移動時間が終了したら移動を停止する
            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
    }

    public void StartScroll()
    {
        // 強制スクロールを開始する
        transform.position = startPosition;
        elapsedTime = 0.0f;
        isMoving = true;
    }
}

