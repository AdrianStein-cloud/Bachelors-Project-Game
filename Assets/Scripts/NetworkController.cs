using Unity.Netcode;
using UnityEngine;

public class NetworkController : MonoBehaviour
{
    public void BecomeHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void BecomeClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
