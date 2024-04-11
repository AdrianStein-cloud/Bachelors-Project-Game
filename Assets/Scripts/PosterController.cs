using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterController : MonoBehaviour
{
    [SerializeField] string posterName;

    bool isPosterSet;
    bool isWandererFast;
    Transform player;

    private void Awake()
    {
        isPosterSet = PlayerPrefs.GetInt(posterName) == 1;
        player = FindObjectOfType<PlayerMovement>().transform;
        isWandererFast = name.Contains("Fast");
    }

    private void FixedUpdate()
    {
        if (!isPosterSet && isWandererFast && Vector3.Distance(player.position, transform.position) <= 100f)
        {
            SetPoster();
        }
    }

    public void SetPoster()
    {
        if (isPosterSet) return;
        PlayerPrefs.SetInt(posterName, 1);
        isPosterSet = true;
    }
}
