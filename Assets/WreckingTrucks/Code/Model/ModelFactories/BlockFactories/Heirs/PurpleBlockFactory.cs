public class PurpleBlockFactory : BlockFactory
{
    public PurpleBlockFactory(int initialPoolSize, int maxPoolCapacity)
                       : base(initialPoolSize, maxPoolCapacity) { }

    protected override Block CreateModel()
    {
        return new PurpleBlock();
    }
}