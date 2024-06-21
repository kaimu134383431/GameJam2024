using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;         // 弾の速度
    [SerializeField] float lifetime = 2f;       // 弾の寿命
    [SerializeField] int damage = 5;                // 弾のダメージ

    private Rigidbody2D rb2D;           // Rigidbody2Dコンポーネント
    private Renderer rend;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Rigidbody2Dコンポーネントを取得
        rb2D.velocity = transform.right * speed; // 弾を右方向に移動させる
        rend = GetComponent<Renderer>();

        Destroy(gameObject, lifetime); // 指定したlifetime後に弾を破壊する
    }

    void Update()
    {
        // カメラの境界を取得
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 弾が他のオブジェクトに衝突したときの処理
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            // ここに敵にダメージを与える処理を追加
            Destroy(gameObject); // 衝突したら弾を破壊する
        }
    }
}
