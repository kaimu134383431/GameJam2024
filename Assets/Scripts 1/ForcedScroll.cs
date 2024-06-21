using UnityEngine;

public class ForcedScroll : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;

    private float elapsedTime = 0.0f;
    private bool isMoving = false;

    // プレイヤーオブジェクトの参照
    private GameObject player;
    // スクロールの速度
    private float scrollSpeed;
    // プレイヤーの移動速度
    private float playerMoveSpeed;

    void Start()
    {
        // スクロール速度を計算
        scrollSpeed = Vector3.Distance(startPosition, endPosition) / moveTime;

        // プレイヤーオブジェクトを取得
        player = GameObject.FindGameObjectWithTag("Player");

        // プレイヤーの移動速度を設定
        if (player != null)
        {
            playerMoveSpeed = scrollSpeed;
        }

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

            // プレイヤーもスクロールの方向に移動させる
            if (player != null)
            {
                Vector3 scrollDirection = (endPosition - startPosition).normalized;
                player.transform.position += scrollDirection * playerMoveSpeed * Time.deltaTime;
            }

            // 移動時間が終了したら移動を停止する
            if (t >= 1.0f)
            {
                isMoving = false;
                Destroy(gameObject);
            }
        }
    }

    public void StartScroll()
    {
        // 強制スクロールを開始する
        transform.position = startPosition;
        elapsedTime = 0.0f;
        isMoving = true;

        // FollowCameraの対象にする
        GameObject followCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (followCamera != null)
        {
            followCamera.GetComponent<FollowCamera>().SetTarget(gameObject);
        }
    }
}
