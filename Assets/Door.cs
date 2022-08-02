using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    bool close = true;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 12 && close)
        {
            anim.Play("Open");
            close = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 12 && !close)
        {
            anim.Play("Close");
            close = true;
        }
    }
}
