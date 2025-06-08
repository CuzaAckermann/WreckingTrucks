using System;
using System.Collections.Generic;

public class Space
{
    private Field _field;
    private Mover _mover;
    private IModelsProduction _modelsProduction;
    private IModelPresenterSource _modelPresentersSource;
    private FieldFiller _fieldFiller;
    private ModelPresenterBinder _binderModelToModelPresenter;

    private TickEngine _tickEngine;

    public Space(Field field,
                 Mover mover,
                 IModelsProduction modelsProduction,
                 IModelPresenterSource modelPresentersProduction,
                 FieldFiller fieldFiller,
                 ModelPresenterBinder binderModelToModelPresenter)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _modelsProduction = modelsProduction ?? throw new ArgumentNullException(nameof(modelsProduction));
        _modelPresentersSource = modelPresentersProduction ?? throw new ArgumentNullException(nameof(modelPresentersProduction));
        _fieldFiller = fieldFiller ?? throw new ArgumentNullException(nameof(fieldFiller));
        _binderModelToModelPresenter = binderModelToModelPresenter ?? throw new ArgumentNullException(nameof(binderModelToModelPresenter));

        _tickEngine = new TickEngine();
    }

    public void Prepare(FillingCard<Type> fillingCard)
    {
        List<Model> models = _modelsProduction.CreateModels(CreateProductionPlan(fillingCard));

        FillingCard<Model> fillingCardModels = new FillingCard<Model>(fillingCard.Length, fillingCard.Width);

        for (int i = 0; i < models.Count; i++)
        {
            RecordModelToPosition<Type> recordModelToPosition = fillingCard.GetRecord(i);

            fillingCardModels.Add(new RecordModelToPosition<Model>(models[i],
                                                                   recordModelToPosition.LocalY,
                                                                   recordModelToPosition.LocalX));
        }

        _fieldFiller.StartFilling(fillingCardModels);

        _tickEngine.AddTickable(_mover);
        _tickEngine.AddTickable(_fieldFiller);
    }

    public void Start()
    {
        _binderModelToModelPresenter.Enable();
        _tickEngine.Continue();
    }

    public void Update(float deltaTime)
    {
        _tickEngine.Tick(deltaTime);
    }

    public void Exit()
    {
        _tickEngine.Pause();
        _binderModelToModelPresenter.Disable();
    }

    private List<Type> CreateProductionPlan(FillingCard<Type> fillingCard)
    {
        var types = new List<Type>(fillingCard.Width * fillingCard.Length);

        for (int y = 0; y < fillingCard.Length; y++)
        {
            for (int x = 0; x < fillingCard.Width; x++)
            {
                types.Add(fillingCard.GetRecord(x, y).Model);
            }
        }

        return types;
    }
}