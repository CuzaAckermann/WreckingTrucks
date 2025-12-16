using System;

public class ModelRemovedWaitingState
{
    private readonly Field _field;

    private Action<int, int, int> _handlerForModelRemoved;

    private bool _isSubscribed;

    public ModelRemovedWaitingState(Field field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
    }

    public void Enter(Action<int, int, int> handlerOnModelRemoved)
    {
        if (_isSubscribed == false)
        {
            _handlerForModelRemoved = handlerOnModelRemoved ?? throw new ArgumentNullException(nameof(handlerOnModelRemoved));

            _field.ModelRemoved += _handlerForModelRemoved;

            _isSubscribed = true;
        }
        else
        {
            Logger.Log("Already subscribed");
        }
    }

    public void Exit()
    {
        if (_isSubscribed)
        {
            _field.ModelRemoved -= _handlerForModelRemoved;

            _isSubscribed = false;
        }
        else
        {
            Logger.Log("Already unsubscribed");
        }
    }
}