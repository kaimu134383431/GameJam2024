using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;

    [SerializeField] GameObject[] itemPrefabs;

    protected Rigidbody2D rb2D;

    protected virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        DropItem();
        Destroy(gameObject);
    }

    void DropItem()
    {
        int dropChance = Random.Range(0, 100);
        for (int itemIndex = 0; itemIndex < itemPrefabs.Length; itemIndex++)
        {
            if (dropChance < 150) // 50%の確率でアイテムをドロップする
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


    protected abstract void Move();
}
