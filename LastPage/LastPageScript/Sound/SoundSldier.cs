using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSldier : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.MenuSelect);
    }

    public void OnPointerUp(PointerEventData eventDate)
    {
        GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.MenuSelect);
    }
}
