using System;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    private SpriteRenderer spriteR;

    [SerializeField] private Sprite upgradeAvailable;

    private bool upgradeCanBeBought;

    public int upgradeCost;

    [SerializeField] private Sprite upgradeUnavailable;

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

        GameCore.Instance.Data.EarnedMoney -= upgradeCost;
        OnUpgraded();
        CheckIfUpgradeAvailable(GameCore.Instance.Data.EarnedMoney);
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