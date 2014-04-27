using UnityEngine;
using System.Collections;

public class ShadowFishScurry : MonoBehaviour 
{
	float swimVelocity = 5.0f;
	float leftLimit = -16.5f;
	float rightLimit = 16.5f;

	// Use this for initialization
	void Start () 
    { 

	}
	
	// Update is called once per frame
	void Update () 
    {
		Vector3 scale = transform.localScale;
		Vector3 pos = transform.position;
		if(transform.position.x > rightLimit || transform.position.x < leftLimit )
		{
			//Debug.Log("Just died");
			Destroy(gameObject);
			Destroy (this);
		}
		else
		{
			if(scale.y > 0 )
				transform.position = new Vector3(pos.x - swimVelocity * Time.deltaTime, pos.y, pos.z);
			if(scale.y < 0 )
				transform.position = new Vector3(pos.x + swimVelocity * Time.deltaTime, pos.y, pos.z);
		}
			
	}

    void FixedUpdate()
    {
        
    }


}