using UnityEngine;

public class TorchGridSpawner : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;

    public int startingMoves;
    public float spacingX;
    public float spacingY;

    public GameObject torchPrefab;
    public PuzzleManagerLO PM;

    private void Start()
    {
        GenerateGrid();
        PM.GenerateRandom(startingMoves);
    }

    void GenerateGrid()
    {
        PM.torches = new Torch[rows, cols];

        for (int row  = 0; row < rows; row++ )
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col * spacingX, 0f, row * spacingY);

                GameObject obj = Instantiate(torchPrefab, transform.position + position, transform.rotation, transform);

                TorchInteract torchInteract = obj.GetComponent<TorchInteract>();

                torchInteract.row = row;
                torchInteract.col = col;
                torchInteract.puzzleManager = PM;

                Torch torch = obj.GetComponent<Torch>();

                PM.torches[row, col] = torch;
            }

            
        }
    }

}
