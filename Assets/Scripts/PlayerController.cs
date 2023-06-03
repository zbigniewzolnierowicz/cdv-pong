using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float maxVertical = 4.5f;
    public InputAction move;
    
    void Start()
    {
        Debug.Log("[INFO] Instancing player controller");
        Debug.Log("[INFO] Enabling movement");
        move.Enable();
    }

    void Update()
    {
        var yAxis = move.ReadValue<float>();
        var position = this.transform.position;
        position += Vector3.up * (yAxis * Time.deltaTime * speed);
        
        if (position.y > -maxVertical && position.y < maxVertical)
        {
            this.transform.position = position;
        }
    }
}