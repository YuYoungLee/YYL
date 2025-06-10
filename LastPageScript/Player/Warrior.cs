using UnityEngine;
using UnityEngine.InputSystem;

public class Warrior : Player
{
    bool defendStatus = false;      //M2 ���� ó��
    [SerializeField] Transform hitTr;

    [Header("Effect")]
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] ParticleSystem shieldEffect;
    private void FixedUpdate()
    {
        UpdatePlayer();
    }   //�÷��̾� ������

    public override void Initialize()
    {
        hitEffect.Stop();
        shieldEffect.Stop();
        data = Resources.Load<WarriorData>("ScriptableObject/WarriorData");
        soundData = Resources.Load<SoundClipData>("ScriptableObject/SoundData/WarriorSoundData");
        audioSource = gameObject.AddComponent<AudioSource>();
        playerTrans = GetComponent<Transform>();
        targetCamera = GetComponentInChildren<Geometry>();
        SetGeometry();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        SetPlayerData();    //�÷��̾� ������ ����
        SetSkill();         //�÷��̾� ��ų ������ ����
    }


    #region warriorInput
    public override void InputM1(InputAction.CallbackContext context)
    {
        if(PlaySkill(SkillKey.Skill_M1))
        {
            anim.SetTrigger("SetAttackM1");
            audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Attack), GameManager.Instance.GetSoundManager.EffectVolume);
        }
    }   //M1 �⺻����
    public override void InputStopM1(InputAction.CallbackContext context)
    {
    }
    public override void InputM2(InputAction.CallbackContext context)
    {
        if (PlaySkill(SkillKey.Skill_M2))
        {
            shieldEffect.Play();
            anim.SetBool("SetAttackM2", true);
            defendStatus = true;
        }
    }   //M2 ��� ����
    public override void InputStopM2(InputAction.CallbackContext context)
    {
        shieldEffect.Stop();
        anim.SetBool("SetAttackM2", false);
        defendStatus = false;
    }
    public override void SkillE(InputAction.CallbackContext context)
    {
        if (PlaySkill(SkillKey.Skill_E))
        {
            anim.SetTrigger("SkillE");
            audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Attack), GameManager.Instance.GetSoundManager.EffectVolume);
        }
    }
    public override void SkillQ(InputAction.CallbackContext context)
    {
        if (PlaySkill(SkillKey.Skill_Q))
        {
            anim.SetTrigger("SkillQ");
            audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Attack), GameManager.Instance.GetSoundManager.EffectVolume);
        }
    }
    public override void SkillR(InputAction.CallbackContext context)
    {
        if (PlaySkill(SkillKey.Skill_R))
        {
            anim.SetTrigger("SkillR");
            audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Attack), GameManager.Instance.GetSoundManager.EffectVolume);
        }
    }

    #endregion
    public override void Damaged(int enemyDamege)
    {
        if (dieStatus) return;      //�׾��� �� ����
        stat.Damaged(enemyDamege, defendStatus);    //��� �����϶� ����� �氨

        hitEffect.Play();
        uiManager().GetPlayerUI.GetActionPanel.SetHp(stat.Hp, stat.HpMax);
        audioSource.PlayOneShot(soundData.GetAudioClip(SFXPlayerSoundType.Damaged));
        if (stat.Hp <= 0)
        {
            dieStatus = true;
            anim.SetTrigger("Die");
            GameManager.Instance.SetInputDisable();
            GameManager.Instance.GetSoundManager.EffectPlayOneShot(SFXEffectType.EndGame);
            uiManager().GetPlayerUI.GetGameOverObj.SetActive(true);
        }
        //Todo �÷��̾� ������ ó��
    }   //������ �������� �Ծ����� ó��
}
