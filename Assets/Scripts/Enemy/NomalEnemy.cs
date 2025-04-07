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
    [SerializeField] private float circleRadius = 3f; // 円の半径
    [SerializeField] private float circleSpeed = 2f;  // 円の速度

    // 射撃に関連するフィールド
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 10f;

    [SerializeField] private float chaseDuration = 3f; // 追跡を続ける秒数
    private float chaseStartTime;

    private float nextFire = 0f;
    private float startTime;
    private bool isActive = false;
    private bool isStarted = false;

    private Renderer renderer;

    public enum EnemyBehaviorPattern
    {
        StraightMove,
        SinWaveMove,
        RushToPlayer,
        CircleMove
    }

    public enum EnemyShootPattern
    {
        PlayerTrackingShoot,
        FixedDirectionShoot,
        NoShoot,
        Radial,
        ThreeWayShoot // 3wayショットを追加
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

        Move();
        Shoot();
    }

    protected override void Move()
{
    // 敵が画面内に入っているかどうかを確認
    if (!IsVisible())
    {
        rb2D.velocity = Vector2.zero; // 画面外なら速度を0に
        return;
    }

    
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
            if (!isStarted)
            {
                isStarted = true;
                chaseStartTime = Time.time;
            }

            // 規定秒を経過したら追跡をやめる
            if (Time.time - chaseStartTime <= chaseDuration)
            {
                RushToPlayer();
            }
            else
            {
                //rb2D.velocity = Vector2.zero; // 停止
            }
            break;
        case EnemyBehaviorPattern.CircleMove:
            CircleMove();
            break;
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
            case EnemyShootPattern.Radial:
                FireRadial();
                break;
            case EnemyShootPattern.ThreeWayShoot: // 3wayショットを追加
                ShootThreeWay();
                break;
        }
    }

    void ShootPlayerTracking()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.parent.position;
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

    void FireRadial()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            int bulletCount = 12;
            float angleStep = 360f / bulletCount;
            float angle = 0f;

            for (int i = 0; i < bulletCount; i++)
            {
                float bulletDirX = bulletSpawn.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float bulletDirY = bulletSpawn.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY, 0);
                Vector3 bulletMoveDirection = (bulletVector - bulletSpawn.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
                bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

                angle += angleStep;
            }
        }
    }

    void ShootThreeWay()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            // 3wayの弾の角度と間隔
            float angleOffset = 165f;
            float angleStep = 30f;

            for (int i = 0; i < 3; i++)
            {
                Quaternion bulletRotation = bulletSpawn.rotation * Quaternion.Euler(0, 0, angleOffset + i * angleStep);
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletRotation);
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
            }
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

    void CircleMove()
    {
        float angle = (Time.time - startTime) * circleSpeed;
        Vector2 circlePosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;
        rb2D.position = circlePosition;
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= -0.2 && screenPoint.x <= 1.2 && screenPoint.y >= -0.2 && screenPoint.y <= 1.2;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }


    }

    protected override void Die()
    {
        DropItem();
        SEManager.Instance.PlaySE("EnemyDead");
        Destroy(gameObject);
    }
}
