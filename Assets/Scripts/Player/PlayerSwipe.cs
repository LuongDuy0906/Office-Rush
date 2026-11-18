using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwipe : MonoBehaviour
{
    [SerializeField] private float laneChangeSpeed = 6f;
    [SerializeField] private float jumpForce = 6f;

    private Rigidbody body;
    private InputAction pressAction;
    private Pointer activePointer;
    private Vector2 startPosition;
    private float targetX;
    private bool isGround = true;
    private bool isJump;

    public bool IsGround => isGround;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        targetX = body.position.x;

        pressAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Pointer>/press");

        pressAction.started += OnPress;
        pressAction.canceled += OnRelease;
    }

    private void OnEnable() => pressAction.Enable();
    private void OnDisable() => pressAction.Disable();
    private void OnDestroy() => pressAction.Dispose();

    private void OnPress(InputAction.CallbackContext context)
    {
        activePointer = context.control.device as Pointer;
        if (activePointer != null)
            startPosition = activePointer.position.ReadValue();
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        if (activePointer == null || context.control.device != activePointer)
            return;

        Vector2 delta = activePointer.position.ReadValue() - startPosition;
        activePointer = null;

        DetectSwipeHorizontalOrVertical(delta);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = body.linearVelocity;
        float distanceX = targetX - body.position.x;

        velocity.x = Mathf.Clamp(
            distanceX / Time.fixedDeltaTime,
            -laneChangeSpeed,
            laneChangeSpeed);

        body.linearVelocity = velocity;
        Jump();
    }

    private void DetectSwipeHorizontalOrVertical(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (Mathf.Abs(delta.x) >= Screen.width * 0.08f)
                targetX = Mathf.Clamp(
                    targetX + Mathf.Sign(delta.x) * 1.5f, -1.5f, 1.5f);
        }
        else if (delta.y > Screen.height * 0.05f)
        {
            isJump = true;
        }
    }

    private void Jump()
    {
        if (isGround && isJump)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
            isJump = false;
        }
    }
}
