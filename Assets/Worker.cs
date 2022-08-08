using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Worker : StaffBase<WorkerModelType>
{
    House myHouse;
    protected StateMachine<StaffBase<WorkerModelType>> m_Statemachine;
    public StateMachine<StaffBase<WorkerModelType>> stateMachine { get { return m_Statemachine; } }
    private void Awake()
    {
        m_Statemachine = new StateMachine<StaffBase<WorkerModelType>>(this);
        m_Statemachine.SetcurrentState(StaffIdle<WorkerModelType>.instance);
        m_Statemachine.SetcurrentState(StaffIdle<WorkerModelType>.instance);
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        stateMachine.Update();
        switch (state)
        {
            case StaffState.Idle:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffIdle<WorkerModelType>.instance);
                }
                break;
            case StaffState.Move:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffMove<WorkerModelType>.instance);
                }
                break;
            case StaffState.Rotage:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffRotage<WorkerModelType>.instance);
                }
                break;
            case StaffState.Work:
                if (currentState != state)
                {
                    currentState = state;
                    stateMachine.ChangeState(StaffWork<WorkerModelType>.instance);
                }
                break;
            default:
                break;
        }
    }
    public void ChangeHouse(House house) {
        myHouse = house;
    }
    public House GetHouse() {
        return myHouse;
    }
    
}

