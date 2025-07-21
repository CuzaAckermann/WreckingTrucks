public interface IModelCreator<out M> where M : Model
{
    public M Create();
}