using UnityEngine;

public class ShadowPuzzleManager : MonoBehaviour
{

    [SerializeField] private TestPlace placed;
    [SerializeField] public Transform PedestalOne;
    [SerializeField] public Transform PedestalTwo;

    [SerializeField] public Transform targetPedestalOne;
    [SerializeField] public Transform targetPedestalTwo;

    public bool correct = false;

    public void checkIfCorrect()
    {
        if (PedestalOne.position == targetPedestalOne.position && PedestalTwo.position == targetPedestalTwo.position)
        {
            if(PedestalOne.rotation == targetPedestalOne.rotation && PedestalTwo.rotation == targetPedestalTwo.rotation)
            {
                correct = true;
            }
            else
            {
                correct = false;
            }
        }

    }
}
