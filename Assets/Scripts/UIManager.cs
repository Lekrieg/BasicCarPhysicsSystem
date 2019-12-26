using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI speedometerText;

	readonly float CONVERSION_VALUE = 3.6f;

	void Start()
	{
		speedometerText = GameObject.Find("SpeedometerText").GetComponent<TextMeshProUGUI>();
	}

	public virtual void UpdateSpeedometer(float speed)
	{
		float s = speed * CONVERSION_VALUE;

		speedometerText.text = Mathf.Abs(Mathf.Round(s)) + " Km/H";
	}
}
