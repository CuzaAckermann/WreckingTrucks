using UnityEngine;

public class DeltaTimeCoefficientChanger : MonoBehaviour
{
    [Header("Slow Time")]
    [SerializeField] private KeyCode _slowTimeButton = KeyCode.Alpha1;
    [SerializeField] private float _slowTimeCoefficient;

    [Header("Normal Time")]
    [SerializeField] private KeyCode _normalTimeButton = KeyCode.Alpha2;
    [SerializeField] private float _normalTimeCoefficient;

    [Header("Fast Time")]
    [SerializeField] private KeyCode _fastTimeButton = KeyCode.Alpha3;
    [SerializeField] private float _fastTimeCoefficient;

    public float DeltaTimeCoefficient { get; private set; }

    public void Initialize()
    {
        DeltaTimeCoefficient = _normalTimeCoefficient;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_slowTimeButton))
        {
            OnDeltaTimeChanged(_slowTimeCoefficient);
        }
        else if (Input.GetKeyDown(_normalTimeButton))
        {
            OnDeltaTimeChanged(_normalTimeCoefficient);
        }
        else if (Input.GetKeyDown(_fastTimeButton))
        {
            OnDeltaTimeChanged(_fastTimeCoefficient);
        }
    }

    private void OnDeltaTimeChanged(float deltaTime)
    {
        DeltaTimeCoefficient = deltaTime;
    }
}