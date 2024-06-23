using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] private EnemyBehaviorPattern behaviorPattern = EnemyBehaviorPattern.StraightMove;
    [SerializeField] private EnemyShootPattern shootPattern = EnemyShootPattern.PlayerTrackingShoot;

    // 移動に関連するフィールド
    [SerializeField] private Vector2 moveDirection = Vector2.right;
    [SerializeField] private float amplitude = 5f;
    [SerializeField] private float frequency = 2f;
    [SerializeField] private float rushSpeed = 7f;

    // 射撃に関連するフィールド
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 10f;

    private float nextFire = 0f;
    private float startTime;
    private bool isActive = false;

    private Renderer renderer;

    public enum EnemyBehaviorPattern
    {
        StraightMove,
        SinWaveMove,
        RushToPlayer
    }

    public enum EnemyShootPattern
    {
        PlayerTrackingShoot,
        FixedDirectionShoot,
        NoShoot
    }

    protected override void Start()
    {
        base.Start();
        startTime = Time.time;
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!isActive)
        {
            if (renderer.isVisible)
            {
                isActive = true;
            }
            else
            {
                return;
            }
        }

        // This part executes only if isActive is true.
        Move();
        Shoot();
    }


    protected override void Move()
    {
        switch (behaviorPattern)
        {
            case EnemyBehaviorPattern.StraightMove:
                rb2D.velocity = moveDirection * speed;
                break;
            case EnemyBehaviorPattern.SinWaveMove:
                float horizontalMovement = amplitude * Mathf.Sin(5 * Mathf.PI * frequency * (Time.time - startTime));
                rb2D.velocity = new Vector2(-speed, horizontalMovement);
                break;
            case EnemyBehaviorPattern.RushToPlayer:
                RushToPlayer();
                break;
            // 他の移動パターンを追加する場合はここにcaseを追加する
        }
    }

    void Shoot()
    {
        switch (shootPattern)
        {
            case EnemyShootPattern.PlayerTrackingShoot:
                ShootPlayerTracking();
                break;
            case EnemyShootPattern.FixedDirectionShoot:
                ShootFixedDirection();
                break;
            case EnemyShootPattern.NoShoot:
                break;
            // 他の射撃パターンを追加する場合はここにcaseを追加する
        }
    }

    void ShootPlayerTracking()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Vector2 direction = (playerPosition - bulletSpawn.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
    }

    void ShootFixedDirection()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * -bulletSpeed;
        }
    }

    void RushToPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb2D.velocity = direction * rushSpeed;
        }
    }

    // ゲームオブジェクトとの衝突時の処理
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }

    protected override void Die()
    {
        DropItem();
        SEManager.Instance.PlaySE("EnemyDead");
        Destroy(gameObject);
    }
}
