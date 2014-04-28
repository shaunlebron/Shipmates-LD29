using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	public GameObject waterRingPrefab;
	public GameObject fishPrefab;

	private MeshRenderer ballRenderer;
	private ParticleSystem smokeParticle;
	
	private float fMaxHeight = -4.75f;
	private float fMinHeight = -8f;

	public float LifeModifier { get; set; }

	private Rigidbody m_MyRB;

	
	private Vector3 m_OriginPos;

	private Vector3 m_fGravity;
	private Transform m_MyTransform;
	
	
	private bool m_bFishSpawned = false;
	private bool m_bBallStopped = false;
	// Use this for initialization
	void Start () {

		
		m_MyRB = rigidbody;
		m_fGravity = Physics.gravity;
		
		m_MyTransform = transform;
		smokeParticle = m_MyTransform.FindChild ("BallParticles").GetComponent<ParticleSystem>();
		ballRenderer = GetComponent<MeshRenderer> ();
		m_OriginPos = m_MyTransform.position;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//m_MyRB.AddForce (-m_fGravity);

	}

	void Update()
	{
		
		if(Vector3.Distance(m_MyTransform.position, m_OriginPos) >= LifeModifier || m_MyTransform.position.y > fMaxHeight
			|| m_MyTransform.position.y <= fMinHeight)
		{
			if(!m_bBallStopped)
			{
				m_bBallStopped = true;
				smokeParticle.enableEmission = false;
				ballRenderer.enabled = false;
				m_MyRB.constraints = RigidbodyConstraints.FreezeAll;
				if(CheckForPirateCollision())
				{
					// scaring away the fishes
//					if(GameObject.Find ("ShadowFish") != null)
//					{
//						GameObject.Find ("ShadowFish").GetComponent<ShadowFish>().ScaredAway = true;
//					}
				}
				else
				{
					
					if(!m_bFishSpawned)
					{
						Instantiate(fishPrefab, m_MyTransform.position, Quaternion.identity);
						m_bFishSpawned = true;
					}
				}
			}
			
			
			Destroy(gameObject, 2.0f);
		}
		
	}
	
	bool CheckForPirateCollision()
	{
		if(GameObject.Find ("BaddiePirateShip") == null)
			return false;
		Transform pirateShip = GameObject.Find("BaddiePirateShip").transform;
		if(pirateShip != null)
		{
			//Debug.Log (Vector3.Distance (m_MyTransform.position, pirateShip.position));
			if(Vector3.Distance (m_MyTransform.position, pirateShip.position) <= 2.5f)
			{
				//add sound effect
				StartCoroutine(coFadePirateShip(pirateShip));
				return true;
				//send message to signal ship sank
			}
		}
		else
		{
			//Debug.Log ("No ship found");
		}
		
		
		
		return false;
	
	}

	IEnumerator coFadePirateShip(Transform ship)
	{
		//fade logo
        Color origCLR = ship.renderer.material.GetColor("_Color");
        Color clr = origCLR;
        float dir = 1;
        for (int i = 0; i < 100; i++)
        {
            clr.a -= 0.01f * dir;
            ship.renderer.materials[0].SetColor("_Color", clr);
            yield return new WaitForSeconds(0.02f);
        }
	}
}
