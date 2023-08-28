using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    private float _headHP = 100f;
    private float _bodyHP = 100f;
    private float _legsHP = 100f;

    private void Dead() 
    { 
        gameObject.SetActive(false);
    }

    public void GetDamage(float damage, Sigment sigment)  
    { 
        switch (sigment) 
        {
            case Sigment.Head:
                _headHP -= damage;
                break;
            case Sigment.Body: 
                _bodyHP -= damage;
                break;
            case Sigment.Legs: 
                _legsHP -= damage;
                break;
        }

        if (isDead())
        {
            Dead();
        }
    }

    private bool isDead() 
    { 
        return _headHP < 0f || 
               _bodyHP < 0f || 
               _legsHP < 0f;
    }

    private void Update()
    {
    }

}

public enum Sigment
{
    Head,
    Body,
    Legs
}