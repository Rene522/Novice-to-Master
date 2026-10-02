using TMPro;
using UnityEngine;

public class Interact : MonoBehaviour
{
    RaycastHit hit;
    public float range = 100f;
    public TextMeshProUGUI InteractPrompt;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {




        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {


            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                string promptkey = interactableObject.interactKey.ToString();
                KeyCode PromptKeycode = interactableObject.interactKey;
                InteractPrompt.text = ("Press " + promptkey + " To Interact");
                InteractPrompt.gameObject.SetActive(true);
                if (Input.GetKeyDown(PromptKeycode))
                {
                    Debug.Log("hit interactable");

                    if (interactableObject != null)
                    {
                        Debug.Log("activate Effect");
                        interactableObject.InteractEffect();
                    }
                }

            }
            else
            {
                Debug.Log("Not interactable");
                InteractPrompt.gameObject.SetActive(false);

            }



        }
        else 
        {
            InteractPrompt.gameObject.SetActive(false);
        }
        
    }
    
}

