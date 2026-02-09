using System;

public abstract class ModelFactory<M> : Factory<M> where M : class, IDestroyable
{
    protected readonly ModelSettings ModelSettings;
    protected readonly IMoverCreator MoverCreator;
    protected readonly IRotatorCreator RotatorCreator;

    public ModelFactory(FactorySettings factorySettings,
                        ModelSettings modelSettings)
                 : base(factorySettings)
    {
        if (factorySettings == null)
        {
            throw new ArgumentNullException(nameof(factorySettings));
        }

        ModelSettings = modelSettings ?? throw new ArgumentNullException(nameof(modelSettings));
        MoverCreator = CreateMoverCreator();
        RotatorCreator = CreateRotatorCreator();
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