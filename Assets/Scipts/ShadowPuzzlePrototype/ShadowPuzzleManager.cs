using UnityEngine;

public class ShadowPuzzleManager : MonoBehaviour
{

    [SerializeField] private TestPlace placed;
    [SerializeField] public Transform PedestalOneTransform;
    [SerializeField] public Transform PedestalTwoTransform;

    [SerializeField] public TestPlace PedestalOne;
    [SerializeField] public TestPlace PedestalTwo;

    [SerializeField] public Transform targetPedestalOne;
    [SerializeField] public Transform targetPedestalTwo;

    [SerializeField] public GameObject pedestalOneObject;

    [SerializeField] public GameObject pedestalTwoObject;

    public bool correct = false;

    public void checkIfCorrect()
    {
        bool correctItems = PedestalOne.placedItemPrefab == pedestalOneObject && PedestalTwo.placedItemPrefab == pedestalTwoObject;

        bool correctRotations = PedestalOneTransform.rotation == targetPedestalOne.rotation && PedestalTwoTransform.rotation == targetPedestalTwo.rotation;

        bool correctPositions = PedestalOneTransform.position == targetPedestalOne.position && PedestalTwoTransform.position == targetPedestalTwo.position;

        correct = correctItems && correctRotations && correctPositions;

        if (correct)
        {
            Debug.Log("HURRRAY");
        }
        else
        {
            Debug.Log("NO");
        }
    }
}
