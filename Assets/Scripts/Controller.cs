using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("joystick button 1")) {
            Debug.Log ("ボタンが押された111111111111111111");
        }
        
        if (Input.GetKey("joystick button 3")) {
            Debug.Log ("ボタンが押された33333333333333333");
        }
        
        if (Input.GetKey("joystick button 2")) {
            Debug.Log ("ボタンが押された222222222222222222");
        }
        
        if (Input.GetKey("joystick button 4")) {
            Debug.Log ("ボタンが押された44444444444444444");
        }
        
        if (Input.GetKey("joystick button 5")) {
            Debug.Log ("ボタンが押された555555555555555555");
        }
        
    }
}
