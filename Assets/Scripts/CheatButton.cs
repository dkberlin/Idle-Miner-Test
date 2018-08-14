using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatButton : MonoBehaviour
{
    [SerializeField]
    private int moneyToCheat;

    private void OnMouseDown()
    {
        GameCore.Instance.Data.EarnedMoney += moneyToCheat;
    }
}
