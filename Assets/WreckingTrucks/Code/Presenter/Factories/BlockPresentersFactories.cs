using System;
using UnityEngine;

public class BlockPresentersFactories : MonoBehaviour
{
    [SerializeField] private GreenBlockPresenterFactory _greenBlockPresenterFactories;
    [SerializeField] private OrangeBlockPresenterFactory _orangeBlockPresenterFactories;
    [SerializeField] private PurpleBlockPresenterFactory _purpleBlockPresenterFactories;

    public void Initiailize()
    {
        _greenBlockPresenterFactories.Initialize();
        _orangeBlockPresenterFactories.Initialize();
        _purpleBlockPresenterFactories.Initialize();
    }

    public Presenter GetBlockPresenter(Block block)
    {
        switch (block)
        {
            case GreenBlock:
                return _greenBlockPresenterFactories.Create();

            case OrangeBlock:
                return _orangeBlockPresenterFactories.Create();

            case PurpleBlock:
                return _purpleBlockPresenterFactories.Create();

            default:
                throw new InvalidOperationException($"No factory for {block}");
        }
    }
}