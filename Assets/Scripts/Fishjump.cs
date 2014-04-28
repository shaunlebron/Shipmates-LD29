using UnityEngine;
using System.Collections;

public class Fishjump : MonoBehaviour {
    const float jumpSpeed = 123f;
    public GameObject splash;
    public GameObject splash2;
	public GameObject shadowFish;
	// Use this for initialization
    Intro intro;
	void Start () 
    {
        intro = GameObject.Find("Intro").GetComponent<Intro>();
        StartCoroutine(coJump());
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    const float jumpSpeedMin = 50f;
    const float jumpSpeedMax = 150f;
    const float jumpSpinMin = 850f;
    const float jumpSpinMax = 1000f;
    const float jumpDist = 10f;
    const float jumpHeight = 5f;
    IEnumerator coJump()
    {
        audio.PlayOneShot(intro.GP8);

        //Lag to jump
        renderer.enabled = false;
        rigidbody.useGravity = false;
        //yield return new WaitForSeconds(Random.Range(0.2f,0.5f));

        //jump
        StartCoroutine(coSplash(splash, false));
        renderer.enabled = true;
        rigidbody.useGravity = true;
        
        Vector3 origPos = transform.position;
        rigidbody.useGravity = true;
        renderer.enabled = true;
        rigidbody.AddForce(Vector3.up * Random.Range(jumpSpeedMin, jumpSpeedMax));
        rigidbody.AddTorque(new Vector3(0,0,Random.Range(50,200)));

        yield return new WaitForSeconds(Random.Range (0.5f, 0.9f));

        StartCoroutine(coSplash(splash2, false));

        rigidbody.useGravity = false;

		Instantiate (shadowFish, transform.position, shadowFish.transform.rotation);
        shadowFish.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        Destroy(GameObject.Find("fish(Clone)"), 0.2f);
    }

    IEnumerator coSplash(GameObject s, bool first)
    {
        //TODO DoSplash
        renderer.enabled = false;
        if (first)
            s = GameObject.Find("FishSplash");
        else
            s = GameObject.Find("FishSplash2");
        s.renderer.enabled = true;

        //scale splash
        s.transform.position = transform.position;
        for (int i = 0; i < 20; i++)
        {
            Vector3 scale = s.transform.localScale;
            scale.x += 0.06f;
            scale.y += 0.06f;
            s.transform.localScale = scale;
            yield return new WaitForSeconds(0.02f);
        }
        s.renderer.enabled = false;
    }
}