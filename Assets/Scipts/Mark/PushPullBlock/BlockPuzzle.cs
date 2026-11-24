using TMPro;
using UnityEngine;

/// <summary>
/// This class represents a block puzzle that consists of multiple block spaces. The puzzle is considered complete when all the block spaces are filled with the correct blocks. 
/// It also manages the display of a completion message when the puzzle is solved.
/// </summary>
public class BlockPuzzle : MonoBehaviour
{
    [Tooltip("An array of block spaces that must be filled to complete the puzzle.")]
    [SerializeField] BlockSpace[] spaces = new BlockSpace[0];
    [Tooltip("The message to display when the puzzle is completed.")]
    [SerializeField] TMP_Text completionMessage;

    /// <summary>
    /// Called once per frame to check if the puzzle is completed and update the completion message accordingly.
    /// </summary>
    private void Update()
    {
        if (!completionMessage) // If the completion message is not assigned, do nothing
        {
            return;
        }

        bool allFilled = spaces.Length > 0; // Assume all spaces are filled if there are any spaces to check

        foreach (BlockSpace space in spaces) // Iterate through each block space in the spaces array
        {
            if (!space || !space.IsFilled()) // If the space is null or not filled, set allFilled to false and break out of the loop
            {
                allFilled = false;
                break;
            }
        }
        completionMessage.gameObject.SetActive(allFilled); // Set the active state of the completion message based on whether all spaces are filled
    }
}
