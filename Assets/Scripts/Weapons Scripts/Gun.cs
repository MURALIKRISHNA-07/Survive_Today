using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private Animator animator;

    public int damage = 10;
    public float range = 100f;
    public float fireRate = 15f;

    public int MaxRefill_Ammo = 2;

    public int maxAmmo = 10;
    public int currentAmmo = 0;
    public float reloadTime = 2f;

    private float NextTimetoFire = 0f;

    public Camera FpsCam;
    public GameObject WeapomCamera;

    public GameObject Crosshair;

    public Sprite NormalCrossHair;
    public Sprite ScopedCrossHair;

    public GameObject MuzzkeImpact;
    public ParticleSystem MuzzleFlash;
    public PlayerStats playerStats;

    public GameObject ReloadTime;

    //Zoomm

    public int zoom = 40;
    public int Normal = 60;

    private bool isZoomed;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        playerStats.BulletsAvailable(currentAmmo);
        playerStats.ReloadAvailable(MaxRefill_Ammo);

    }

    // Update is called once per frame
    void Update()
    {
        // Zoom();
        if (WeaponSwitching.isreloading)
            return;

        if (currentAmmo <= 0 && MaxRefill_Ammo > 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
                return;
            }
        }

        Zoom();

        playerStats.BulletsAvailable(currentAmmo);
        if (Input.GetButtonDown("Fire1") && Time.time > NextTimetoFire && currentAmmo > 0)
        {
            NextTimetoFire = Time.time + (1f / fireRate);
           StartCoroutine(Shoot());
        }
        if(GameManager.instance.iswave)
        {
            MaxRefill_Ammo = 10;
            playerStats.ReloadAvailable(MaxRefill_Ammo);
        }
      
    }
    IEnumerator Reload()
    {
        WeaponSwitching.isreloading = true;
        animator.SetBool("reload", true);
        ReloadTime.GetComponent<ReloadTimer>().count = reloadTime;
        ReloadTime.SetActive(true);
        AudioManager.instance.Play_reloadsound();
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        playerStats.BulletsAvailable(currentAmmo);
        MaxRefill_Ammo--;
        WeaponSwitching.isreloading = false;
        animator.SetBool("reload", false);
        playerStats.ReloadAvailable(MaxRefill_Ammo);
    }

    IEnumerator Shoot()
    {

        animator.SetBool("shoot", true);
        yield return new WaitForSeconds(0.1f);
        AudioManager.instance.Play_Gunsound();
        currentAmmo--;
        playerStats.BulletsAvailable(currentAmmo);
       
        RaycastHit hit;

        if (Physics.Raycast(FpsCam.transform.position, FpsCam.transform.forward, out hit, range))
        {
            EnemyController eC = hit.collider.GetComponent<EnemyController>();
            if (eC)
            {
                eC.Currenthealth -= damage;
                eC.aggro = true;
            }
        }
        //var obj= Instantiate(MuzzleFlash, MuzzkeImpact.transform.position,Quaternion.identity);
        //// var obj = Instantiate(MuzzkeImpact, hit.point, Quaternion.LookRotation(hit.normal));
        // Destroy(obj, 1f);

        MuzzleFlash.Play();
        animator.SetBool("shoot", false);
        StopCoroutine(Shoot());
    }
 
    public void Zoom()
    {
        if (Input.GetButtonDown("Fire2"))
            isZoomed = true;
        else if (Input.GetButtonUp("Fire2"))
            isZoomed = false;

        animator.SetBool("Scope", isZoomed);

        if (isZoomed)
           StartCoroutine( ZoomIn());
        else
            Zoomout();
    }

    public void Normal_CH(bool state)
    {
        if (state)
        {
            Crosshair.GetComponent<Image>().sprite = NormalCrossHair;

        }
        else
        {
            Crosshair.GetComponent<Image>().sprite = ScopedCrossHair;
        }
    }
    IEnumerator ZoomIn()
    {
        yield return new WaitForSeconds(0.2f);
        Normal_CH(false);
        WeapomCamera.SetActive(false);

        FpsCam.fieldOfView = zoom;
    }

    void Zoomout()
    {
        Normal_CH(true);
        WeapomCamera.SetActive(true);
        FpsCam.fieldOfView = Normal;
    }
}
