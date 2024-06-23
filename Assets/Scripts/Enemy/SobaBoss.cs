using UnityEngine;
using System.Collections;
public class SobaBoss : BossEnemy
{
    private bool isUdonDefeated = false; // UdonBoss が倒されたかどうかのフラグ

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

        // UdonBoss の倒されたときのイベントに登録
        UdonBoss.UdonDefeated += OnUdonDefeated;
    }

    protected override void Move()
    {
        // 上下に移動するパターン
        rb2D.velocity = new Vector2(0, Mathf.Sin(Time.time * speed));
    }

    protected override void FirePattern()
    {
        // UdonBoss が倒されていない場合は通常の攻撃パターンを実行
        if (!isUdonDefeated)
        {
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
        else
        {
            // UdonBoss が倒された後の攻撃パターンを実行
            // 例えば新しい攻撃パターンのメソッドをここで呼び出す
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

    // UdonBoss が倒されたときの処理
    private void OnUdonDefeated()
    {
        isUdonDefeated = true;
        // ここでSobaBoss の行動パターンを変化させる処理を追加する
        // 例えば攻撃パターンを変更したり、新しい攻撃を追加したりする

        // ダメージを通常に戻す
        isInvincible = false;
    }

    protected override void Die()
    {
        base.Die();
        // UdonBoss の倒されたイベントから解除
        UdonBoss.UdonDefeated -= OnUdonDefeated;
    }
}

