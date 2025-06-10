using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticle : MonoBehaviour
{
    public ParticleSystem particle;
    //�ݶ��̴��� �������� ������ ó��
    public void Play()
    {
        this.gameObject.SetActive(true);
        particle.Play();
    }

    public void Stop()
    {
        particle.Stop();
        this.gameObject.SetActive(false);
    }

    public void SetMove(Vector3 pos)
    {
        this.gameObject.transform.localPosition = pos;
    }   //���� ��ǥ�� �̵�

    public void ResetMove()
    {
        this.gameObject.transform.position = Vector3.zero;
    }   //��ǥ �ʱ�ȭ
}
