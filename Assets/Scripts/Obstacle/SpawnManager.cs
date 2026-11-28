using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private ObjectPooler objectPooler;
    [SerializeField] private float spawnAhead = 5f;
    [SerializeField] private float defaultSpawnInterval = 1f;

    private Transform playerTransform;
    private float spawnInterval;
    private float defaultY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = FindAnyObjectByType<PlayerSwipe>().transform;
        spawnInterval = defaultSpawnInterval;
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

        for(int lane = 0; lane < 3; lane++)
        {
            if(lane == emptyLane) continue;

            float x = (lane - 1) * 1.5f;
            Vector3 spawnPosition = new Vector3(x, defaultY, playerTransform.position.z + spawnAhead);
            objectPooler.GetPool(spawnPosition);
        }         
    }
}
