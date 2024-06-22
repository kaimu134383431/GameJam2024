using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float speed = 5f;            // プレイヤーの移動速度
    [SerializeField] GameObject bulletPrefab;     // 弾のプレハブ
    [SerializeField] Transform bulletSpawn;       // 弾の生成位置
    [SerializeField] float fireRate = 0.5f;       // 射撃の間隔
    [SerializeField] int maxHealth = 3;           // プレイヤーの最大ライフ

    private Rigidbody2D rb2D;             // Rigidbody2Dコンポーネント
    private float nextFire = 0f;          // 次に弾を撃てる時間
    private int currentHealth;            // 現在のライフ
    private Camera mainCamera;            // メインカメラ
    private float halfWidth;              // プレイヤーの幅の半分
    private float halfHeight;             // プレイヤーの高さの半分

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); // Rigidbody2Dコンポーネントを取得
        currentHealth = maxHealth;          // ゲーム開始時にライフを最大に設定
        mainCamera = Camera.main;           // メインカメラを取得

        // プレイヤーの幅と高さを計算
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
            halfHeight = sr.bounds.extents.y;
        }
        else
        {
            halfWidth = 0.5f; // デフォルト値（必要に応じて調整）
            halfHeight = 0.5f; // デフォルト値（必要に応じて調整）
        }
    }

    void Update()
    {
        Move();                             // プレイヤーの移動
        Shoot();                            // プレイヤーの射撃
        ClampPosition();                    // プレイヤーの位置を画面内に制限
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        float tmpspeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)) tmpspeed /= 2;

        Vector2 movement = new Vector2(moveX, moveY) * tmpspeed;
        rb2D.velocity = movement;
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.Z) && Time.time > nextFire)
        {
            SEManager.Instance.PlaySE("PlayerShoot");
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        }
    }

    void ClampPosition()
    {
        // プレイヤーの位置を取得
        Vector3 position = transform.position;

        // カメラのビューの境界を取得
        Vector3 minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.transform.position.z));

        // プレイヤーの位置を境界内に制限（プレイヤーのサイズを考慮）
        position.x = Mathf.Clamp(position.x, minScreenBounds.x + halfWidth, maxScreenBounds.x - halfWidth);
        position.y = Mathf.Clamp(position.y, minScreenBounds.y + halfHeight, maxScreenBounds.y - halfHeight);

        // 位置を更新
        transform.position = position;
    }

    public void TakeDamage(int amount)
    {
        SEManager.Instance.PlaySE("PlayerDamage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SEManager.Instance.PlaySE("PlayerDead");
        // プレイヤーが死んだときの処理
        Debug.Log("Player died!");
        // ここにゲームオーバーの処理を追加することができます
        Destroy(gameObject);
        GameOver();
    }

    void GameOver()
    {
        // ゲームオーバー処理
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }

}
