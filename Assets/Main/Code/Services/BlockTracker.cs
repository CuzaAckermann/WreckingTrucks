using System;
using System.Collections.Generic;

public class BlockTracker
{
    private readonly BlockFieldCreator _blockFieldCreator;
    private readonly Dictionary<ColorType, Queue<Block>> _blocksByType;

    private Field _field;

    private bool _isSubscribed = false;

    public BlockTracker(BlockFieldCreator blockFieldCreator)
    {
        _blockFieldCreator = blockFieldCreator ?? throw new ArgumentNullException(nameof(blockFieldCreator));
        _blocksByType = new Dictionary<ColorType, Queue<Block>>();

        SubscribeToBlockFieldCreator();
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

    private void SubscribeToBlockFieldCreator()
    {
        _blockFieldCreator.BlockFieldCreated += OnBlockFieldCreated;
    }

    private void UnsubscribeFromBlockFieldCreator()
    {
        _blockFieldCreator.BlockFieldCreated -= OnBlockFieldCreated;
    }

    private void OnBlockFieldCreated(Field blockField)
    {
        if (_field != null)
        {
            UnsubscribeFromBlockField();
        }

        _field = blockField ?? throw new ArgumentNullException(nameof(blockField));

        SubscribeToBlockField();
    }

    private void SubscribeToBlockField()
    {
        if (_isSubscribed == false)
        {
            OnFirstModelsUpdated(_field.GetFirstModels());

            _field.Destroyed += OnDestroyed;
            _field.FirstModelsUpdated += OnFirstModelsUpdated;

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromBlockField()
    {
        if (_isSubscribed)
        {
            _field.Destroyed -= OnDestroyed;
            _field.FirstModelsUpdated -= OnFirstModelsUpdated;

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
        if (_blocksByType.ContainsKey(block.ColorType) == false)
        {
            _blocksByType[block.ColorType] = new Queue<Block>();
            _blocksByType[block.ColorType].Enqueue(block);
            TargetDetected?.Invoke();
        }
        else
        {
            if (_blocksByType[block.ColorType].Count > 0)
            {
                if (_blocksByType[block.ColorType].Contains(block) == false)
                {
                    _blocksByType[block.ColorType].Enqueue(block);
                }
            }
            else
            {
                _blocksByType[block.ColorType].Enqueue(block);
                TargetDetected?.Invoke();
            }
        }
    }
}