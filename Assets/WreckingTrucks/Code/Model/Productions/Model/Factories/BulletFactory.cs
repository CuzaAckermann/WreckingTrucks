public class BulletFactory : ModelFactory<Bullet>
{
    public BulletFactory(int initialPoolSize, int maxPoolCapacity)
    {
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected override Bullet CreateModel()
    {
        return new Bullet();
    }
}