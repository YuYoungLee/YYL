using UnityEngine;

public interface IPlayerAnim
{
    public Animator SetAnim { set; }
    public void Move(float xDir, float yDir);
    public void Stop();
}
