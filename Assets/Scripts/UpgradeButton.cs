using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private GameObject workingArea;

    public SpriteRenderer spriteR { get; private set; }
    public Sprite upgradeAvailable { get; private set; }
    public Sprite upgradeUnavailable { get; private set; }
    public bool upgradeCanBeBought;
    public int upgradeCost { get; private set; }

    public event Action OnUpgraded;

    private void Awake()
    {
        spriteR = GetComponent<SpriteRenderer>();
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