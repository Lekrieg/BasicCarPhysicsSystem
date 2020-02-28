using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private GameObject carSeat;
    [SerializeField]
    private GameObject[] playerModels;

    private void Awake()
    {
        instance = this;

        playerModels = Resources.LoadAll<GameObject>("Prefabs/Player/");
    }

    public void SetModel(int modelIndex)
    {
        InstantiateCar();

        carSeat = GameObject.Find("Seat");

        GameObject playerPrefabModel = Instantiate(playerModels[modelIndex], carSeat.transform);

        playerPrefabModel.transform.parent = carSeat.transform;
    }
    private void InstantiateCar()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/BasicCar"));
    }
}
