using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f; // �e�̑��x
    [SerializeField] int damage = 1; // �e�̃_���[�W
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = transform.up * speed; // �e��O���ɐi�߂�
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject); // �Փ˂�����e��j�󂷂�
        }*/ //�����̃R�����g�A�E�g/**/��Enemy���ł��Ă������
    }
}