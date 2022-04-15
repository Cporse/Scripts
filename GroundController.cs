using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public static GroundController Instance;

    private Color firstColor;
    private GameObject[] door;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        door = GameObject.FindGameObjectsWithTag("barrier");
        firstColor = door[0].GetComponent<Renderer>().material.color;
    }

    //Beginning Of FunctionS,
    public void DoorForGreenColor()
    {
        for (int i = 0; i < door.Length; i++)
        {
            door[i].GetComponent<Renderer>().material.color = Color.green;
        }
    }
    public void DoorForDefaultColor()
    {
        for (int i = 0; i < door.Length; i++)
        {
            door[i].GetComponent<Renderer>().material.color = firstColor;
        }
    }

    //End Of FunctionS.
}