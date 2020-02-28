using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	private GameObject button0;
	private GameObject button1;
    private TextMeshProUGUI speedometerText;

	void Start()
	{
		speedometerText = GameObject.Find("SpeedometerText").GetComponent<TextMeshProUGUI>();
		button0 = GameObject.Find("Button0");
		button1 = GameObject.Find("Button1");

		button0.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { SetModel(0); });
		button1.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { SetModel(1); });

		speedometerText.transform.gameObject.SetActive(false);
	}

	private void SetModel(int index)
	{
		button0.SetActive(false);
		button1.SetActive(false);
		speedometerText.transform.gameObject.SetActive(true);

		GameManager.instance.SetModel(index);

	}

	public void UpdateSpeedometer(float speed)
	{
		speedometerText.text = Mathf.Abs(Mathf.Round(speed)) + " Km/H";
	}
}
