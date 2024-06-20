using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        firePoint=this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(firePoint.position.x<-20){
            Destroy(gameObject); 
        }
    }
}
