using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float poolSize;
    
    private Queue<GameObject> avalaibleObstacle= new();
    private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            obstacle.SetActive(false);
            avalaibleObstacle.Enqueue(obstacle);
        } 

        mainCamera = Camera.main;
    }

    void Update()
    {
        CheckObstacle();
    }

    public void GetPool(Vector3 spawnPosition)
    {
        if(avalaibleObstacle != null)
        {
            GameObject people = avalaibleObstacle.Dequeue();
            people.transform.position = spawnPosition;
            people.SetActive(true);
        }

        return;
    }

    public void RestorePool(GameObject obstacle)
    {
        obstacle.SetActive(false);
        avalaibleObstacle.Enqueue(obstacle);
    }

    private void CheckObstacle()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject obstacle = transform.GetChild(i).gameObject;
            if (!obstacle.activeSelf) continue;

            if(obstacle.transform.position.z < mainCamera.transform.position.z)
            {
                RestorePool(obstacle);
            }
        }
    }
}
