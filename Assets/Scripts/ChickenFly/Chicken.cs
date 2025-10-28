using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Animator animator;
    [SerializeField] float fowardSpeed = 6f;
    float flapForce = 10f;
    float deathCooldown = 0f;

    bool isDead = false;

    bool isFlap = false;

    public bool godMode = false;

    private Vector2 initialState;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        initialState = transform.position;
    }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            gameManager.GameOver();
            Reset();
        }
        else
        {
            if(Time.timeScale == 0f)
            {
                return;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    isFlap = true;
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        Vector2 velocity = _rigidbody.velocity;
        velocity.x = fowardSpeed;

        if (isFlap)
        {
            velocity.y += flapForce;
            isFlap = false;
        }
        _rigidbody.velocity = velocity;

        float angle = Mathf.Clamp((_rigidbody.velocity.y * 10f), -90, 90);
        float lerpAngle = Mathf.Lerp(transform.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * 5);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (godMode)
            return;

        if (isDead)
            return;

        isDead = true;
        deathCooldown = 1f;

        animator.SetInteger("isDie", 1);
    }

    public void Reset()
    {
        transform.position = initialState;
        isDead = false;
        animator.Play("Idle");
        animator.SetInteger("isDie", 0);
    }

}
