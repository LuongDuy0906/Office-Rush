using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSwipe))]
public class PlayerCollision : MonoBehaviour
{
    private readonly HashSet<Collider> groundContacts = new HashSet<Collider>();
    private PlayerSwipe playerSwipe;
    private Rigidbody body;

    private void Awake()
    {
        playerSwipe = GetComponent<PlayerSwipe>();
        body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision) => UpdateGroundContact(collision);
    private void OnCollisionStay(Collision collision) => UpdateGroundContact(collision);

    private void OnCollisionExit(Collision collision)
    {
        if (groundContacts.Remove(collision.collider))
            playerSwipe.SetGrounded(groundContacts.Count > 0);
    }

    private void UpdateGroundContact(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        if (body.linearVelocity.y <= 0.1f)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y <= 0.5f) continue;

                groundContacts.Add(collision.collider);
                playerSwipe.SetGrounded(true);
                return;
            }
        }

        if (groundContacts.Remove(collision.collider))
            playerSwipe.SetGrounded(groundContacts.Count > 0);
    }
}
