using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float lifeTime;
    public bool shell;
    private void Start()
    {
        if(shell) StartCoroutine(DestroyAfterSec());
    }

    IEnumerator DestroyAfterSec()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground")) FMODUnity.RuntimeManager.PlayOneShot("event:/Shooting/ShellOnGround");
    }
}
