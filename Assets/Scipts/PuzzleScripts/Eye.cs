using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;

public class Eye : PuzzleItem
{
    public Direction dir;
    public Sprite[] sprites = new Sprite[3];

    public TextMeshProUGUI see;
    public TextMeshProUGUI want;
    public string seeText;
    public string wantText;

    public ItemType goalType; //Im choosing to do ItemType over Types since i want to set the "want" text using this var

    protected override void Start()
    {
        CheckParticles();

        want.text = (wantText + goalType.ReturnText());
    }

    public void Update()
    {
        if (dir == Direction.Forward)
        {
            Look(transform.TransformDirection(Vector3.forward));
        }
        else if (dir == Direction.Left)
        {
            Look(transform.TransformDirection(Vector3.left));
        }
        else if (dir == Direction.Right)
        {
            Look(transform.TransformDirection(Vector3.right));
        }
    }

    public void Look(Vector3 Direction)
    {
        RaycastHit hit;


        if (Physics.Raycast(this.transform.position, Direction, out hit))
        {
            if (hit.transform.gameObject.TryGetComponent<ItemType>(out ItemType output))
            {
                see.text = (seeText + output.ReturnText());

                if (isSolved != true)
                {
                    if (output.type == goalType.type)
                    {
                        Solve();
                    }
                }
            }
        }
    }

    public void ChangeDirection(bool isLeft)
    {
        if (dir == Direction.Forward)
        {
            dir = isLeft ? Direction.Left : Direction.Right;
        }
        else if (dir == Direction.Left)
        {
            dir = isLeft ? Direction.Right : Direction.Forward;
        }
        else if (dir == Direction.Right)
        {
            dir = isLeft ? Direction.Forward : Direction.Left;
        }

        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer rend))
        {
            if (dir == Direction.Forward)
            {
                rend.sprite = sprites[1];
            }
            else if (dir == Direction.Left)
            {
                rend.sprite = sprites[0];
            }
            else if (dir == Direction.Right)
            {
                rend.sprite = sprites[2];
            }
        }
    }
}

public enum Direction
{ 
    Forward,
    Left,
    Right
}


//to do
//reflection?