using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    private GameObject workingArea;

    [SerializeField]
    Sprite upgradeAvailable;

    [SerializeField]
    Sprite upgradeUnavailable;

    private SpriteRenderer spriteR;

    public int upgradeCost;
    private bool upgradeCanBeBought = false;

    public event Action OnUpgraded;

    private void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }
    public void OnMouseDown()
    {
        if (upgradeCanBeBought && OnUpgraded != null)
        {
            GameCore.Instance.Data.earnedMoney -= upgradeCost;
            OnUpgraded();
            CheckIfUpgradeAvailable(GameCore.Instance.Data.earnedMoney);
        }
    }

    public void CheckIfUpgradeAvailable(int currentMoney)
    {
        if (upgradeCost <= currentMoney)
        {
            upgradeCanBeBought = true;
            spriteR.sprite = upgradeAvailable;
        }
        else
        {
            upgradeCanBeBought = false;
            spriteR.sprite = upgradeUnavailable;
        }
    }
}
