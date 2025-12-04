using System;
using System.Collections.Generic;

public class BlockPresenterShaker : ITickable
{
    private readonly List<BlockPresenter> _createdBlockPresenters;
    private readonly List<BlockPresenter> _activeBlockPresenters;

    private IBlockPresenterCreationNotifier _notifier;

    public BlockPresenterShaker(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        _createdBlockPresenters = new List<BlockPresenter>();
        _activeBlockPresenters = new List<BlockPresenter>();
    }

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Clear()
    {
        _createdBlockPresenters.Clear();
        _activeBlockPresenters.Clear();
    }

    public void Prepare(IBlockPresenterCreationNotifier notifier)
    {
        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));
    }

    public void Enable()
    {
        _notifier.BlockPresenterCreated += AddBlockPresenter;

        for (int i = 0; i < _createdBlockPresenters.Count; i++)
        {
            SubscibeToBlockPresenter(_createdBlockPresenters[i]);
        }

        Activated?.Invoke(this);
    }

    public void Disable()
    {
        Deactivated?.Invoke(this);

        _notifier.BlockPresenterCreated -= AddBlockPresenter;

        for (int i = 0; i < _createdBlockPresenters.Count; i++)
        {
            UnsubscribeFromBlockPresenter(_createdBlockPresenters[i]);
        }
    }

    public void Tick(float _)
    {
        if (_activeBlockPresenters.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _createdBlockPresenters.Count; i++)
        {
            _createdBlockPresenters[i].Jelly.Shake();
        }

        //for (int i = _activeBlockPresenters.Count - 1; i >= 0; i--)
        //{
        //    _activeBlockPresenters[i].Jelly.Shake();
        //}
    }

    private void AddBlockPresenter(BlockPresenter blockPresenter)
    {
        if (blockPresenter == null)
        {
            throw new ArgumentNullException(nameof(blockPresenter));
        }

        if (_createdBlockPresenters.Contains(blockPresenter))
        {
            throw new InvalidOperationException($"{nameof(blockPresenter)} is already contained");
        }

        _createdBlockPresenters.Add(blockPresenter);
        SubscibeToBlockPresenter(blockPresenter);
    }

    private void RemoveBlockPresenter(BlockPresenter blockPresenter)
    {
        if (blockPresenter == null)
        {
            throw new ArgumentNullException(nameof(blockPresenter));
        }

        if (_createdBlockPresenters.Contains(blockPresenter) == false)
        {
            throw new InvalidOperationException($"{nameof(blockPresenter)} not found");
        }

        UnsubscribeFromBlockPresenter(blockPresenter);
        _createdBlockPresenters.Remove(blockPresenter);
    }

    private void SubscibeToBlockPresenter(BlockPresenter blockPresenter)
    {
        blockPresenter.LifeTimeFinished += OnLifeTimeFinished;
        blockPresenter.ManipulationStarted += OnManipulationStarted;
        blockPresenter.ManipulationCompleted += OnManipulationCompleted;
    }

    private void UnsubscribeFromBlockPresenter(BlockPresenter blockPresenter)
    {
        blockPresenter.LifeTimeFinished -= OnLifeTimeFinished;
        blockPresenter.ManipulationStarted -= OnManipulationStarted;
        blockPresenter.ManipulationCompleted -= OnManipulationCompleted;
    }

    private void OnLifeTimeFinished(Creatable presenter)
    {
        if (presenter is BlockPresenter blockPresenter)
        {
            RemoveBlockPresenter(blockPresenter);
        }
    }

    private void OnManipulationStarted(BlockPresenter blockPresenter)
    {
        _activeBlockPresenters.Add(blockPresenter);
    }

    private void OnManipulationCompleted(BlockPresenter blockPresenter)
    {
        blockPresenter.HesitationFinished += OnHesitationFinished;
    }

    private void OnHesitationFinished(BlockPresenter blockPresenter)
    {
        blockPresenter.HesitationFinished -= OnHesitationFinished;
        _activeBlockPresenters.Remove(blockPresenter);
    }
}