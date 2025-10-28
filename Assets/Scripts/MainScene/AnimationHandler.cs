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

        bool isMoving = obj.magnitude > deadZone; // manitued는 벡터의 길이, 크기를 나타낸다.
                                                  // 두 점 사이의 직선 거리를 계산 할 때 사용
                                                  // isMoving은 현재 프레임에서 캐릭터기 이동 중인지 여부를 판단
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
        //animator.SetFloat("MoveX", obj.x); // 벡터 obj를 Animator Controller에 전달
        //animator.SetFloat("MoveY", obj.y); // 정확한 이동 방향에 맞는 애니메이션 재생

        //int newState;
        //if (Mathf.Abs(obj.x) > Mathf.Abs(obj.y) // 절댓값 비교
        //    newState = obj.x > 0 ? MoveRight : MoveLeft; // 0보다 크면 Right, 작으면 Left
        //else if (Mathf.Abs(obj.y) > 0)
        //    newState = obj.y > 0 ? MoveUp : MoveDown;


    }
}
