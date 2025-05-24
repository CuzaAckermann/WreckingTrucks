public class OrangeBlockFactory : ModelFactory
{
    public OrangeBlockFactory(int initialPoolSize, int maxPoolCapacity) : base(initialPoolSize, maxPoolCapacity) { }

    protected override Model CreateModel()
    {
        return new OrangeBlock();
    }
}