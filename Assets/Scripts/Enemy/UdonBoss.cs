using UnityEngine;
using System.Collections;
public class UdonBoss : BossEnemy
{
    public static event System.Action UdonDefeated; // UdonBoss が倒されたときのイベント

    private float angle = 0f; // 円運動の角度

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

    protected override void Move()
    {
        float radius = 2f; // 円の半径
        float centerX = location.x; // 円の中心のX座標
        float centerY = location.y; // 円の中心のY座標

        angle += speed * Time.deltaTime; // 角度を更新

        float newX = centerX + radius * Mathf.Cos(angle);
        float newY = centerY + radius * Mathf.Sin(angle);

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
