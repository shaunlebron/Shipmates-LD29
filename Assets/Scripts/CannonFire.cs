using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {

	public Transform cannonBallPrefab;

	private float fMinPower = 0.25f;
	private float fMinDistance = 0.5f;
	private float fForceMultiplier = 15;
	public Transform[] portholeTransforms;

	private int m_iCannonIndex = 0;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
//	void Update () {
//
//	}

	public void FireCannon(Vector3 direction, float power)
	{
		//Debug.Log ("Power " + power + " Direction.y " + direction.y);
		if (power <= fMinPower) 
		{
			power = fMinPower;
		}
		Rigidbody ballRB;
		Transform newBall = Instantiate (cannonBallPrefab, portholeTransforms[m_iCannonIndex].position - Vector3.forward, Quaternion.identity) as Transform;

		ballRB = newBall.rigidbody;
		newBall.GetComponent<CannonBall> ().LifeModifier = (fMinDistance + (fForceMultiplier * 0.5f) * power);
		ballRB.AddForce (direction * (power * fForceMultiplier), ForceMode.Impulse);

		m_iCannonIndex++;

		if (m_iCannonIndex >= portholeTransforms.Length) 
		{
			m_iCannonIndex = 0;
		}

	}
}
