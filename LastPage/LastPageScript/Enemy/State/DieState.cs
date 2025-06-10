using UnityEngine;

[CreateAssetMenu(fileName = "DieState", menuName = "ScriptableObject/FSM Asset/DieState", order = 5)]
public class DieState : ScriptableObject, IState
{
    public void Enter(Enemy owner)
    {
        owner.Stop();
        owner.Die();
    }

    public void Excute(Enemy owner)
    {

    }

    public void Exit(Enemy owner)
    {

    }
}
