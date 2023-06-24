using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float height = 10f;
    private float maxVertical;
    public InputAction move;
    
    void Start()
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log("[INFO] Instancing player controller");
            Debug.Log("[INFO] Enabling movement");
        }

        maxVertical = (height / 2) - (this.transform.localScale.y / 2);
        
        move.Enable();
    }

    void Update()
    {
        // Calculate position
        var yAxis = move.ReadValue<float>();
        var position = this.transform.position;
        position += Vector3.up * (yAxis * Time.deltaTime * speed);

        // Check if within bounds
        if (position.y < -maxVertical || position.y > maxVertical) return;
        
        // Set position
        this.transform.position = position;
    }
}
