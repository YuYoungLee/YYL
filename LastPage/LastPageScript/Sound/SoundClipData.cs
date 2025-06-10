using UnityEngine;

public enum SFXEffectType
{
    ClickButton,
    CancleButton,
    MenuSelect,
    EndGame,
    Default,
}   //UI ����Ʈ ����

public enum SFXPlayerSoundType
{
    Damaged,
    Attack,
    Move,
    Jump,
    GetItem,
    Default,
}   //�÷��̾� ����

public enum SFXEnemySoundType
{
    Damaged,
}   //���� ����

public enum SFXBossSoundType
{
    Attack,
}   //���� ���� ����

public enum SFXBGMType
{
    MainMenu,
    Stage1,
    Stage2,
    Stage3,
    BossStage,
    EndBoss,
    Default,
}   //����� Ÿ��

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
