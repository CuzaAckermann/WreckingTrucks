using System;

public class Space
{
    private readonly Field _field;
    private readonly Mover _mover;
    private readonly Filler _fieldFiller;

    private readonly ModelPresenterBinder _binderModelPresenter;
    private readonly FillingCardModelCreator _fillingCardModelCreator;

    private readonly TickEngine _tickEngine;

    public Space(Field field,
                 Mover mover,
                 Filler fieldFiller,
                 ModelPresenterBinder binderModelPresenter,
                 FillingCardModelCreator fillingCardModelCreator)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _fieldFiller = fieldFiller ?? throw new ArgumentNullException(nameof(fieldFiller));

        _binderModelPresenter = binderModelPresenter ?? throw new ArgumentNullException(nameof(binderModelPresenter));
        _fillingCardModelCreator = fillingCardModelCreator ?? throw new ArgumentNullException(nameof(fillingCardModelCreator));

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
        _fieldFiller.StartFilling(_fillingCardModelCreator.CreateFillingCard(spaceSettings.FillingCard));

        _tickEngine.AddTickable(_mover);
        _tickEngine.AddTickable(_fieldFiller);
    }

    public void Start()
    {
        _binderModelPresenter.Enable();
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

        _binderModelPresenter.Disable();
        _mover.Disable();
    }
}