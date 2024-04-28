using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerManager : NetworkBehaviour
{
    private void Start()
    {
        if (!IsOwner)
        {
            GetComponentInChildren<PlayerMovement>().enabled = false;
            GetComponent<AudioSource>().enabled = false;
            transform.Find("Camera").gameObject.SetActive(false);
        }
        else
        {
            UnitySingleton<PlayerManager>.Instance.SpawnedPlayer(GetComponentInChildren<PlayerMovement>().gameObject);
        }
    }
}
