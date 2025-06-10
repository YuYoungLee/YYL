using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� https://gall.dcinside.com/mgallery/board/view/?id=game_dev&no=132556&s_type=search_subject_memo&s_keyword=.ED.8F.AC.EB.AC.BC.EC.84.A0&page=1
//����2 http://happyryu.tistory.com/131
//��ǥ ������ https://blog.naver.com/PostView.nhn?isHttpsRedirect=true&blogId=gotripgo&logNo=140088163468


public class T_Effect : MonoBehaviour
{
    public Projectile projectile;
    public Rigidbody playerRigid;

    int count = 0;
    void Start()
    {
        projectile.Initialize(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile.ReadyFire(playerRigid.transform.position, this.gameObject.transform.position))
        {
            projectile.Fire(5);
            ++count;
            Debug.Log("Count : " + count);
        }
    }

}
