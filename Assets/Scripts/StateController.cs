using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public abstract class StateController<State> : MonoBehaviour, IStateProcessController<State>
{

    public List<SerializableTuple<State, StateProcess<State>>> stateProcess;
    public List<SerializableTuple<State, StateInterrupt>> stateInterrupts;

    public State start;

    [SerializeField] protected State currentState;

    protected Dictionary<State, StateProcess<State>> stateProcessMap;
    protected Dictionary<State, StateInterrupt> stateInterruptsMap;
    protected Dictionary<State, MonoBehaviour> stateMap;


    private void Start()
    {
        stateMap = stateProcess
            .Select(pair => new { state = pair.Key, script = (MonoBehaviour)pair.Value })
            .Concat(stateInterrupts.Select(pair => new { state = pair.Key, script = (MonoBehaviour)pair.Value}))
            .ToDictionary(pair => pair.state, pair => pair.script);

        stateProcessMap = stateProcess.ToDictionary(s => s.Key, s => s.Value);
        stateInterruptsMap = stateInterrupts.ToDictionary(s => s.Key, s => s.Value);
        stateProcessMap.ToList().ForEach(pair => {
            var stateProcess = pair.Value;
            stateProcess.enabled = false;
            stateProcess.StateController = this;
        });



        currentState = start;
        stateProcessMap[start].enabled = true;
    }

    public void SwitchState(State state) 
    {
        stateMap[currentState].enabled = false;
        currentState = state;
        stateMap[state].enabled = true;
    }

    public void InterruptWith(State state)
    {
        State formerState = currentState;
        stateInterruptsMap[state].Done = () => SwitchState(formerState);

        SwitchState(state);
    }
}

[Serializable]
public class SerializableTuple<K,V>
{
    [SerializeField]
    public K Key;
    [SerializeField]
    public V Value;
}
