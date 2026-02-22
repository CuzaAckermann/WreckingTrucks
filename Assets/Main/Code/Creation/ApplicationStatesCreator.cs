using System.Collections.Generic;

public class ApplicationStatesCreator
{
    public List<ApplicationState> Create()
    {
        return new List<ApplicationState>
        {
            new PrepareApplicationState(),
            new StartApplicationState(),
            new UpdateApplicationState(),
            new StopApplicationState(),
            new FinishApplicationState(),
            new FocusApplicationState(),
            new PauseApplicationState(),
            new QuitApplicationState()
        };
    }
}