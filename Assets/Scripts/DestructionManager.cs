using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructionManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    public Transform player;
    public GameObject buildingPrefab;
    public Button button;

    public List<Building> buildings;

    public void DestroyBuilding()
    {
        if (buildings.Count == 0) return;

        button.interactable = false;
        Building whichBuilding = buildings[Random.Range(0, buildings.Count)];
        whichBuilding.DestoryMe(this);

        cam.Follow = whichBuilding.transform;
    }

    public void SpawnBuildings()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(buildingPrefab);
            go.transform.position = (Vector2)Random.insideUnitSphere * 15;
            buildings.Add(go.GetComponent<Building>());
        }
    }

    public void RemoveBuilding(Building building)
    {
        buildings.Remove(building);
        button.interactable = true;
        cam.Follow = player;
    }
}
