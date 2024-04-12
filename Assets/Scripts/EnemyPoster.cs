using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoster : MonoBehaviour
{
    [SerializeField] string posterName;
    [SerializeField] bool alwaysVisible;

    private void Start()
    {
        SetPoster();
        FindObjectOfType<ElevatorButton>().LeaveDungeon += SetPoster;
    }

    void SetPoster()
    {
        if (!alwaysVisible)
        {
            GetComponent<MeshRenderer>().enabled = PlayerPrefs.GetInt(posterName) == 1;
        }
    }
}
