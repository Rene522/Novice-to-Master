using UnityEngine;
using UnityEngine.Windows;

public class EdittablePage : Page
{
    public Page parent;
    public bool isLeft;

    public Sprite AltSprite;
    public Sprite OriginalAlt; //Not Required, but i want to use it for the silly little pencil script
    public SillyPencilScript sillyAnim; //Not required, but added for fun visuals

    public float timer;
    private float reset = 0.5f;

    public bool test;

    public override void Update()
    {
        //if (parent.isTurning)
        if (parent.isTurning)
        {
            FoldPage(); 
            sr.sortingOrder = 5;
        }
        else
        {
            if (parent.isBehind)
            {
                sr.sortingOrder = 1;
            }
            else
            {
                sr.sortingOrder = 3;
            }
            
            Unfold();
        }

        timer -= Time.unscaledDeltaTime;

        if (isLeft && UnityEngine.Input.GetKey(KeyCode.LeftArrow) && parent.isBehind == false && sr.sprite != null)
        {
            UpdateAltSprite();
        }
        else if (isLeft == false && UnityEngine.Input.GetKey(KeyCode.RightArrow) && parent.isBehind == false && sr.sprite != null)
        {
            UpdateAltSprite();
        }
    }

    public void UpdateAltSprite()
    {
        if (timer <= 0)
        {
            SwapSprites();

            if (pageSprite[0] == null)
            {
                CheckIfUpdateSprite(1);
            }
            else if (pageSprite[1] == null)
            {
                CheckIfUpdateSprite(0);
            }

            timer = reset;
        }
    }

    public void SwapSprites()
    {
        if (pageSprite[0] != null)
        {
            Sprite temp = pageSprite[0];
            pageSprite[0] = AltSprite;
            AltSprite = temp;
        }
        else if (pageSprite[1] != null)
        {
            Sprite temp = pageSprite[1];
            pageSprite[1] = AltSprite;
            AltSprite = temp;
        }
    }

    public void CheckIfUpdateSprite(int pos)
    { 
        //This is so the player cant edit a page "not visible"
        //before, with the debug testing (pressing arrows to edit), the sprite would suddenly appear on the wrong page

        if (sr.sprite == AltSprite)
        {
            sr.sprite = pageSprite[pos];
            CheckAnimation(); //not required, just for animation
        }
    }

    public void CheckAnimation()
    {
        //Not Required method, only for animation
        if (sillyAnim != null)
        {
            if (AltSprite == OriginalAlt)
            {
                sillyAnim.StartAnim(false);
            }
            else
            {
                sillyAnim.StartAnim(true);
            }
        }
    }

    public override void FoldPage()
    {
        targetPosition = new Vector2(journalCam.ScreenToWorldPoint(inputs.journalPoint).x, maxSize[1]);

        targetPosition.x = this.transform.position.x - targetPosition.x;

        //Gets a new position based on the x Coordinate of the mouse and with the height of the page
        if (targetPosition.x <= 0)
        {
            //Flips + updates the sprite if the page is dragged over to the other side
            targetPosition.x *= -1;
            UpdateSprite(pageSprite[0]);

            sr.flipX = true;
        }
        else
        {
            //Unflips the page if its on the starting side

            UpdateSprite(pageSprite[1]);
            sr.flipX = false;
        }

        if (targetPosition.x > maxSize[0])
        {
            //Basically clamps the x size to not let the player make a suuuper long page
            targetPosition.x = maxSize[0];
        }

        sr.size = targetPosition;
    }
}
