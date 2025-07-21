public class GreenBlockFactory : BlockFactory<GreenBlock>
{
    public GreenBlockFactory(FactorySettings factorySettings)
                      : base(factorySettings)
    {

    }

    protected override GreenBlock CreateElement()
    {
        return new GreenBlock();
    }
}