public abstract class SwitchedApplicationState : ApplicationState, ISwitchedApplicationState
{
    public bool IsActive { get; private set; }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;

        Trigger();
    }
}