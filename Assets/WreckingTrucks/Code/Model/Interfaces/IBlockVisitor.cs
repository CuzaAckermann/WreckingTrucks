public interface IBlockVisitor
{
    public GreenBlock Visit(GreenBlock greenBlock);

    public OrangeBlock Visit(OrangeBlock orangeBlock);

    public PurpleBlock Visit(PurpleBlock purpleBlock);
}