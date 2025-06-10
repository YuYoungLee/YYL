using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Enemy : MonoBehaviour
{
    int iHp = 100;

    public void Hit(int iDamege)
    {
        iHp -= iDamege;
        Debug.Log("Hit" + iHp);
    }
}
