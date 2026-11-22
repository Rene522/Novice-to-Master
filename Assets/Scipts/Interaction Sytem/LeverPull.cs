using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LeverPull : MonoBehaviour
{
    [SerializeField] private InspectionCameraController _cameraController;

    [SerializeField] private Transform LeverAxis;
    [SerializeField] private float sensitivity;
    [SerializeField] private float maxPull;
    [SerializeField] private float minPull;

    [SerializeField] private UnityEvent onLeverPulled;
    [SerializeField] private UnityEvent onInteractionFinished;

    [SerializeField] private float currentPull;
    void Start()
    {
        currentPull = minPull;
    }

    // Update is called once per frame
    void Update()
    {
        PullLever();
    }


    public void PullLever()
    {
        if (_cameraController.isInspecting)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            currentPull -= mouseDelta.y * sensitivity;

            currentPull = Mathf.Clamp(currentPull, minPull, maxPull);

            LeverAxis.localRotation = Quaternion.Euler(currentPull, 0f, 0f);

            if (currentPull >= maxPull)
            {
                FinishInteraction();
            }
        }

        else { return; }
    }

    public void FinishInteraction()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        onLeverPulled?.Invoke();
        onInteractionFinished?.Invoke();
        _cameraController.StopInspection();
    }

}
