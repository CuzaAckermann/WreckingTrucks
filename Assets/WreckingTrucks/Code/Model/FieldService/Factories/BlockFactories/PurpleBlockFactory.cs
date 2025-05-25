public class PurpleBlockFactory : BlockFactory<PurpleBlock>
{
    public PurpleBlockFactory(int initialPoolSize, int maxPoolCapacity)
                       : base(initialPoolSize, maxPoolCapacity) { }

    protected override PurpleBlock CreateBlock()
    {
        return new PurpleBlock();
    }
}