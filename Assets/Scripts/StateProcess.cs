using System;
using UnityEngine;

public class StateProcess<T> : MonoBehaviour
{
    public IStateProcessController<T> StateController { get; set; }
}
