using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class BossEnemy : Enemy
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform projectileSpawn;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected GameObject forcedScrollPrefab;
    protected float nextFire = 0f;
    protected int attackPhase = 0; // 現在の攻撃フェーズ
    protected float attackSwitchTime = 5f; // 各攻撃フェーズの持続時間
    protected float nextSwitchTime = 0f; // 次の攻撃フェーズに移る時間
    protected Vector3 location;
    
    //勝手に追加
    [SerializeField] protected float waitingTime = 1f;
    public bool isWait = true; //プレイヤーがボスに到達したらfalse
    protected bool isInvincible;

    public static event Action<BossEnemy> OnAnyBossDefeated; // どのボスでも撃破時に通知
    public event Action<BossEnemy> OnBossDefeated;  // 個別ボス撃破イベント（ForcedScrollMultiが使う）

    // スクロール到達時に呼ぶ
    public virtual void OnScrollReached()
    {
        isInvincible = false;  // 無敵解除
        isWait = false;        // 行動開始
    }

    protected virtual void FixedUpdate()
    {
        Move();

        if (isWait) return;

        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            return;
        }

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
        if (collision.gameObject.CompareTag("damege"))
        {
            // プレイヤーにダメージを与える処理を追加
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }

    protected override void Die()
    {
        SEManager.Instance.PlaySE("BossDead");

        // 通知を送る
        OnAnyBossDefeated?.Invoke(this);

        DropItem();
        // 弾幕をすべて破棄する
        DestroyAllProjectiles();
        if (forcedScrollPrefab != null) Instantiate(forcedScrollPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        //HideHealthBar();
        base.Die();
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
