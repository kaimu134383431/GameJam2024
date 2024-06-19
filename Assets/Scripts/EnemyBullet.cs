using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
public class BulletEnemy : Enemy 
{ 
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform projectileSpawn; 
    [SerializeField] private float fireRate = 2f; 
    private float nextFire = 0f; 
    protected override void Start() 
    { 
        base.Start(); 
    } 
    void Update() 
    { 
        Move(); 
        Shoot(); 
    } 
    protected override void Move() 
    { 
        rb2D.velocity = new Vector2(-speed, 0); 
    } 
    protected override void Shoot() 
    { 
        if (Time.time > nextFire) 
        { 
            nextFire = Time.time + fireRate; 
            Instantiate(projectilePrefab, 
            projectileSpawn.position, 
            projectileSpawn.rotation).GetComponent<Rigidbody2D>().velocity = Vector2.down * 5f; 
        } 
    } 
}