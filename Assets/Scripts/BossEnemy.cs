using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    protected override void Move()
    {
        float frequency=1.0f,amplitude=1.0f;
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        // Rigidbody2Dの速度を更新
        rb2D.velocity = new Vector2(0,yOffset);
    }
    protected override void Shoot()
    {

    }
}
