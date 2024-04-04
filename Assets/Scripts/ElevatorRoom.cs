using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ElevatorRoom : MonoBehaviour
{
    [SerializeField] Animator entranceAnim;
    [SerializeField] Animator exitAnim;
    [SerializeField] float elevatorTime;

    public bool Moving { get; private set; }

    float startPositionY;

    private void Start()
    {
        startPositionY = transform.position.y;
    }

    public void ToggleEntranceElevator(bool open)
    {
        entranceAnim.SetTrigger(open ? "Open" : "Close");
        FindObjectOfType<ElevatorEntrance>().ToggleElevator(open);
    }

    public void ToggleExitElevator(bool open)
    {
        exitAnim.SetTrigger(open ? "Open" : "Close");
        FindObjectOfType<ElevatorExit>().ToggleElevator(open);
    }

    public void Enter()
    {
        Moving = true;
        StartCoroutine(Go(0f));
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(elevatorTime);
            ToggleExitElevator(true);
            Moving = false;

            var navmeshSurface = FindFirstObjectByType<NavMeshSurface>();
            navmeshSurface.BuildNavMesh();
        }
    }

    public void Exit()
    {
        Moving = true;
        StartCoroutine(Go(startPositionY));
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            yield return new WaitForSeconds(elevatorTime);
            ToggleEntranceElevator(true);
            Moving = false;
        }
    }

    IEnumerator Go(float posY)
    {
        float elapsedTime = 0;
        var startValue = transform.position.y;
        while (elapsedTime < elevatorTime)
        {
            transform.position = transform.position.WithY(Mathf.Lerp(startValue, posY, elapsedTime / elevatorTime));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.position = transform.position.WithY(posY);
    }
}
