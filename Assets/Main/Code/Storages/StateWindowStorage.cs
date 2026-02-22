using System.Collections.Generic;

public class StateWindowStorage : Storage<StateWindowBase>, IWindowStorage
{
    public StateWindowStorage(List<StateWindowBase> storagables) : base(storagables)
    {

    }
}