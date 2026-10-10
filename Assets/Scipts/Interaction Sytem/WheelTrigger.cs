using UnityEngine;

public class WheelTrigger : Interactable
{
    public WheelSpin spinner;
    public bool MoveLeft;
    public override void InteractEffect()
    {
        spinner.Move(MoveLeft);
    }
}
