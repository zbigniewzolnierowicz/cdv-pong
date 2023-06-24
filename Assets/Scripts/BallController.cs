using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    private const float MinSpeedRange = 2f;
    private const float MaxSpeedRange = 20f;

    [Range(MinSpeedRange, MaxSpeedRange)] public float minSpeed = MinSpeedRange;

    [Range(MinSpeedRange, MaxSpeedRange)] public float maxSpeed = MaxSpeedRange;

    public bool serveAutomatically = false;
    public bool serveRandomly = false;

    public InputAction serveBallAction;

    public ScoreController scoreController;
    private bool _leftServing = true;
    private Rigidbody2D _rb;

    private Vector2 _vel = Vector2.zero;

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
        OnScore(col.tag);
        StartCoroutine(BallServeCoroutine());
    }

    private IEnumerator BallServeCoroutine(int seconds = 1)
    {
        // Serve the ball automatically
        if (serveAutomatically)
        {
            yield return new WaitForSecondsRealtime(seconds);
        }
        // Serve the ball manually
        else
        {
            yield return new WaitUntil(() => serveBallAction.triggered);
        }

        ServeBall();
    }

    /// <summary>
    /// Converts a boolean to direction
    /// </summary>
    /// <returns>-1f if left, 1f if right</returns>
    float GetLeftRight(Func<bool> isLeft)
    {
        return isLeft() ? -1f : 1f;
    }

    bool Randomize()
    {
        return Random.value % 2 == 0;
    }

    bool ServeToLoser()
    {
        return _leftServing;
    }

    Vector2 CalculateVelocity(Vector2 direction, float speed)
    {
        return direction * speed;
    }

    void ServeBall()
    {
        var x = serveRandomly
            // Generate random ball start direction
            ? GetLeftRight(Randomize)
            // Serve to the loser
            : GetLeftRight(ServeToLoser);

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

    void OnScore(string side)
    {
        switch (side)
        {
            case "Left Trigger":
                scoreController.OnRightScore();
                _leftServing = true;
                break;
            case "Right Trigger":
                scoreController.OnLeftScore();
                _leftServing = false;
                break;
        }
    }
}