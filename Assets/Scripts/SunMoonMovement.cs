﻿using UnityEngine;
using System.Collections;

public class SunMoonMovement : MonoBehaviour
{
	const float arcSize = 13f;
	float totalDuration = 6f;
	const float travelDegrees = 100f;

    public bool endgame = false;
    public bool stopped = false;

    GameObject sun;
    GameObject moon;
    Light light;

	Vector3 sunPivot;
	Vector3 moonPivot;

	// Use this for initialization
	void Start ()
	{
        sun = GameObject.Find("Sun");
        moon = GameObject.Find("Moon");
        light = GameObject.Find("SunLight").GetComponent<Light>();

		sunPivot = sun.transform.position;
		sunPivot.x -= arcSize * Mathf.Cos(Mathf.Deg2Rad * (90.0f + travelDegrees/2.0f));
		sunPivot.y -= arcSize * Mathf.Sin(Mathf.Deg2Rad * (90.0f + travelDegrees/2.0f));

		moonPivot = moon.transform.position;
		moonPivot.x -= arcSize * Mathf.Cos(Mathf.Deg2Rad * (270.0f - travelDegrees/2.0f));
		moonPivot.y -= arcSize * Mathf.Sin(Mathf.Deg2Rad * (270.0f - travelDegrees/2.0f));

        StartCoroutine(coMoveSunMoon());
	}	

	// Update is called once per frame
	void Update ()
	{
	
	}

    float greenShift = 0.001f;
    float blueshift = 0.005f;
    float lightIntensityShift = 0.01f;
    bool firstreset = true;
    IEnumerator coMoveSunMoon()
    {
		float time = 0.0f;	// TODO: get the elapsed round time from the master control program
		bool finished = false;
        Color clrOrig = light.color;
        Color clr = clrOrig;
        float origIntensity = light.intensity;

        bool fadein = false;
		while (!finished)
		{

			float percentTraveled = time / totalDuration;
			Vector3 pos;
			float angle;

			angle = Mathf.Deg2Rad * (90.0f - travelDegrees * (percentTraveled - 0.5f));
			pos.x = sunPivot.x + Mathf.Cos(angle) * arcSize;
			pos.y = sunPivot.y + Mathf.Sin(angle) * arcSize;
			pos.z = sunPivot.z;
			sun.transform.position = pos;

			angle = Mathf.Deg2Rad * (270.0f + travelDegrees * (percentTraveled - 0.5f));
			pos.x = moonPivot.x + Mathf.Cos(angle) * arcSize;
			pos.y = moonPivot.y + Mathf.Sin(angle) * arcSize;
			pos.z = moonPivot.z;
			moon.transform.position = pos;
 
            //color shift
            float clrDir=1;
            if (time <= totalDuration / 2)
                clrDir = 1f;
            else
                clrDir = -1f;

            clr.g += greenShift  *clrDir;
            clr.b += blueshift  *clrDir;
            light.color = clr;
            light.intensity -= lightIntensityShift*clrDir;

			yield return new WaitForSeconds(0.02f);
			time += 0.02f;

            if (fadein)
            {
                //Light fade In
                for (int i = 0; i < 100; i++)
                {
                    light.intensity += .025f;
                    yield return new WaitForSeconds(0.02f);
                }
                fadein = false;
            }

            if (time >= totalDuration)
            {
                fadein = true;

                //reset time
                time = 0.0f;

                //if input set total time to one day
                if (firstreset)
                {
                    totalDuration = 80f;
                    greenShift = 0.0000751f;
                    blueshift = 0.000375f;
                    lightIntensityShift = 0.000751f;
                }
                firstreset = false;

                //Dim out
                for (int i = 0; i < 100;i++ )
                {
                    light.intensity -= .025f;
                    yield return new WaitForSeconds(0.02f);
                }
                //reset color
                clr = clrOrig;

                //check for endgame
                if (endgame)
                {
                    finished = true;
                    stopped = true; // flag for endgame seq.
                }
            }
		}
	}
}
