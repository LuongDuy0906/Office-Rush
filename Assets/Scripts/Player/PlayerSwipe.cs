using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerSwipe : MonoBehaviour
{
    private const float LaneWidth = 1.5f;
    private const float DirectionDominance = 1.2f;
    private static readonly int IsJumpParameter = Animator.StringToHash("isJump");

    [SerializeField] private float laneChangeSpeed = 6f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField, Range(0.01f, 0.2f)] private float swipeThresholdRatio = 0.06f;
    [SerializeField, Min(0.01f)] private float jumpBufferTime = 0.15f;

    private Rigidbody body;
    private Animator animator;
    private InputAction pressAction;
    private Pointer activePointer;
    private Vector2 startPosition;
    private int targetLane;
    private bool gestureHandled;
    private bool isGround;
    private bool isJumping;
    private float jumpRequestedUntil = float.NegativeInfinity;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        targetLane = Mathf.Clamp(Mathf.RoundToInt(body.position.x / LaneWidth), -1, 1);

        pressAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Pointer>/press");

        pressAction.started += OnPress;
        pressAction.canceled += OnRelease;
    }

    private void OnEnable() => pressAction.Enable();

    private void OnDisable()
    {
        activePointer = null;
        gestureHandled = true;
        jumpRequestedUntil = float.NegativeInfinity;
        pressAction.Disable();
    }

    private void OnDestroy() => pressAction.Dispose();

    private void OnPress(InputAction.CallbackContext context)
    {
        if (activePointer != null) return;

        activePointer = context.control.device as Pointer;
        if (activePointer == null) return;

        startPosition = activePointer.position.ReadValue();
        gestureHandled = false;
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        if (activePointer == null || context.control.device != activePointer) return;

        if (!gestureHandled)
            TryHandleSwipe(activePointer.position.ReadValue() - startPosition);

        activePointer = null;
    }

    private void Update()
    {
        if (activePointer != null && !gestureHandled)
            TryHandleSwipe(activePointer.position.ReadValue() - startPosition);

        if (animator != null)
            animator.SetBool(IsJumpParameter, isJumping);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = body.linearVelocity;
        float targetX = targetLane * LaneWidth;
        float distanceX = targetX - body.position.x;

        velocity.x = Mathf.Clamp(
            distanceX / Time.fixedDeltaTime,
            -laneChangeSpeed,
            laneChangeSpeed);

        body.linearVelocity = velocity;

        if (!isGround || Time.time > jumpRequestedUntil) return;

        jumpRequestedUntil = float.NegativeInfinity;
        isGround = false;
        isJumping = true;
        body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void TryHandleSwipe(Vector2 delta)
    {
        float threshold = Mathf.Min(Screen.width, Screen.height) * swipeThresholdRatio;
        float horizontal = Mathf.Abs(delta.x);
        float vertical = Mathf.Abs(delta.y);

        if (Mathf.Max(horizontal, vertical) < threshold) return;

        if (horizontal >= vertical * DirectionDominance)
        {
            targetLane = Mathf.Clamp(targetLane + (int)Mathf.Sign(delta.x), -1, 1);
            gestureHandled = true;
        }
        else if (vertical >= horizontal * DirectionDominance)
        {
            if (delta.y > 0f)
                jumpRequestedUntil = Time.time + jumpBufferTime;

            gestureHandled = true;
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGround = grounded;
        if (grounded) isJumping = false;
    }
}
