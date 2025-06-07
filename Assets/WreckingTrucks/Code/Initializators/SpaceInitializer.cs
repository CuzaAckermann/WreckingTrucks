using System;
using UnityEngine;

public abstract class SpaceInitializer<M, MF> : MonoBehaviour, ISpaceCreator where M : Model
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
    [SerializeField] protected float _frequencyForRowFiller = 0.5f;
    [SerializeField] protected float _frequencyForCascadeFiller = 0.05f;
    [SerializeField] protected float _frequencyForZigZagFiller = 0.1f;

    [Header("Factory Settings")]
    [SerializeField] protected FactorySettings _factorySettings;

    public Space CreateSpace(LevelSettings levelSettings)
    {
        if (levelSettings == null)
            throw new ArgumentNullException(nameof(levelSettings));

        Field field = CreateField(levelSettings);
        Mover mover = CreateMover(field);
        ModelsProduction<M, MF> production = CreateModelsProduction();
        PresentersProduction<M> presentersProduction = CreatePresentersProduction();
        FieldFiller fieldFiller = CreateFieldFiller(field);
        ModelPresenterBinder binder = CreateModelPresenterBinder(field, presentersProduction);

        return new Space(field,
                         mover,
                         production,
                         presentersProduction,
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

    private FieldFiller CreateFieldFiller(Field field)
    {
        FieldFiller filler = new FieldFiller(field);

        filler.AddFillingStrategy(new RowFiller(field, _frequencyForRowFiller));
        filler.AddFillingStrategy(new CascadeFiller(field, _frequencyForCascadeFiller));
        filler.AddFillingStrategy(new ZigZagFiller(field, _frequencyForZigZagFiller));

        return filler;
    }

    private ModelPresenterBinder CreateModelPresenterBinder(Field field,
                                                            PresentersProduction<M> presentersProduction)
    {
        return new ModelPresenterBinder(field, presentersProduction);
    }
}