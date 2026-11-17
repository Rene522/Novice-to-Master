using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Crouch : MonoBehaviour
{

    [SerializeField] CharacterController playerController;
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchSpeed;
    [SerializeField] float defaultHeight;
    public Transform player;
    public bool isCrouching = false;
    StarterAssetsInputs inputs;
    PlayerInput playerInput;

    CapsuleCollider playerCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerCollider = GetComponentInChildren<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inputs || !playerInput || !playerController)
        {
            return;
        }

        if (inputs.crouch)
        {
            isCrouching = true;
        }

        if (!inputs.crouch && isCrouching)
        {
            isCrouching = false;
        }

        if (isCrouching)
        {
            playerController.height = Mathf.MoveTowards(playerController.height, crouchHeight, crouchSpeed * Time.deltaTime);
            playerCollider.height = Mathf.MoveTowards(playerCollider.height, crouchHeight, crouchSpeed * Time.deltaTime);

            if (playerController.height <= crouchHeight || playerCollider.height <= crouchHeight)
            {
                playerController.height = crouchHeight;
                playerCollider.height = crouchHeight;
            }

        }

        if (!isCrouching)
        {
            playerController.height = Mathf.MoveTowards(playerController.height, defaultHeight, crouchSpeed * Time.deltaTime);
            playerCollider.height = Mathf.MoveTowards(playerCollider.height, defaultHeight, crouchSpeed * Time.deltaTime);
            if (playerController.height >= defaultHeight || playerCollider.height >= defaultHeight)
            {
                playerController.height = defaultHeight;
                playerCollider.height = defaultHeight;
            }
        }
    }
}
