using UnityEngine;
using System.Collections;

public class BossEnemy : Enemy
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float fireRate = 1f;
    private float nextFire = 0f;

    private int attackPhase = 0;        // ���݂̍U���t�F�[�Y
    private float attackSwitchTime = 5f; // �e�U���t�F�[�Y�̎�������
    private float nextSwitchTime = 0f;   // ���̍U���t�F�[�Y�Ɉڂ鎞��

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
        // �{�X�̓��ʂȈړ��p�^�[��������
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
            attackPhase = (attackPhase + 1) % 3; // �U���t�F�[�Y��؂�ւ���
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
            bullet.GetComponent<Rigidbody2D>().velocity = projectileSpawn.right * -10f; // projectileSpawn�̉E�����ɑ��x��^����

            yield return new WaitForSeconds(0.05f); // �e�̔��ˊԊu
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
            // �v���C���[�Ƀ_���[�W��^���鏈����ǉ�
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }
}
