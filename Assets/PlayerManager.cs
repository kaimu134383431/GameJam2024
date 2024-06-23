using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] float speed = 5f; // プレイヤーの移動速度
    [SerislizeField] GameObject bulletPrefab; // 弾のプレハブ
    [SerislizeField] Transform bulletSpawn; // 弾の生成位置
    [SerislizeField] float fireRate = 0.5f; // 射撃の間隔
    [SerislizeField] int maxHealth = 3; // プレイヤーの最大ライフ

    private Rigidbody2D rb2D; //Rigidbody2D コンポーネント
    private float nextFire = 0f; // 次に弾を撃てる時間
    private int currentHealth; // 現在のライフ

    void Start()
    {
        rb2D = GetComponent<Rugidbody2D>(); // Rigidbody2D コンポーネントを取得
        currentHealth = maxHealth; // ゲーム開始時にライフを最大に設定
    }

    void Update()
    {
        Move(); // プレイヤーの移動
        Shoot(); // プレイヤーの射撃
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
        // プレイヤーが死んだときの処理
        Debug.Log("Player died!");
        Destroy(gameObject);
    }

}