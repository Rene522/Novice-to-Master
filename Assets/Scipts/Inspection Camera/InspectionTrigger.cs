using UnityEngine;

public class InspectionTrigger : Interactable
{
    [SerializeField] InspectionCameraController inspectionCameraController;
    [SerializeField] Transform inspectionViewPoint;

    public override void InteractEffect()
    {
        if (!inspectionCameraController)
        {
            return;
        }

        inspectionCameraController.StartInspection(inspectionViewPoint);
    }
}
