using UnityEngine;
using UnityEngine.Events;

public class InspectionTrigger : Interactable
{
    [SerializeField] InspectionCameraController inspectionCameraController;
    [SerializeField] Transform inspectionViewPoint;
    [SerializeField] bool usePlayerView;

    [SerializeField] InspectionManager inspectionManager;
    [SerializeField, TextArea] string inspectionMessage;
    [SerializeField] bool canInteract;
    [SerializeField] string interactionPrompt = "Press {key} to push";

    [SerializeField] UnityEvent onInspectionAction = new UnityEvent();

    bool hasActivated;


    public override void InteractEffect()
    {
        if (!inspectionCameraController)
        {
            return;
        }

        inspectionCameraController.StartInspection(inspectionViewPoint, usePlayerView, transform);
        inspectionManager.SetInspection(this);

        if (canInteract)
        {
            if (!hasActivated)
            {
                inspectionManager.ShowMessage(interactionPrompt);
            }
        }
        else
        {
            inspectionManager.ShowMessage(inspectionMessage);
        }
    }

    public void Activate()
    {
        if (!canInteract || hasActivated || onInspectionAction.GetPersistentEventCount() == 0)
        {
            return;
        }

        hasActivated = true;
        onInspectionAction.Invoke();
        inspectionManager.HideMessage();
    }

    public bool HasActivated()
    {
        return hasActivated;
    }
}
