
using System.Collections;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int symbolCount = 4;
    [SerializeField, Min(0)]
    private int correctPosition = 0;
    [SerializeField]
    private Vector3 localRotationAxis = Vector3.forward;
    [SerializeField, Min(0.01f)]
    private float rotationDuration = 0.2f;

    public int SymbolCount => symbolCount;
    public int CurrentPosition => currentPosition;
    public int CorrectPosition => correctPosition;

    private int currentPosition;
    private Quaternion startingRotation;
    private Coroutine rotationCoroutine;
    private bool isRotating;

    private void Awake()
    {
        symbolCount = Mathf.Max(1, symbolCount);
        currentPosition = Mathf.Clamp(currentPosition, 0, symbolCount - 1);
        correctPosition = Mathf.Clamp(correctPosition, 0, symbolCount - 1);

        startingRotation = transform.localRotation;
        ApplyRotation();
    }

    public bool RotateClockwise()
    {
        return RotateSteps(1);
    }

    public bool RotateAnticlockwise()
    {
        return RotateSteps(-1);
    }

    public bool RotateSteps(int steps)
    {
        if (isRotating || steps == 0)
            return false;

        currentPosition = (currentPosition + steps)
            % symbolCount;

        if (currentPosition < 0)
            currentPosition += symbolCount;

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(AnimateRotation());

        return true;
    }

    public bool IsCorrect()
    {
        return currentPosition == correctPosition;
    }

    private IEnumerator AnimateRotation()
    {
        isRotating = true;

        Quaternion start = transform.localRotation;

        Quaternion target = GetTargetRotation();

        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / rotationDuration
            );
            t = t * t * (3f - 2f * t);

            transform.localRotation = Quaternion.Slerp(
                start, target, t
            );

            yield return null;
        }

        transform.localRotation = target;

        isRotating = false;
        rotationCoroutine = null;
    }


    private Quaternion GetTargetRotation()
    {
        Vector3 axis = localRotationAxis.normalized;

        float angle = 360f * currentPosition / symbolCount;

        return startingRotation *
            Quaternion.AngleAxis(angle, axis);
    }


    private void ApplyRotation()
    {
        transform.localRotation = GetTargetRotation();
    }
}

