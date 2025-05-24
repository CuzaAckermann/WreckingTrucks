public class PurpleBlockFactory : ModelFactory
{
    public PurpleBlockFactory(int initialPoolSize, int maxPoolCapacity) : base(initialPoolSize, maxPoolCapacity) { }

    protected override Model CreateModel()
    {
        return new PurpleBlock();
    }
}