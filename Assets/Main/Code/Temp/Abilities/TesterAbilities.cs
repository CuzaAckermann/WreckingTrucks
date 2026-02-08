using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    [Header("Testing Abilities")]
    [SerializeField] private GlobalTime _globalTime;
    [SerializeField] private CartrigeBoxManipulator _cartrigeBoxManipulator;
    
    public void Init(Stopwatch stopwatch,
                     DeltaTimeCoefficientDefiner deltaTimeCoefficientDefiner,
                     EventBus eventBus,
                     BackgroundInput backgroundInput)
    {
        _globalTime.Init(stopwatch,
                         deltaTimeCoefficientDefiner);
        _cartrigeBoxManipulator.Init(eventBus, backgroundInput);

        eventBus.Invoke(new CreatedSignal<ICommandCreator>(_cartrigeBoxManipulator));
    }
}