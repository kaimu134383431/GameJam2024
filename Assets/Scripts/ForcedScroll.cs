using UnityEngine;
//スクロールのためのオブジェクト
public class ForcedScroll : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;
    public BossEnemy boss;
    public bool isToGoal = false;

    private float elapsedTime = 0.0f;
    private bool isMoving = false;

    // プレイヤーオブジェクトの参照
    private GameObject player;
    private GameObject goal;
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
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (bossObject != null)
        {
            boss = bossObject.GetComponent<BossEnemy>();
        }
        goal = GameObject.FindGameObjectWithTag("Goal");
        /*
        if (isToGoal)
        {
            if (boss != null) startPosition = boss.transform.position;
            if (goal != null)
            {
                Vector3 newPosition = goal.transform.position;
                newPosition.x -= 5.0f;
                endPosition = newPosition;
            }
        }
        else
        {
            if (player != null) startPosition = player.transform.position;
            if (boss != null)
            {
                Vector3 newPosition = boss.transform.position;
                newPosition.x -= 5.0f;
                endPosition = newPosition;
            }
        }*/

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
                if (boss != null) { 
                    boss.ShowHealthBar();
                    boss.setInvincible(false);
                }
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
