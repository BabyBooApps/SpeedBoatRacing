using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopMovement : MonoBehaviour
{
    float speed;
    public float deltaSpeed;
    public float speedRotation;
    Transform player;

    float currentSpeed;
    float currentSpeedRotation;
    Rigidbody rb;
    float health = 1;
    int collisionCount = 0;
    public bool damaging = false;

    bool running = true;

    public Material deathMat;
    public TrailRenderer[] trails;

    public GameObject[] carPieces;

    public ParticleSystem engineBrokeEff;

    public GameObject policeLight, foamparticlesystem;

    public AudioSource audioSource;
    public AudioClip crash;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        speed = player.GetComponent<PlayerMovement>().speed + deltaSpeed;
    }

    void Update()
    {
        if (running)
        {
            rb.velocity = transform.forward * currentSpeed;

            Vector3 targetRot = new Vector3(player.position.x, transform.position.y, player.position.z); 
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetRot - transform.position, currentSpeedRotation, 0));
        }

        if (Vector3.Distance(transform.position,player.position)<17)
        {
            currentSpeed = speed;
            currentSpeedRotation = speedRotation - 0.01f;
        }
        else
        {
            currentSpeed = speed + 2.5f;
            currentSpeedRotation = speedRotation;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddExplosionForce(100, transform.position + new Vector3(0, 0, 5), 100.0f);
        }

    }

    IEnumerator SlowMotionFrap()
    {
        Time.timeScale = 0.35f;
        yield return new WaitForSecondsRealtime(0.35f);
        Time.timeScale = 1;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.CompareTag("Cop")) return;
        
        if ((collision.gameObject.CompareTag("Cop") && collision.relativeVelocity.magnitude > 25) && Time.timeScale == 1)
        {
            StartCoroutine(SlowMotionFrap());
        }

        if ((collision.gameObject.CompareTag("Cop") || collision.gameObject.CompareTag("CopBroken")) 
           /* && collision.relativeVelocity.magnitude >2*/)
        {
            collisionCount++;
            if (collisionCount>0 && !damaging)
            {
                //Debug.Log("BOOM   "+);
                StartCoroutine(Damage(collision.contacts[0].point));
                if (collision.gameObject.CompareTag("Cop") && !collision.gameObject.GetComponent<CopMovement>().damaging)
                {
                    collision.gameObject.GetComponent<CopMovement>().StartCoroutine(collision.gameObject.GetComponent<CopMovement>().Damage(collision.contacts[0].point));
                }
                
            }


        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cop") || collision.gameObject.CompareTag("CopBroken"))
        {
            collisionCount--;
        }
    }



    public IEnumerator Damage(Vector3 point)
    {
        damaging = true;
        while (collisionCount>0)
        {
            health--;
            if (health <= 0)
            {

                DestroyCar(point);

            }
            yield return new WaitForSeconds(1f);
        }
        damaging = false;
    }


    public void DestroyCar(Vector3 point)
    {
        running = false;
        rb.constraints = RigidbodyConstraints.None;
        //Destroy(gameObject);
        rb.AddExplosionForce(5000, point - new Vector3(0,.5f,0), 2f);
        rb.AddTorque(transform.right * 10000);

        StartCoroutine(DetachPieces());

        GetComponentInChildren<MeshRenderer>().sharedMaterial = deathMat;

        foreach (TrailRenderer t in trails)
        {
            t.emitting = false;
        }
        gameObject.tag = "CopBroken";

        StartCoroutine(CompleteDestroying());
        player.GetComponentInChildren<PlayerCircleCombo>().IncreaseCombo(point);
        StartCoroutine( StartSmoke());
        policeLight.SetActive(false);

        if(foamparticlesystem != null)
        foamparticlesystem.SetActive(false);

        audioSource.clip = null;
        audioSource.PlayOneShot(crash);
    }
    
    IEnumerator StartSmoke()
    {
        yield return new WaitForSeconds(2);
        engineBrokeEff.Play();
    }
    IEnumerator CompleteDestroying()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator DetachPieces()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject piece in carPieces)
        {
            piece.GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }
    }
}
