using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticle : MonoBehaviour
{
    public ParticleSystem particle;
    //콜라이더가 있을때와 없을때 처리
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
    }   //로컬 좌표로 이동

    public void ResetMove()
    {
        this.gameObject.transform.position = Vector3.zero;
    }   //좌표 초기화
}
