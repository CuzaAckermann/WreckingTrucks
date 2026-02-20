using System;

public abstract class ModelFactory<M> : Factory where M : Model
{
    protected readonly ModelSettings ModelSettings;
    protected readonly IMoverCreator MoverCreator;
    protected readonly IRotatorCreator RotatorCreator;

    public ModelFactory(FactorySettings factorySettings,
                        ModelSettings modelSettings)
                 : base(factorySettings)
    {
        Validator.ValidateNotNull(modelSettings);

        ModelSettings = modelSettings;
        MoverCreator = CreateMoverCreator();
        RotatorCreator = CreateRotatorCreator();
    }

    public override Type GetCreatableType()
    {
        return typeof(M);
    }

    protected virtual IMoverCreator CreateMoverCreator()
    {
        return new LinearMoverCreator(ModelSettings.Movespeed);
    }

    protected virtual IRotatorCreator CreateRotatorCreator()
    {
        return new LinearRotatorCreator(ModelSettings.RotationSpeed);
    }
}