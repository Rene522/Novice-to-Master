using UnityEngine;

public class ShieldTrigger : Interactable
{
    public Shield shield;
    public int moveAmount;

    public override void InteractEffect()
    {
        shield.UpdateAll(moveAmount);
    }
}
