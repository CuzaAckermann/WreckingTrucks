using System;
using UnityEngine;

public abstract class SpaceCreator<M, MF> : MonoBehaviour, ISpaceCreator where M : Model
                                                                         where MF : ModelFactory<M>
{
    [Header("Field Settings")]
    [SerializeField] protected Transform _position;
    [SerializeField] protected float _intervalBetweenModels;
    [SerializeField] protected float _distanceBetweenModels;

    [Header("Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovables = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    [Header("Filler Settings")]

    [Header("Row Filler Settings")]
    [SerializeField] protected float _frequencyForRowFiller = 0.5f;

    [Header("Cascade Filler Settings")]
    [SerializeField] protected float _frequencyForCascadeFiller = 0.05f;

    [Header("Rain Filler Settings")]
    [SerializeField] private float _frequencyForRainFiller = 0.1f;
    [SerializeField] private int _minAmountModelsAtTime = 3;
    [SerializeField] private int _maxAmountModelsAtTime = 5;
    [SerializeField] private int _rainHeight = 20;

    [Header("Factory Settings")]
    [SerializeField] protected FactorySettings _factorySettings;

    public void Initialize()
    {
        InitializePresenterFactories();
    }

    public Space CreateSpace(LevelSettings levelSettings)
    {
        if (levelSettings == null)
        {
            throw new ArgumentNullException(nameof(levelSettings));
        }

        Field field = CreateField(levelSettings);
        Mover mover = CreateMover(field);
        ModelsProduction<M, MF> production = CreateModelsProduction();
        PresentersProduction<M> presentersProduction = CreatePresentersProduction();
        Filler fieldFiller = CreateFieldFiller(field);
        ModelPresenterBinder binder = CreateModelPresenterBinder(field, presentersProduction);

        return new Space(field,
                         mover,
                         production,
                         fieldFiller,
                         binder);
    }

    protected abstract Field CreateField(LevelSettings levelSettings);

    protected abstract ModelsProduction<M, MF> CreateModelsProduction();

    protected abstract PresentersProduction<M> CreatePresentersProduction();

    protected abstract void InitializePresenterFactories();

    private Mover CreateMover(Field field)
    {
        return new Mover(field,
                         _capacityMovables,
                         _movementSpeed,
                         _minSqrDistanceToTargetPosition);
    }

    private Filler CreateFieldFiller(Field field)
    {
        Filler filler = new Filler(field);

        filler.AddFillingStrategy(new RowFiller(_frequencyForRowFiller));
        filler.AddFillingStrategy(new CascadeFiller(_frequencyForCascadeFiller));
        filler.AddFillingStrategy(new RainFiller(_frequencyForRainFiller,
                                                 _minAmountModelsAtTime,
                                                 _maxAmountModelsAtTime,
                                                 _rainHeight));

        return filler;
    }

    private ModelPresenterBinder CreateModelPresenterBinder(Field field,
                                                            PresentersProduction<M> presentersProduction)
    {
        return new ModelPresenterBinder(field, presentersProduction);
    }
}