using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour {

	public GameObject waterRingPrefab;
	public GameObject fishPrefab;

	private MeshRenderer ballRenderer;
	private ParticleSystem smokeParticle;
	private float fTimeToLive = 2f;
	private float fMaxHeight = -4.5f;

	public float LifeModifier { get; set; }

	private Rigidbody m_MyRB;

	private float m_fLifeTimer;
	private Vector3 m_fMaxDistance;

	private Vector3 m_fGravity;
	private Transform m_MyTransform;
	private float m_fHalfTime;
	private float m_fStartTime;
	private bool m_bProjectileReachedApex = false;
	private bool m_bFishSpawned = false;

	// Use this for initialization
	void Start () {

		m_fHalfTime = 9999;
		m_fStartTime = Time.time;
		m_MyRB = rigidbody;
		m_fGravity = Physics.gravity;
		m_fLifeTimer = Time.time + fTimeToLive;
		m_MyTransform = transform;
		smokeParticle = m_MyTransform.FindChild ("BallParticles").GetComponent<ParticleSystem>();
		ballRenderer = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		m_MyRB.AddForce (-m_fGravity);

	}

	void Update()
	{
		if (Time.time >= m_fLifeTimer || m_MyTransform.position.y >= fMaxHeight) 
		{
			m_MyRB.constraints = RigidbodyConstraints.FreezeAll;
			smokeParticle.enableEmission = false;
			ballRenderer.enabled = false;
			if(!m_bFishSpawned)
			{
				Instantiate(fishPrefab, m_MyTransform.position, Quaternion.identity);
				m_bFishSpawned = true;
			}
			Destroy(gameObject, 2.0f);
		}
		if (m_MyRB.velocity.y > 0 && !m_bProjectileReachedApex) 
		{
			m_bProjectileReachedApex = true;
			m_fHalfTime = Time.time - m_fStartTime;
			m_fLifeTimer = Time.time + (m_fHalfTime + LifeModifier);
		}
	}


}
