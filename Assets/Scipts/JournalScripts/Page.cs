using UnityEngine;
using StarterAssets;
public class Page : MonoBehaviour
{
    [SerializeField] public StarterAssetsInputs inputs;
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

    public virtual void Update()
    {
        if (isTurning)
        {
            //Boosts sort order when moving
            FoldPage();
            sr.sortingOrder = 4;
        }
        else
        {
            if (isBehind)
            {

                sr.sortingOrder = 0;
            }
            else
            {
                sr.sortingOrder = 2;
            }
            Unfold();
        }
    }

    public virtual void FoldPage()
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
            sr.flipX = false;
        }

        if (targetPosition.x > maxSize[0])
        {
            //Basically clamps the x size to not let the player make a suuuper long page
            targetPosition.x = maxSize[0];
        }

        sr.size = targetPosition;
    }

    public virtual void Unfold()
    {
        sr.size = new Vector2(Mathf.Lerp(maxSize[0], sr.size.x, 0.8f), maxSize[1]);
    }

    public virtual void UpdateSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }
}
