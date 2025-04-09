using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //[SerializeField] float speed = 5f;          // �e�̑��x
    [SerializeField] int damage = 5;                // �e�̃_���[�W
    private Rigidbody2D rb2D;         // Rigidbody2D �R���|�[�l���g
    private Renderer rend;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();

        //rb2D.velocity = transform.right * speed; //���i������

    }

    void Update()
    {
        // �e�̐i�s�����ɒe�̐擪����������
        if (rb2D.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

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
        if (other.CompareTag("damege"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().TakeDamage(damage);
            // �����Ƀv���C���[�Ƀ_���[�W��^���鏈����ǉ�
            Destroy(gameObject); // �Փ˂�����e��j�󂷂�
        }

    }
}
