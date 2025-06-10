using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticlePlayer : MonoBehaviour
{
    private ParticleSystem particle;
    private float playTime = 0.0f;
    private Coroutine particleCoroutine = null;

    [Range(0.0f, 10.0f)]public float setPlayTime = 0.0f;

    public void Initialize()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
        playTime = setPlayTime;
    }

    public bool Play(Vector3 position)
    {
        this.transform.position = position;
        if (particleCoroutine != null) return false;
        this.gameObject.SetActive(true);
        particleCoroutine = StartCoroutine(ParticleCoroutine());
        return true;
    }

    public void Stop()
    {
        this.transform.position = Vector3.zero;     //초기화
        StopCoroutine(ParticleCoroutine());
        particleCoroutine = null;
        this.gameObject.SetActive(false);
    }

    private IEnumerator ParticleCoroutine()
    {
        //파티클 플레이
        particle.Play();
        yield return new WaitForSeconds(playTime);
        //멈춤처리 밑 비활성화
        particle.Stop();
        Stop();
    }
}
