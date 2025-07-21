public class BlockSpace : Space<Field>
{
    public BlockSpace(Field field,
                      Mover mover,
                      Filler filler,
                      ModelFinalizer modelFinalizer)
               : base(field,
                      mover,
                      filler,
                      modelFinalizer)
    {

    }
}