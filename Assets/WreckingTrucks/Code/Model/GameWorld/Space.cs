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

    public void Prepare(IEnumerable<Row> rows)
    {
        List<Model> models = _modelsProduction.CreateModels(CreateProductionPlan(rows));
        _fieldFiller.StartFilling(new Queue<Model>(models));

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

    private List<Type> CreateProductionPlan(IEnumerable<Row> rows)
    {
        List<Type> types = new List<Type>();

        foreach (Row row in rows)
        {
            for (int j = 0; j < row.Models.Count; j++)
            {
                types.Add(row.Models[j]);
            }
        }

        return types;
    }
}