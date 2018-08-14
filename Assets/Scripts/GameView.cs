using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Text moneyInfo { get; protected set; }

    public void UpgradeMoneyTextField(int money)
    {
        moneyInfo.text = "Money earned: " + money;
    }

}
