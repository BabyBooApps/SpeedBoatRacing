using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopInstantiator : MonoBehaviour
{
    public GameObject cop;
    public Transform point;
    public float timeBetweenSpawns;
    Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(SpawnCops());
    }

    IEnumerator SpawnCops()
    {
        while (true)
        {
            transform.eulerAngles = new Vector3(0, Random.Range(0,360), 0);
            yield return new WaitForSeconds(timeBetweenSpawns);
            Instantiate(cop, point.position, Quaternion.LookRotation(player.position - point.position));
        }
    }

    public void StopAllCops()
    {
        foreach (CopMovement c  in FindObjectsOfType<CopMovement>())
        {
            c.StopAllCoroutines();
            c.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            c.GetComponent<AudioSource>().enabled = false;
        }
    }
}
