using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : Item
{
    [SerializeField] GameObject throwablePrefab;
    [SerializeField] float throwForce;
    [SerializeField] float offsetAngle;
    [SerializeField, Range(0, 90)] float minAngle;
    [SerializeField, Range(0, 90)] float maxAngle;
    [SerializeField] float minTorque;
    [SerializeField] float maxTorque;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] int maxAmount;

    Transform player;
    int currentAmount;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentAmount = maxAmount;
    }

    void Throw()
    {
        if (currentAmount <= 0) return;
        currentAmount--;

        var offset = new Vector3(0f, offsetPosition.y) + player.forward * offsetPosition.z + player.right * offsetPosition.x;
        var instance = Instantiate(throwablePrefab, player.position + offset, Quaternion.identity);
        var rb = instance.GetComponent<Rigidbody>();

        var angle = Vector3.Angle(player.up, Camera.main.transform.forward) - 90;
        angle = Mathf.Clamp(angle + offsetAngle, -minAngle, maxAngle);
        var direction = Quaternion.AngleAxis(angle, Camera.main.transform.right) * player.forward;

        rb.AddForce(direction * throwForce, ForceMode.Impulse);
        var x = (Random.Range(0, 2) == 0 ? 1f : -1f) * Random.Range(minTorque, maxTorque);
        var z = (Random.Range(0, 2) == 0 ? 1f : -1f) * Random.Range(minTorque, maxTorque);
        rb.AddTorque(x, 0f, z, ForceMode.Impulse);
    }

    public override void Primary()
    {
        Throw();
    }
}
