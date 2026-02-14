public class TrunkPresenter : Presenter
{
    public override void Bind(Model model)
    {
        if (model is Trunk trunk)
        {
            trunk.Placeable.SetPosition(Transform.position);
            trunk.Placeable.SetForward(transform.forward);
        }

        base.Bind(model);
    }
}