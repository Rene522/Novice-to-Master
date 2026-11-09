using UnityEngine;

public class ApatheticEye : Eye
{
    protected override void Start()
    {
        CheckParticles();

        want.text = wantText;

        UpdateSprite();
    }
}
