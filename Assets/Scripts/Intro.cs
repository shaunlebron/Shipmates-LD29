using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    IEnumerator coStartIntro()
    {
        yield return new WaitForSeconds(1f);
    }
}
