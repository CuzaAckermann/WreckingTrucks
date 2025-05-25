public class OrangeBlockFactory : BlockFactory<OrangeBlock>
{
    public OrangeBlockFactory(int initialPoolSize, int maxPoolCapacity)
                       : base(initialPoolSize, maxPoolCapacity) { }

    protected override OrangeBlock CreateBlock()
    {
        return new OrangeBlock();
    }
}