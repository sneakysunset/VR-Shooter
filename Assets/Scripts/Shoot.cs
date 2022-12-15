using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Shoot : MonoBehaviour
{
    public float range = 1000;
    public float damage = 10;
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
    private void Start()
    {
        timer = cooldown;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (firing) OnHold();
    }


    public void OnEndFire() => firing = false;

    void OnHold()
    {
        if (timer > cooldown)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, raycastLayerMask))
            {
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
    }

    public void ThrowRayCast()
    {
        if (rapidFire)
        {
            firing = true;
            return;
        }

        if (timer > cooldown)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, range))
            {
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
        foreach(Transform children in transform)
        {
            children.gameObject.layer = 2;
        }
    }

    public void OnEndGrab()
    {
        firing = false;
        foreach (Transform children in transform)
        {
            children.gameObject.layer = 0;
        }
    }
}
