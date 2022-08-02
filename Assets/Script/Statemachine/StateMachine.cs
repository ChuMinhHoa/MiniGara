using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private T m_Owner;
    private State<T> currentState;
    public StateMachine( T owner){
        m_Owner = owner;
    }
    public void SetcurrentState(State<T> s) { currentState = s; }
    public void ChangeState(State<T> s) {
        if (currentState != null)
            currentState.End(m_Owner);
        currentState = s;
        currentState.Enter(m_Owner);
    }
    public void Update() {
        currentState.Execute(m_Owner);
    }
}
