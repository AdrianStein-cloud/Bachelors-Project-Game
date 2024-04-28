using UnityEngine;
using Unity.Netcode;

public abstract class NetworkInteractable : Interactable
{
    [ServerRpc(RequireOwnership = false)]
    protected void InteractServerRpc() => InteractClientRpc();

    [ClientRpc]
    private void InteractClientRpc() => Interact();

    protected abstract void Interact();
}
