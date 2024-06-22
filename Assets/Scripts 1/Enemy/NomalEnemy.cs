using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] float moveDirection = 1f;
    [SerializeField] GameObject homingBulletPrefab; // ���@�_���e�̃v���n�u
    [SerializeField] Transform bulletSpawn;         // �e�̐����ʒu
    [SerializeField] float fireRate = 1f;           // �ˌ��̊Ԋu
    private float nextFire = 0f;          // ���ɒe�����Ă鎞��

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        Move();
        Shoot();
    }

    protected override void Move()
    {
        rb2D.velocity = new Vector2(moveDirection * speed, rb2D.velocity.y);
    }

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(homingBulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletSpawn.right * -10f; // projectileSpawn�̉E�����ɑ��x��^����
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
