using UnityEngine;

public class Shield : MonoBehaviour
{
    public int Position; // 0 = 1, 1 = 45, 2 = 90, etc

    public float angle;

    public float timer;
    public float reset;

    public SpinnyObject spinner;

    public Sprite[] sprites = new Sprite[8];

    public FunnelType[] funnels  =  new FunnelType[8];
    public GameObject[] tempVisuals = new GameObject[8];

    public WheelManager manager;
    public bool DebugMode;

    private void Update()
    {
        if (DebugMode)
        {
            CheckKeys();
        }

        spinner.Spin(-angle);
    }

    public void CheckKeys()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (Input.GetKey(KeyCode.A))
            {
                UpdateAll(-1);

                timer = reset;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                UpdateAll(1);

                timer = reset;
            }
        }
    }

    public void UpdateAll(int positionMod)
    {
        UpdatePosition(positionMod);
        UpdateSprite();
        UpdateFunnels();
        TempUpdateVisuals();

        manager.Run();

        angle = Position * 45;
    }


    public void UpdatePosition(int addition)
    { 
        Position += addition;

        if (Position >= 8)
        {
            Position = 0;
        }
        else if (Position <= -1)
        {
            Position = 7;
        }
    }

    public void UpdateSprite()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer rend))
        { 
            rend.sprite = sprites[Position];
        }
    }

    public void UpdateFunnels()
    {
        funnels = new FunnelType[8] { FunnelType.None, FunnelType.None , FunnelType.None , FunnelType.None , FunnelType.None , FunnelType.None , FunnelType.None , FunnelType.None };

        SetFunnel(FunnelType.Open, Position);
        SetFunnel(FunnelType.Open, Position + 3);
        SetFunnel(FunnelType.Open, Position + 5);

    }

    public void SetFunnel(FunnelType type, int number)
    {
        if (number >= 8)
        { 
            number -= 8;
        }

        funnels[number] = type;
    }

    public void TempUpdateVisuals()
    {
        int counter = 0;
        foreach (FunnelType type in funnels)
        {
            if (type == FunnelType.Open && tempVisuals[counter] != null)
            {
                tempVisuals[counter].SetActive(false);
            }
            else if (tempVisuals[counter] != null)
            {
                tempVisuals[counter].SetActive(true);
            }

            counter++;
        }
    }
}


public enum FunnelType
{
    None,
    Closed,
    Mesh,
    Open
}
