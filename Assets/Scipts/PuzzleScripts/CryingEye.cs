using UnityEngine;

public class CryingEye : Eye
{
    public string altSeeText;
    public bool isCrying;

    public Sprite[] CryingClearSprites = new Sprite[3];
    public Sprite[] CryingMossSprites = new Sprite[3];

    protected override void Start()
    {
        base.Start();

        CheckBehind();
    }

    public override void SetText()
    {
        if (isCrying)
        {
            see.text = altSeeText;
        }
        else
        {
            see.text = seeText;
        }
    }

    public override void SetText(string input)
    {
        if (isCrying)
        {
            see.text = altSeeText;
        }
        else
        {
            see.text = seeText + input;
        }
    }

    public override void Update()
    {
        base.Update();

        CheckBehind();
        UpdateSprite();
    }

    public override void UpdateSprite()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer rend))
        {
            if (isCrying)
            {
                if (isMoss)
                {
                    SelectSprite(rend, CryingMossSprites);
                }
                else
                {
                    SelectSprite(rend, CryingClearSprites);
                }
            }
            else
            {
                if (isMoss)
                {
                    SelectSprite(rend, mossSprites);
                }
                else
                {
                    SelectSprite(rend, clearSprites);
                }
            }
        }
    }
    public void CheckBehind()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.transform.position - new Vector3(1, 0, 0), transform.TransformDirection(Vector3.back), out hit))
        {
            if (hit.transform.gameObject.TryGetComponent<ItemType>(out ItemType output))
            {
                if (output.GetTypes() == Types.Water)
                {
                    isCrying = true;
                    SetText();
                }
                else
                {
                    isCrying = false;
                }
            }
            else
            {
                isCrying = false;
            }
        }
    }
}