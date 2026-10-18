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


    StarterAssetsInputs inputs;
    Interact playerInteractor;
    InputActionMap inspectionInputs;

    bool isInspecting;

    void Awake()
    {
        inputs = firstPersonController.GetComponent<StarterAssetsInputs>();
        playerInteractor = firstPersonController.GetComponent<Interact>();
        inspectionInputs = firstPersonController.GetComponent<PlayerInput>().actions.FindActionMap("Inspection");
    }

    private void Update()
    {
        if (isInspecting && inputs.back)
        {
            StopInspection();
        }
    }

    public void SyncTextCamara(CinemachineBrain brain)
    {
        textCamera.fieldOfView = brain.OutputCamera.fieldOfView;
    }

    public void StartInspection(Transform inspectionViewPoint)
    {
        firstPersonController.enabled = false;
        playerInteractor.enabled = false;
        inspectionInputs.Enable();

        inspectionCamera.transform.SetPositionAndRotation(inspectionViewPoint.position, inspectionViewPoint.rotation);
        inspectionCamera.gameObject.SetActive(true);

        isInspecting = true;
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

        inputs.InteractInput(false);
        firstPersonController.enabled = true;
        playerInteractor.enabled = true;
    }
}
