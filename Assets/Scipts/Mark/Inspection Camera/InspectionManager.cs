using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectionManager : MonoBehaviour
{
    [SerializeField] GameObject inspectionPanel;
    [SerializeField] TMP_Text inspectionMessage;

    [SerializeField] PlayerInput playerInput;

    InspectionTrigger currentInspection;

    private void Awake()
    {
        HideMessage();
    }
    public void SetInspection(InspectionTrigger inspection)
    {
        currentInspection = inspection;
    }

    public void ShowMessage(string message, float duration = 0f)
    {
        CancelInvoke(nameof(HideMessage));

        if (string.IsNullOrWhiteSpace(message))
        {
            HideMessage();
            return;
        }

        InputAction action = playerInput.actions.FindAction("Inspection/Interact");
        string promtKey = action.GetBindingDisplayString(group: playerInput.currentControlScheme);
        inspectionMessage.text = message.Replace("{key}", promtKey);
        inspectionPanel.SetActive(true);

        if (duration > 0f)
        {
            Invoke(nameof(HideMessage), duration);
        }
    }

    public void HideMessage()
    {
        CancelInvoke(nameof(HideMessage));
        inspectionPanel.SetActive(false);
    }


    public void ActivateInspection()
    {
        if (currentInspection)
        {
            currentInspection.Activate();
        }
    }

    public void ClearInspection()
    {
        currentInspection = null;
        HideMessage();
    }
}
