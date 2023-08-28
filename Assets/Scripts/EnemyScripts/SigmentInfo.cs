using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigmentInfo : MonoBehaviour
{
    public Enemy enemy;
    public Sigment sigment;
    
    
    public void GetDamage(float damage)
    {
        enemy.GetDamage(damage, sigment);
    }
}
