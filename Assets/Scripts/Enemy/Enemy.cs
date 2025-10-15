using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected int probability = 100;
    //public int maxHealth;

    [SerializeField] GameObject[] itemPrefabs;

    protected Rigidbody2D rb2D;

    protected virtual void Start()
    {
        //maxHealth = health;
        rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            SEManager.Instance.PlaySE("EnemyDamage");
        }
    }

    protected virtual void Die()
    {
        DropItem();
        Destroy(gameObject);
    }

    protected void DropItem()
    {
        for (int itemIndex = 0; itemIndex < itemPrefabs.Length; itemIndex++)
        {
            int dropChance = Random.Range(0, 100);
            if (dropChance < probability) // 50%の確率でアイテムをドロップする
            {
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Vector3 dropPosition = transform.position + randomOffset;
                GameObject item = Instantiate(itemPrefabs[itemIndex], transform.position, Quaternion.identity);


                FloatingItem floatingItem = item.GetComponent<FloatingItem>();
                if (floatingItem != null)
                {
                    floatingItem.SetTargetPosition(dropPosition);
                }
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }


    protected abstract void Move();
}
