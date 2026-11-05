using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    RaycastHit hit;
    public float range = 100f;
    public TextMeshProUGUI RotatePrompt;

    [SerializeField] float feedbackDuration = 2f;

    StarterAssetsInputs inputs;
    PlayerInput playerInput;

    bool isShowingFeedback;
    float feedbackTimer;

    void Start()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        playerInput = GetComponentInParent<PlayerInput>();

        if (RotatePrompt)
        {
            RotatePrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!inputs || !playerInput || !RotatePrompt || !Camera.main)
        {
            return;
        }

        bool interactPressed = inputs.rotate;
        inputs.RotateInput(false);

        if (isShowingFeedback)
        {
            feedbackTimer -= Time.deltaTime;

            if (feedbackTimer <= 0f)
            {
                isShowingFeedback = false;
                RotatePrompt.gameObject.SetActive(false);
            }

            return;
        }



        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable interactableObject =
                    hit.collider.GetComponent<Interactable>();

                if (HandleInteractable(interactableObject, interactPressed))
                {
                    return;
                }
            }
        }

        if (CheckNearbyColliders(interactPressed))
        {
            return;
        }

        RotatePrompt.gameObject.SetActive(false);
    }

    bool HandleInteractable(Interactable interactableObject, bool interactPressed)
    {
        if (!interactableObject || !interactableObject.isActiveAndEnabled)
        {
            return false;
        }

        if (!(interactableObject is RotateObject))
        {
            return false;
        }

        string promptText = "Press R to rotate";

        if (string.IsNullOrWhiteSpace(promptText))
        {
            return false;
        }

        InputAction action = playerInput.actions.FindAction("Player/Rotate");

        if (action == null)
        {
            return false;
        }

        string promptKey = action.GetBindingDisplayString(group: playerInput.currentControlScheme);

        RotatePrompt.text = promptText.Replace("{key}", promptKey);
        RotatePrompt.gameObject.SetActive(true);

        if (interactPressed)
        {
            string feedbackText = interactableObject.GetFeedbackText();

            Debug.Log("hit interactable");
            Debug.Log("activate Effect");

            interactableObject.InteractEffect();

            if (string.IsNullOrWhiteSpace(feedbackText))
            {
                RotatePrompt.gameObject.SetActive(false);
            }
            else
            {
                RotatePrompt.text = feedbackText;
                feedbackTimer = feedbackDuration;
                isShowingFeedback = true;
                //Debug.Log("Not interactable");
                RotatePrompt.gameObject.SetActive(true);

            }
        }

        return true;
    }

    bool CheckNearbyColliders(bool interactPressed)
    {
        Collider[] colliders = Physics.OverlapSphere(Camera.main.transform.position, range);

        foreach (Collider nearbyCollider in colliders)
        {
            if (!nearbyCollider.CompareTag("Interactable"))
            {
                continue;
            }

            Interactable interactableObject = nearbyCollider.GetComponent<Interactable>();

            if (!interactableObject || !interactableObject.CanUseNearbyPrompt())
            {
                continue;
            }

            Vector3 directionToObject = nearbyCollider.bounds.center - Camera.main.transform.position;

            if (Vector3.Dot(Camera.main.transform.forward, directionToObject.normalized) < 0.5f)
            {
                continue;
            }

            if (HandleInteractable(interactableObject, interactPressed))
            {
                return true;
            }
        }
        return false;
    }
}
