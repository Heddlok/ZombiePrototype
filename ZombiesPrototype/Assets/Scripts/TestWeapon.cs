using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;                       
public class TestWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 9f;
    public float range = 100f;
    public float fireRate = 5f;

    [Header("Ammo Settings")]
    public int magazineSize = 8;
    public int reserveAmmo   = 32;
    public float reloadTime  = 2f;

    [Header("References")]
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    [Header("UI")]
    [Tooltip("Assign your AmmoText here")]
    [SerializeField] private TMP_Text ammoText;   // ← slot for the UI

    private float nextTimeToFire = 0f;
    private int   currentAmmo;
    private bool  isReloading = false;

    private void Awake()
    {
        currentAmmo = magazineSize;
        UpdateAmmoUI();                          // ← initialize display
    }

    private void Update()
    {
        if (isReloading) return;

        // Manual reload
        if (Keyboard.current.rKey.wasPressedThisFrame
            && currentAmmo < magazineSize
            && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Fire
        if (Mouse.current.leftButton.isPressed
            && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                currentAmmo--;
                Debug.Log($"Shot fired. Ammo: {currentAmmo}/{magazineSize} | Reserve: {reserveAmmo}");
                UpdateAmmoUI();                  // ← update on shot
                Shoot();

                if (currentAmmo <= 0 && reserveAmmo > 0)
                {
                    Debug.Log("Out of ammo! Reloading...");
                    StartCoroutine(Reload());
                }
            }
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded   = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo   += ammoToReload;
        reserveAmmo   -= ammoToReload;
        UpdateAmmoUI();                          // ← update on reload

        Debug.Log($"Reloaded. Ammo: {currentAmmo}/{magazineSize} | Reserve: {reserveAmmo}");
        isReloading = false;
    }

    private void Shoot()
    {
        if (muzzleFlash != null) muzzleFlash.Play();

        if (Physics.Raycast(
            fpsCamera.transform.position,
            fpsCamera.transform.forward,
            out var hit,
            range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            // your existing hit logic...
        }
    }

    /// <summary>
    /// Updates the ammo UI text.
    /// </summary>
    private void UpdateAmmoUI()
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo}/{reserveAmmo}";
    }
}
