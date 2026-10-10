using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WheelManager : MonoBehaviour
{
    public Shield shield;
    public WheelSpin wheel;

    public Vector3[] TempPositions = new Vector3[8];

    public bool checkForFall;

    public List<WheelObject> wheelObjects = new List<WheelObject>();

    public void Run()
    {
        MoveItems();
    }

    public void UpdateItems(int input)
    {
        if (wheelObjects != null)
        {
            foreach (WheelObject obj in wheelObjects)
            {
                obj.RelativePosition += input;
                if (obj.RelativePosition >= 8)
                {
                    obj.RelativePosition -= 8;
                }
                else if (obj.RelativePosition < 0)
                {
                    obj.RelativePosition += 8;
                }

                obj.desiredPos = TempPositions[obj.RelativePosition];
            }
        }

        Run();
    }

    public void MoveItems()
    {
        List<int> fallFrom = new List<int>();
        List<int> fallTo = new List<int>();

        if (Compare(0))
        {
            checkForFall = true;
            fallFrom.Add(0);
        }
        if (Compare(1))
        {
            checkForFall = true;
            fallFrom.Add(1);
        }
        if (Compare(7))
        {
            checkForFall = true;
            fallFrom.Add(7);
        }

        if (checkForFall)
        {
            if (Compare(4))
            {
                fallTo.Add(4);
            }
            if (Compare(5))
            {
                fallTo.Add(5);
            }
            if (Compare(3))
            {
                fallTo.Add(3);
            }
        }

        if (fallFrom != null && fallTo != null)
        {
            for (int i = 0; i < fallFrom.Count; i++)
            {
                for (int j = 0; j < fallTo.Count; j++)
                {
                    CheckItems(fallFrom[i], fallTo[j]);
                }
            }
        }

        //017,453
    }

    public bool Compare(int mod)
    {
        if (shield.funnels[mod] == FunnelType.Open)
        {
            return true;
        }

        return false;
    }

    public void CheckItems(int startVal, int endVal)
    {
        foreach (WheelObject obj in wheelObjects)
        {
            if (obj.RelativePosition == startVal)
            {
                obj.RelativePosition = endVal;
                obj.desiredPos = TempPositions[obj.RelativePosition];
                obj.isCenter = true;
            }
        }
    }

    public int WheelAddCheck(int mod)
    {
        int temp = mod - wheel.Position;

        if (temp >= 8)
        {
            temp -= 8;
        }
        else if (temp < 0)
        {
            temp += 8;
        }

        return temp;
    }
}

//change obj to objective position + relative position
//obj pos is where it is relative to the wheel
//rel pos is where it is relative to the player