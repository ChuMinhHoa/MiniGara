using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Worker : StaffBase
{
    House myHouse;
    public void ChangeHouse(House house) {
        myHouse = house;
    }
    public House GetHouse() {
        return myHouse;
    }
    
}

