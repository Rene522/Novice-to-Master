using UnityEngine;

public class TorchInteract : Interactable
{
    public int row;
    public int col;
    public PuzzleManagerLO puzzleManager;


    public override void InteractEffect()
    {
        puzzleManager.ClickTorch(row, col);
    }
}
