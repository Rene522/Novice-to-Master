using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    public int Position; // 0 = 1, 1 = 45, 2 = 90, etc

    public float angle;

    public float timer;
    public float reset;

    public SpinnyObject spinner;
    public WheelManager manager;

    public bool DebugTestMode;

    private void Update()
    {
        if (DebugTestMode)
        {
            CheckKeys();
        }

        spinner.Spin(angle);
    }

    public void CheckKeys()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(true);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Move(false);
            }
        }
    }

    public void Move(bool isLeft)
    {
        if (isLeft)
        {
            UpdatePosition(1);

            manager.UpdateItems(-1);

            timer = reset;
            angle = Position * 45;
        }
        else
        {
            UpdatePosition(-1);

            manager.UpdateItems(1);

            timer = reset;
            angle = Position * 45;
        }
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
}
