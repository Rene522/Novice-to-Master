using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class RotateObject : Interactable
{

    [SerializeField] Transform player;
    [SerializeField] FirstPersonController playerController;
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField] float grabDistance = 1.5f;

    [SerializeField] float moveSpeed = 2f;

    [SerializeField] float gridSize = 1f;

    public Quaternion startQuaternion;

    public Quaternion targetRotation;
    bool isRotating = false;
    bool isGrabbed = false;
    float horizontalInput;
    static RotateObject grabbedBlock;
    Rigidbody rb;
    StarterAssetsInputs inputs;
    InputAction rotateAction;
    static int lastInteractionFrame = -1;
    bool movementPressed = false;

    BoxCollider blockCollider;
    CharacterController characterController;

    bool isMoving = false;
    bool movePressed = false;

    bool isPulling = false;
    Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        blockCollider = GetComponent<BoxCollider>();

        inputs = player.GetComponent<StarterAssetsInputs>();

        PlayerInput playerInput = player.GetComponent<PlayerInput>();

        rotateAction = playerInput.actions.FindAction("Player/Interact", true);

        startQuaternion = transform.rotation;

        characterController = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrabbed)
            return;

        if (rotateAction.WasPressedThisFrame())
            return;

        if (isRotating || isMoving)
            return;

        if (Mathf.Abs(inputs.move.x) > 0.5f)
        {
            if (!movementPressed)
            {
                movementPressed = true;

                if (inputs.move.x < 0)
                    rotateBlock(1f);
                else
                    rotateBlock(-1f);
            }

            return;
        }

        movementPressed = false;

        if (Mathf.Abs(inputs.move.y) > 0.5f)
        {
            if (movePressed)
                return;

            movePressed = true;

            float stepDirection = inputs.move.y > 0 ? 1f : -1f;

            MoveBlock(stepDirection);
        }
        else
        {
            movePressed = false;
        }
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed * Time.fixedDeltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
                movementPressed = false;

                ReleaseBlock();
            }
        }

        if (isMoving)
        {
            Vector3 nextPosition = Vector3.MoveTowards(rb.position,targetPosition, moveSpeed * Time.fixedDeltaTime);

            rb.MovePosition(nextPosition);

            if (Vector3.Distance(nextPosition, targetPosition) < 0.001f)
            {
                rb.MovePosition(targetPosition);
                isMoving = false;

                ReleaseBlock();
            }
        }
    }

    void rotateBlock(float rotateAmount)
    {
        targetRotation = transform.rotation * Quaternion.Euler(0f, rotateAmount * 90f, 0f);

        isRotating = true;
    }

    public override void InteractEffect()
    {
        if (isRotating)
        {
            return;
        }
        if (grabbedBlock && grabbedBlock != this)
        {
            return;
        }
        if (lastInteractionFrame == Time.frameCount)
        {
            return;
        }

        lastInteractionFrame = Time.frameCount;

        if (isGrabbed)
        {
            ReleaseBlock();
        }
        else
        {
            if (!CanGrabBlock())
            {
                return;
            }

            grabbedBlock = this;
            isGrabbed = true;
            playerController.movementLocked = true;
        }
    }

    bool CanMoveBlock(Vector3 direction)
    {
        Vector3 centre = blockCollider.bounds.center;
        Vector3 halfSize = blockCollider.bounds.extents;
        halfSize -= Vector3.one * 0.02f;

        RaycastHit[] hits = Physics.BoxCastAll(centre, halfSize, direction, Quaternion.identity, gridSize, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.attachedRigidbody == rb)
            {
                continue;
            }
            if (hit.collider.transform.IsChildOf(player))
            {
                continue;
            }
            return false;      
        }
        return true;
    }

    void MoveBlock(float stepDirection)
    {
        Vector3 direction = rb.position - player.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            direction = new Vector3(Mathf.Sign(direction.x), 0f, 0f);
        }
        else
        {
            direction = new Vector3(0f, 0f, Mathf.Sign(direction.z));
        }

        direction *= stepDirection;

        isPulling = stepDirection < 0f;

        if (!CanMoveBlock(direction))
            return;

        if (isPulling && !CanMovePlayer(-direction))
            return;

        targetPosition = rb.position + direction * gridSize;

        isMoving = true;
    }

    bool CanMovePlayer(Vector3 direction)
    {
        Bounds bounds = characterController.bounds;

        float radius = Mathf.Max(bounds.extents.x, bounds.extents.z);
        float halfHeight = Mathf.Max(bounds.extents.y - radius, 0f);

        Vector3 topPoint = bounds.center + Vector3.up * halfHeight;
        Vector3 bottomPoint = bounds.center - Vector3.up * halfHeight;

        radius -= 0.02f;

        RaycastHit[] hits = Physics.CapsuleCastAll(topPoint, bottomPoint, radius, direction, gridSize, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(player))
            {
                continue;
            }
            if (hit.collider.attachedRigidbody == rb)
            {
                continue;
            }
            return false;
        }
        return true;
    }

    public override string GetPromptText()
    {
        if (grabbedBlock != null && grabbedBlock != this)
            return "";

        if (!isGrabbed && !CanGrabBlock())
            return "";

        return base.GetPromptText();
    }

    bool CanGrabBlock()
    {
        Vector3 distanceToBlock = rb.position - player.position;
        distanceToBlock.y = 0f;

        return distanceToBlock.magnitude <= grabDistance;
    }
    void ReleaseBlock()
    {
        isGrabbed = false;
        horizontalInput = 0f;

        if (grabbedBlock == this)
        {
            grabbedBlock = null;
            playerController.movementLocked = false;
        }
    }
}
