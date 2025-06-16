using System;
using System.Collections.Generic;

public class Space
{
    private readonly Field _field;
    private readonly Mover _mover;
    private readonly IModelsProduction _modelsProduction;
    private readonly Filler _fieldFiller;
    private readonly ModelPresenterBinder _binderModelToModelPresenter;

    private readonly ProductionPlanCreator _productionPlanCreator;
    private readonly TickEngine _tickEngine;

    public Space(Field field,
                 Mover mover,
                 IModelsProduction modelsProduction,
                 Filler fieldFiller,
                 ModelPresenterBinder binderModelToModelPresenter)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _modelsProduction = modelsProduction ?? throw new ArgumentNullException(nameof(modelsProduction));
        _fieldFiller = fieldFiller ?? throw new ArgumentNullException(nameof(fieldFiller));
        _binderModelToModelPresenter = binderModelToModelPresenter ?? throw new ArgumentNullException(nameof(binderModelToModelPresenter));

        _productionPlanCreator = new ProductionPlanCreator();
        _tickEngine = new TickEngine();
    }

    public void Clear()
    {
        _field.Clear();
        _mover.Clear();
        _fieldFiller.Clear();
    }

    public void Prepare(SpaceSettings spaceSettings)
    {
        _fieldFiller.StartFilling(CreateFillingCardModels(spaceSettings.FillingCard));

        _tickEngine.AddTickable(_mover);
        _tickEngine.AddTickable(_fieldFiller);
    }

    public void Start()
    {
        _binderModelToModelPresenter.Enable();
        _mover.Enable();
        _tickEngine.Continue();
    }

    public void Update(float deltaTime)
    {
        _tickEngine.Tick(deltaTime);
    }

    public void Stop()
    {
        _tickEngine.Pause();
        _binderModelToModelPresenter.Disable();
        _mover.Disable();
    }

    public bool TryRemoveModel(Model model)
    {
        return _field.TryRemoveModel(model);
    }

    private FillingCard<Model> CreateFillingCardModels(FillingCard<Type> fillingCard)
    {
        List<Model> models = _modelsProduction.CreateModels(_productionPlanCreator.CreateProductionPlan((fillingCard)));
        FillingCard<Model> fillingCardModels = new FillingCard<Model>(fillingCard.Length, fillingCard.Width);

        for (int i = 0; i < models.Count; i++)
        {
            RecordModelToPosition<Type> recordModelToPosition = fillingCard.GetRecord(i);

            fillingCardModels.Add(new RecordModelToPosition<Model>(models[i],
                                                                   recordModelToPosition.NumberOfRow,
                                                                   recordModelToPosition.NumberOfColumn));
        }

        return fillingCardModels;
    }
}