using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //[SerializeField] float speed = 5f;          // 弾の速度
    [SerializeField] int damage = 5;                // 弾のダメージ
    private Rigidbody2D rb2D;         // Rigidbody2D コンポーネント
    private Renderer rend;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();

        //rb2D.velocity = transform.right * speed; //直進させる

    }

    void Update()
    {
        // 弾の進行方向に弾の先頭を向かせる
        if (rb2D.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

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
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
            // ここにプレイヤーにダメージを与える処理を追加
            Destroy(gameObject); // 衝突したら弾を破壊する
        }

    }
}
