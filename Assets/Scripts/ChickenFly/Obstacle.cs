using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float highPosY = 1f;
    public float lowPosY = -2f;

    public float holeSizeMin = 2f;
    public float holeSizeMax = 3.5f;

    public Transform treeObject;

    public float widthPadding = 5f;
    GameManager gameManager;

    public Vector3 SetRandomPlace(Vector3 lastPosition, int obstacleCount)
    {
        float holeSize = Random.Range(holeSizeMin, holeSizeMax);
        float halfHoleSize = holeSize / 2;
        treeObject.localPosition = new Vector3(0, halfHoleSize);

        Vector3 placePostion = lastPosition + new Vector3(widthPadding, 0);
        placePostion.y = Random.Range(lowPosY, highPosY);

        transform.position = placePostion;

        return placePostion;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Chicken player = collision.GetComponent<Chicken>();
        if (player != null)
            GameManager.Instance.AddScore(1);
    }
}
