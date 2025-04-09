using UnityEngine;
using System.Collections;

public class ExplodingBullet : MonoBehaviour
{
    //[SerializeField] float speed = 5f;          // �e�̑��x
    [SerializeField] int damage = 5;                // �e�̃_���[�W
    public GameObject splitProjectilePrefab;
    public float explosionDelay = 2f;
    public float splitSpeed = 5f;
    private Rigidbody2D rb2D;         // Rigidbody2D �R���|�[�l���g
    private Renderer rend;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rend = GetComponent<Renderer>();
        StartCoroutine(ExplodeAfterDelay());
        //rb2D.velocity = transform.right * speed; //���i������
    }

    void Update()
    {
        // �J�����̋��E���擾
        if (!IsVisible())
        {
            StartCoroutine(ExplodeImmediately());
        }
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0.01 && screenPoint.x <= 0.99 && screenPoint.y >= 0.01 && screenPoint.y <= 0.99;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("damege"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().TakeDamage(damage);
            // �v���C���[�Ƀ_���[�W��^���鏈����ǉ�
            StartCoroutine(ExplodeImmediately());
        }
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
        Destroy(gameObject); // ���̒e��j��
    }

    IEnumerator ExplodeImmediately()
    {
        Explode();
        yield return null;
        Destroy(gameObject); // ���̒e��j��
    }

    private void Explode()
    {
        int bulletCount = 6;
        float angleStep = 360f / bulletCount;
        float angle = 0f;
        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);
            Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector3 bulletMoveDirection = (bulletVector - transform.position).normalized;
            GameObject bullet = Instantiate(splitProjectilePrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * splitSpeed;
            angle += angleStep;
        }
    }
}
