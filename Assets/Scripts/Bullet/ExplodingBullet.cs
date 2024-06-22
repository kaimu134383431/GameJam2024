using UnityEngine;
using System.Collections;

public class ExplodingBullet : MonoBehaviour
{
    //[SerializeField] float speed = 5f;          // 弾の速度
    [SerializeField] int damage = 5;                // 弾のダメージ

    public GameObject splitProjectilePrefab;
    public float explosionDelay = 2f;
    public float splitSpeed = 5f;

    private Rigidbody2D rb2D;         // Rigidbody2D コンポーネント
    private Renderer rend;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        StartCoroutine(Explode());

        //rb2D.velocity = transform.right * speed; //直進させる

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
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
            // ここにプレイヤーにダメージを与える処理を追加
            Destroy(gameObject); // 衝突したら弾を破壊する
        }
        
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionDelay);

        int bulletCount = 6;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector3 bulletMoveDirection = (bulletVector - transform.position).normalized;

            GameObject bullet = Instantiate(splitProjectilePrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * splitSpeed;

            angle += angleStep;
        }

        Destroy(gameObject); // 元の弾を破壊
    }
}
