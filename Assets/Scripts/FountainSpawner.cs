using UnityEngine;

public class FountainSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;   // �������̃v���n�u
    [SerializeField] private float spawnInterval = 0.2f; // �����Ԋu
    [SerializeField] private Vector2 launchSpeedRange = new Vector2(3f, 6f); // �����x�͈̔�
    [SerializeField] private float angleRange = 45f;  // ���E�����̊p�x�����_�����i�x�j
    [SerializeField] private float destroyY = -6f;    // �폜���鍂��
    [SerializeField] private float gravity = 1f;      // �d�͔{��

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnProjectile();
            timer = 0f;
        }
    }

    void SpawnProjectile()
    {
        if (prefabs.Length == 0) return;

        // �����_���ȃv���n�u��I��
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        // Rigidbody2D�ŕ���������ݒ�
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb == null) rb = obj.AddComponent<Rigidbody2D>();

        rb.gravityScale = gravity;

        // �����_���Ȋp�x�ŏ�����ɑł��グ
        float angle = Random.Range(-angleRange, angleRange);
        float speed = Random.Range(launchSpeedRange.x, launchSpeedRange.y);
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;

        rb.linearVelocity = direction * speed;

        // ��莞�Ԍ�܂��͉�ʊO�ō폜
        obj.AddComponent<AutoDestroy>().SetDestroyY(destroyY);
    }
}

public class AutoDestroy : MonoBehaviour
{
    private float destroyY = -6f;
    private float lifeTime = 10f;
    private float timer = 0f;

    public void SetDestroyY(float y)
    {
        destroyY = y;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (transform.position.y < destroyY || timer > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
