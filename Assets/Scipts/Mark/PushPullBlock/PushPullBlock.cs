using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;

/// <summary>
/// This class represents a push/pull block that can be interacted with by the player. It allows the player to grab, push, and pull the block within a grid-based movement system. 
/// The block can only be moved if there are no obstacles in the way, and it can also move the player when being pulled.
/// </summary>
public class PushPullBlock : Interactable
{
    [Tooltip("The player transform")]
    [SerializeField] Transform player;
    [Tooltip("The player controller")]
    [SerializeField] FirstPersonController playerController;
    [Tooltip("The grid size for movement")]
    [SerializeField] float gridSize = 1f;
    [Tooltip("The speed of block movement")]
    [SerializeField] float moveSpeed = 2f;
    [Tooltip("The distance at which the block can be grabbed")]
    [SerializeField] float grabDistance = 1.5f;
    [SerializeField] bool isLocked;
    [SerializeField] InspectionTrigger unlockSwitch;
    [SerializeField] string lockedMessage = "Something is holding it in place.";
    [SerializeField] InspectionManager inspectionManager;

    Rigidbody rb; // The rigidbody of the block
    StarterAssetsInputs inputs; // The input handler for the player
    InputAction interactAction; // The input action for interacting with the block
    CharacterController characterController; // The character controller of the player
    Collider blockCollider; // The box collider of the block

    static PushPullBlock grabbedBlock; // The currently grabbed block
    static int lastInteractionFrame = -1; // The last frame in which an interaction occurred

    bool isGrabbed = false; // Whether the block is currently grabbed
    bool movementPressed = false; // Whether the movement input is currently pressed
    bool isMoving = false; // Whether the block is currently moving
    bool isPulling = false; // Whether the block is being pulled (as opposed to pushed)

    Vector3 targetPosition; // The target position for the block to move to

    /// <summary>
    /// Initializes the block's components and input actions.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigidbody component of the block
        blockCollider = GetComponent<Collider>(); // Get the collider component of the block
        inputs = player.GetComponent<StarterAssetsInputs>(); // Get the input handler component of the player
        PlayerInput playerInput = player.GetComponent<PlayerInput>(); // Get the player input component of the player
        interactAction = playerInput.actions.FindAction("Player/Interact", true); // Find the interact action in the player's input actions
        characterController = player.GetComponent<CharacterController>(); // Get the character controller component of the player   
    }

    /// <summary>
    /// Updates the block's state based on player input and movement.
    /// </summary>
    private void Update()
    {
        if (isLocked && unlockSwitch && unlockSwitch.HasActivated())
        {
            Unlock();
        }
        if (!isGrabbed) // If the block is not grabbed, do nothing
        {
            return;
        }
        if (interactAction.WasPressedThisFrame()) // If the interact action was pressed this frame, trigger the interaction effect
        {
            InteractEffect();
            return;
        }
        if (isMoving) // If the block is currently moving, do nothing
        {
            return;
        }
        if (Mathf.Abs(inputs.move.y) <= 0.5f) // If the vertical movement input is not significant, reset the movementPressed flag and do nothing
        {
            movementPressed = false;
            return;
        }
        if (movementPressed) // If the movement input was already pressed, do nothing
        {
            return;
        }

        movementPressed = true; // Set the movementPressed flag to true to indicate that the movement input is being processed
        if (inputs.move.y > 0)// If the vertical movement input is positive, move the block forward
        {
            MoveBlock(1f);
        }
        else // If the vertical movement input is negative, move the block backward
        {
            MoveBlock(-1f);
        }
    }

    /// <summary>
    /// Moves the block towards the target position and handles player movement if pulling.
    /// </summary>
    void FixedUpdate()
    {
        if (!isMoving) // If the block is not currently moving, do nothing
        {
            return;
        }

        Vector3 nextPosition = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime); // Calculate the next position for the block to move towards the target position at the specified speed
        Vector3 movement = nextPosition - rb.position; // Calculate the movement vector for the block based on the next position and current position

        if (isPulling) // If the block is being pulled, move the player along with the block
        {
            Vector3 previousPlayerPosition = player.position; // Store the player's previous position before moving
            characterController.Move(movement); // Move the player using the character controller based on the block's movement

            movement = player.position - previousPlayerPosition; // Calculate the actual movement of the player after moving
            movement.y = 0f; // Ignore vertical movement for the block's movement calculation

            nextPosition = rb.position + movement; // Update the next position for the block based on the player's movement

            if (movement.magnitude < Mathf.Epsilon) // If the player couldn't move (e.g., due to collision), release the block and stop moving
            {
                ReleaseBlock(); 
                return;
            }
        }

        rb.MovePosition(nextPosition); // Move the block's rigidbody to the next position

        if (nextPosition == targetPosition) // If the block has reached the target position, release the block and stop moving
        {
            ReleaseBlock();
        }
    }

    /// <summary>
    /// Checks if the block can move in the specified direction without colliding with other objects.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool CanMoveBlock(Vector3 direction, out float moveDistance)
    {
        moveDistance = gridSize;
        Bounds localBounds;

        if (blockCollider is BoxCollider boxCollider)
        {
            localBounds = new Bounds(boxCollider.center, boxCollider.size);
        }
        else
        {
            MeshCollider meshCollider = (MeshCollider)blockCollider;
            localBounds = meshCollider.sharedMesh.bounds;
        }

        Vector3 centre = blockCollider.transform.TransformPoint(localBounds.center);
        Vector3 scale = blockCollider.transform.lossyScale;

        Vector3 halfSize = new Vector3(Mathf.Max(localBounds.extents.x * Mathf.Abs(scale.x) - 0.02f, 0.001f), Mathf.Max(localBounds.extents.y * Mathf.Abs(scale.y) - 0.02f, 0.001f), Mathf.Max(localBounds.extents.z * Mathf.Abs(scale.z) - 0.02f, 0.001f));

        RaycastHit[] hits = Physics.BoxCastAll(centre, halfSize, direction, blockCollider.transform.rotation, gridSize + 0.02f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

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

            moveDistance = Mathf.Min(moveDistance, Mathf.Max(0f, hit.distance - 0.02f));
        }

        return moveDistance > 0.001f;
    }

    /// <summary>
    /// Checks if the player can move in the specified direction without colliding with other objects while pulling the block.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    bool CanMovePlayer(Vector3 direction, float moveDistance)
    {
        Bounds bounds = characterController.bounds; // Get the bounds of the player's character controller

        float radius = Mathf.Max(bounds.extents.x, bounds.extents.z); // Get the maximum radius of the player's character controller in the horizontal plane
        float halfHeight = Mathf.Max(bounds.extents.y - radius, 0f); // Calculate the half height of the player's character controller, ensuring it's non-negative

        Vector3 topPoint = bounds.center + Vector3.up * halfHeight; // Calculate the top point of the capsule for the player's character controller
        Vector3 bottomPoint = bounds.center - Vector3.up * halfHeight; // Calculate the bottom point of the capsule for the player's character controller

        radius -= 0.02f; // Slightly reduce the radius to avoid edge collisions

        RaycastHit[] hits = Physics.CapsuleCastAll(topPoint, bottomPoint, radius, direction, moveDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore); // Perform a capsule cast in the specified direction to check for potential collisions while moving the player

        foreach (RaycastHit hit in hits) // Iterate through all the hits from the capsule cast
        {
            if (hit.collider.transform.IsChildOf(player)) // If the hit collider belongs to the player or its children, ignore it
            {
                continue;
            }
            if (hit.collider.attachedRigidbody == rb) // If the hit collider belongs to the block being pulled, ignore it
            {
                continue;
            }
            return false; // If any other collider is hit, the player cannot move in that direction while pulling the block
        }
        return true; // If no other colliders are hit, the player can move in that direction while pulling the block
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stepDirection"></param>
    void MoveBlock(float stepDirection)
    {
        Vector3 direction = transform.InverseTransformDirection(rb.position - player.position); // Calculate the direction from the player to the block

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))// Determine whether the block is more aligned with the x-axis or z-axis for movement
        {
            direction = new Vector3(Mathf.Sign(direction.x), 0, 0); // Set the direction to be along the x-axis based on the sign of the x component
        }
        else // If the block is more aligned with the z-axis, set the direction to be along the z-axis based on the sign of the z component
        {
            direction = new Vector3(0, 0, Mathf.Sign(direction.z)); // Set the direction to be along the z-axis based on the sign of the z component
        }

        direction = transform.TransformDirection(direction);
        direction *= stepDirection; // Scale the direction by the stepDirection to determine whether to move forward or backward
        isPulling = stepDirection < 0; // Set the isPulling flag based on whether the block is being pulled (stepDirection < 0) or pushed (stepDirection > 0)

        if (!CanMoveBlock(direction, out float moveDistance))// If the block cannot move in the specified direction, release the block and stop moving
        {
            ReleaseBlock();
            return;
        }
        if (isPulling && !CanMovePlayer(direction, moveDistance)) // If the block is being pulled and the player cannot move in the specified direction, release the block and stop moving
        {
            ReleaseBlock();
            return;
        }
        targetPosition = rb.position + direction * moveDistance; // Calculate the target position for the block to move to based on the current position, direction, and grid size

        isMoving = true; // Set the isMoving flag to true to indicate that the block is currently moving
    }

    /// <summary>
    /// Releases the block, resetting its state and allowing the player to move freely again.
    /// </summary>
    void ReleaseBlock()
    {
        isMoving = false; // Set the isMoving flag to false to indicate that the block is no longer moving
        isGrabbed = false; // Set the isGrabbed flag to false to indicate that the block is no longer grabbed
        movementPressed = false; // Reset the movementPressed flag to false to allow for new movement input

        if (grabbedBlock == this) // If the currently grabbed block is this block, reset the grabbedBlock reference and unlock player movement
        {
            grabbedBlock = null; // Reset the grabbedBlock reference to null to indicate that no block is currently grabbed
            playerController.movementLocked = false; // Unlock player movement by setting the movementLocked flag in the player controller to false
        }
    }

    /// <summary>
    /// Checks if the player is within the grab distance of the block, allowing for interaction.
    /// </summary>
    /// <returns></returns>
    bool CanGrabBlock()
    {
        Vector3 distanceToBlock = rb.position - player.position; // Calculate the distance vector from the player to the block
        distanceToBlock.y = 0f; // Ignore vertical distance for grab distance calculation
        return distanceToBlock.magnitude <= grabDistance; // Return true if the player is within the grab distance of the block, allowing for interaction; otherwise, return false
    }
    public void Unlock()
    {
        isLocked = false;
    }

    public void LockInPlace()
    {
        ReleaseBlock();
        rb.isKinematic = true;
        enabled = false;
    }

    /// <summary>
    /// Returns whether the block is currently moving.
    /// </summary>
    /// <returns></returns>
    public bool IsMoving()
    {
        return isMoving;
    }

    /// <summary>
    /// Handles the interaction effect when the player interacts with the block, either grabbing or releasing it based on its current state.
    /// </summary>
    public override void InteractEffect()
    {
        if (isLocked)
        {
            inspectionManager.ShowMessage(lockedMessage, 2f);
            return;
        }

        if (grabbedBlock && grabbedBlock != this) // If another block is currently grabbed and it's not this block, do nothing
        {
            return;
        }
        if (isMoving) // If the block is currently moving, do nothing
        {
            return;
        }
        if (lastInteractionFrame == Time.frameCount) // If the interaction effect has already been triggered in the current frame, do nothing
        {
            return;
        }
        
        lastInteractionFrame = Time.frameCount; // Update the lastInteractionFrame to the current frame to prevent multiple interactions in the same frame

        if (isGrabbed) // If the block is currently grabbed, release it
        {
            ReleaseBlock();
        }
        else // If the block is not currently grabbed, attempt to grab it
        {
            if (!CanGrabBlock()) // If the player is not within the grab distance of the block, do nothing
            {
                return;
            }

            grabbedBlock = this; // Set the grabbedBlock reference to this block to indicate that it is now grabbed
            isGrabbed = true; // Set the isGrabbed flag to true to indicate that the block is now grabbed
            playerController.movementLocked = true; // Lock player movement by setting the movementLocked flag in the player controller to true
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string GetPromptText()
    {
        if (grabbedBlock && grabbedBlock != this) // If another block is currently grabbed and it's not this block, return an empty prompt text
        {
            return "";
        }
        if (!isGrabbed && !CanGrabBlock()) // If the block is not currently grabbed and the player is not within the grab distance of the block, return an empty prompt text
        {
            return "";
        }
        return base.GetPromptText(); // Otherwise, return the default prompt text from the base class
    }

    public override string GetFeedbackText()
    {
        if (isLocked)
        {
            return "";
        }

        return base.GetFeedbackText();
    }

    /// <summary>
    /// Called when the component is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (isGrabbed) // If the block is currently grabbed when the component is disabled, release it to reset its state
        {
            ReleaseBlock();
        }
    }
}
