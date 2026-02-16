public class ApplicationStateStorage
{
    public ApplicationStateStorage()
    {
        PrepareApplicationState = new PrepareApplicationState();
        StartApplicationState = new StartApplicationState();

        UpdateApplicationState = new UpdateApplicationState();

        StopApplicationState = new StopApplicationState();
        FinishApplicationState = new FinishApplicationState();

        FocusApplicationState = new FocusApplicationState();
        PauseApplicationState = new PauseApplicationState();
        QuitApplicationState = new QuitApplicationState();
    }

    public PrepareApplicationState PrepareApplicationState { get; private set; }

    public StartApplicationState StartApplicationState { get; private set; }

    public UpdateApplicationState UpdateApplicationState { get; private set; }

    public StopApplicationState StopApplicationState { get; private set; }

    public FinishApplicationState FinishApplicationState { get; private set; }

    public FocusApplicationState FocusApplicationState { get; private set; }

    public PauseApplicationState PauseApplicationState { get; private set; }

    public QuitApplicationState QuitApplicationState { get; private set; }
}
