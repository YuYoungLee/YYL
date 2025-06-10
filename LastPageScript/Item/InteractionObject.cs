using UnityEngine;

public abstract class InteractionObject : MonoBehaviour
{
    protected bool interactionStatus = true;

    public bool InteractionStatus { get { return interactionStatus; } private set { } }
    public abstract void InteractionPlayer();   //플레이어 상호작용

    public void SetActive(bool activeStatus) { this.gameObject.SetActive(activeStatus); }
}
