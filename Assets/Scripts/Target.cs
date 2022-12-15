using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float HP = 50;
    public float delayFractioner = 600;
    public enum typeOfSurface { metal, wood, glass, human }
    public typeOfSurface surface;
    public bool destroyable = false;
    public GameObject metalPSYS;

    public void TakeDamage(float damage, float distance, Vector3 pointPos, Vector3 normal)
    {
        StartCoroutine(ImpactDelayed(distance, pointPos, normal));
        if(destroyable) HP -= damage;
        if (HP <= 0) Destroy(this.gameObject);
    }

    IEnumerator ImpactDelayed(float distance, Vector3 pointPos, Vector3 normal)
    {
        yield return new WaitForSeconds(Mathf.Clamp(distance / delayFractioner, .1f, 1f));
        switch (surface)
        {
            case typeOfSurface.metal:
                FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/Bullet Impact Metal", pointPos);
                Transform tr = Instantiate(metalPSYS, pointPos, Quaternion.identity).transform;
                tr.up = normal;
                break;
            case typeOfSurface.wood:
                break;
            case typeOfSurface.glass:
                break;
            case typeOfSurface.human:
                break;
            default:
                break;
        }
    }
}
