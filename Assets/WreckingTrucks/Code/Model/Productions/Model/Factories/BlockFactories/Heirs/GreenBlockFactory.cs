public class GreenBlockFactory : BlockFactory
{
    public GreenBlockFactory(int initialPoolSize, int maxPoolCapacity)
                      : base(initialPoolSize, maxPoolCapacity) { }

    protected override Block CreateModel()
    {
        return new GreenBlock();
    }
}