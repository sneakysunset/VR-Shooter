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
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        Player = GameObject.FindObjectOfType<Camera>().transform;
        timer = targetRefresh;
    }


    void Update()
    {
        timer += Time.deltaTime;
        if(timer > targetRefresh)
        {
            timer = 0;
            navMesh.destination = Player.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathTriggerZone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
