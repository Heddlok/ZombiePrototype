using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class TestWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage   = 9f;
    public float range    = 100f;
    public float fireRate = 5f;

    [Header("Ammo Settings")]
    public int   magazineSize = 8;
    public int   reserveAmmo  = 32;
    public float reloadTime   = 2f;

    [Header("References")]
    public Camera        fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject     impactEffect;

    [Header("UI")]
    [SerializeField] private TMP_Text ammoText;

    private float nextTimeToFire;
    private int   currentAmmo;
    private bool  isReloading;

    private void Awake()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < magazineSize && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Mouse.current.leftButton.isPressed && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                currentAmmo--;
                UpdateAmmoUI();
                Shoot();

                if (currentAmmo <= 0 && reserveAmmo > 0)
                    StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded   = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;
        UpdateAmmoUI();

        isReloading = false;
    }

    private void Shoot()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (Physics.Raycast(
                fpsCamera.transform.position,
                fpsCamera.transform.forward,
                out RaycastHit hit,
                range,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent<Zombie>(out var zombie))
            {
                zombie.TakeDamage(Mathf.RoundToInt(damage));
            }
            else if (hit.collider.TryGetComponent<Target>(out var target))
            {
                target.TakeDamage(damage);
            }

            if (impactEffect != null)
            {
                var impactGO = Instantiate(
                    impactEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo}/{reserveAmmo}";
    }
}
