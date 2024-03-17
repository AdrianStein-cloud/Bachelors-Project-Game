using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionRoom : MonoBehaviour
{
    public GameObject left, right, back, front;
    public GameObject top, bottom;


    public void Scale(Vector2 abs, Vector2 diff)
    {
        bool leftToRight = diff.x > 0;



        Vector3 sideScale = new Vector3(1, 23, abs.y);
        left.transform.localScale = sideScale;
        right.transform.localScale = sideScale;
        left.transform.localPosition = new Vector3(-abs.x / 2 - 1, 0, 0);
        right.transform.localPosition = new Vector3(abs.x / 2, 0, 0);

        Vector3 topBotScale = new Vector3(abs.x / 10, 1, abs.y / 10);
        top.transform.localScale = topBotScale;
        bottom.transform.localScale = topBotScale;
        top.transform.localPosition = new Vector3(0, 23, abs.y / 2);
        bottom.transform.localPosition = new Vector3(0, 0, abs.y / 2);

        Vector3 forwardScale = new Vector3(1, 23, abs.x);
        front.transform.localPosition = new Vector3(-abs.x / 2, 0, abs.y);
        back.transform.localPosition = new Vector3(-abs.x / 2, 0, 1);
        front.transform.localScale = forwardScale;
        back.transform.localScale = forwardScale;

        if (leftToRight)
        {
            var doorWitdthVector = new Vector3(0, 0, -12);
            back.transform.localPosition += new Vector3(12, 0, 0);
            back.transform.localScale += doorWitdthVector;
            front.transform.localScale += doorWitdthVector;
        }
        else
        {
            var doorWitdthVector = new Vector3(0, 0, -12);
            front.transform.localPosition += new Vector3(12, 0, 0);
            front.transform.localScale += doorWitdthVector;
            back.transform.localScale += doorWitdthVector;
        }
    }

    public void ApplyMaterials(Material wallMaterial, Material floorMaterial, Material ceilingMaterial)
    {
        foreach (GameObject wall in new GameObject[] {left, right, front, back})
        {
            wall.GetComponent<MeshRenderer>().material = wallMaterial;
        }
        bottom.GetComponent<MeshRenderer>().material = floorMaterial;
        top.GetComponent<MeshRenderer>().material = ceilingMaterial;
    }
}
