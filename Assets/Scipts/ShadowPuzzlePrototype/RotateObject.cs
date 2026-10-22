using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class RotateObject : Interactable
{

    [SerializeField] Transform player;
    [SerializeField] FirstPersonController playerController;
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField] float grabDistance = 1.5f;

    public Quaternion startQuaternion;

    public Quaternion targetRotation;
    bool isRotating = false;
    bool isGrabbed = false;
    float horizontalInput;
    static RotateObject grabbedBlock;
    Rigidbody rb;
    StarterAssetsInputs inputs;
    InputAction interactAction;
    static int lastInteractionFrame = -1;
    bool movementPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        inputs = player.GetComponent<StarterAssetsInputs>();

        PlayerInput playerInput = player.GetComponent<PlayerInput>();

        interactAction = playerInput.actions.FindAction("Player/Interact", true);

        startQuaternion = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrabbed)
        {
            return;
        }
        if (interactAction.WasPressedThisFrame())
        {
            InteractEffect();
            return;
        }
        if (isRotating)
        {
            return;
        }
        if (Mathf.Abs(inputs.move.x) <= 0.5f)
        {
            movementPressed = false;
            return;
        }
        if (movementPressed)
        {
            return;
        }

        movementPressed = true;
        if (inputs.move.x < 0)
        {
            rotateBlock(1f);
        }
        else
        {
            rotateBlock(-1f);
        }
    }

    void FixedUpdate()
    {
        if (!isRotating)
            return;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
            movementPressed = false;

            ReleaseBlock();
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
