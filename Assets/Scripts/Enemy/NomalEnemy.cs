using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] private Vector2 moveDirection = Vector2.right;  // デフォルトの移動方向を右に設定
    [SerializeField] GameObject bulletPrefab; // 自機狙い弾のプレハブ
    [SerializeField] Transform bulletSpawn;         // 弾の生成位置
    [SerializeField] float fireRate = 1f;           // 射撃の間隔
    private float nextFire = 0f;          // 次に弾を撃てる時間

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        Move();
        Shoot();
    }

    protected override void Move()
    {
        rb2D.velocity = moveDirection * speed;
    }

    protected override void Die()
    {
        SEManager.Instance.PlaySE("EnemyDead");
        DropItem();
        Destroy(gameObject);
    }

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * -10f; // projectileSpawnの右方向に速度を与える
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える処理を追加
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }
}
