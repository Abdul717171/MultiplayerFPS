using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public GameObject impactEffect;
    private float range = 2f;

    private int damageAmount = 10;
    private void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 2);
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider nearByObjects in colliders)
        {
            if (nearByObjects.tag == "Player")
            {
                StartCoroutine(FindObjectOfType<playerManager>().TakeDamage(damageAmount));
            }
        }
        this.enabled = false;
    }
}
