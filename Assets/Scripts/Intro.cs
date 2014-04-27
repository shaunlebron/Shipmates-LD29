using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
    public GameObject logo;
    public GameObject credits;
    public GameObject dock;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(coStartIntro());
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    IEnumerator coStartIntro()
    {
        //fadein logo
        Color origCLR = logo.renderer.material.GetColor("_Color");
        Color clr = origCLR;
        float dir = 1;
        for (int i = 0; i < 200; i++ )
        {
            if (i == 100)
            {
                dir = -1;
                yield return new WaitForSeconds(1.0f);
            }
                
            clr.a += 0.01f*dir;
            logo.renderer.materials[0].SetColor("_Color", clr);
            yield return new WaitForSeconds(0.04f);
        }
            yield return new WaitForSeconds(1f);
    }
}
