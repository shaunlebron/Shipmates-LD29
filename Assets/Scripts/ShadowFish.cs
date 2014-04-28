using UnityEngine;
using System.Collections;

public class ShadowFish : MonoBehaviour 
{
	
	Vector3 initialPosition;
	float swimVelocity = 1.0f;
	float swimRadius = 1.0f;
	float timer = 0.0f;
	
	public bool ScaredAway {get; set;}
	
	// Use this for initialization
	void Start () 
    { 
		ScaredAway = false;
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
    bool swimming = true;
	void FixedUpdate () 
    {
        if (swimming)
        {
            //Debug.Log(timer);
            float dist = (transform.position.x - initialPosition.x);
            if (dist > swimRadius)
            {
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x, Mathf.Abs(scale.y), scale.z);
                swimVelocity = -Mathf.Abs(swimVelocity);
            }
            else if (dist < -swimRadius)
            {
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x, -Mathf.Abs(scale.y), scale.z);
                swimVelocity = Mathf.Abs(swimVelocity);
            }

            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x + swimVelocity * Time.deltaTime, pos.y,0 );
        }

        //Check dist to bobber
        GameObject bobber = GameObject.Find("Bobber(Clone)");
        if (bobber != null)
        {
            float dist = Vector3.Distance(bobber.transform.position, transform.position);
            if (dist < 3f && swimming)
            {
                //Catch Fish
                swimming = false;
                StartCoroutine(coCatchFish());
            }
        }

		//timer is temporary, this conditional needs to be activated by a ship hit event
        //timer += 1.0f;
        //if(timer == 240 || ScaredAway)
        //{
        //    gameObject.AddComponent("ShadowFishScurry");
        //    Destroy(this);
        //}
		
	}

    IEnumerator coCatchFish()
    {
        //GotoBobber
        Transform bobber = GameObject.Find("Bobber(Clone)").transform;
        Vector3 targetpos = bobber.position;
        targetpos.y -= 0.5f;

        bool finished = false;
        for (int i=0; i<50; i++)
        {
            transform.position = Vector3.Lerp(transform.position, targetpos, Time.deltaTime * 2);
            yield return new WaitForSeconds(0.02f);
        }

        //Nibble
        GameObject splash = GameObject.Find("BobberSplash");
        splash.renderer.enabled = true;
        StartCoroutine(GameObject.Find("Scene").GetComponent<Inputs>().coSplash(splash.transform));
        Destroy(GameObject.Find("Bobber(Clone)").GetComponent<BoatMovement>());
        Destroy(GameObject.Find("Bobber(Clone)").GetComponent<Bobber>());

        GameObject.Find("Scene").GetComponent<Inputs>().caughtFish = true;

        float dir = 1;
        Vector3 newpos = bobber.transform.position;
        for (int i = 0; i < 10; i++)
        {
            newpos.y -= 0.005f * dir;
            bobber.transform.position = newpos;
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < 10; i++)
        {
            newpos.y += 0.005f * dir;
            bobber.transform.position = newpos;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++)
        {
            newpos.y -= 0.005f * dir;
            bobber.transform.position = newpos;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++)
        {
            newpos.y += 0.005f * dir;
            bobber.transform.position = newpos;
            yield return new WaitForSeconds(0.01f);
        }

        //StartCoroutine(GameObject.Find("Scene").GetComponent<Inputs>().coBobberNibble());
        //yield return new WaitForSeconds(Random.Range(1.2f, 2.5f));

        //Animate to boat
        finished = false;
        while (!finished)
        {
            Vector3 boatpos = GameObject.Find("OldMan").transform.position;
            transform.position = Vector3.Lerp(transform.position, boatpos, Time.deltaTime*2);
            bobber.position = Vector3.Lerp(bobber.position, boatpos, Time.deltaTime * 2);

            if (Vector3.Distance(transform.position, boatpos) < 0.5f)
                finished = true;
            yield return new WaitForSeconds(0.02f);
        }

        //Destroy fish
        Destroy(this);
        Destroy(bobber.gameObject);
    }
}
