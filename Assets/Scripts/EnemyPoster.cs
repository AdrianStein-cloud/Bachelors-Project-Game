using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoster : MonoBehaviour
{
    [SerializeField] string posterName;

    private void Start()
    {
        SetPoster();
        FindObjectOfType<ElevatorButton>().LeaveDungeon += SetPoster;
    }

    void SetPoster()
    {
        gameObject.SetActive(PlayerPrefs.GetInt(posterName) == 1);
    }
}
