using UnityEngine;
using System.Collections;

public class ShadowFish : MonoBehaviour 
{
	
	Vector3 initialPosition;
	float swimVelocity = 1.0f;
	float swimRadius = 1.0f;

	// Use this for initialization
	void Start () 
    { 
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
		float dist = (transform.position.x - initialPosition.x);
		if (dist > swimRadius) {
			Vector3 scale = transform.localScale;
			transform.localScale = new Vector3(scale.x, Mathf.Abs(scale.y), scale.z);
			swimVelocity = -Mathf.Abs (swimVelocity);
		}
		else if (dist < -swimRadius) {
			Vector3 scale = transform.localScale;
			transform.localScale = new Vector3(scale.x, -Mathf.Abs(scale.y), scale.z);
			swimVelocity = Mathf.Abs (swimVelocity);
		}
		
		Vector3 pos = transform.position;
		transform.position = new Vector3(pos.x + swimVelocity * Time.deltaTime, pos.y, pos.z);
	}

    void FixedUpdate()
    {
        
    }


}
