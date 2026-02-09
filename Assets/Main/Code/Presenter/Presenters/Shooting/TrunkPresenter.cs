public class TrunkPresenter : Presenter
{
    public override void Bind(Model model)
    {
        if (model is Trunk trunk)
        {
            trunk.PositionManipulator.SetPosition(Transform.position);
            trunk.PositionManipulator.SetForward(transform.forward);
        }

        base.Bind(model);
    }
}