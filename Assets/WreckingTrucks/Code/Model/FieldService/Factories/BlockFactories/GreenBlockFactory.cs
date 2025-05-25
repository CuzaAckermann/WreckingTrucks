public class GreenBlockFactory : BlockFactory<GreenBlock>
{
    public GreenBlockFactory(int initialPoolSize, int maxPoolCapacity)
                      : base(initialPoolSize, maxPoolCapacity) { }

    protected override GreenBlock CreateBlock()
    {
        return new GreenBlock();
    }
}