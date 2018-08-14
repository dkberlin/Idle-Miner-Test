using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Text moneyInfo;

    public void UpgradeMoneyTextField(int money)
    {
        moneyInfo.text = "Money earned: " + money;
    }

}
