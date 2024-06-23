using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f; // 弾の速度
    [SerializeField] int damage = 1; // 弾のダメージ
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = transform.up * speed; // 弾を前方に進める
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); // 衝突したら弾を破壊する
        }*/ //ここのコメントアウト/**/はEnemyができてから消す
    }
}