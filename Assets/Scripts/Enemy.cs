using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{ 
    [SerializeField] protected float speed = 2f; 
    [SerializeField] protected int damage = 1; 
    protected Rigidbody2D rb2D; 
    protected int currentHealth; 
    protected virtual void Start() 
    { 
        rb2D = GetComponent<Rigidbody2D>(); 
        currentHealth = 3; // ここでデフォルトのライフを設定します 
    } 
    protected abstract void Move(); 
    protected abstract void Shoot();
    public void TakeDamage(int amount)
    { 
        currentHealth -= amount; 
        if (currentHealth <= 0) 
        { 
            Die();
        } 
    }
    protected void Die() 
    { 
        //DropItem(); 
        Destroy(gameObject); 
    } 
    /* 
    private void DropItem() 
    { 
        if (dropItems.Length > 0) 
        { 
            int randomIndex = Random.Range(0, dropItems.Length); 
            Instantiate(dropItems[randomIndex], transform.position, Quaternion.identity); 
        } 
    }*/ 
}
