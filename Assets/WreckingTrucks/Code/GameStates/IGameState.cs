public interface IGameState
{
    public void Enter();

    public void Update(float deltaTime);

    public void Exit();
}