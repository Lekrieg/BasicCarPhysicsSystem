using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI speedometerText;

	void Start()
	{
		speedometerText = GameObject.Find("SpeedometerText").GetComponent<TextMeshProUGUI>();
	}

	public virtual void UpdateSpeedometer(float speed)
	{
		speedometerText.text = Mathf.Abs(Mathf.Round(speed)) + " Km/H";
	}
}
