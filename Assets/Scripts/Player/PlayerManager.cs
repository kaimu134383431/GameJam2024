using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [SerializeField] GameObject damcol;
    [SerializeField] float speed = 3f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibleDuration = 0.8f; // 無敵時間（秒）

    private Rigidbody2D rb2D;
    private float nextFire = 0f;
    private int currentHealth;
    private Camera mainCamera;
    private float halfWidth;
    private float halfHeight;
    private bool isInvincible = false; // 無敵状態かどうか
    private float invincibleTimer = 0f; // 無敵時間のカウント
    private SpriteRenderer spriteRenderer; // プレイヤーのスプライトレンダラー


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
            halfHeight = sr.bounds.extents.y;
        }
        else
        {
            halfWidth = 0.5f;
            halfHeight = 0.5f;
        }
    }

    void Update()
    {
        Move();
        Shoot();
        ClampPosition();
        HandleInvincibility();  // 無敵時間を管理
        Moveitemcollider();
    }
    void Moveitemcollider()
    {
        damcol.transform.position = this.transform.position;

    }
    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        if(moveX == 0 && moveY == 0)
        {
            moveX = Input.GetAxis("DPadHorizontal");
            moveY = Input.GetAxis("DPadVertical");
        }
        float tmpspeed = speed;
        if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)||Input.GetKey("joystick button 0")) tmpspeed /= 2;
        Vector2 movement = new Vector2(moveX, moveY);
        rb2D.velocity = movement.normalized * tmpspeed;
    }

    void Shoot()
    {
        if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Return) || Input.GetKey("joystick button 2")) && Time.time > nextFire)
        {
            SEManager.Instance.PlaySE("PlayerShoot");
            nextFire = Time.time + fireRate;
            Vector3 newPosition = bulletSpawn.position + new Vector3(1, 0.25f, 0);
            Instantiate(bulletPrefab, newPosition, bulletSpawn.rotation);
        }
    }

    void ClampPosition()
    {
        Vector3 position = transform.position;
        Vector3 minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.transform.position.z));
        position.x = Mathf.Clamp(position.x, minScreenBounds.x + halfWidth, maxScreenBounds.x - halfWidth);
        position.y = Mathf.Clamp(position.y, minScreenBounds.y + halfHeight - 0.5f, maxScreenBounds.y - halfHeight);
        transform.position = position;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Takedamage called");
        if (isInvincible) return;  // 無敵時間中はダメージを無効化
        SEManager.Instance.PlaySE("PlayerDamage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log("Takedamage called22222");
            StartInvincibility();  // ダメージを受けたら無敵状態にする

        }
    }

    void Die()
    {
        SEManager.Instance.PlaySE("PlayerDead");
        Debug.Log("Player died!");
        Destroy(gameObject);
        GameOver();
    }

    void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
    }

    void StartInvincibility()
    {
        isInvincible = true;
        invincibleTimer = invincibleDuration;
        // 半透明にする
        Color color = spriteRenderer.color;
        color.a = 0.5f; // 透明度を50%に設定
        spriteRenderer.color = color;
    }

    void HandleInvincibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            // 点滅処理
            spriteRenderer.enabled = Mathf.FloorToInt(invincibleTimer * 10) % 2 == 0;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
                spriteRenderer.enabled = true;  // スプライトを表示状態に戻す
                // 元の透明度に戻す
                Color color = spriteRenderer.color;
                color.a = 1f; // 透明度を元に戻す
                spriteRenderer.color = color;
            }
        }
    }

    // 現在のHPを返すメソッド
    public int GetHealth()
    {
        return currentHealth;
    }

    // 最大HPを返すメソッド
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
