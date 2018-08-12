using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool activated = false;

    [SerializeField] private Sprite activeSprite;

    [SerializeField] private Sprite inactiveSprite;

    public Sprite managerAvailable;

    public bool managerBought;
    public bool managerCanBeBought;

    public int managerCooldown;

    public int managerCost;

    public Sprite managerUnavailable;

    public SpriteRenderer spriteRenderer;

    [SerializeField] private GameObject workingPlace;

    public float ManagerBonus { get; set; }

    public event Action OnManagerBought;
    public event Action OnManagerActivated;

    private void Start()
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
            spriteRenderer.sprite = inactiveSprite;
            GameCore.Instance.CheckIfManagerAvailable();
        }
    }
}