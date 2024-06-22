using UnityEngine;
using System.Collections;

public class WhiteNoodleBoss : BossEnemy
{
    protected override void Move()
    {
        // 上下に移動するパターン
        rb2D.velocity = new Vector2(0, Mathf.Sin(Time.time * speed));
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


}
