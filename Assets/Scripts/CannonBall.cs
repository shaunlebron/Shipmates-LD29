using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	public GameObject waterRingPrefab;
	public GameObject fishPrefab;

	private MeshRenderer ballRenderer;
	private ParticleSystem smokeParticle;
	
	private float fMaxHeight;

	public float LifeModifier { get; set; }

	private Rigidbody m_MyRB;

	
	private Vector3 m_OriginPos;

	private Vector3 m_fGravity;
	private Transform m_MyTransform;
	
	
	private bool m_bFishSpawned = false;

	// Use this for initialization
	void Start () {

		
		m_MyRB = rigidbody;
		m_fGravity = Physics.gravity;
		
		m_MyTransform = transform;
		smokeParticle = m_MyTransform.FindChild ("BallParticles").GetComponent<ParticleSystem>();
		ballRenderer = GetComponent<MeshRenderer> ();
		m_OriginPos = m_MyTransform.position;
		fMaxHeight = m_OriginPos.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		m_MyRB.AddForce (-m_fGravity);

	}

	void Update()
	{
		
		if(Vector3.Distance(m_MyTransform.position, m_OriginPos) >= LifeModifier || m_MyTransform.position.y >= fMaxHeight)
		{
			smokeParticle.enableEmission = false;
			ballRenderer.enabled = false;
			if(!m_bFishSpawned)
			{
				Instantiate(fishPrefab, m_MyTransform.position, Quaternion.identity);
				m_bFishSpawned = true;
			}
			Destroy(gameObject, 2.0f);
		}
		
	}


}
