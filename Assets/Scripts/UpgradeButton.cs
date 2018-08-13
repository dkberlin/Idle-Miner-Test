using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private GameObject workingArea;

    public SpriteRenderer spriteR;
    public Sprite upgradeAvailable;
    public Sprite upgradeUnavailable;
    public bool upgradeCanBeBought;
    public int upgradeCost;

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
}