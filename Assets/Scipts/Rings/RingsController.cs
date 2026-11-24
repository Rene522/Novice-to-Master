
using UnityEngine;
using UnityEngine.Events;

public class RingsController : MonoBehaviour
{
    [SerializeField] private Ring innerRing;
    [SerializeField] private Ring middleRing;
    [SerializeField] private Ring outerRing;
    [SerializeField] private bool lockWhenSolved = true;
    [SerializeField] private UnityEvent onPuzzleSolved;

    public GameObject wall;
    public bool IsSolved { get; private set; }
    public Ring InnerRing => innerRing;
    public Ring MiddleRing => middleRing;
    public Ring OuterRing => outerRing;

    public void RotateInnerClockwise()
    {
        RotateRing(innerRing, 1);
    }

    public void RotateInnerAnticlockwise()
    {
        RotateRing(innerRing, -1);
    }

    public void RotateMiddleClockwise()
    {
        RotateRing(middleRing, 1);
    }

    public void RotateMiddleAnticlockwise()
    {
        RotateRing(middleRing, -1);
    }

    public void RotateOuterClockwise()
    {
        RotateRing(outerRing, 1);
    }

    public void RotateOuterAnticlockwise()
    {
        RotateRing(outerRing, -1);
    }

    private void RotateRing(Ring ring, int steps)
    {
        if (IsSolved && lockWhenSolved)
            return;

        if (ring == null)
        {
            Debug.LogWarning(
                "A ring reference is missing on RingController.",
                this
            );
            return;
        }

        if (ring.RotateSteps(steps))
            CheckSolution();
    }

    public void CheckSolution()
    {
        if (IsSolved)
            return;

        if (innerRing == null ||
            middleRing == null ||
            outerRing == null)
        {
            return;
        }

        if (innerRing.IsCorrect() &&
            middleRing.IsCorrect() &&
            outerRing.IsCorrect())
        {
            IsSolved = true;

            Debug.Log("Puzzle solved!", this);

            Destroy(wall);

            onPuzzleSolved?.Invoke();
        }
    }
}
