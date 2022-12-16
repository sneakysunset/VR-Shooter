using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class NavMeshEnnemy : MonoBehaviour
{
    NavMeshAgent navMesh;
    Transform Player;
    public float targetRefresh = 1f;
    private float timer;
    Target target;
    Animator animator;
    Rigidbody rb;
    float ogSpeed;
    void Start()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
        target = GetComponent<Target>();
        navMesh = GetComponent<NavMeshAgent>();
        ogSpeed = navMesh.speed;
        Player = GameObject.FindObjectOfType<Camera>().transform;
        timer = targetRefresh;
    }
    bool flag;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > targetRefresh && !flag)
        {
            timer = 0;
            
            navMesh.destination = Player.position;
        }

        if(target.HP <= 0 && !flag)
        {
            flag = true;
            animator.Play("Death", 0);
            navMesh.speed = 0;
            navMesh.enabled = false;
            //rb.isKinematic = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) navMesh.speed = ogSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathTriggerZone") && !flag)
        {
            animator.Play("Attack", 0);
            navMesh.speed = 0;

            
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
