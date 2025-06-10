using UnityEngine;

public class GUI : MonoBehaviour
{
    public void SetActive(bool activeStatus)
    {
        if (activeStatus) { GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.MenuSelect); }
        else 
        { GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.ClickButton); }
        this.gameObject.SetActive(activeStatus); 
    } 
}
