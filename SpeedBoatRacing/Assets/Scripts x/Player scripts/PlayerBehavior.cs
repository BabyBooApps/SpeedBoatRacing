using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public Color playerColor;
    public CopInstantiator copInstantiator;
    public GameObject dustEffect;
    public GameObject spotLight;

    public int health = 3;

    public GameObject canvas;

    int collisionCount = 0;

    bool damaging = false;

    float recoveryTime = 2.0f;

    public GameObject comboCanvas;

    public GameObject trail1, trail2;

    public Material mainMaterial;
    Color mainMatColor;
    public GameObject selectedCar;
    public List<GameObject> carPieces;
    public List<GameObject> foamObjects;

    public PlayerCircleCombo playerCombo;

    public GameObject splashEffect;

    public AudioSource audioSource;

    public AudioClip crash;
    public AudioClip collect;
    public AudioClip combo;

    public void playCrashSound()
    {
        audioSource.PlayOneShot(crash);
    }
    public void playCollectSound()
    {
        audioSource.PlayOneShot(collect);
    }
    public void playComboSound()
    {
        audioSource.PlayOneShot(combo);
    }

    private void Awake()
    {
        mainMatColor = playerColor;
        mainMaterial.color = playerColor;
    }

    private void Start()
    {
        UpdateCarPieces();
    }

    public void StartPlaying()
    {
        GetComponent<PlayerMovement>().startControling();
        copInstantiator.enabled = true;

        Time.timeScale = 1;
    }

    public void UpdateCarPieces()
    {
        carPieces = new List<GameObject>();
        foreach (Rigidbody p in selectedCar.GetComponentsInChildren<Rigidbody>())
        {
            carPieces.Add(p.gameObject);
        }

    }

    public void GameOver()
    {
        StartCoroutine(SlowMotionShoc());
        health = 0;
        canvas.GetComponent<HealthBar>().UpdateHearts(health);
        copInstantiator.StopAllCoroutines();
        gameObject.tag = "Untagged";

        GetComponent<PlayerMovement>().StopMoving();
        copInstantiator.StopAllCops();

        StartCoroutine(DetachPieces());

        //
        spotLight.SetActive(false);
        dustEffect.SetActive(false);
        DisableFoamPS();
        comboCanvas.SetActive(false);
        trail1.SetActive(false);
        trail2.SetActive(false);

        canvas.GetComponent<UIOperations>().ActiveGameOverSceen();

        audioSource.clip = null;
        playCrashSound();
    }

    IEnumerator DetachPieces()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject piece in carPieces)
        {
            piece.GetComponent<Rigidbody>().isKinematic = false;
            yield return new WaitForSeconds(Random.Range(0.05f,0.15f));
        }
    }

    IEnumerator SlowMotionShoc()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (!gameObject.CompareTag("Cop")) return;*/

        /*if ((collision.gameObject.CompareTag("Cop") && collision.relativeVelocity.magnitude > 0) && Time.timeScale == 1)
        {
            StartCoroutine(SlowMotionFrap());
        }*/

        if ((collision.gameObject.CompareTag("Cop") /*|| collision.gameObject.CompareTag("CopBroken")*/)
            /*&& collision.relativeVelocity.magnitude >10*/)
        {
            collisionCount++;
            StartCoroutine(Damage());
            if (collisionCount > 0 && !damaging)
            {
                //Debug.Log("BOOM   "+);
               
                /*if (collision.gameObject.CompareTag("Cop") && !collision.gameObject.GetComponent<CopMovement>().damaging)
                {
                    collision.gameObject.GetComponent<CopMovement>().StartCoroutine(collision.gameObject.GetComponent<CopMovement>().Damage(collision.contacts[0].point));
                }*/
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Diamond"))
        {
            ParticleSystem splash = Instantiate(splashEffect, other.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            splash.Play();
            Destroy(other.gameObject);
            canvas.GetComponent<UIOperations>().CollectDiamond();
            playCollectSound();
        }
    }

    public void OnDamage()
    {
        StartCoroutine(Damage());
    }

    public IEnumerator Damage()
    {
        damaging = true;

        health--;
        canvas.GetComponent<HealthBar>().UpdateHearts(health);
        if (health <= 0)
        {
            GameOver();
            //DestroyCar(point);

        }
        StartCoroutine(HittedAnimation());
        yield return new WaitForSeconds(recoveryTime);
        while (collisionCount > 0)
        {
            //health--;
            //canvas.GetComponent<HealthBar>().UpdateHearts(health);
            //if (health <= 0)
            //{
            //    GameOver();
            //    //DestroyCar(point);

            //}
            //StartCoroutine(HittedAnimation());
            //yield return new WaitForSeconds(recoveryTime);
        }
        damaging = false;
    }

    IEnumerator HittedAnimation()
    {        
        gameObject.layer = 10;
        SetMaterialsToTransparent();
        yield return new WaitForSeconds(0.5f);
        SetOriginMaterials();
        yield return new WaitForSeconds(0.2f);
        SetMaterialsToTransparent();
        yield return new WaitForSeconds(0.5f);
        SetOriginMaterials();
        yield return new WaitForSeconds(0.2f);
        SetMaterialsToTransparent();
        yield return new WaitForSeconds(0.2f);
        SetOriginMaterials();
        yield return new WaitForSeconds(0.1f);
        SetMaterialsToTransparent();
        yield return new WaitForSeconds(0.1f);
        SetOriginMaterials();
        gameObject.layer = 0;
    }

    void SetOriginMaterials()
    {
        StandardShaderUtils.ChangeRenderMode(mainMaterial, StandardShaderUtils.BlendMode.Opaque);
        mainMaterial.color = mainMatColor;
    }
    void SetMaterialsToTransparent()
    {
        StandardShaderUtils.ChangeRenderMode(mainMaterial, StandardShaderUtils.BlendMode.Transparent);
        mainMaterial.color = Color.white;
    }

    public void SetMainColor(Color color)
    {
        mainMatColor = color;
        mainMaterial.color = mainMatColor;
    }

    public void DisableFoamPS()
    {
        if (foamObjects.Count <= 0) return;

        for(int i = 0; i < foamObjects.Count; i++)
        {
            foamObjects[i].SetActive(false);
        }
    }
}
