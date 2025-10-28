using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    private static readonly int MoveDown = Animator.StringToHash("MoveDown");
    private static readonly int MoveLeft = Animator.StringToHash("MoveLeft");
    private static readonly int MoveRight = Animator.StringToHash("MoveRight");
    private static readonly int Idle = Animator.StringToHash("Idle");

    protected Animator animator;

    private int currentState;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        currentState = -1;
    }

    public void Move(Vector2 obj)
    {
        float deadZone = 0.1f;

        bool isMoving = obj.magnitude > deadZone; // manitued�� ������ ����, ũ�⸦ ��Ÿ����.
                                                  // �� �� ������ ���� �Ÿ��� ��� �� �� ���
                                                  // isMoving�� ���� �����ӿ��� ĳ���ͱ� �̵� ������ ���θ� �Ǵ�
        animator.SetBool(IsMoving, isMoving);

        if (isMoving)
        {
            if (obj.x > 0)
            {
                animator.Play(MoveRight);
            }
            else if (obj.x < 0)
            {
                animator.Play(MoveLeft);
            }
            else
            {
                if (obj.y > 0)
                {
                    animator.Play(MoveUp);
                }
                else if (obj.y < 0)
                {
                    animator.Play(MoveDown);
                }
            }
        }
        else
        {
            animator.Play(Idle);
        }

        //// animator.SetBool(IsMoving, obj.magnitude > .1f);
        //animator.SetFloat("MoveX", obj.x); // ���� obj�� Animator Controller�� ����
        //animator.SetFloat("MoveY", obj.y); // ��Ȯ�� �̵� ���⿡ �´� �ִϸ��̼� ���

        //int newState;
        //if (Mathf.Abs(obj.x) > Mathf.Abs(obj.y) // ���� ��
        //    newState = obj.x > 0 ? MoveRight : MoveLeft; // 0���� ũ�� Right, ������ Left
        //else if (Mathf.Abs(obj.y) > 0)
        //    newState = obj.y > 0 ? MoveUp : MoveDown;


    }
}
