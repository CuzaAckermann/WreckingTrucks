using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    [Header("Testing Abilities")]
    [SerializeField] private GlobalTime _globalTime;
    [SerializeField] private CartrigeBoxManipulator _cartrigeBoxManipulator;
    
    public void Init(StopwatchCreator stopwatchCreator,
                     DeltaTimeCoefficientDefiner deltaTimeCoefficientDefiner,
                     DispencerCreator cartrigeBoxDispencerCreator)
    {
        _globalTime.Init(stopwatchCreator.Create(),
                         deltaTimeCoefficientDefiner);
        _cartrigeBoxManipulator.Init(stopwatchCreator.Create(),
                                     cartrigeBoxDispencerCreator);
    }
}