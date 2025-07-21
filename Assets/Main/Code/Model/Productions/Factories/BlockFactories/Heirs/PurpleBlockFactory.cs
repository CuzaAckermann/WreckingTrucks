public class PurpleBlockFactory : BlockFactory<PurpleBlock>
{
    public PurpleBlockFactory(FactorySettings factorySettings)
                       : base(factorySettings)
    {

    }

    protected override PurpleBlock CreateElement()
    {
        return new PurpleBlock();
    }
}