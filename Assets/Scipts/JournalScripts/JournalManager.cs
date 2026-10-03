using UnityEngine;
using UnityEngine.InputSystem;

public class JournalManager : MonoBehaviour
{
    public bool isActive;
    public float buffer;
    public float bufferReset;

    public GameObject Journal;

    private void Update()
    {
        buffer -= Time.unscaledDeltaTime;

        if (Input.GetKey(KeyCode.J) && buffer <= 0)
        {
            if (!isActive)
            {
                //Brings up the Journal menu + activates all the objects and camera
                Time.timeScale = 0;
                Journal.SetActive(true);
                Cursor.lockState = CursorLockMode.None; //Unlocks the cursor to let the player flip pages :D
            }
            else
            {
                //Resumes time + hides the journal stuff
                Time.timeScale = 1.0f;
                Journal.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked; //relocks the cursor :>
            }

            isActive = !isActive;
            buffer = bufferReset; //Buffer to stop the journal from flashing constantly when the button is pressed
        }
    }
}
