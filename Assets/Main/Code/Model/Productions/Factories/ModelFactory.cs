using System;

public abstract class ModelFactory<M> : Factory<M> where M : class, IDestroyable
{
    protected readonly ModelSettings ModelSettings;

    public ModelFactory(FactorySettings factorySettings,
                        ModelSettings modelSettings)
                 : base(factorySettings)
    {
        if (factorySettings == null)
        {
            throw new ArgumentNullException(nameof(factorySettings));
        }

        ModelSettings = modelSettings ?? throw new ArgumentNullException(nameof(modelSettings));
    }
}