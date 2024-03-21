using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionRoom : MonoBehaviour
{
    public GameObject left, right, back, front;
    public GameObject top, bottom;

    public GameObject frontAboveDoor, backAboveDoor;

    const int doorHeight = 23;
    const int roomHeight = 30;
    const int doorWidth = 12;


    public void Scale(Vector2 scale, Vector2 diff)
    {
        bool leftToRight = diff.x > 0;



        Vector3 sideScale = new Vector3(1, roomHeight, scale.y);
        left.transform.localScale = sideScale;
        right.transform.localScale = sideScale;
        left.transform.localPosition = new Vector3(-scale.x / 2 - 1, 0, 0);
        right.transform.localPosition = new Vector3(scale.x / 2, 0, 0);

        Vector3 topBotScale = new Vector3(scale.x / 10, 1, scale.y / 10);
        top.transform.localScale = topBotScale;
        bottom.transform.localScale = topBotScale;
        top.transform.localPosition = new Vector3(0, roomHeight, scale.y / 2);
        bottom.transform.localPosition = new Vector3(0, 0, scale.y / 2);

        Vector3 forwardScale = new Vector3(1, doorHeight, scale.x);
        front.transform.localPosition = new Vector3(-scale.x / 2, 0, scale.y);
        back.transform.localPosition = new Vector3(-scale.x / 2, 0, 1);
        front.transform.localScale = forwardScale;
        back.transform.localScale = forwardScale;

        frontAboveDoor.transform.localPosition = new Vector3(-scale.x / 2, doorHeight, scale.y);
        backAboveDoor.transform.localPosition = new Vector3(-scale.x / 2, doorHeight, 1);

        Vector3 aboveDoorScale = new Vector3(1, roomHeight - doorHeight, scale.x);
        frontAboveDoor.transform.localScale = aboveDoorScale;
        backAboveDoor.transform.localScale = aboveDoorScale;

        if (leftToRight)
        {
            var doorWitdthVector = new Vector3(0, 0, -doorWidth);
            back.transform.localPosition += new Vector3(doorWidth, 0, 0);
            back.transform.localScale += doorWitdthVector;
            front.transform.localScale += doorWitdthVector;
        }
        else
        {
            var doorWitdthVector = new Vector3(0, 0, -doorWidth);
            front.transform.localPosition += new Vector3(doorWidth, 0, 0);
            front.transform.localScale += doorWitdthVector;
            back.transform.localScale += doorWitdthVector;
        }

        if(Mathf.Approximately(scale.x, doorWidth))
        {
            front.SetActive(false);
            back.SetActive(false);
        }
    }

    public void ApplyMaterials(Material wallMaterial, Material floorMaterial, Material ceilingMaterial)
    {
        foreach (GameObject wall in new GameObject[] {left, right, front, back, frontAboveDoor, backAboveDoor})
        {
            wall.GetComponent<MeshRenderer>().material = wallMaterial;
        }
        bottom.GetComponent<MeshRenderer>().material = floorMaterial;
        top.GetComponent<MeshRenderer>().material = ceilingMaterial;
    }
}
