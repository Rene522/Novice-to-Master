using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class JournalManager : MonoBehaviour
{
    [SerializeField] StarterAssetsInputs inputs;
    public bool isActive;
    public float buffer;
    public float bufferReset;

    public GameObject Journal;

    InputActionMap journalInputs;

    private void Awake()
    {
        journalInputs = inputs.GetComponent<PlayerInput>().actions.FindActionMap("Journal");
    }

    private void Update()
    {
        buffer -= Time.unscaledDeltaTime;

        if (inputs.journal && buffer <= 0)
        {
            inputs.JournalInput(false);
            if (!isActive)
            {
                //Brings up the Journal menu + activates all the objects and camera
                Time.timeScale = 0;
                Journal.SetActive(true);
                journalInputs.Enable();
                Cursor.lockState = CursorLockMode.None; //Unlocks the cursor to let the player flip pages :D
            }
            else
            {
                //Resumes time + hides the journal stuff
                Time.timeScale = 1.0f;
                journalInputs.Disable();
                inputs.JournalClickInput(false);
                Journal.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked; //relocks the cursor :>
            }

            isActive = !isActive;
            buffer = bufferReset; //Buffer to stop the journal from flashing constantly when the button is pressed
        }
    }
}
