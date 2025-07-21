using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BlockRowSettings
{
    [SerializeField] private List<BlockSequence> _sequences = new List<BlockSequence>();

    public IReadOnlyList<BlockSequence> Sequences => _sequences;

    public void Add(BlockSequence blockSequence)
    {
        if (blockSequence == null)
        {
            throw new ArgumentNullException(nameof(blockSequence));
        }

        if (_sequences.Contains(blockSequence))
        {
            throw new InvalidOperationException(nameof(blockSequence));
        }

        _sequences.Add(blockSequence);
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _sequences.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _sequences.RemoveAt(index);
    }

    public void Validate(int maxWidth)
    {
        int totalBlocks = 0;

        for (int i = 0; i < _sequences.Count; i++)
        {
            int remainingSpace = maxWidth - totalBlocks;

            TrimSequence(_sequences[i], remainingSpace);

            totalBlocks += _sequences[i].Amount;

            if (totalBlocks >= maxWidth)
            {
                if (i + 1 < _sequences.Count)
                {
                    _sequences.RemoveRange(i + 1, _sequences.Count - (i + 1));
                }

                break;
            }
        }
    }

    public int GetUsedWidth()
    {
        return _sequences.Sum(s => s.Amount);
    }

    private void TrimSequence(BlockSequence blockSequence, int remainingSpace)
    {
        if (blockSequence.Amount > remainingSpace)
        {
            blockSequence.Amount = remainingSpace;
        }
    }
}