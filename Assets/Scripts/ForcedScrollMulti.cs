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
    private bool isMoving = false;

    private GameObject player;
    private Vector3 lastScrollPosition;

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
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        ScrollPoint currentPoint = points[currentIndex];
        Vector3 startPos = (currentIndex == 0) ? lastScrollPosition : points[currentIndex - 1].position;
        Vector3 endPos = currentPoint.position;

        float t = Mathf.Clamp01(elapsedTime / currentPoint.moveTime);
        Vector3 newScrollPos = Vector3.Lerp(startPos, endPos, t);

        // プレイヤーをスクロール差分だけ移動
        Vector3 delta = newScrollPos - transform.position;
        player.transform.position += delta;

        // スクロール位置を更新
        transform.position = newScrollPos;

        if (t >= 1f)
        {
            isMoving = false;
            lastScrollPosition = endPos;

            // ボスがいる場合は停止してボスを待つ
            if (currentPoint.boss != null)
            {
                currentPoint.boss.isWait = false;
                // ボスが撃破されたら次のスクロールを再開するように設定
            }
            else
            {
                // ボスなしなら次のスクロールへ
                currentIndex++;
                if (currentIndex < points.Count)
                    StartNextScroll();
            }
        }
    }

    void StartNextScroll()
    {
        elapsedTime = 0f;
        isMoving = true;

        // カメラの追従対象を設定
        GameObject followCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (followCamera != null)
            followCamera.GetComponent<FollowCamera>().SetTarget(gameObject);
    }
}
