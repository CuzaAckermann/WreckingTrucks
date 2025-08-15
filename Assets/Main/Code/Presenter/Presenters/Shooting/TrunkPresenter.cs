public class TrunkPresenter : Presenter
{
    public override void Bind(Model model)
    {
        if (model is Trunk trunk)
        {
            trunk.SetPosition(Transform.position);
            trunk.SetDirectionForward(transform.forward);
        }

        base.Bind(model);
    }
}