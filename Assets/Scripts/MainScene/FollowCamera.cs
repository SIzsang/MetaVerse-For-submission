using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float minX, MaxX, minY, MaxY;

    void Start() // private�� ���� > private void Start()�� ����
    {
        if (target == null) return;
    }

    void Update()
    {
        
    }

    void LateUpdate() // ī�޶� ���� ��ũ��Ʈ�� LateUpdate�� ���
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;

        pos.x = target.position.x;
        pos.y = target.position.y;

        pos.x = Mathf.Clamp(pos.x, minX, MaxX); // Mathf.Clamp ���� ������ ������ ���� �ʵ��� ����
        pos.y = Mathf.Clamp(pos.y, minY, MaxY); // (��, �ּҰ�, �ִ밪)

        transform.position = pos;
    }
}
