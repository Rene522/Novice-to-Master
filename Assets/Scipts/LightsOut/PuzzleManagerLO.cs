using System;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManagerLO : MonoBehaviour
{
    public Torch[,] torches = new Torch[5, 5];

    public void ClickTorch(int row, int col)
    {
        ToggleTorch(row, col);
        
        ToggleTorch(row -1 , col);
        ToggleTorch(row + 1, col);
        ToggleTorch(row, col - 1);
        ToggleTorch(row, col + 1);

        CheckWin();
    }

    void ToggleTorch(int row, int col)
    {
        if (row < 0 || row >= 5 || col < 0 || col >= 5)
        {
            return;
        }


        torches[row, col].Toggle();
    }

    void CheckWin()
    {
        foreach (Torch torch in torches)
        {
            if (torch.isOn)
            {
                return;
            }
            
        }
        Debug.Log("Puzzle Solved");
    }

    public void GenerateRandom(int moves)
    {
        for (int i = 0; i < moves; i++)
        {
            int row = UnityEngine.Random.Range(0, 5);
            int col = UnityEngine.Random.Range(0, 5);

            ClickTorch(row, col);
        }
    }
}
