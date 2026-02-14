public class TickRegulator
{
    private readonly TickEngine _tickEngine;
    private readonly IDeveloperInput _input;

    public TickRegulator(TickEngine tickEngine, IDeveloperInput input)
    {
        Validator.ValidateNotNull(tickEngine, input);

        _tickEngine = tickEngine;
        _input = input;
    }

    public void Enter()
    {
        _tickEngine.Pause();
    }

    public void Exit()
    {
        _tickEngine.Continue();
    }

    private void OnInputStateChanged(InputState<IInput> inputState)
    {
        if (inputState is not LevelSelectionInputState ||
            inputState is not PausedInputState)
        {
            return;
        }

        _tickEngine.Pause();
    }
}