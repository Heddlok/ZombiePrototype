using UnityEngine;
using UnityEngine.InputSystem;      // for Mouse and Keyboard
using System.Collections;
using Unity.VisualScripting;         // for IEnumerator

public class TestWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 8f;
    public float range = 100f;
    public float fireRate = 5f;

    [Header("Ammo Settings")]
    [Tooltip("Rounds per magazine")]
    public int magazineSize = 8;
    [Tooltip("Total reserve ammo")]
    public int reserveAmmo = 32;
    [Tooltip("Seconds to reload")]
    public float reloadTime = 2f;

    [Header("References")]
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    private void Awake()
    {
        // start full
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        // If we’re in the middle of a reload, do nothing else
        if (isReloading)
            return;


        // Reload input
            if (Keyboard.current.rKey.wasPressedThisFrame
                && currentAmmo < magazineSize
                && reserveAmmo > 0)
            {
                StartCoroutine(Reload());
                return;
            }

        // Fire input
        if (Mouse.current.leftButton.isPressed 
            && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                currentAmmo--;
                Debug.Log($"Shot fired. Ammo: {currentAmmo}/{magazineSize} | Reserve: {reserveAmmo}");
                Shoot();

                // Automatically reload if out of ammo
                if (currentAmmo <= 0 && reserveAmmo > 0)
                {
                    Debug.Log("Out of ammo! Reloading...");
                    StartCoroutine(Reload());
                }
            }
            else
            {
                // Magazine is empty and reload in progress or not needed
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Optionally play reload animation or sound

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded   = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        Debug.Log($"Reloaded. Ammo: {currentAmmo}/{magazineSize} | Reserve: {reserveAmmo}");
        isReloading = false;
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (Physics.Raycast(
            fpsCamera.transform.position,
            fpsCamera.transform.forward,
            out var hit, 
            range))
        {
            Debug.Log("Hit: " + hit.transform.name);

            var target = hit.transform.GetComponent<Target>();
            if (target != null)
                target.TakeDamage(damage);

            if (impactEffect != null)
            {
                var impactGO = Instantiate(
                    impactEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
                Destroy(impactGO, 2f);
            }
        }
    }
}
