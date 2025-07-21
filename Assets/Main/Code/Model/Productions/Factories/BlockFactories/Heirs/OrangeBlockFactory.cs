public class OrangeBlockFactory : BlockFactory<OrangeBlock>
{
    public OrangeBlockFactory(FactorySettings factorySettings)
                       : base(factorySettings)
    {

    }

    protected override OrangeBlock CreateElement()
    {
        return new OrangeBlock();
    }
}