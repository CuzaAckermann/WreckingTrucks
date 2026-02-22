using System.Collections.Generic;

public class ApplicationStateStorage : Storage<ApplicationState>
{
    public ApplicationStateStorage(List<ApplicationState> applicationStates) : base(applicationStates)
    {
        
    }
}
