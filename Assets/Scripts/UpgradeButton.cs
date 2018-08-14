using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private GameObject workingArea;
    [SerializeField] private Sprite upgradeUnavailable;
    [SerializeField] private Sprite upgradeAvailable;
    [SerializeField] private SpriteRenderer spriteR;

    public bool upgradeCanBeBought;
    public int upgradeCost { get; private set; }

    public event Action OnUpgraded;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
    }

    public void SetUpgradeAvailable(bool available)
    {
        spriteR.sprite = available ? upgradeAvailable : upgradeUnavailable;
    }

    public void OnMouseDown()
    {
        if (!upgradeCanBeBought || OnUpgraded == null)
        {
            return;
        }

        GameCore.Instance.CheckIfUpgradeAvailable();
        GameCore.Instance.Data.EarnedMoney -= upgradeCost;
        OnUpgraded();
    }

    public void SetNewUpgradeCost(int cost)
    {
        upgradeCost = cost;
    }
}