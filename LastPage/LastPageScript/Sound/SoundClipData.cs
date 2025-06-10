using UnityEngine;

public enum SFXEffectType
{
    ClickButton,
    CancleButton,
    MenuSelect,
    EndGame,
    Default,
}   //UI 이펙트 사운드

public enum SFXPlayerSoundType
{
    Damaged,
    Attack,
    Move,
    Jump,
    GetItem,
    Default,
}   //플레이어 사운드

public enum SFXEnemySoundType
{
    Damaged,
}   //몬스터 사운드

public enum SFXBossSoundType
{
    Attack,
}   //보스 몬스터 사운드

public enum SFXBGMType
{
    MainMenu,
    Stage1,
    Stage2,
    Stage3,
    BossStage,
    EndBoss,
    Default,
}   //배경음 타입

[CreateAssetMenu(fileName = "SoundClipData", menuName = "ScriptableObject/SFX/SoundClipData", order = 1)]
public class SoundClipData : ScriptableObject
{
    [SerializeField] AudioClip[] soundClip;

    public AudioClip GetAudioClip(SFXEffectType type)
    { return soundClip[(int)type]; }

    public AudioClip GetAudioClip(SFXPlayerSoundType type)
    { return soundClip[(int)type]; }

    public AudioClip GetAudioClip(SFXBGMType type)
    { return soundClip[(int)type]; }

    public AudioClip GetAudioClip(SFXEnemySoundType type)
    { return soundClip[(int)type]; }

    public AudioClip GetAudioClip(SFXBossSoundType type)
    { return soundClip[(int)type]; }
}
