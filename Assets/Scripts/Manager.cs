using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject workingPlace;
    public SpriteRenderer spriteRenderer;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public Sprite managerAvailable;
    public Sprite managerUnavailable;
    public bool activated = false;
    public bool managerBought;
    public bool managerCanBeBought;
    public bool isCoolingDown = false;
    public int managerBonusTime;
    public int managerCost;

    public event Action OnManagerBought;
    public event Action OnManagerActivated;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (managerBought && OnManagerActivated != null)
        {
            OnManagerActivated();
        }

        else if (!managerBought && managerCanBeBought && OnManagerBought != null)
        {
            GameCore.Instance.Data.EarnedMoney -= managerCost;
            managerBought = true;
            OnManagerBought();
            spriteRenderer.sprite = activeSprite;
            GameCore.Instance.CheckIfManagerAvailable();
        }
    }
}