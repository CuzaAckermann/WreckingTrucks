using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    [Header("Testing Abilities")]
    [SerializeField] private GlobalTime _globalTime;
    [SerializeField] private CartrigeBoxManipulator _cartrigeBoxManipulator;
    
    public void Init(Stopwatch stopwatch,
                     IAmount deltaTimeFactor,
                     EventBus eventBus,
                     DeveloperInput backgroundInput,
                     ApplicationStateStorage applicationStateStorage)
    {
        _globalTime.Init(stopwatch,
                         deltaTimeFactor);
        _cartrigeBoxManipulator.Init(eventBus, backgroundInput);

        eventBus.Invoke(new CreatedSignal<ICommandCreator>(_cartrigeBoxManipulator));
    }
}