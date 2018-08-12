using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public SpriteRenderer spriteR;

    public Sprite upgradeAvailable;

    public bool upgradeCanBeBought;

    public int upgradeCost;

    public Sprite upgradeUnavailable;

    [SerializeField] private GameObject workingArea;

    public event Action OnUpgraded;

    private void Start()
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