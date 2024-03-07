using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateProcessController<T>
{
    void SwitchState(T state);
    void InterruptWith(T state);
}
