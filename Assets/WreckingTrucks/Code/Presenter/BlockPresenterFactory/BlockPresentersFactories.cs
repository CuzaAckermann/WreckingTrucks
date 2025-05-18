using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockPresentersFactories : MonoBehaviour
{
    [SerializeField] private List<BlockPresenterFactory> _blockPresenterFactories;

    public BlockPresenter GetBlockPresenter(BlockType blockType)
    {
        for (int i = 0;  i < _blockPresenterFactories.Count; i++)
        {
            //if (_blockPresenterFactories[i].Type == blockType)
            //{
            //    return _blockPresenterFactories[i].GetPresenter();
            //}
        }

        Logger.Log("Прок");

        throw new InvalidOperationException($"No factory for {blockType}");
    }
}