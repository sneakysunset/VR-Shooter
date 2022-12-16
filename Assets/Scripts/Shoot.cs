using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Shoot : MonoBehaviour
{
    public float range = 1000;
    public float damage = 10;
    public float magazin = 30;
    public float currentAmmoes;
    public float cooldown = .3f;
    private float timer;
    public UnityEvent particleOnFireEvent;
    public bool rapidFire = true;
    bool firing;
    public GameObject Bullet, Shell;
    public Transform bulletSpawnPoint, shellSpawnPoint;
    public float bulletFireStrength, shellExpulsionStrength;
    public float bulletDeviationStrength = .1f;
    public float shellDeviationStrength = .5f;
    public LayerMask raycastLayerMask;
    bool startReloading;
    float reloadingCD;
    public float timeBeforeReloadingComplete = 1f;
    private void Start()
    {
        timer = cooldown;
        currentAmmoes = magazin;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (firing) OnHold();
        if (startReloading) reloadingCD += Time.deltaTime;
    }


    public void OnEndFire() => firing = false;

    void OnHold()
    {
        if (timer > cooldown && currentAmmoes > 0)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, raycastLayerMask))
            {
                currentAmmoes--;

                FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/Bullet Impact Metal");
                particleOnFireEvent.Invoke();
                print(hit.transform.name);
                Target target = hit.transform.GetComponent<Target>();

                Rigidbody bulletRB = Instantiate(Bullet, bulletSpawnPoint.position, transform.rotation).GetComponentInChildren<Rigidbody>();
                Rigidbody shellRB = Instantiate(Shell, shellSpawnPoint.position, transform.rotation).GetComponentInChildren<Rigidbody>();

                Vector3 bulletDirection = (transform.forward + (transform.up * Random.Range(-bulletDeviationStrength, bulletDeviationStrength)) + (transform.right * Random.Range(-bulletDeviationStrength, bulletDeviationStrength))).normalized;
                bulletRB.AddForce(bulletDirection * bulletFireStrength, ForceMode.Impulse);

                Vector3 shellDirection = (transform.up + (transform.forward * Random.Range(-shellDeviationStrength, shellDeviationStrength)) + (transform.right * Random.Range(-shellDeviationStrength, shellDeviationStrength))).normalized;
                shellRB.AddForce(shellDirection * shellExpulsionStrength, ForceMode.Impulse);

                if (target != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    target.TakeDamage(damage, distance, hit.point, hit.normal);
                }
            }
            timer = 0;
        }
        else if(timer > cooldown)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/NoAmmo");
            timer = 0;
        }
    }

    public void ThrowRayCast()
    {
        if (rapidFire)
        {
            firing = true;
            return;
        }

        if (timer > cooldown && currentAmmoes > 0)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
                currentAmmoes--;
                FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/Bullet Impact Metal");
                particleOnFireEvent.Invoke();
                Target target = hit.transform.GetComponent<Target>();

                Rigidbody bulletRB = Instantiate(Bullet, bulletSpawnPoint.position, transform.rotation).GetComponentInChildren<Rigidbody>();
                Rigidbody shellRB = Instantiate(Shell, shellSpawnPoint.position, transform.rotation).GetComponentInChildren<Rigidbody>();

                Vector3 bulletDirection = (transform.forward + (transform.up * Random.Range(-bulletDeviationStrength, bulletDeviationStrength)) + (transform.right * Random.Range(-bulletDeviationStrength, bulletDeviationStrength))).normalized;
                bulletRB.AddForce(bulletDirection * bulletFireStrength, ForceMode.Impulse);

                Vector3 shellDirection = (-2 * transform.forward + (transform.up * Random.Range(-shellDeviationStrength, shellDeviationStrength)) + (transform.right * Random.Range(-shellDeviationStrength, shellDeviationStrength))).normalized;
                shellRB.AddForce(shellDirection * shellExpulsionStrength, ForceMode.Impulse);

                if (target != null)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    target.TakeDamage(damage, distance, hit.point, hit.normal);
                }
            }
            timer = 0;
        }
    }

    public void OnGrab()
    {
        startReloading = false;
        if(reloadingCD > timeBeforeReloadingComplete)
        {
            currentAmmoes = magazin;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/Reload");
        }

        foreach(Transform children in transform)
        {
            children.gameObject.layer = 2;
        }
    }

    public void OnEndGrab()
    {
        firing = false;
        startReloading = true;
        reloadingCD = 0;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/Weapon offHands");
        foreach (Transform children in transform)
        {
            children.gameObject.layer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        startReloading = false;
    }
}
