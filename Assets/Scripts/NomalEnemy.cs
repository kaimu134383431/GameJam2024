using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalEnemy : Enemy
{         
    
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private float fireRate = 2f; 
    private float nextFire = 0f; 
    [SerializeField]private Vector2 pointA = new Vector2(-5, 0);   // 開始点の初期値
    [SerializeField]private Vector2 pointB = new Vector2(5, 0);   // 終了点の初期値
    Vector2 target; 
    protected override void Start() 
    { 
        base.Start(); 
        target=pointA;
    } 
    protected override void Move()
    {
        Vector2 direction = (target - rb2D.position).normalized;
        
        // 定速度で移動
        rb2D.velocity = direction * speed;
        
        // ターゲットに近づいたら次のターゲットに切り替え
        if (Vector2.Distance(rb2D.position, target) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
        }
    }
    protected override void Shoot() 
    { 
        if (Time.time > nextFire) 
        { 
            nextFire = Time.time + fireRate; 
            Instantiate(projectilePrefab, 
            this.transform.position, 
            Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f; 
        } 
    } 
} 