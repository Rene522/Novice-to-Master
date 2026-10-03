using UnityEngine;

public class Page : MonoBehaviour
{
    public SpriteRenderer sr;

    public int[] maxSize = new int[2]; //max size for tiling reasons
    public Sprite[] pageSprite;

    public bool isTurning; //Is the page being moved
    public bool isBehind; //Should the page have a lower order in renderering because its behind another page

    public Vector2 targetPosition;
    public Camera journalCam;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer rend))
        {
            sr = rend;
        }
    }

    private void Update()
    {
        if (isTurning)
        {
            //Boosts sort order when moving
            sr.sortingOrder = 1;
            FoldPage();
        }
        else
        {
            if (isBehind)
            {
                //lowers sort order depending on if its the top page or not
                sr.sortingOrder = -2;
            }
            else
            {
                sr.sortingOrder = -1;
            }
            Unfold();
        }
    }

    public void FoldPage()
    {
        targetPosition = new Vector2(journalCam.ScreenToWorldPoint(Input.mousePosition).x, maxSize[1]);

        targetPosition.x = this.transform.position.x - targetPosition.x;

        //Gets a new position based on the x Coordinate of the mouse and with the height of the page

        if (targetPosition.x <= 0)
        {
            //Flips + updates the sprite if the page is dragged over to the other side
            targetPosition.x *= -1;
            sr.sprite = pageSprite[0];
            sr.flipX = true;
        }
        else
        {
            //Unflips the page if its on the starting side
            sr.sprite = pageSprite[1];
            sr.flipX = false;
        }

        if (targetPosition.x > maxSize[0])
        {
            //Basically clamps the x size to not let the player make a suuuper long page
            targetPosition.x = maxSize[0];
        }

        sr.size = targetPosition;
    }

    public void Unfold()
    {
        //Returns the page to its max size (or resting point) when not being dragged

        sr.size = new Vector2(Mathf.Lerp(maxSize[0], sr.size.x, 0.8f), maxSize[1]);
    }
}
