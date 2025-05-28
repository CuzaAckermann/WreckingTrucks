public class OrangeBlockFactory : BlockFactory
{
    public OrangeBlockFactory(int initialPoolSize, int maxPoolCapacity)
                       : base(initialPoolSize, maxPoolCapacity) { }

    protected override Block CreateModel()
    {
        return new OrangeBlock();
    }
}