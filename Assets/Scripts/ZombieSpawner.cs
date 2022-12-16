using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public float MaxRangeSpawn, MinRangeSpawn;
    public GameObject Zombie;
    public float spawnMinTimer, spawnMaxTimer;
    private float spawnTimer;
    private float timer;
    public float timeToAccelerateSpawnFrequency;
    public AnimationCurve minTimerAccelerationCurve, maxTimerAccelerationCurve;
    public float minMinTimerValue, minMaxTimerValue;

    private void Start()
    {
        StartCoroutine(randomTimerAcceleration(spawnMinTimer, spawnMaxTimer));
        spawnTimer = Random.Range(spawnMinTimer, spawnMaxTimer);
    }

    private Vector3 GetInstantiationPosition()
    {
        Vector3 spawnPos = new Vector3(Random.insideUnitCircle.x * MaxRangeSpawn, 0, Random.insideUnitCircle.y * MaxRangeSpawn);
/*        RaycastHit hit;
        if (Physics.Raycast(spawnPos, Vector3.up, out hit, Mathf.Infinity, 8))
        {
            if (hit.transform.CompareTag("Ground"))
                spawnPos.y = hit.point.y;
            else print(hit.transform.name);

        }
        else print("nothing found");*/

        if (Vector3.Distance(new Vector3(spawnPos.x, 0, spawnPos.z), new Vector3(transform.position.x, 0, transform.position.z)) < MinRangeSpawn)
        {
            spawnPos =  GetInstantiationPosition();
        }

        return spawnPos;

    }

    public void InstantiateZombie()
    {
        Vector3 spawnPos = GetInstantiationPosition();
        Instantiate(Zombie, spawnPos, Quaternion.identity);
    }

    private void Update()
    {
        if (timer > spawnTimer)
        {
            timer = 0;
            spawnTimer = Random.Range(spawnMinTimer, spawnMaxTimer);
            InstantiateZombie();
        }

        timer += Time.deltaTime;
    }

    IEnumerator randomTimerAcceleration(float minValue, float maxValue)
    {
        float i = 0;
        while(i < 1)
        {
            i += Time.deltaTime * (1 / timeToAccelerateSpawnFrequency);
            spawnMinTimer = Mathf.Lerp(minValue, minMinTimerValue, minTimerAccelerationCurve.Evaluate(i));
            spawnMaxTimer = Mathf.Lerp(maxValue, minMaxTimerValue, maxTimerAccelerationCurve.Evaluate(i));
            yield return new WaitForEndOfFrame();
        }
        spawnMinTimer = minMinTimerValue;
        spawnMaxTimer = minMaxTimerValue;
        yield return null;
    }
}
