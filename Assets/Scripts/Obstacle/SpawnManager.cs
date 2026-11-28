using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<ObjectPooler> objectPoolers;
    [SerializeField] private float spawnAhead = 5f;
    [SerializeField] private float defaultSpawnInterval = 1f;

    private Transform playerTransform;
    private float spawnInterval;
    private float defaultY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerSwipe>().transform;
        spawnInterval = 0;
        defaultY = playerTransform.position.y;
    }

    // Update is called once per frame
    void Update()
    {   
        if(spawnInterval > 0)
        {
            spawnInterval -= Time.deltaTime;
        }
        else
        {
            SpawnObstacle();    
            spawnInterval = defaultSpawnInterval;        
        }
    }

    private void SpawnObstacle()
    {
        int emptyLane = Random.Range(0, 3);
        int gap = Random.Range(2, 5);

        int spawned = 0;

        for(int lane = 0; lane < 3; lane++)
        {
            if(lane == emptyLane) continue;
            int randomIndex = Random.Range(0, objectPoolers.Count);

            float x = (lane - 1) * 1.5f;
            float z = playerTransform.position.z + spawnAhead + (spawned == 0? 0f : gap);

            Vector3 spawnPosition = new Vector3(x, defaultY, z);
            objectPoolers[randomIndex].GetPool(spawnPosition);

            spawned++;
        }         
    }
}
