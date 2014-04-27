using UnityEngine;
using System.Collections;

public class Fishjump : MonoBehaviour {
    const float jumpSpeed = 123f;
    public GameObject splash;

	// Use this for initialization
	void Start () 
    {
      
        StartCoroutine(coJump());
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    const float jumpSpeedMin = 350f;
    const float jumpSpeedMax = 500f;
    const float jumpSpinMin = 350f;
    const float jumpSpinMax = 500f;
    const float jumpDist = 10f;
    const float jumpHeight = 5f;
    IEnumerator coJump()
    {
        Vector3 origPos = transform.position;
        rigidbody.useGravity = true;
        renderer.enabled = true;
        rigidbody.AddForce(Vector3.up * Random.Range(jumpSpeedMin, jumpSpeedMax));
        //rigidbody.AddForce(new Vector3(Random.Range(jumpSpeedMin, jumpSpeedMax), );
        rigidbody.AddTorque(new Vector3(0,0,Random.Range(50,200)));

        yield return new WaitForSeconds(Random.Range (1.5f, 3.0f));

        //TODO DoSplash
        renderer.enabled = false;
        splash.renderer.enabled = true;

        //scale splash
        splash.transform.position = transform.position;
        for (int i = 0; i < 10; i++ )
        {
            Vector3 scale = splash.transform.localScale;
            scale.x += 0.5f;
            scale.y += 0.5f;
            splash.transform.localScale = scale;
            yield return new WaitForSeconds(0.02f);
        }

        rigidbody.useGravity = false;
        Destroy(gameObject, 0.2f);
    }

}