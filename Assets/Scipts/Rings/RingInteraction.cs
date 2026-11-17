
using UnityEngine;
using UnityEngine.InputSystem;

public class RingInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 4f;
    [SerializeField] private LayerMask interactionLayers = ~0;
    [SerializeField] private RingsController puzzleController;

    private Ring targetedRing;
    public AudioManager audioManager;

    private void Update()
    {
        DetectRing();

        if (targetedRing == null)
            return;

        if (Keyboard.current == null)
            return;

        // E rotates the targeted ring clockwise.
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            RotateTargetedRing(1);
            audioManager.PlaySFX("ringTurn");
        }

        // Q rotates the targeted ring anticlockwise.
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            RotateTargetedRing(-1);
        }
    }

    private void DetectRing()
    {
        targetedRing = null;

        if (playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers,
            QueryTriggerInteraction.Ignore))
        {
            targetedRing = hit.collider.GetComponentInParent<Ring>();
        }
    }

    private void RotateTargetedRing(int direction)
    {
        if (puzzleController == null || targetedRing == null)
            return;

        if (direction > 0)
        {
            if (targetedRing == GetInnerRing())
                puzzleController.RotateInnerClockwise();
            else if (targetedRing == GetMiddleRing())
                puzzleController.RotateMiddleClockwise();
            else if (targetedRing == GetOuterRing())
                puzzleController.RotateOuterClockwise();
        }
        else
        {
            if (targetedRing == GetInnerRing())
                puzzleController.RotateInnerAnticlockwise();
            else if (targetedRing == GetMiddleRing())
                puzzleController.RotateMiddleAnticlockwise();
            else if (targetedRing == GetOuterRing())
                puzzleController.RotateOuterAnticlockwise();
        }
    }

    private Ring GetInnerRing()
    {
        return puzzleController.InnerRing;
    }

    private Ring GetMiddleRing()
    {
        return puzzleController.MiddleRing;
    }

    private Ring GetOuterRing()
    {
        return puzzleController.OuterRing;
    }

    private Ring GetRingField(string fieldName)
    {
        var field = typeof(RingsController).GetField(
            fieldName,
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic
        );

        if (field == null)
            return null;

        return field.GetValue(puzzleController) as Ring;
    }
}

