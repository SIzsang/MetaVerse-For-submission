using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCount = 4;
    public int obstacleCount = 0;
    public Vector3 obstacleLastPosition = Vector3.zero;

    private Vector3[] initialObstaclePositions;
    private Vector3[] initialBgPositions;
    private Obstacle[] obstacles;
    private GameObject[] backgrounds;

    // Start is called before the first frame update
    void Start()
    {
        obstacles = FindObjectsOfType<Obstacle>();
        obstacleCount = obstacles.Length;
        initialObstaclePositions = new Vector3[obstacleCount];

        for (int i = 0; i < obstacleCount; i++)
        {
            initialObstaclePositions[i] = obstacles[i].transform.position;
        }

        
        backgrounds = GameObject.FindGameObjectsWithTag("BackGround");
        initialBgPositions = new Vector3[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            initialBgPositions[i] = backgrounds[i].transform.position;
        }

        
        obstacleLastPosition = obstacles[0].transform.position;
        for (int i = 0; i < obstacleCount; i++)
        {
            obstacleLastPosition = obstacles[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggered: " + collision.name);

        if (collision.CompareTag("BackGround"))
        {
            float widthOfBgObject = ((CompositeCollider2D)collision).bounds.size.x;
            Vector3 pos = collision.transform.position;

            pos.x += widthOfBgObject * numBgCount;
            collision.transform.position = pos;
            return;
        }

        Obstacle obstacle = collision.GetComponent<Obstacle>();
        if (obstacle)
        {
            obstacleLastPosition = obstacle.SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }

    public void Reset()
    {
        
        for (int i = 0; i < obstacleCount; i++)
        {
            obstacles[i].transform.position = initialObstaclePositions[i];
        }

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position = initialBgPositions[i];
        }

        obstacleLastPosition = initialObstaclePositions[obstacleCount - 1];

        obstacleLastPosition = obstacles[0].transform.position;
        for (int i = 0; i < obstacleCount; i++)
        {
            obstacleLastPosition = obstacles[i].SetRandomPlace(obstacleLastPosition, obstacleCount);
        }
    }
}
