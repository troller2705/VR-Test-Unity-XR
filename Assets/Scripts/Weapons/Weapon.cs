using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody), typeof(XRGrabInteractable))]

public class Weapon : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public Projectile bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] protected Animator gunAnimator;
    [SerializeField] public Transform barrelLocation;
    [SerializeField] protected Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")][SerializeField] protected float destroyTimer = 2f;
    [Tooltip("Bullet Speed")]protected float shotPower = 2f;
    [Tooltip("Casing Ejection Speed")][SerializeField] protected float ejectPower = 150f;
    [SerializeField] private float recoilForce = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private bool rifle = false;
    [SerializeField] private float fireRate = 0f;

    private Rigidbody rigidBody;
    private XRGrabInteractable interactableWeapon;
    private WaitForSeconds wait;

    [System.Obsolete]
    protected virtual void Awake()
    {
        interactableWeapon = GetComponent<XRGrabInteractable>();
        rigidBody = GetComponent<Rigidbody>();
        SetupInteractableWeaponEvents();
    }

    private void Start()
    {
        if (rifle)
        {
            wait = new WaitForSeconds(1 / fireRate);
        }

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        if (GetComponentInChildren<GunAttachmentSystem>().attachmentPoints[2].currentAttachment != null)
        {
            barrelLocation.position = barrelLocation.position + GetComponentInChildren<GunAttachmentSystem>().attachmentPoints[2].currentAttachment.GetComponent<Mesh>().bounds.size;
        }
    }

    [System.Obsolete]
    private void SetupInteractableWeaponEvents()
    {
        interactableWeapon.onSelectEntered.AddListener(PickUpWeapon);
        interactableWeapon.onSelectExited.AddListener(DropWeapon);
        interactableWeapon.onActivate.AddListener(StartShooting);
        interactableWeapon.onDeactivate.AddListener(StopShooting);
    }

    private void PickUpWeapon(XRBaseInteractor interactor)
    {
        //interactor.GetComponent<MeshHidder>().Hide();
    }

    private void DropWeapon(XRBaseInteractor interactor)
    {
        //interactor.GetComponent<MeshHidder>().Show();
    }

    protected virtual void StartShooting(XRBaseInteractor interactor)
    {
        if (rifle)
        {
            StartCoroutine(ShootingCO());
        }
        else
        {
            gunAnimator.SetTrigger("Fire");
        }
    }

    protected virtual void StopShooting(XRBaseInteractor interactor)
    {
        if (rifle)
        {
            gunAnimator.SetBool("Fire", false);
            StopAllCoroutines();
        }
    }

    protected virtual void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        ApplyRecoil();

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        Projectile projectileInstance = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        projectileInstance.Init(this);
        projectileInstance.Launch();
    }

    private IEnumerator ShootingCO()
    {
        while (true)
        {
            gunAnimator.SetBool("Fire", true);
            yield return wait;
        }
    }

    private void ApplyRecoil()
    {
        rigidBody.AddRelativeForce(Vector3.back * recoilForce, ForceMode.Impulse);
    }

    public float GetShootingForce()
    {
        return shotPower;
    }

    public float GetDamage()
    {
        return damage;
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }
}
