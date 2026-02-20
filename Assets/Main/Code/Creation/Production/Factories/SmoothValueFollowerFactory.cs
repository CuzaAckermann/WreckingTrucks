using System;

public class SmoothValueFollowerFactory : Factory
{
    private readonly SmoothValueFollowerSettings _settings;

    public SmoothValueFollowerFactory(FactorySettings factorySettings,
                                      SmoothValueFollowerSettings settings)
                               : base(factorySettings)
    {
        Validator.ValidateNotNull(settings);

        _settings = settings;
    }

    public override Type GetCreatableType()
    {
        return typeof(SmoothValueFollower);
    }

    protected override IDestroyable CreateElement()
    {
        return new SmoothValueFollower(_settings.Speed, _settings.InitialValue);
    }
}