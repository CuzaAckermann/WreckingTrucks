using UnityEngine;

public class AIClickerCreator : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private float _startDelay = 3;
    [SerializeField, Range(0.1f, 2)] private float _minFrequency;
    [SerializeField, Range(2, 5)] private float _maxFrequency;

    public AIClicker CreateAIClicker()
    {
        return new AIClicker(_startDelay, _minFrequency, _maxFrequency);
    }
}