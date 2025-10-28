using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected AnimationHandler animationHandler;

    [SerializeField] private SpriteRenderer ChracterRenderer; // 비주얼 역할 / MainSprite는 보여지는 모습
    [SerializeField] private float Speed = 5f;

    protected Vector2 movementDirection = Vector2.zero;

    public Vector2 MovementDirection { get { return movementDirection; } }
    

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if(animationHandler != null)
            animationHandler.Move(movementDirection);
    }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction) // direction * Speed = 실제 이동량
                                             // 방향 * 속도 = 실제 이동량
    {
        direction = direction * Speed;

        _rigidbody.velocity = direction;
    }
}
