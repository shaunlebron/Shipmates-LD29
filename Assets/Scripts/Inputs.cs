using UnityEngine;
using System.Collections;

public class Inputs : MonoBehaviour
{
    bool disabled = false;

	const float arrowGrowSpeed = 1.5f; // time it takes to get to full power
	const float MinArrowLength = 1.0f;
	const float MaxArrowLength = 2.0f;
	const float PoleAngleRange = 45.0f; // in degrees
	const float PoleReturnSpeed = 45.0f; // in degrees per second
    public GameObject bobberPrefab;

	float mouseDownStartTime;
	bool mouse0Targeting;
	bool mouse1Targeting;

	GameObject targetingArrowHead;
	GameObject targetingArrowBody;
    GameObject bobberAnchor;

	Vector3 targetingArrowFishermanAnchorPos;
	Vector3[] targetingArrowPirateAnchorPos = new Vector3[4];

	const int numPiratePorts = 4;
	int nextPiratePort = 0;
	string[] piratePortNames = new string[4] {"Port1", "Port2", "Port3", "Port4"};

	GameObject fishingPole;
	float currentPoleAngle = 0; // from -PoleAngleRange to PoleAngleRange
	bool poleReturning = false;

	public CannonFire cannonfire;
    Bobber bobbeScript;

	// Use this for initialization
	void Start ()
	{
		targetingArrowHead = GameObject.Find("ArrowHead");
		targetingArrowBody = GameObject.Find("Arrow");
		targetingArrowHead.renderer.enabled = false;
		targetingArrowBody.renderer.enabled = false;

		GameObject anchor = GameObject.Find("FishermanAnchor");
        bobberAnchor = GameObject.Find("BobberAnchor");
		targetingArrowFishermanAnchorPos = anchor.transform.position;

		anchor = GameObject.Find(piratePortNames[0]);
		targetingArrowPirateAnchorPos[0] = anchor.transform.position;

		anchor = GameObject.Find(piratePortNames[1]);
		targetingArrowPirateAnchorPos[1] = anchor.transform.position;

		anchor = GameObject.Find(piratePortNames[2]);
		targetingArrowPirateAnchorPos[2] = anchor.transform.position;

		anchor = GameObject.Find(piratePortNames[3]);
		targetingArrowPirateAnchorPos[3] = anchor.transform.position;

		mouse0Targeting = false;
		mouse1Targeting = false;
		
		currentPoleAngle = 0;
		fishingPole = GameObject.Find("Pole");
        //StartCoroutine(coPoleReturn());
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (disabled)
            return;

		// Get the world position of the mouse based on the z-depth of the targeting arrow anchor
		Vector3 mouseWorldPosition = Input.mousePosition;
		mouseWorldPosition.z = Vector3.Dot(targetingArrowFishermanAnchorPos - Camera.main.transform.position, Camera.main.transform.forward);
		mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseWorldPosition);

        if (Input.GetMouseButtonDown(0) && !mouse0Targeting && !mouse1Targeting)
		{
			mouseDownStartTime = Time.fixedTime;
			mouse0Targeting = true;
			poleReturning = false;
		}

        if (Input.GetMouseButtonDown(1) && !mouse0Targeting && !mouse1Targeting)
		{
			mouseDownStartTime = Time.fixedTime;
			mouse1Targeting = true;
		}

		if (Input.GetMouseButton(0) && mouse0Targeting) // fisherman
		{
            if (bobber != null)
                Destroy(bobber);

			// Draw targeting arrow
			float heldTime = Time.fixedTime - mouseDownStartTime;
			float power = Mathf.Clamp(heldTime / arrowGrowSpeed, 0.0f, 1.0f);
			Vector3 arrowDirection = (mouseWorldPosition - targetingArrowFishermanAnchorPos).normalized;
			UpdateArrow(targetingArrowFishermanAnchorPos, arrowDirection, power);

			if (arrowDirection.x > 0)
			{
				currentPoleAngle = Mathf.Lerp(0, -PoleAngleRange, arrowDirection.x);
			}
			else
			{
				currentPoleAngle = Mathf.Lerp(0, PoleAngleRange, -arrowDirection.x);
			}
		}

		if (Input.GetMouseButton(1) && mouse1Targeting) // pirate ship
		{
			// Draw targeting arrow
			float heldTime = Time.fixedTime - mouseDownStartTime;
			float power = Mathf.Clamp(heldTime / arrowGrowSpeed, 0.0f, 1.0f);
			Vector3 arrowDirection = (mouseWorldPosition - targetingArrowPirateAnchorPos[nextPiratePort]).normalized;
			UpdateArrow(targetingArrowPirateAnchorPos[nextPiratePort], arrowDirection, power);
		}

        if (Input.GetMouseButtonUp(0) && mouse0Targeting)
		{
			// Shoot bobber
            mouse0Targeting = false;
			targetingArrowHead.renderer.enabled = false;
			targetingArrowBody.renderer.enabled = false;
			poleReturning = true;
			currentPoleAngle = 0;

            float heldTime = Time.fixedTime - mouseDownStartTime;
            float power = Mathf.Clamp(heldTime / arrowGrowSpeed, 0.0f, 1.0f);
            Vector3 arrowDirection = (mouseWorldPosition - targetingArrowFishermanAnchorPos).normalized; 
            StartCoroutine(coTossBobber(arrowDirection, power));
		}

        if (Input.GetMouseButtonUp(1) && mouse1Targeting)
		{
			// Shoot ball
			mouse1Targeting = false;
			targetingArrowHead.renderer.enabled = false;
			targetingArrowBody.renderer.enabled = false;
			
			Vector3 arrowDirection = (mouseWorldPosition - targetingArrowPirateAnchorPos[nextPiratePort]).normalized;
			float heldTime = Time.fixedTime - mouseDownStartTime;
			float power = Mathf.Clamp(heldTime / arrowGrowSpeed, 0.0f, 1.0f);
			cannonfire.FireCannon(arrowDirection, power);

			if (++nextPiratePort >= numPiratePorts)
				nextPiratePort = 0;
		}

		// Draw the fishing pole
		float currentPoleRadians = currentPoleAngle * Mathf.Deg2Rad;
		Vector3 poleDirection = new Vector3(Mathf.Sin(currentPoleRadians), Mathf.Cos(currentPoleRadians), 0.0f);
		float poleScale = Mathf.Lerp(Mathf.Cos(PoleAngleRange), 1.0f, Mathf.Abs(currentPoleAngle)/PoleAngleRange); // scale the pole down so it looks like it is leaning back
		Vector3 polePosition = targetingArrowFishermanAnchorPos + poleDirection * poleScale * 0.5f; // center of the pole quad
		fishingPole.transform.up = poleDirection;
		//fishingPole.transform.right = Vector3.Cross(fishingPole.transform.forward, fishingPole.transform.up); // Not correct, but Unity updated the 'right' when we updated the 'up' above
		//fishingPole.transform.position = polePosition;
		//fishingPole.transform.localScale = new Vector3(poleScale/16, poleScale, poleScale);
	}

    GameObject bobber;
    IEnumerator coTossBobber(Vector3 dir, float power)
    {
        bobber = Instantiate(bobberPrefab) as GameObject;
        bobber.transform.position = bobberAnchor.transform.position;
    
        bobber.rigidbody.AddForce(dir * (power * 10), ForceMode.Impulse);

        float ymin = Random.Range(-8f, -4.4f);
        bool finished = false;
        while (!finished)
        {
            if (bobber.transform.position.y < ymin)
            {
                //stop bobber fall
                bobber.rigidbody.useGravity = false;
                bobber.rigidbody.velocity = Vector3.zero;
                bobber.GetComponent<Bobber>().enabled = true;
                bobber.GetComponent<BoatMovement>().enabled = true;
                GameObject.Find("BaddiePirateShip").renderer.enabled = true;
				finished = true;
            }

            yield return new WaitForSeconds(0.02f);       
        }


        
    }

	void UpdateArrow(Vector3 anchor, Vector3 arrowDirection, float power)
	{
		float arrowLength = Mathf.Lerp(MinArrowLength, MaxArrowLength, power);

		float arrowHeadLength = 1.0f/3.0f;
		float arrowBodyLength = arrowLength - arrowHeadLength;

		Vector3 arrowHeadMid = anchor + (arrowDirection * (arrowLength - arrowHeadLength / 2.0f));
		Vector3 arrowBodyMid = anchor + (arrowDirection * arrowBodyLength / 2.0f);

		targetingArrowHead.transform.up = -arrowDirection;
		targetingArrowHead.transform.right = Vector3.Cross(targetingArrowHead.transform.forward, targetingArrowHead.transform.up);
		targetingArrowHead.transform.position = arrowHeadMid;
		targetingArrowHead.transform.localScale = new Vector3(arrowHeadLength, arrowHeadLength, 1);

		targetingArrowBody.transform.up = targetingArrowHead.transform.up;
		targetingArrowBody.transform.right = targetingArrowHead.transform.right;
		targetingArrowBody.transform.position = arrowBodyMid;
		targetingArrowBody.transform.localScale = new Vector3(arrowHeadLength, arrowBodyLength, 1);

		targetingArrowHead.renderer.enabled = true;
		targetingArrowBody.renderer.enabled = true;
	}

	// co routine to return the pole back upright after you let the mouse button go
	/*
    IEnumerator coPoleReturn()
    {
		bool finished = false;
		float timeStep = 0.1;
		while (!finished)
		{
			if (poleReturning)
			{
				if (currentPoleAngle > 0)
					currentPoleAngle = Mathf.Clamp(
			}
		}
	}
	*/

    public void SetDisabled(bool dis)
    {
        disabled = dis;
    }
}
