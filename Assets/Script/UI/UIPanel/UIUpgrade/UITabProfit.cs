using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabProfit : MonoBehaviour
{
    [SerializeField] Text moneyEarnCurrentText;
    [SerializeField] Text moneyEarnNextText;
    [SerializeField] Text energyCurrentText;
    [SerializeField] Text energyNextText;
    public void InitData(IRoomControler roomControler, int itemID)
    {
        moneyEarnCurrentText.text = roomControler.GetCurrentMoneyEarn(itemID).ToString();
        moneyEarnNextText.text = roomControler.GetCurrentMoneyEarn(itemID).ToString();
        energyCurrentText.text = roomControler.GetCurrentMoneyEarn(itemID).ToString();
        energyNextText.text = roomControler.GetCurrentMoneyEarn(itemID).ToString();
    }
}
