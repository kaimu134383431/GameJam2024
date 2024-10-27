using UnityEngine;
using System.Collections;

public class UdonBoss : BossEnemy
{
    public static event System.Action UdonDefeated; // UdonBoss が倒されたときのイベント

    private float angle = 0f; // 円運動の角度
    private Coroutine fireRandomBulletsCoroutine;


    protected override void Start()
    {
        base.Start();
        nextSwitchTime = Time.time + attackSwitchTime;
        location = transform.position;
        isInvincible = true;

        // 体力バーのインスタンスを生成してCanvasに配置する
        if (healthSliderPrefab != null)
        {
            healthSliderInstance = Instantiate(healthSliderPrefab, new Vector3(0, 200, 0), Quaternion.identity);
            healthSliderInstance.transform.SetParent(GameObject.FindWithTag("Canvas").transform, false);
            healthSliderInstance.value = 1f; // 初期値は最大値で設定
            HideHealthBar();
        }
        else
        {
            Debug.LogError("HealthSliderPrefab not found in Resources.");
        }

        
    }

    protected override void FixedUpdate()
    {
        // 親クラスのFixedUpdateメソッドを呼び出す
        base.FixedUpdate();

        // オフセットを変更する
        RectTransform rt = healthSliderInstance.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-10, 165); // 少し左にオフセット
    }


    protected override void Move()
    {
        // ボスの現在位置を継続する
        if (attackPhase != previousAttackPhase)
        {
            // フェーズが変わった場合のみ、現在の位置を更新
            location = rb2D.position;
            previousAttackPhase = attackPhase;
        }

        switch (attackPhase)
        {
            case 0:
                MoveInCircle();
                break;
            case 1:
                MoveBackAndForth();
                break;
            case 2:
                ZigzagMovement();
                break;
        }
    }

    private int previousAttackPhase = -1;

    void MoveInCircle()
    {
        float radius = 2f; // 円の半径
        float centerX = location.x; // 円の中心のX座標
        float centerY = location.y; // 円の中心のY座標
        angle += speed * Time.deltaTime; // 角度を更新
        float newX = centerX + radius * Mathf.Cos(angle);
        float newY = centerY + radius * Mathf.Sin(angle);
        rb2D.position = new Vector2(newX, newY);
    }

    void MoveBackAndForth()
    {
        float range = 3f;
        float speedModifier = 2f;
        float newX = location.x + Mathf.PingPong(Time.time * speedModifier, range) - range / 2;
        rb2D.position = new Vector2(newX, rb2D.position.y);
    }

    void ZigzagMovement()
    {
        float zigzagSpeed = 3f;
        float zigzagWidth = 2f;
        float newX = location.x + Mathf.Sin(Time.time * zigzagSpeed) * zigzagWidth;
        float newY = location.y + Mathf.Cos(Time.time * zigzagSpeed) * zigzagWidth / 2;
        rb2D.position = new Vector2(newX, newY);
    }




    protected override void SwitchAttackPhase()
    {
        if (Time.time > nextSwitchTime)
        {
            attackPhase = (attackPhase + 1) % 4; // 攻撃フェーズを切り替える
            nextSwitchTime = Time.time + attackSwitchTime;
        }
    }

    protected override void FirePattern()
    {
        if (!GetComponent<Renderer>().isVisible) // ボスが画面外にいる場合は処理を終了する
        {
            return;
        }

        switch (attackPhase)
        {
            case 0:
                FireStraight();
                break;
            case 1:
                FireRadial();
                break;
            case 2:
                if (fireRandomBulletsCoroutine != null)
                {
                    StopCoroutine(fireRandomBulletsCoroutine);
                }
                fireRandomBulletsCoroutine = StartCoroutine(FireRandomBullets());
                break;
            case 3:
                if (fireRandomBulletsCoroutine != null)
                {
                    StopCoroutine(fireRandomBulletsCoroutine);
                }
                FireAllDirections();
                break;
        }
    }

    IEnumerator FireRandomBullets()
    {
        while (true)
        {
            // 画面上部のランダムな位置から弾を発射
            float randomX = Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.8f, 0f)).x, Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.8f, 0f)).x);
            Vector3 spawnPosition = new Vector3(randomX, Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)).y, 0f);
            GameObject bullet = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * 5f; // 画面下方向に速度を設定
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f)); // ランダムな間隔で発射
        }
    }


    public override void TakeDamage(int damage)
    {
        if (!isInvincible)
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
        UpdateHealthUI();
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
            bullet.GetComponent<Rigidbody2D>().velocity = projectileSpawn.right * -10f;
            yield return new WaitForSeconds(0.05f);
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

    protected override void Die()
    {
        base.Die();
        // UdonBoss が倒されたときのイベントを発生させる
        UdonDefeated?.Invoke();
    }
}
