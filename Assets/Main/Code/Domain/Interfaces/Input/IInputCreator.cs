public interface IInputCreator<I> where I : IInput
{
    public I Create();
}