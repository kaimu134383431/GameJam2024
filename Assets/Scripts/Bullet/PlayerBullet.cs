using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;         // �e�̑��x
    [SerializeField] float lifetime = 2f;       // �e�̎���
    [SerializeField] int damage = 5;                // �e�̃_���[�W

    private Rigidbody2D rb2D;           // Rigidbody2D�R���|�[�l���g
    private Renderer rend;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Rigidbody2D�R���|�[�l���g���擾
        rb2D.linearVelocity = transform.right * speed; // �e���E�����Ɉړ�������
        rend = GetComponent<Renderer>();

        Destroy(gameObject, lifetime); // �w�肵��lifetime��ɒe��j�󂷂�
    }

    void Update()
    {
        // �J�����̋��E���擾
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �e�����̃I�u�W�F�N�g�ɏՓ˂����Ƃ��̏���
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            // �����ɓG�Ƀ_���[�W��^���鏈����ǉ�
            Destroy(gameObject); // �Փ˂�����e��j�󂷂�
        }
    }
}
