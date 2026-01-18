using System;

public class PresenterCreatedSignal<P> where P : Presenter
{
    private readonly P _presenter;

    public PresenterCreatedSignal(P presenter)
    {
        _presenter = presenter ? presenter : throw new ArgumentNullException(nameof(presenter));
    }

    public P Presenter => _presenter;
}