using System;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public bool activated = false;

    [SerializeField] private Sprite activeSprite;

    [SerializeField] private Sprite inactiveSprite;

    [SerializeField] private Sprite managerAvailable;

    public bool managerBought;
    private bool managerCanBeBought;

    public int managerCooldown;

    public int managerCost;

    [SerializeField] private Sprite managerUnavailable;

    private SpriteRenderer spriteRenderer;

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
        }
    }

    public void CheckIfManagerAvailable(int earnedMoney)
    {
        if (managerCost <= earnedMoney)
        {
            managerCanBeBought = true;
            spriteRenderer.sprite = managerAvailable;
        }
        else
        {
            managerCanBeBought = false;
            spriteRenderer.sprite = managerUnavailable;
        }
    }
}