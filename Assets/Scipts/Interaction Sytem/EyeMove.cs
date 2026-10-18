using UnityEngine;

public class EyeMove : Interactable
{
    public Eye eye;
    public override void InteractEffect()
    {
        eye.ChangeDirection(true);
    }
}
