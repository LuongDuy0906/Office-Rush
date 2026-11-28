using System.Collections.Generic;
using UnityEngine;

public class ResetRunWay : MonoBehaviour
{
    [SerializeField] private List<GameObject> runWays;
    [SerializeField] private float resetOffset = 15f;
    private Camera mainCamera;
    private float runwayLength = 27.5f;

    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateRunWay();
    }

    private void UpdateRunWay()
    {
        float currentRunWayPosition = runWays[0].transform.position.z;
        GameObject newRunWay = runWays[0];

        if(currentRunWayPosition < mainCamera.transform.position.z + resetOffset)
        {
            Vector3 newPosition = newRunWay.transform.position;
            newPosition.z = runWays[runWays.Count - 1].transform.position.z + runwayLength;
            newRunWay.transform.position = newPosition;
            runWays.Remove(runWays[0]);
            runWays.Add(newRunWay);
        }
    }
}
