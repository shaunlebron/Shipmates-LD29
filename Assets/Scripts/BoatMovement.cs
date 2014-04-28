using UnityEngine;
using System.Collections;

public class BoatMovement : MonoBehaviour 
{
    public float bobSpeed = 2.5f;
    public float bobHeight = 0.03f;
    public float maxRotate = 2.0f; //  in degrees

    public GameObject boatFishingUpper;
    public GameObject boatFishingLower;
    public GameObject boatPirateUpper;
    public GameObject boatPirateLower;

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
        //boatFishingUpper = GameObject.Find("BoatFishingUpper");
        //boatFishingLower = GameObject.Find("BoatFishingLower");
        //boatPirateUpper = GameObject.Find("BoatPirateUpper");
        //boatPirateLower = GameObject.Find("BoatPirateLower");

		boatFishingUpperStartPos = boatFishingUpper.transform.position;
		boatFishingLowertartPos = boatFishingLower.transform.position;
        if (boatPirateUpper != null)
		    boatPirateUpperStartPos = boatPirateUpper.transform.position;
        if (boatPirateLower != null)
		boatPirateLowerStartPos = boatPirateLower.transform.position;

		boatFishingUppperOrgTransform = boatFishingUpper.transform;
		boatFishingLowerOrgTransform = boatFishingLower.transform;
        if (boatPirateUpper != null)
            boatPirateUpperOrgTransform = boatPirateUpper.transform;
        if (boatPirateLower != null) 
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

            if (boatFishingUpper != null)
            {
                boatFishingUpper.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotate);
                pos = boatFishingUpperStartPos;
                pos.y += delta;
                boatFishingUpper.transform.position = pos;
            }

            if (boatFishingLower != null)
            {
                boatFishingLower.transform.eulerAngles = new Vector3(0.0f, 0f, -rotate);
                pos = boatFishingLowertartPos;
                pos.y -= delta;
                boatFishingLower.transform.position = pos;
            }

            if (boatPirateUpper != null)
            {
                boatPirateUpper.transform.eulerAngles = new Vector3(0.0f, 0.0f, rotate);
                pos = boatPirateUpperStartPos;
                pos.y += delta;
                boatPirateUpper.transform.position = pos;
            }

            if (boatPirateLower != null)
            {
                boatPirateLower.transform.eulerAngles = new Vector3(0.0f, 0.0f, -rotate);
                pos = boatPirateLowerStartPos;
                pos.y -= delta;
                boatPirateLower.transform.position = pos;
            }

			yield return new WaitForSeconds(0.02f);
			time += 0.02f;
             
        }
    }


}
