public class ModelFinalizerCreator
{
    public ModelFinalizer Create(EventBus eventBus)
    {
        return new ModelFinalizer(eventBus);
    }
}