using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompassController : Item
{
    [SerializeField] Transform target;

    bool isSelected = false;

    GameObject compass;
    Transform needle;
    Animator anim;

    private void Awake()
    {
        compass = Camera.main.transform.Find("Compass").gameObject;
        anim = compass.GetComponent<Animator>();
        needle = compass.transform.Find("Pivot");
        compass.SetActive(false);
    }

    private void Start()
    {
        var elevator = FindObjectOfType<ElevatorButton>();
        if (elevator != null) 
            elevator.EnterDungeon += () =>
            {
                var exit = FindObjectOfType<ElevatorExit>();
                if (exit != null) target = exit.transform;
            };
        var exit = FindObjectOfType<ElevatorExit>();
        if (target == null && exit != null) target = exit.transform;
    }

    private void Update()
    {
        if (!isSelected || target == null) return;

        var dir = target.position - transform.position;
        dir.y = 0;
        var angle = Vector3.Angle(Vector3.forward, dir);
        var cross = Vector3.Cross(Vector3.forward, dir);
        if (cross.y < 0) angle = 360 - angle;
        needle.localRotation = Quaternion.Euler(0, angle - Camera.main.transform.eulerAngles.y, 0);
    }

    public override void Select()
    {
        var exit = FindObjectOfType<ElevatorExit>();
        if (exit != null) target = exit.transform;
        StopAllCoroutines();
        compass.SetActive(true);
        anim.SetTrigger("Toggle");
        isSelected = true;
    }

    public override void Deselect()
    {
        anim.SetTrigger("Toggle");
        isSelected = false;
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1f);
            compass.SetActive(false);
        }
    }
}
