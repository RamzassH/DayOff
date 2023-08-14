using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daaa : MonoBehaviour
{
    public Enemy enemy;
    public Sigment sigment;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.GetDamage(collision.gameObject.GetComponentInParent<ChController>().damage, sigment);
    }
}
