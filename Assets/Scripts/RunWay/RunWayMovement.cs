using UnityEngine;

public class RunWayMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        MoveMent();
    }

    private void MoveMent()
    {
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }
}
