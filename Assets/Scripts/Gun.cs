using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Gun : MonoBehaviour
{
    public Transform fpsCam;
    public float range = 200;
    public float impactForce = 150;
    public int damageAmount = 20;

    public int fireRate = 10;
    private float nextTimeToFire = 0;

    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public int currentAmmo;
    public int maxAmmo = 100;
    public int magazineAmmo = 1000;

    public float reloadTime = 2f;
    public bool isReloading;

    //public Animator animator;

    private InputAction shoot;

    private void Awake()
    {
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");
        shoot.AddBinding("<Gamepad>/buttonSouth");

        shoot.performed += Shoot;
        shoot.Enable();

        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        isReloading = false;
        //animator.SetBool("isReloading", false);
    }

    private void OnDisable()
    {
        shoot.performed -= Shoot;
        shoot.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (currentAmmo == 0 && magazineAmmo == 0)
        {
            //animator.SetBool("isShooting", false);
            return;
        }

        if (isReloading)
            return;

        //animator.SetBool("isShooting", true);

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
        }
    }

    private void Update()
    {
        if (isReloading)
            return;

        bool isShooting = shoot.ReadValue<float>() > 0.5f;
        //animator.SetBool("isShooting", isShooting);

        if (currentAmmo == 0 && magazineAmmo > 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void Fire()
    {
        AudioManager.instance.Play("Shoot");
        muzzleFlash.Play();
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.position + fpsCam.forward, fpsCam.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            Enemy e = hit.transform.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damageAmount);
                return;
            }

            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact = Instantiate(impactEffect, hit.point, impactRotation);
            impact.transform.parent = hit.transform;
            Destroy(impact, 5);
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        //animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime);
        //animator.SetBool("isReloading", false);
        if (magazineAmmo >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            magazineAmmo -= maxAmmo;
        }
        else
        {
            currentAmmo = magazineAmmo;
            magazineAmmo = 0;
        }
        isReloading = false;
    }
}
