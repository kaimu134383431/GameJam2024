using UnityEngine;
using UnityEngine.UI;

public abstract class BossEnemy : Enemy
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform projectileSpawn;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected GameObject forcedScrollPrefab;
    protected float nextFire = 0f;
    [SerializeField] protected Slider healthSliderPrefab; // 体力バーのPrefab
    protected Slider healthSliderInstance; // 体力バーのインスタンス
    protected int attackPhase = 0; // 現在の攻撃フェーズ
    protected float attackSwitchTime = 5f; // 各攻撃フェーズの持続時間
    protected float nextSwitchTime = 0f; // 次の攻撃フェーズに移る時間
    protected Vector3 location;
    protected bool isInvincible;

    //勝手に追加
    [SerializeField] protected float waitingTime = 1f;
    public bool isWait = true; //プレイヤーがボスに到達したらfalse

    protected virtual void FixedUpdate()
    {
        if (isWait) return;
        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            return;
        }
        RectTransform rt = healthSliderInstance.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(325, 50); // 幅と高さを設定
        rt.anchorMin = new Vector2(1, 0.5f); // 右側に寄せる
        rt.anchorMax = new Vector2(1, 0.5f); // 右側に寄せる
        rt.pivot = new Vector2(1, 0.5f); // 中心を右端に設定
        rt.anchoredPosition = new Vector2(-10, 195); // 少し左にオフセット
        UpdateHealthUI();
        Move();
        Shoot();
        SwitchAttackPhase();
    }

    protected virtual void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            FirePattern();
        }
    }

    protected virtual void FirePattern()
    {
        // 攻撃パターンを実装 (子クラスでオーバーライド)
    }

    protected virtual void SwitchAttackPhase()
    {
        if (Time.time > nextSwitchTime)
        {
            attackPhase = (attackPhase + 1) % 4; // 攻撃フェーズを切り替える
            nextSwitchTime = Time.time + attackSwitchTime;
        }
    }

    void OnTrigerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーにダメージを与える処理を追加
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }

    protected override void Die()
    {
        SEManager.Instance.PlaySE("BossDead");
        DropItem();
        Destroy(healthSliderInstance.gameObject); // 体力バーのインスタンスを破棄
        // 弾幕をすべて破棄する
        DestroyAllProjectiles();
        if (forcedScrollPrefab != null) Instantiate(forcedScrollPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        HideHealthBar();
    }

    protected void UpdateHealthUI()
    {
        float normalizedHealth = (float)health / maxHealth;
        if (healthSliderInstance != null)
        {
            healthSliderInstance.value = normalizedHealth; // 体力バーの値を更新

            /*// 体力バーのサイズを小さくして右寄せ
            RectTransform rt = healthSliderInstance.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(325, 50); // 幅と高さを設定
            rt.anchorMin = new Vector2(1, 0.5f); // 右側に寄せる
            rt.anchorMax = new Vector2(1, 0.5f); // 右側に寄せる
            rt.pivot = new Vector2(1, 0.5f); // 中心を右端に設定
            rt.anchoredPosition = new Vector2(-10, 195); // 少し左にオフセット*/
            healthSliderInstance.interactable = false;
        }
    }

    public void ShowHealthBar()
    {
        if (healthSliderInstance != null)
        {
            healthSliderInstance.gameObject.SetActive(true);
        }
    }

    public void HideHealthBar()
    {
        if (healthSliderInstance != null)
        {
            healthSliderInstance.gameObject.SetActive(false);
        }
    }

    void DestroyAllProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }

    public void setInvincible(bool inv)
    {
        isInvincible = inv;
    }
}
