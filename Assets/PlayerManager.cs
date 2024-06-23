using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float speed = 5f; // �v���C���[�̈ړ����x
    [SerislizeField] GameObject bulletPrefab; // �e�̃v���n�u
    [SerislizeField] Transform bulletSpawn; // �e�̐����ʒu
    [SerislizeField] float fireRate = 0.5f; // �ˌ��̊Ԋu
    [SerislizeField] int maxHealth = 3; // �v���C���[�̍ő僉�C�t

    private Rigidbody2D rb2D; //Rigidbody2D �R���|�[�l���g
    private float nextFire = 0f; // ���ɒe�����Ă鎞��
    private int currentHealth; // ���݂̃��C�t

    void Start()
    {
        rb2D = GetComponent<Rugidbody2D>(); // Rigidbody2D �R���|�[�l���g���擾
        currentHealth = maxHealth; // �Q�[���J�n���Ƀ��C�t���ő�ɐݒ�
    }

    void Update()
    {
        Move(); // �v���C���[�̈ړ�
        Shoot(); // �v���C���[�̎ˌ�
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY) * speed;
        rb2D.velocity = movement;
    }

    void Shoot()
    {
        if (Input.Get(Keycode.Z) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // �v���C���[�����񂾂Ƃ��̏���
        Debug.Log("Player died!");
        Destroy(gameObject);
    }

}