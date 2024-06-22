using UnityEngine;
using System.Collections;

public class BossEnemy : Enemy
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float fireRate = 1f;
    private float nextFire = 0f;

    private int attackPhase = 0;        // 現在の攻撃フェーズ
    private float attackSwitchTime = 5f; // 各攻撃フェーズの持続時間
    private float nextSwitchTime = 0f;   // 次の攻撃フェーズに移る時間

    protected override void Start()
    {
        base.Start();
        nextSwitchTime = Time.time + attackSwitchTime;
    }

    void FixedUpdate()
    {
        Move();
        Shoot();
        SwitchAttackPhase();
    }

    protected override void Move()
    {
        // ボスの特別な移動パターンを実装
        rb2D.velocity = new Vector2(0, Mathf.Sin(Time.time) * speed);
    }

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            switch (attackPhase)
            {
                case 0:
                    FireStraight();
                    break;
                case 1:
                    FireRadial();
                    break;
                case 2:
                    FireAllDirections();
                    break;
            }
        }
    }

    void SwitchAttackPhase()
    {
        if (Time.time > nextSwitchTime)
        {
            attackPhase = (attackPhase + 1) % 3; // 攻撃フェーズを切り替える
            nextSwitchTime = Time.time + attackSwitchTime;
        }
    }

    void FireStraight()
    {
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        for (int i=0;i<20;i++)
        {
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = projectileSpawn.right * -10f; // projectileSpawnの右方向に速度を与える

            yield return new WaitForSeconds(0.05f); // 弾の発射間隔
        }
    }



    void FireRadial()
    {
        int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = projectileSpawn.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = projectileSpawn.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector3 bulletMoveDirection = (bulletVector - projectileSpawn.position).normalized;

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * 5f;

            angle += angleStep;
        }
    }

    void FireAllDirections()
    {
        int bulletCount = 32;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = projectileSpawn.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = projectileSpawn.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector3 bulletMoveDirection = (bulletVector - projectileSpawn.position).normalized;

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * 5f;

            angle += angleStep;
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
