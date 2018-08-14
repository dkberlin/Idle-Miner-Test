using System.Collections;
using UnityEngine;

public class Miner : WorkerBase
{
    [SerializeField] private MineContainer shaftContainer;
    public int timesUpdated = 0;

    private MineShaft minerMineShaft;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        minerMineShaft = GetComponentInParent<MineShaft>();
        loadingPosition = minerMineShaft.endOfMine;
        unloadingPosition = minerMineShaft.container;
        shaftContainer = minerMineShaft.elevatorShaft;
        capacity = Mathf.RoundToInt(GameCore.Instance.GetAmountOfMineshafts() * GameCore.Instance.Data.NewShaftValueMultiplier) + 5;
    }

    private void OnMouseDown()
    {
        OnWorkerClicked();
    }

    public override void OnArrivedAtLoadingPosition()
    {
        spriteRenderer.sprite = busyIcon;
        StartCoroutine(LoadCapacity());
    }

    public override void OnArrivedAtUnloadingPosition()
    {
        spriteRenderer.sprite = busyIcon;
        StartCoroutine(UnloadCapacity());
    }

    private IEnumerator LoadCapacity()
    {
        yield return new WaitForSeconds(timeToLoad);
        currentLoad = capacity;
        Debug.LogWarning("worker has currently loaded " + currentLoad);
        isFullyLoaded = true;
        spriteRenderer.sprite = workerIcon;
        StopAllCoroutines();
    }

    private IEnumerator UnloadCapacity()
    {
        yield return new WaitForSeconds(timeToUnload);

        var spaceInContainer = shaftContainer.maxCapacity - shaftContainer.CurrentCapacity;

        if (spaceInContainer == 0)
        {
            Debug.LogWarning("Miner cant unload.");
        }

        if (spaceInContainer >= currentLoad)
        {
            int containerCap = shaftContainer.CurrentCapacity;
            shaftContainer.SetNewContainerCapacity(containerCap += currentLoad);
            currentLoad = 0;
            shaftContainer.SetContainerCapacityText();
            isFullyLoaded = false;
        }
        else
        {
            var loadToStore = currentLoad - spaceInContainer;
            currentLoad -= loadToStore;
            shaftContainer.SetNewContainerCapacity(shaftContainer.maxCapacity);
            shaftContainer.SetContainerCapacityText();
            shaftContainer.isFullyLoaded = true;
            isFullyLoaded = false;
        }

        spriteRenderer.sprite = workerIcon;
        if (!hasManager)
        {
            active = false;
        }

        StopAllCoroutines();
    }

    public override void OnWorkerClicked()
    {
        if (!active || !hasManager)
        {
            active = true;
        }
    }

    private void Update()
    {
        if (active)
        {
            if (!isFullyLoaded)
            {
                transform.position = Vector2.MoveTowards(transform.position, loadingPosition.transform.position,
                    walkingSpeed * Time.deltaTime);
            }
            else if (isFullyLoaded)
            {
                transform.position = Vector2.MoveTowards(transform.position, unloadingPosition.transform.position,
                    walkingSpeed * Time.deltaTime);
            }
        }

        if (Vector2.Distance(transform.position, loadingPosition.transform.position) < 0.1f && !isFullyLoaded)
        {
            OnArrivedAtLoadingPosition();
        }

        if (Vector2.Distance(transform.position, unloadingPosition.transform.position) < 0.1f && isFullyLoaded)
        {
            OnArrivedAtUnloadingPosition();
        }
    }
}