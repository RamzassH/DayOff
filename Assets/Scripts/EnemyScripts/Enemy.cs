using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    private float headHP = 100f;
    private float bodyHP = 100f;
    private float legsHP = 100f;



    private void Dead() 
    { 
        gameObject.SetActive(false);
    }

    public void GetDamage(float damage, Sigment sigment)  
    { 
        switch (sigment) 
        {
            case Sigment.Head:
                headHP -= damage;
                break;
            case Sigment.Body: 
                bodyHP -= damage;
                break;
            case Sigment.Legs: 
                legsHP -= damage;
                break;
        }
    }

    private bool isDead() 
    { 
        return headHP < 0f || bodyHP < 0f || legsHP < 0f;
    }

    private void Update()
    {
        if (isDead()) 
        { 
            Dead();
        }
    }

}

public enum Sigment
{
    Head,
    Body,
    Legs
}