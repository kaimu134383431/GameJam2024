using UnityEngine;
using System.Collections.Generic;

public class ForcedScrollMulti : MonoBehaviour
{
    [System.Serializable]
    public class ScrollPoint
    {
        public Vector3 position;   // 停止地点（ボスやゴール）
        public float moveTime;     // その区間のスクロール時間
        public BossEnemy boss;     // その地点にいるボス（いなければ null）
    }

    public List<ScrollPoint> points = new List<ScrollPoint>();
    private int currentIndex = 0;

    private float elapsedTime = 0f;
    private bool isMoving = true;

    private GameObject player;
    private Vector3 lastScrollPosition;
    private bool bossJustAppeared = false;  // ボス出現イベントしたかどうか

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        if (points.Count == 0)
        {
            Debug.LogError("No scroll points set!");
            return;
        }

        lastScrollPosition = transform.position;
        StartNextScroll();
    }

    void Update()
    {
        if (currentIndex >= points.Count) return;

        ScrollPoint currentPoint = points[currentIndex];

        if (isMoving)
        {
            // スクロール開始位置と終了位置
            Vector3 startPos = (currentIndex == 0) ? lastScrollPosition : points[currentIndex - 1].position;
            Vector3 endPos = currentPoint.position;


            // 進捗割合 t を計算
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / currentPoint.moveTime);


            // スクロール位置を補間
            Vector3 newScrollPos = Vector3.Lerp(startPos, endPos,t);

            // プレイヤーもスクロール差分だけ移動
            if (player != null)
            {
                Vector3 delta = newScrollPos - transform.position;
                player.transform.position += delta;
            }

            // スクロール位置を更新
            transform.position = newScrollPos;
        }

        // 到達したらスクロールを止める
        if (elapsedTime >= currentPoint.moveTime)
        {
            /*transform.position = endPos; // 完全に固定
            player.transform.position += endPos - newScrollPos; // プレイヤー差分を補正
            lastScrollPosition = endPos; // 次区間の開始位置を更新*/
            isMoving = false;

            // ボス出現処理（1度だけ）
            if (currentPoint.boss != null && !bossJustAppeared)
            {
                currentPoint.boss.OnScrollReached();
                bossJustAppeared = true;
            }

            // 倒したら次スクロールへ
            if (currentPoint.boss == null && bossJustAppeared)
            {
                currentIndex++;
                StartNextScroll();
            }
        }
    }



    void StartNextScroll()
    {
        elapsedTime = 0f;
        isMoving = true;
        bossJustAppeared = false;

        // カメラの追従対象を更新
        GameObject followCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (followCamera != null)
            followCamera.GetComponent<FollowCamera>().SetTarget(gameObject);
    }
}
