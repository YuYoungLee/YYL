using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] SoundClipData effectData;
    [SerializeField] SoundClipData bgmData;
    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioSource bgmSource;
    [Range(0, 1.0f)] private float effectVolume = 1.0f;
    [Range(0, 1.0f)] private float bgmVolume = 1.0f;

    public float EffectVolume => effectVolume;

    public void SetEffectVolume(float setVolume)
    {
        effectVolume = Mathf.Clamp01(setVolume);
        effectSource.volume = effectVolume;
    }   //사운드 볼륨 설정

    public void SetBGMVolume(float setVolume)
    {
        bgmVolume = Mathf.Clamp01(setVolume);
        bgmSource.volume = bgmVolume;
    }   //사운드 볼륨 설정

    public void EffectPlayOneShot(SFXEffectType type)
    {
        effectSource.PlayOneShot(effectData.GetAudioClip(type), effectVolume);
    }   //사운드 한번 플레이

    public void BGMPlayLoop(SFXBGMType type)
    {
        bgmSource.volume = bgmVolume;
        bgmSource.clip = bgmData.GetAudioClip(type);
        bgmSource.Play();
    }   //브금 플레이
}
