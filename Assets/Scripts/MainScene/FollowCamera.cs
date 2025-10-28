using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float minX, MaxX, minY, MaxY;

    void Start() // private이 생략 > private void Start()와 동일
    {
        if (target == null) return;
    }

    void Update()
    {
        
    }

    void LateUpdate() // 카메라 추적 스크립트는 LateUpdate에 사용
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;

        pos.x = target.position.x;
        pos.y = target.position.y;

        pos.x = Mathf.Clamp(pos.x, minX, MaxX); // Mathf.Clamp 값이 일정한 범위를 넘지 않도록 제한
        pos.y = Mathf.Clamp(pos.y, minY, MaxY); // (값, 최소값, 최대값)

        transform.position = pos;
    }
}
