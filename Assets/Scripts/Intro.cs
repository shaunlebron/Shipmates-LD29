using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
    public GameObject logo;
    public GameObject credits;
    public GameObject dock;
    public GameObject instructions;
    public ParticleSystem parts;
    public GameObject boatReflection;
    public GameObject boatReflectionPirate;
    public GameObject kid;
    public GameObject kidAnimation;
    public GameObject gPa;


    Inputs input;
    
    public AudioClip GP1;
    public AudioClip GP2;
    public AudioClip GP3;
    public AudioClip GP4;
    public AudioClip GP5;
    public AudioClip GP6;
    public AudioClip GP7;
    public AudioClip GP8;
    public AudioClip GP9;
    public AudioClip GP10;

    public AudioClip KT1;
    public AudioClip KT2;
    public AudioClip KT3;
    public AudioClip KT4;
    public AudioClip KT5;
    public AudioClip KT6;
    public AudioClip KT7;
    public AudioClip KT8;
    public AudioClip KT9;
    public AudioClip KT10;
    public AudioClip KT11;
    public AudioClip KT12;
    public AudioClip KT13;
    public AudioClip KT14;

    const float endGameTime = 80;

    //AudioSource audio;

	// Use this for initialization
	void Start () 
    {
        input = GameObject.Find("Scene").GetComponent<Inputs>();

        StartCoroutine(coLogo());
        StartCoroutine(coMoveDock());
        StartCoroutine(coGripe());
        StartCoroutine(coEndGame());
        
        input.SetDisabled(true);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    IEnumerator coLogo()
    {
        //lag
        yield return new WaitForSeconds(1.5f);

        //fade logo
        Color origCLR = logo.renderer.material.GetColor("_Color");
        Color clr = origCLR;
        float dir = 1;
        for (int i = 0; i < 400; i++)
        {
            if (i == 200)
            {
                dir = -1;
                yield return new WaitForSeconds(1.0f);
            }

            clr.a += 0.01f * dir;
            logo.renderer.materials[0].SetColor("_Color", clr);
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator coInstructions()
    {
        //lag
        yield return new WaitForSeconds(1.5f);

        //fade logo
        Color origCLR = instructions.renderer.material.GetColor("_Color");
        Color clr = origCLR;
        float dir = 1;
        for (int i = 0; i < 200; i++)
        {
            if (i == 100)
            {
                dir = -1;
                yield return new WaitForSeconds(1.0f);
            }

            clr.a += 0.01f * dir;
            instructions.renderer.materials[0].SetColor("_Color", clr);
            yield return new WaitForSeconds(0.02f);
        }
    }
    IEnumerator coGripe()
    {
        //KidGripe
        audio.PlayOneShot(KT1);
        yield return new WaitForSeconds(1.5f);
        audio.PlayOneShot(GP1);
        yield return new WaitForSeconds(5.3f);

        audio.PlayOneShot(KT2);
        yield return new WaitForSeconds(2.6f);              
        //audio.PlayOneShot(GP2);
        //yield return new WaitForSeconds(3.4f);

        //audio.PlayOneShot(KT3);
        //yield return new WaitForSeconds(3f);
        audio.PlayOneShot(GP3);
        yield return new WaitForSeconds(4.8f);
        audio.PlayOneShot(KT3);
        yield return new WaitForSeconds(3f);

        //enable gameinputs
        StartCoroutine(coInstructions());
        input.SetDisabled(false);

        StartCoroutine(coImaPirate());
    }

    IEnumerator coMoveDock()
    {
        //MoveDock
        Vector3 pos = dock.transform.position;
        //parts.constantForce.relativeForce = new Vector3(-2,0,0);
        for (int i = 0; i < 400; i++)
        {
            pos.x -= 0.03f;
            dock.transform.position = pos;
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator coImaPirate()
    {
        Color br = boatReflection.renderer.material.GetColor("_Color");
        Color brp = boatReflectionPirate.renderer.material.GetColor("_Color");
        Color cKid = kid.renderer.material.GetColor("_Color");
        Color cGPA = gPa.renderer.material.GetColor("_Color");
       
        //Crossfade reflection
        for (int i = 0; i<160;i++)
        {
            br.a -= 0.01f;
            brp.a += 0.005f;
            cKid.a += 0.003f;
            cGPA.a += 0.003f;

            boatReflection.renderer.material.SetColor("_Color", br);
            boatReflectionPirate.renderer.material.SetColor("_Color", brp);
            kid.renderer.material.SetColor("_Color", cKid);
            gPa.renderer.material.SetColor("_Color", cGPA);
            yield return new WaitForSeconds(0.02f);
        }

        //Change to animated girl
        kid.renderer.enabled = false;
        kidAnimation.renderer.enabled = true;

        audio.PlayOneShot(KT5);
        
    }

    
    IEnumerator coEndGame()
    {
        //Wait for end game
        yield return new WaitForSeconds(endGameTime);

        //Set Sun to endgame and wait for it to finish
        GameObject.Find("SunMoon").GetComponent<SunMoonMovement>().endgame = true;
        //while (!GameObject.Find("SunMoon").GetComponent<SunMoonMovement>().stopped)
        //    yield return new WaitForSeconds(0.3f);

        //time to go home
        audio.PlayOneShot(GP4);
        yield return new WaitForSeconds(2.2f);

        StartCoroutine(coMoveDockIn());

        //girl happy/sad response
        if (GameObject.Find("Scene").GetComponent<Inputs>().sunkaShip)
        {
            audio.PlayOneShot(KT1);
            yield return new WaitForSeconds(4f);
        }
        else
        {
            audio.PlayOneShot(KT6);
            yield return new WaitForSeconds(2.3f);
        }
        //ShowCredits
        StartCoroutine(coCredits());

        //SuccessFailureGirl
        if (GameObject.Find("Scene").GetComponent<Inputs>().sunkaShip)
        {
            audio.PlayOneShot(KT7);
            yield return new WaitForSeconds(2f);
        }
        else
        {
            audio.PlayOneShot(KT8);
            yield return new WaitForSeconds(4f);
        }
        //SuccessFailureGrandpa
        if (GameObject.Find("Scene").GetComponent<Inputs>().caughtFish)
        {
            audio.PlayOneShot(GP5);
            yield return new WaitForSeconds(4f);
        }
        else
        {
            audio.PlayOneShot(GP6);
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator coMoveDockIn()
    {
        //Move Dock in
        GameObject.Find("Scene").GetComponent<Inputs>().SetDisabled(true);
        dock.transform.localPosition = new Vector3(
            22, //move dock offscreen right
            dock.transform.localPosition.y,
            dock.transform.localPosition.z);
        while (dock.transform.localPosition.x > 11.65f)
        {
            dock.transform.localPosition = new Vector3(
                dock.transform.localPosition.x - 0.03f, //move dock offscreen right
                dock.transform.localPosition.y,
                dock.transform.localPosition.z);
            yield return new WaitForSeconds(0.02f);
        }
        Debug.Log(dock.transform.position.x);

    }

    IEnumerator coCredits()
    {
        //fade logo
        Color origCLR = credits.renderer.material.GetColor("_TintColor");
        Color clr = origCLR;
        float dir = 1;
        for (int i = 0; i < 100; i++)
        {
            clr.a += 0.01f * dir;
            credits.renderer.materials[0].SetColor("_TintColor", clr);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
