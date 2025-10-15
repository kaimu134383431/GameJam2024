using UnityEngine;
using System.Collections;

public class SobaBoss : BossEnemy
{
    [SerializeField] protected GameObject projectilePrefab2;
    private bool isUdonDefeated = false; // UdonBoss が倒されたかどうかのフラグ

    protected override void Start()
    {
        base.Start();
        nextSwitchTime = Time.time + attackSwitchTime;
        location = transform.position;
        isInvincible = true;

        // UdonBoss の倒されたときのイベントに登録
        UdonBoss.UdonDefeated += OnUdonDefeated;
    }

    protected override void Move()
    {
        // 上下に移動するパターン
        rb2D.linearVelocity = new Vector2(0, Mathf.Sin(Time.time * speed));

    }

    protected override void SwitchAttackPhase()
    {
        if (!isUdonDefeated)
        {
            if (Time.time > nextSwitchTime)
            {
                attackPhase = (attackPhase + 1) % 2; // 攻撃フェーズを切り替える
                nextSwitchTime = Time.time + attackSwitchTime;
            }
        }
        else
        {
            
            if (Time.time > nextSwitchTime)
            {
                attackPhase = (attackPhase + 1) % 4; // 攻撃フェーズを切り替える
                nextSwitchTime = Time.time + attackSwitchTime;
            }
        }
    }

    protected override void FirePattern()
    {
        // UdonBoss が倒されていない場合は通常の攻撃パターンを実行
        if (!isUdonDefeated)
        {
            switch (attackPhase)
            {
                case 0:
                    FireAllDirections();
                    break;
                case 1:
                    Fire5Way();
                    break;
            }
        }
        else
        {
            // UdonBoss が倒された後の攻撃パターンを実行
            switch (attackPhase)
            {
                case 0:
                    FireRadial();
                    FireAllDirections();
                    break;
                case 1:
                    FireRing();
                    break;
                case 2:
                    FireRadial();
                    Fire3WayAimed();
                    break;
                case 3:
                     FireWinder();
                    break;
            }
        }
    }

    void FireStraight()
    {
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = projectileSpawn.right * -10f;

            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return new WaitForSeconds(0.05f);
        }
    }

    void Fire3WayAimed()
    {
        int bulletCount = 3;
        float angleStep = 15f; // 弾の角度間隔（例えば15度ずつ）
        float startAngle = -angleStep * (bulletCount - 1) / 2; // 開始角度

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = playerObject.transform.position; // プレイヤーの現在位置を取得

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * (playerPosition - (Vector2)projectileSpawn.position).normalized;

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * 10f;

            float Bangle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Bangle);
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
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * 5f;

            float Bangle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Bangle);

            Bangle += angleStep;
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
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * 5f;

            float Bangle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Bangle);

            angle += angleStep;
        }
    }

    void FireRing()
    {
        int bulletCount = 36; // 弾の数
        float angleStep = 10f; // 弾の角度間隔
        float startAngle = 0f; // 開始角度
        float radius = 1.5f; // 円の半径
        float bulletSpeed = 5.0f;
        float rotationSpeed = 200f; // 回転速度

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

            Vector3 spawnPosition = projectileSpawn.position + new Vector3(x, y, 0);
            Vector3 direction = Quaternion.Euler(0, 0, angle + Time.time * rotationSpeed) * Vector3.right;

            GameObject bullet = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * bulletSpeed;

            float Bangle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Bangle);
        }
    }

    void FireWinder()
    {
        float winderAngleRange = 45f; // 左右の最大振り幅
        float winderAngleSpeed = 2f; // 振り幅の変化速度
        float baseAngle = 180f; // 基本の角度

        StartCoroutine(FireWinderPattern(winderAngleRange, winderAngleSpeed, baseAngle));
    }

    IEnumerator FireWinderPattern(float angleRange, float angleSpeed, float baseAngle)
    {
        float bulletSpeed = 5.0f;
        while (true)
        {
            // 振り幅の角度を計算
            float winderAngle = baseAngle + angleRange * Mathf.Sin(Time.time * angleSpeed);
            Vector3 direction = new Vector3(Mathf.Cos(winderAngle * Mathf.Deg2Rad), Mathf.Sin(winderAngle * Mathf.Deg2Rad), 0);

            GameObject bullet = Instantiate(projectilePrefab2, projectileSpawn.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction * bulletSpeed;

            float Bangle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, Bangle);

            yield return new WaitForSeconds(0.5f); // 発射間隔
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!isUdonDefeated) // UdonBoss が倒されていない場合は無敵
        {
            isInvincible = true; // 無敵にする
        }

        if (!isInvincible) // 無敵でない場合にのみダメージを受ける
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
            else
            {
                SEManager.Instance.PlaySE("EnemyDamage");
            }
        }

        // UpdateHealthUI();
    }


    // UdonBoss が倒されたときの処理
    private void OnUdonDefeated()
    {
        isUdonDefeated = true;
        fireRate = 0.5f;
        speed = 5f;
        // ここでSobaBoss の行動パターンを変化させる処理を追加する
        // 例えば攻撃パターンを変更したり、新しい攻撃を追加したりする

        // ダメージを通常に戻す
        isInvincible = false;
    }

    void Fire5Way()
    {
        int bulletCount = 5;
        float angleStep = 10f; // 弾の角度間隔
        float bulletSpeed = 5f;
        float startAngle = -angleStep * (bulletCount - 1) / 2; // 開始角度

        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, startAngle + i * angleStep);
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, rotation);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.right * -bulletSpeed;
        }
    }

    protected override void Die()
    {
        base.Die();
        // UdonBoss の倒されたイベントから解除
        UdonBoss.UdonDefeated -= OnUdonDefeated;
    }
}

