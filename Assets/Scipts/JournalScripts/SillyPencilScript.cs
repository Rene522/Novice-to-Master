using UnityEngine;

public class SillyPencilScript : MonoBehaviour
{
    public Sprite[] drawSprites = new Sprite[3];
    public Sprite[] eraseSprites = new Sprite[3];
    public SpriteRenderer sr;
    public bool MidAnim;
    public bool Animating;
    public bool isDraw;

    public float timer;
    public float reset;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer rend))
        {
            sr = rend;
        }
    }

    private void Update()
    {
        if (Animating)
        {
            timer -= Time.unscaledDeltaTime;

            if (timer <= 0)
            {
                timer = reset;

                if (MidAnim)
                {
                    UpdateSprites(1);
                    MidAnim = false;
                }
                else
                {
                    UpdateSprites(2);
                    Animating = false;
                }
            }
        }
    }

    public void UpdateSprites(int spriteNo)
    {
        if (isDraw)
        {
            sr.sprite = drawSprites[spriteNo];
        }
        else
        { 
            sr.sprite = eraseSprites[spriteNo];
        }
    }

    public void StartAnim(bool isDraw)
    {
        this.isDraw = isDraw;
        timer = reset;

        FlipSprites();
        UpdateSprites(0);

        Animating = true;
        MidAnim = true;
    }

    public void FlipSprites()
    {
        Sprite temp = drawSprites[0];
        drawSprites[0] = drawSprites[2];
        drawSprites[2] = temp;

        temp = eraseSprites[0];
        eraseSprites[0] = eraseSprites[2];
        eraseSprites[2] = temp;
    }
}
