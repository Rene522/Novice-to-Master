using StarterAssets;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectionCameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera inspectionCamera;
    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] CinemachineBrain mainCameraBrain;
    [SerializeField] Camera textCamera;
    [SerializeField] Hotbar hotbar;
    [SerializeField] Transform inspectionObjectPoint;
    [SerializeField] float rotationSpeed = 5f;


    StarterAssetsInputs inputs;
    Interact playerInteractor;
    InputActionMap inspectionInputs;

    CinemachineVolumeSettings inspectionVolume;
    GameObject inspectedItem;
    Transform originalFocusTarget;
    PlayerInput playerInput;

    bool isInspecting;

    void Awake()
    {
        inputs = firstPersonController.GetComponent<StarterAssetsInputs>();
        playerInteractor = firstPersonController.GetComponent<Interact>();
        playerInput = firstPersonController.GetComponent<PlayerInput>();
        inspectionInputs = firstPersonController.GetComponent<PlayerInput>().actions.FindActionMap("Inspection");
        inspectionVolume = inspectionCamera.GetComponent<CinemachineVolumeSettings>();
    }

    private void Update()
    {
        if (inputs.inspect)
        {
            inputs.InspectInput(false);

            if (firstPersonController.enabled && Time.timeScale > 0f)
            {
                ItemData item = hotbar.GetSelectedItem();

                if (item == null)
                {
                    item = playerInteractor.GetLookedAtItem();
                }

                StartItemInspection(item);

            }
        }

        if (isInspecting && inputs.back)
        {
            StopInspection();
        }

        if (isInspecting && inspectedItem && Time.timeScale > 0f)
        {
            RotateItem();
        }
    }

    public void SyncTextCamara(CinemachineBrain brain)
    {
        textCamera.fieldOfView = brain.OutputCamera.fieldOfView;
    }

    public void StartInspection(Transform inspectionViewPoint)
    {
        firstPersonController.enabled = false;
        playerInteractor.InteractPrompt.gameObject.SetActive(false);
        playerInteractor.enabled = false;
        hotbar.enabled = false;
        inspectionInputs.Enable();

        inspectionCamera.transform.SetPositionAndRotation(inspectionViewPoint.position, inspectionViewPoint.rotation);
        inspectionCamera.gameObject.SetActive(true);

        isInspecting = true;
    }

    public void StartItemInspection(ItemData item)
    {
        if (item == null || item.held == null)
        {
            return;
        }

        StartInspection(mainCameraBrain.OutputCamera.transform);
        hotbar.SetHeldItemActive(false);

        inspectedItem = Instantiate(item.held, inspectionObjectPoint.position, inspectionObjectPoint.rotation, inspectionObjectPoint);

        originalFocusTarget = inspectionVolume.FocusTarget;
        inspectionVolume.FocusTarget = inspectedItem.transform;
    }

    public void RotateItem()
    {
        float rotationAmount = rotationSpeed;

        if (playerInput.currentControlScheme == "Gamepad")
        {
            rotationAmount *= Time.deltaTime;
        }

        Vector2 rotation = inputs.look * rotationAmount;

        inspectedItem.transform.Rotate(inspectionCamera.transform.up, -rotation.x, Space.World);
        inspectedItem.transform.Rotate(inspectionCamera.transform.right, -rotation.y, Space.World);
    }

    public void StopInspection()
    {
        if (!isInspecting)
        {
            return;
        }

        isInspecting = false;
        inputs.BackInput(false);
        inspectionInputs.Disable();
        inspectionCamera.gameObject.SetActive(false);

        StartCoroutine(ReturnToPlayer());
    }

    IEnumerator ReturnToPlayer()
    {
        do
        {
            yield return null;
        }
        while (mainCameraBrain.IsBlending);

        if (inspectedItem)
        {
            Destroy(inspectedItem);
            inspectedItem = null;
            inspectionVolume.FocusTarget = originalFocusTarget;
        }

        hotbar.SetHeldItemActive(true);
        hotbar.enabled = true;

        inputs.InteractInput(false);
        firstPersonController.enabled = true;
        playerInteractor.enabled = true;
    }
}
