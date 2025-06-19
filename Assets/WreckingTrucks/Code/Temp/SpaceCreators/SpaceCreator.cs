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

    [Header("Factory Settings")]
    [SerializeField] protected FactorySettings _factorySettings;

    public void Initialize()
    {
        InitializePresenterFactories();
    }

    public Space CreateSpace(SpaceSettings spaceSettings)
    {
        if (spaceSettings == null)
        {
            throw new ArgumentNullException(nameof(spaceSettings));
        }

        Field field = CreateField(spaceSettings.WidthField, spaceSettings.LengthField);
        Mover mover = CreateMover(field);
        Filler fieldFiller = CreateFiller(field);
        PresentersProduction<M> presentersProduction = CreatePresentersProduction();
        ModelPresenterBinder binder = CreateModelPresenterBinder(field, presentersProduction);
        FillingCardModelCreator fillingCardModelCreator = CreateFillingCardModelCreator();

        return new Space(field,
                         mover,
                         fieldFiller,
                         binder,
                         fillingCardModelCreator);
    }

    protected abstract void InitializePresenterFactories();

    protected abstract void CastomizeModelsProduction(ModelsProduction<M, MF> production);

    protected abstract void CastomizePresentersProduction(PresentersProduction<M> production);

    protected abstract void CastomizeFiller(Filler filler);

    protected PresentersProduction<M> CreatePresentersProduction()
    {
        PresentersProduction<M> production = new PresentersProduction<M>();
        CastomizePresentersProduction(production);

        return production;
    }

    private Field CreateField(int width, int length)
    {
        return new Field(_position.position,
                        _position.forward,
                        _position.right,
                        _intervalBetweenModels,
                        _distanceBetweenModels,
                        width,
                        length);
    }

    private Mover CreateMover(Field field)
    {
        return new Mover(field,
                         _capacityMovables,
                         _movementSpeed,
                         _minSqrDistanceToTargetPosition);
    }

    private FillingCardModelCreator CreateFillingCardModelCreator()
    {
        ModelsProduction<M, MF> production = new ModelsProduction<M, MF>();
        CastomizeModelsProduction(production);

        return new FillingCardModelCreator(production);
    }

    private Filler CreateFiller(Field field)
    {
        Filler filler = new Filler(field);
        CastomizeFiller(filler);

        return filler;
    }

    private ModelPresenterBinder CreateModelPresenterBinder(Field field,
                                                            PresentersProduction<M> presentersProduction)
    {
        return new ModelPresenterBinder(field, presentersProduction);
    }
}