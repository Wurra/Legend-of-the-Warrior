
public abstract class BaseState 
{
    public Enemy currentEnemy;
    public abstract void onEnter(Enemy enemy);
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void onExit();
    
}
