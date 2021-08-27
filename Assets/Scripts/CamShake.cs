using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
	private Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;
	private float camshakecount = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;
	public static bool Camshake;
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = this.transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if(Camshake)
		{ CamShakeOn(); }
		
	}

	public void CamShakeOn()
    {
		if (shakeDuration > camshakecount)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			camshakecount += Time.deltaTime * decreaseFactor;
		}
		else
		{
			camshakecount = 0f;
			Camshake = false;
			camTransform.localPosition = originalPos;
		}
	}
}
