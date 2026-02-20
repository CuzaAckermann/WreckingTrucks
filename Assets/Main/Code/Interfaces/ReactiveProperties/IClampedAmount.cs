public interface IClampedAmount : IAmount
{
    public IAmount Min { get; }

    public IAmount Max { get; }
}