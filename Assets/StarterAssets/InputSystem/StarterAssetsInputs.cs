using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool interact;
        public bool journal;
        public bool journalClick;
        public Vector2 journalPoint;
        public bool inspect;
        public bool back;
        public bool slot1;
        public bool slot2;
        public bool slot3;
        public bool slot4;
        public float inventoryScroll;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnJournal(InputValue value)
        {
            JournalInput(value.isPressed);
        }

        public void OnClick(InputValue value)
        {
            JournalClickInput(value.isPressed);
        }
        public void OnPoint(InputValue value)
        {
            JournalPointInput(value.Get<Vector2>());
        }
        public void OnInspect(InputValue value)
        {
            InspectInput(value.isPressed);
        }
        public void OnBack(InputValue value)
        {
            BackInput(value.isPressed);
        }
        public void OnSlot1(InputValue value)
        {
            Slot1Input(value.isPressed);
        }
        public void OnSlot2(InputValue value)
        {
            Slot2Input(value.isPressed);
        }
        public void OnSlot3(InputValue value)
        {
            Slot3Input(value.isPressed);
        }
        public void OnSlot4(InputValue value)
        {
            Slot4Input(value.isPressed);
        }
        public void OnInventoryScroll(InputValue value)
        {
            InventoryScrollInput(value.Get<float>());
        }
#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void JournalInput(bool newJournalState)
        {
            journal = newJournalState;
        }

        public void JournalClickInput(bool newJournalClickState)
        {
            journalClick = newJournalClickState;
        }

        public void JournalPointInput(Vector2 newJournalPoint)
        {
            journalPoint = newJournalPoint;
        }
        public void InspectInput(bool newInspectState)
        {
            inspect = newInspectState;
        }
        public void BackInput(bool newBackState)
        {
            back = newBackState;
        }
        public void Slot1Input(bool newSlot1State)
        {
            slot1 = newSlot1State;
        }
        public void Slot2Input(bool newSlot2State)
        {
            slot2 = newSlot2State;
        }
        public void Slot3Input(bool newSlot3State)
        {
            slot3 = newSlot3State;
        }
        public void Slot4Input(bool newSlot4State)
        {
            slot4 = newSlot4State;
        }
        public void InventoryScrollInput(float newInventoryScroll)
        {
            inventoryScroll = newInventoryScroll;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState
                ? CursorLockMode.Locked
                : CursorLockMode.None;
        }
    }
}
