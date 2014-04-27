using UnityEngine;
using System.Collections;

public class BoatMovement : MonoBehaviour 
{
    const float bobSpeed = 2.5f;
    const float bobHeight = 0.03f;
	const float maxRotate = 2.0f; //  in degrees

    GameObject boatFishingUpper;
    GameObject boatFishingLower;
    GameObject boatPirateUpper;
    GameObject boatPirateLower;

	Vector3 boatFishingUpperStartPos;
	Vector3 boatFishingLowertartPos;
	Vector3 boatPirateUpperStartPos;
	Vector3 boatPirateLowerStartPos;

	Transform boatFishingUppperOrgTransform;
	Transform boatFishingLowerOrgTransform;
	Transform boatPirateUpperOrgTransform;
	Transform boatPirateLowerOrgTransform;

	// Use this for initialization
	void Start () 
    {
        boatFishingUpper = GameObject.Find("BoatFishingUpper");
        boatFishingLower = GameObject.Find("BoatFishingLower");
        boatPirateUpper = GameObject.Find("BoatPirateUpper");
        boatPirateLower = GameObject.Find("BoatPirateLower");

		boatFishingUpperStartPos = boatFishingUpper.transform.position;
		boatFishingLowertartPos = boatFishingLower.transform.position;
		boatPirateUpperStartPos = boatPirateUpper.transform.position;
		boatPirateLowerStartPos = boatPirateLower.transform.position;

		boatFishingUppperOrgTransform = boatFishingUpper.transform;
		boatFishingLowerOrgTransform = boatFishingLower.transform;
		boatPirateUpperOrgTransform = boatPirateUpper.transform;
		boatPirateLowerOrgTransform = boatPirateLower.transform;

        StartCoroutine(coBoatBob());
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void FixedUpdate()
    {
        
    }

    IEnumerator coBoatBob()
    {
		float time = 0.0f;
        bool finished = false;
        while (!finished)
        {
			Vector3 pos;
			float cycle = Mathf.Sin(2.0f * Mathf.PI * time/bobSpeed); // cycles between -1.0f and 1.0f
			float delta = cycle * bobHeight;
			float rotate = cycle * maxRotate;

			boatFishingUpper.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotate);
			pos = boatFishingUpperStartPos;
			pos.y += delta;
			boatFishingUpper.transform.position = pos;

			boatFishingLower.transform.eulerAngles = new Vector3(0.0f, 0.0f, -rotate);
			pos = boatFishingLowertartPos;
			pos.y -= delta;
			boatFishingLower.transform.position = pos;

			boatPirateUpper.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotate);
			pos = boatPirateUpperStartPos;
			pos.y += delta;
			boatPirateUpper.transform.position = pos;

			boatPirateLower.transform.eulerAngles = new Vector3(0.0f, 0.0f, -rotate);
			pos = boatPirateLowerStartPos;
			pos.y -= delta;
			boatPirateLower.transform.position = pos;

			yield return new WaitForSeconds(0.02f);
			time += 0.02f;
             
        }
    }


}
