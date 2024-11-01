using UnityEngine;

public class ForcedScroll1 : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;
    public BossEnemy boss;
    public bool isToGoal = false;

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
            //transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // 移動時間が終了したら無敵解除と表示を行う
            if (t >= 1.0f)
            {
                isMoving = false;
                if (boss != null)
                {
                    boss.ShowHealthBar();
                    boss.setInvincible(false);
                    //追加
                    boss.isWait = false;
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
