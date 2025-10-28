using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFollowCamera : MonoBehaviour
{
    public Transform target;
    float offX;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null) return;
        offX = transform.position.x - transform.position.x;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = target.position.x + offX;
        transform.position = pos;
    }
}
