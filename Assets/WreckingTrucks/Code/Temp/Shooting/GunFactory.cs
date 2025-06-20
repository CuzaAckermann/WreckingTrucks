public class GunFactory : ModelFactory<Gun>
{
    public GunFactory(int initialPoolSize,
                      int maxPoolCapacity)
    {
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected override Gun CreateModel()
    {
        return new Gun();
    }
}