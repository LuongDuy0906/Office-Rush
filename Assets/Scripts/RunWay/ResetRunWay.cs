using System.Collections.Generic;
using UnityEngine;

public class ResetRunWay : MonoBehaviour
{
    [SerializeField] private List<GameObject> runWays;
    private float disapearPosition = -2.14135f;
    private float runwayLength = 27.5f;

    // Update is called once per frame
    void Update()
    {
        UpdateRunWay();
    }

    private void UpdateRunWay()
    {
        float currentRunWayPosition = runWays[0].transform.position.z;
        GameObject newRunWay = runWays[0];

        if(currentRunWayPosition < disapearPosition)
        {
            Vector3 newPosition = newRunWay.transform.position;
            newPosition.z = runWays[runWays.Count - 1].transform.position.z + runwayLength;
            newRunWay.transform.position = newPosition;
            runWays.Remove(runWays[0]);
            runWays.Add(newRunWay);
        }
    }
}
