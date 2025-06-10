using UnityEngine;

public interface IEnemyAnim
{
    Animator SetAnim { set; }
    public void Move(float speed);
    public void Stop();
    public void Attack();
    public void Reset();
}
