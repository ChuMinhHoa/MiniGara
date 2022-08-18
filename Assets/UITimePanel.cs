using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimePanel : MonoBehaviour
{
    [SerializeField] Text timeText;
    public void ChangeTimeText(string timeStr) { timeText.text = timeStr; }
}
