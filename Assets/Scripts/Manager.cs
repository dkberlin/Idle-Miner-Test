using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool activated = false;

    public Sprite activeSprite;

    public Sprite inactiveSprite;

    public Sprite managerAvailable;

    public bool managerBought;
    public bool managerCanBeBought;

    public int managerBonusTime;

    public int managerCost;

    public Sprite managerUnavailable;

    public SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject workingPlace;

    public event Action OnManagerBought;
    public event Action OnManagerActivated;

    public bool isCoolingDown = false;

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