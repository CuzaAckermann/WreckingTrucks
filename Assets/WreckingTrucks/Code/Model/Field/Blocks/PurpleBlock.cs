public class PurpleBlock : Block
{
    public override void Accept(IBlockVisitor blockVisitor)
    {
        blockVisitor.Visit(this);
    }
}