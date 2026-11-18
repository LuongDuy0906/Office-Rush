using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerSwipe playerSwipe;

    void Awake()
    {
        playerSwipe = GetComponent<PlayerSwipe>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            playerSwipe.SetIsGround();
            playerSwipe.SetIsJump();
        }
    }
}
