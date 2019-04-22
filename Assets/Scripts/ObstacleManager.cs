using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject PlaerObj;
    public GameObject[] obstacles;
    public GameObject finishSignObj;
    public int levelLength = 4;

    int amoutOfLevelsEverPlayed = 0;

    GameManager gameManager;

    int obstacleCount;
    int playerDistanceIndex = 0;
    int obstacleIndex = 0;
    int distanceToNext = 50;
    bool _finish = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        obstacleCount = obstacles.Length;
        InstantiateObstacle();
    }

   
    void Update()
    {
        int PlayerDistance = (int)PlaerObj.transform.position.y / (distanceToNext /2);
        if (playerDistanceIndex != PlayerDistance && levelLength > 0)
        {
            levelLength--;
            InstantiateObstacle();
            playerDistanceIndex = PlayerDistance;
        }else if (levelLength == 0 && !_finish)
        {
            _finish = true;
            FinishSign();
        }
    }

    public void FinishSign()
    {
        Instantiate(finishSignObj, new Vector3(0, obstacleIndex * distanceToNext), Quaternion.identity);
    }


    public void InstantiateObstacle()
    {
        int RandomInt = Random.Range(0, obstacleCount - 1);

        if (gameManager.amoutOfLevelsEverPlayed < 1)
        {
            RandomInt = obstacleCount - 1; //Random.Range(0, 2);
        }
        Debug.Log("Levels Played "+gameManager.amoutOfLevelsEverPlayed);

        GameObject newOstacle = Instantiate(obstacles[RandomInt], new Vector3(0, obstacleIndex * distanceToNext),Quaternion.identity);
        newOstacle.transform.SetParent(transform);
        obstacleIndex++;

    }
}
