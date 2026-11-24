using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    RaycastHit hit;
    public float range = 10f;
    public TextMeshProUGUI InteractPrompt;

    public TextMeshProUGUI PlacePrompt;

    [SerializeField] float feedbackDuration = 2f;

    StarterAssetsInputs inputs;
    PlayerInput playerInput;

    bool isShowingFeedback;
    float feedbackTimer;

    TestPlace currentItem;

    void Start()
    {
        inputs = GetComponentInParent<StarterAssetsInputs>();
        playerInput = GetComponentInParent<PlayerInput>();

        if (InteractPrompt)
        {
            InteractPrompt.gameObject.SetActive(false);
        }

        if (PlacePrompt)
        {
            PlacePrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!inputs || !playerInput || !InteractPrompt || !Camera.main)
        {
            return;
        }

        bool interactPressed = inputs.interact;
        inputs.InteractInput(false);

        HandlePlacement();

        if (isShowingFeedback)
        {
            feedbackTimer -= Time.deltaTime;

            if (feedbackTimer <= 0f)
            {
                isShowingFeedback = false;
                InteractPrompt.gameObject.SetActive(false);
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

        InteractPrompt.gameObject.SetActive(false);
    }

    public ItemData GetLookedAtItem()
    {
        RaycastHit inspectionHit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out inspectionHit, range))
        {
            if (inspectionHit.collider.CompareTag("Interactable"))
            {
                PickupItem pickupItem = inspectionHit.collider.GetComponent<PickupItem>();
                if (pickupItem && pickupItem.isActiveAndEnabled)
                {
                    return pickupItem.GetItem();
                }
            }
        }
        return null;
    }

    bool HandleInteractable(Interactable interactableObject, bool interactPressed)
    {
        if (!interactableObject || !interactableObject.isActiveAndEnabled)
        {
            return false;
        }

        string promptText = interactableObject.GetPromptText();

        if (string.IsNullOrWhiteSpace(promptText))
        {
            return false;
        }

        InputAction action = playerInput.actions.FindAction("Player/Interact");

        if (action == null)
        {
            return false;
        }

        string promptKey = action.GetBindingDisplayString(group: playerInput.currentControlScheme);

        InteractPrompt.text = promptText.Replace("{key}", promptKey);
        InteractPrompt.gameObject.SetActive(true);

        if (interactPressed)
        {
            string feedbackText = interactableObject.GetFeedbackText();

            Debug.Log("hit interactable");
            Debug.Log("activate Effect");

            interactableObject.InteractEffect();

            if (string.IsNullOrWhiteSpace(feedbackText))
            {
                InteractPrompt.gameObject.SetActive(false);
            }
            else
            {
                InteractPrompt.text = feedbackText;
                feedbackTimer = feedbackDuration;
                isShowingFeedback = true;
                //Debug.Log("Not interactable");
                InteractPrompt.gameObject.SetActive(true);

            }
        }

        return true;
    }

    void HandlePlacement()
    {
        bool placePressed = inputs.place;
        inputs.PlaceInput(false);

        if (!PlacePrompt)
            return;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit placementHit,range))
        {
            TestPlace placeable = placementHit.collider.GetComponent<TestPlace>();

            if (placeable != null)
            {
                string prompt = placeable.GetPlacePrompt();

                if (!string.IsNullOrWhiteSpace(prompt))
                {
                    InputAction action = playerInput.actions.FindAction("Player/Place");

                    if (action != null)
                    {
                        string key = action.GetBindingDisplayString(group: playerInput.currentControlScheme);

                        PlacePrompt.text = prompt.Replace("{key}", key);
                        PlacePrompt.gameObject.SetActive(true);

                        if (placePressed)
                        {
                            placeable.PlaceEffect();
                        }

                        return;
                    }
                }
            }
        }

        PlacePrompt.gameObject.SetActive(false);
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


