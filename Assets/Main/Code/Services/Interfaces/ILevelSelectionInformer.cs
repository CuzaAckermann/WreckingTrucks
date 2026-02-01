using System;

public interface ILevelSelectionInformer
{
    public event Action<int> IndexSelected;

    public event Action NextSelected;
    
    public event Action PreviousSelected;
    
    public event Action RecreateSelected;

    public event Action NonstopSelected;
}