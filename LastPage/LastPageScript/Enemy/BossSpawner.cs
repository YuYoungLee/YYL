using UnityEngine;
using UnityEngine.Playables;

public class BossSpawner : InteractionObject
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] GameObject geometry;
    public override void InteractionPlayer()
    {
        if (interactionStatus)
        {
            interactionStatus = false;
            playableDirector.gameObject.SetActive(true);
            playableDirector.Play();
            GameManager.Instance.GetEnemyPool.SpawnMonster(Monster.Monster_Boss, this.gameObject.transform.position);
            GameManager.Instance.GetSoundManager.BGMPlayLoop(SFXBGMType.BossStage);
        }   //키 눌렸을 때 보스 몬스터 뽑기
    }

    public void ResetData()
    {
        playableDirector.gameObject.SetActive(false);
        interactionStatus = true;
    }

    public void LookBossSignal()
    {
        GameManager.Instance.GetCameraController.SetTarget(geometry.transform);
    }

    public void ResetLookSignal()
    {
        GameManager.Instance.GetPlayer().SetGeometry();
    }
}
