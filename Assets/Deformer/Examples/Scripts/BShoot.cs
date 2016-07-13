using UnityEngine;
using System.Collections;
using Flashunity.Deformer;

public class BShoot : MonoBehaviour
{
    public float fireRate = .25f;
    public float range = 50;
    //    public ParticleSystem smokeParticles;
    //    public GameObject hitParticles;
    //  public GameObject shootFlare;
    public int damage = 1;
    //    public Transform gunEnd;

    private Camera fpsCam;
    private LineRenderer lineRenderer;
    private WaitForSeconds shotLength = new WaitForSeconds(.07f);
    //    private AudioSource source;
    private float nextFireTime;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //    source = GetComponent<AudioSource>();
        fpsCam = GetComponent<Camera>();
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range))
            {
                var dmgScript = hit.collider.gameObject.GetComponent<BDeform>();

                if (dmgScript != null)
                {
                    dmgScript.Deform(hit.point, fpsCam.transform.forward * damage);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * 100f);
                }


                lineRenderer.SetPosition(0, fpsCam.transform.position + new Vector3(0.1f, -0.1f, 0));
                lineRenderer.SetPosition(1, hit.point);

                //Instantiate(hitParticles, hit.point, Quaternion.identity);

            }
            StartCoroutine(ShotEffect());
        }

    }

    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;
        /*
        source.Play();
        smokeParticles.Play();
        shootFlare.SetActive(true);
        */
        yield return shotLength;
        lineRenderer.enabled = false;
//        shootFlare.SetActive(false);
    }
}
