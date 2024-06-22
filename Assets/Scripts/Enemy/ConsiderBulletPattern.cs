using UnityEngine;
using System.Collections;

public class ConsiderBulletPattern : BossEnemy
{
    public float offset = 10f;
    public GameObject explodePrefab;
    protected override void Move()
    {
        // 上下に移動するパターン
        rb2D.velocity = new Vector2(0, offset*Mathf.Sin(Time.time * speed));
    }

    protected override void FirePattern()
    {
        Fire5WayAimedExplode();
        //FireExplodeRadial();
        //Fire3WayAimed();
        //Fire5WayAimed();
        //FireRandomPositions();
        /*
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
        }*/
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

    void FireRandom()
    {
        int bulletCount = 10;
        float minAngle = -30f;
        float maxAngle = 30f;

        for (int i = 0; i < bulletCount; i++)
        {
            float randomAngle = Random.Range(minAngle, maxAngle);
            Quaternion bulletRotation = Quaternion.Euler(0f, 0f, randomAngle);
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, bulletRotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * -10f;
        }
    }

    void FireRandomPositions()
    {
        int bulletCount = 15;
        float radius = 3f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            Vector3 spawnPosition = projectileSpawn.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
            GameObject bullet = Instantiate(projectilePrefab, spawnPosition, projectileSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * -8f;
        }
    }

    void FireHoming()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Vector3 playerPosition = playerObject.transform.position;
            Vector2 direction = (playerPosition - projectileSpawn.position).normalized;

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 5.0f;
        }
    }

    void Fire3Way()
    {
        int bulletCount = 3;
        float angleStep = 15f; // 弾の角度間隔
        float bulletSpeed = 5f;
        float startAngle = -angleStep * (bulletCount - 1) / 2; // 開始角度

        for (int i = 0; i < bulletCount; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, startAngle + i * angleStep);
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
        }
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
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
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
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f;
        }
    }



    void Fire5WayAimed()
    {
        int bulletCount = 5;
        float angleStep = 10f; // 弾の角度間隔（例えば10度ずつ）
        float startAngle = -angleStep * (bulletCount - 1) / 2; // 開始角度

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = playerObject.transform.position; // プレイヤーの現在位置を取得

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * (playerPosition - (Vector2)projectileSpawn.position).normalized;

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 5f;
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
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
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

            GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            yield return new WaitForSeconds(0.5f); // 発射間隔
        }
    }

    void FireExploding()
    {
        GameObject explodingBullet = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        ExplodingBullet explodingScript = explodingBullet.AddComponent<ExplodingBullet>();
        explodingScript.splitProjectilePrefab = projectilePrefab; // 分裂弾のプレハブを設定
    }

    void FireExplodeRadial()
    {
        int bulletCount = 12;
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            // 炸裂弾を生成する
            GameObject bullet = Instantiate(explodePrefab, projectileSpawn.position, projectileSpawn.rotation);

            // 弾の飛ぶ方向を計算する
            float bulletDirX = Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulletMoveDirection = new Vector3(bulletDirX, bulletDirY, 0);

            // 弾に速度を与える
            bullet.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * 5f;

            angle += angleStep;
        }
    }

    void Fire5WayAimedExplode()
    {
        int bulletCount = 5;
        float angleStep = 10f; // 弾の角度間隔（例えば10度ずつ）
        float startAngle = -angleStep * (bulletCount - 1) / 2; // 開始角度

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPosition = playerObject.transform.position; // プレイヤーの現在位置を取得

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * (playerPosition - (Vector2)projectileSpawn.position).normalized;

            GameObject bullet = Instantiate(explodePrefab, projectileSpawn.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 5f;
        }
    }

    void FireSingleDirection(Vector2 direction)
    {
        GameObject bullet = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 5f;
    }






}
