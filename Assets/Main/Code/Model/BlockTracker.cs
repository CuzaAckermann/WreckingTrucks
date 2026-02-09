using System;
using System.Collections.Generic;

public class BlockTracker
{
    private readonly EventBus _eventBus;
    private readonly Dictionary<ColorType, Queue<Block>> _blocksByType;

    private BlockField _blockField;

    private bool _isSubscribed = false;

    public BlockTracker(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _blocksByType = new Dictionary<ColorType, Queue<Block>>();

        _eventBus.Subscribe<ClearedSignal<Level>>(Clear);

        _eventBus.Subscribe<CreatedSignal<BlockField>>(SetBlockField);
    }

    public event Action TargetDetected;

    public bool TryGetTarget(ColorType requiredType, out Block block)
    {
        block = null;

        if (_blocksByType.TryGetValue(requiredType, out Queue<Block> blocks))
        {
            if (blocks.Count > 0)
            {
                block = blocks.Dequeue();
                block.StayTargetForShooting();

                return true;
            }
        }

        return false;
    }

    public void TakeBlock(Block block)
    {
        if (block == null)
        {
            return;
        }

        block.StayFree();
        SortBlock(block);
    }

    private void Clear(ClearedSignal<Level> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetBlockField);
    }

    private void SetBlockField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetBlockField);

        _blockField = blockFieldCreatedSignal.Creatable;

        SubscribeToBlockField();
    }

    private void SubscribeToBlockField()
    {
        if (_isSubscribed == false)
        {
            OnFirstModelsUpdated(_blockField.GetFirstModels());

            _blockField.Cleared += OnDestroyed;
            _blockField.FirstModelsUpdated += OnFirstModelsUpdated;

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromBlockField()
    {
        if (_isSubscribed)
        {
            _blockField.Cleared -= OnDestroyed;
            _blockField.FirstModelsUpdated -= OnFirstModelsUpdated;

            _isSubscribed = false;
        }
    }

    private void OnDestroyed()
    {
        UnsubscribeFromBlockField();

        _blocksByType.Clear();
    }

    private void OnFirstModelsUpdated(List<Model> models)
    {
        List<Block> blocks = new List<Block>();

        for (int i = 0; i < models.Count; i++)
        {
            if (models[i] is Block block)
            {
                blocks.Add(block);
            }
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            Block block = blocks[i];

            if (block.IsTargetForShooting)
            {
                continue;
            }

            SortBlock(block);
        }
    }

    private void SortBlock(Block block)
    {
        if (_blocksByType.ContainsKey(block.Color) == false)
        {
            _blocksByType[block.Color] = new Queue<Block>();
        }

        if (_blocksByType[block.Color].Count == 0)
        {
            _blocksByType[block.Color].Enqueue(block);
            TargetDetected?.Invoke();

            return;
        }

        if (_blocksByType[block.Color].Contains(block))
        {
            return;
        }

        _blocksByType[block.Color].Enqueue(block);
    }
}