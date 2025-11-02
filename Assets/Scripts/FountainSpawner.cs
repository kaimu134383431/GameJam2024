using UnityEngine;

public class FountainSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;   // 生成候補のプレハブ
    [SerializeField] private float spawnInterval = 0.2f; // 生成間隔
    [SerializeField] private Vector2 launchSpeedRange = new Vector2(3f, 6f); // 初速度の範囲
    [SerializeField] private float angleRange = 45f;  // 左右方向の角度ランダム幅（度）
    [SerializeField] private float destroyY = -6f;    // 削除する高さ
    [SerializeField] private float gravity = 1f;      // 重力倍率

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnProjectile();
            timer = 0f;
        }
    }

    void SpawnProjectile()
    {
        if (prefabs.Length == 0) return;

        // ランダムなプレハブを選択
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        SEManager.Instance.PlaySE("GetItem");

        // Rigidbody2Dで物理挙動を設定
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null) rb = obj.AddComponent<Rigidbody2D>();

        rb.gravityScale = gravity;

        // ランダムな角度で上方向に打ち上げ
        float angle = Random.Range(-angleRange, angleRange);
        float speed = Random.Range(launchSpeedRange.x, launchSpeedRange.y);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

        rb.linearVelocity = direction * speed;

        // 一定時間後または画面外で削除
        obj.AddComponent<AutoDestroy>().SetDestroyY(destroyY);
    }
}

public class AutoDestroy : MonoBehaviour
{
    private float destroyY = -6f;
    private float lifeTime = 10f;
    private float timer = 0f;

    public void SetDestroyY(float y)
    {
        destroyY = y;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (transform.position.y < destroyY || timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
