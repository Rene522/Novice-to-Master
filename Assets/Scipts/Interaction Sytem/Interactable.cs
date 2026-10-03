using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    string promptText = "Press {key} To Interact";

    [SerializeField]
    string feedbackText = "";

    [SerializeField]
    bool allowNearbyPrompt = false;

    public virtual string GetPromptText()
    {
        return promptText;
    }

    public virtual string GetFeedbackText()
    {
        return feedbackText;
    }

    public virtual bool CanUseNearbyPrompt()
    {
        return allowNearbyPrompt;
    }

    public virtual void InteractEffect()
    {
        Debug.Log("default interaction triggered");
    }
}
