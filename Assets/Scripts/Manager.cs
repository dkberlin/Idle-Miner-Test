using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private Sprite managerAvailable;
    [SerializeField] private Sprite managerUnavailable;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private GameObject workingPlace;
    public int managerCost { get; private set; }
    public int managerBonusTime { get; private set; }
    public bool activated = false;
    public bool managerBought;
    public bool managerCanBeBought;
    public bool isCoolingDown = false;

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

    public GameObject GetWorkingPlace()
    {
        return workingPlace;
    }

    public void SetManagerSpriteAvailable(bool available)
    {
        spriteRenderer.sprite = available ? managerAvailable : managerUnavailable;
    }

    public void SetManagerSpriteActive(bool active)
    {
        spriteRenderer.sprite = active ? activeSprite : inactiveSprite;
    }

    public void SetManagerCost(int cost)
    {
        managerCost = cost;
    }

    public void SetManagerBonusTime(int time)
    {
        managerBonusTime = time;
    }
}