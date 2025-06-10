using System.Collections;
using UnityEngine;

public class Teleporter : InteractionObject
{
    [SerializeField] AudioSource audioSource;
    Coroutine changeSceneCoroutine = null;
    public override void InteractionPlayer()
    {
        if(changeSceneCoroutine == null)
        {
            changeSceneCoroutine = StartCoroutine(ChangeSceneCoroutine());
        }
    }

    IEnumerator ChangeSceneCoroutine()
    {
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        SetDisable();
    }

    public void SetActive(Vector3 setPos)
    {
        GameManager.Instance.GetSoundManager.BGMPlayLoop(SFXBGMType.EndBoss);
        this.gameObject.transform.position = setPos;
        this.gameObject.SetActive(true);
    }   //�ڷ���Ʈ �ش� ��ǥ�� Ȱ��ȭ

    public void SetDisable()
    {
        StopCoroutine(changeSceneCoroutine);
        changeSceneCoroutine = null;
        audioSource.Stop();
        this.gameObject.SetActive(false);
        GameManager.Instance.ChangeScene((SceneLoad)Random.Range(1, 4));
    }   //�ʱ�ȭ �� �� ����
}
