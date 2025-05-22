public class OrangeBlock : Block
{
    public override void Accept(IBlockVisitor blockVisitor)
    {
        blockVisitor.Visit(this);
    }
}