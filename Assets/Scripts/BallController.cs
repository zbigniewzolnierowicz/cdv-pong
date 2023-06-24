using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private const float MinSpeedRange = 2f;
    private const float MaxSpeedRange = 20f;
    
    [Range(MinSpeedRange, MaxSpeedRange)]
    public float minSpeed = MinSpeedRange;

    [Range(MinSpeedRange, MaxSpeedRange)]
    public float maxSpeed = MaxSpeedRange;
    
    private Vector2 _vel = Vector2.zero;

    public GameController gameController;

    public bool serveAutomatically = false;
    
    public InputAction serveBallAction;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("[INFO] Instantiating ball");
        if (maxSpeed < minSpeed)
        {
            throw new Exception("Max speed has to be bigger than min speed!");
        }
        
        _rb = GetComponent<Rigidbody2D>();
        serveBallAction.Enable();
        ServeBall();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _rb.velocity = Vector2.Reflect(_vel, col.contacts[0].normal);
        _vel = _rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"[INFO] Entered the following trigger: {col.name}");
        
        ResetPosition();
        gameController.OnScore(col.tag);
        StartCoroutine(BallServeCoroutine());
    }

    private IEnumerator BallServeCoroutine(int seconds = 1)
    {
        if (serveAutomatically)
        {
            yield return new WaitForSecondsRealtime(seconds);
        }
        else
        {
            yield return new WaitUntil(() => serveBallAction.triggered);
        }
        ServeBall();
    }

    /// <summary>
    /// Randomizes sides
    /// </summary>
    /// <returns>-1f if left, 1f if right</returns>
    float GetRandomLeftRight()
    {
        var isLeft = Random.value % 2 == 0;
        return isLeft ? -1f : 1f;
    }

    Vector2 CalculateVelocity(Vector2 direction, float speed)
    {
        return direction * speed;
    }
    
    void ServeBall()
    {
        // Generate random ball start direction
        var x = GetRandomLeftRight();
        var y = Random.Range(-1f, 1f);
        var direction = new Vector2(x, y).normalized;
        var speed = Random.Range(minSpeed, maxSpeed);
        var newVelocity = CalculateVelocity(direction, speed);
        
        // Apply initial velocity
        _vel = newVelocity;
        _rb.velocity = newVelocity;
    }

    void ResetPosition()
    {
        _rb.velocity = Vector2.zero;
        _rb.position = Vector2.zero;
    }
}
