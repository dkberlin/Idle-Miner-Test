using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorWorker : WorkerBase
{
    [SerializeField]
    private ContainerBase groundFloorContainer;
    [SerializeField]
    private List<MineContainer> loadingPositions;
    private SpriteRenderer spriteR;

    public bool shouldBeMoving;
    public int timesUpdated { get; protected set; }
    public int maxSpeedUpgrades { get; protected set; }

    private int index = 1;

    private void Start()
    {
        loadingPosition = loadingPositions[index];
        spriteR = GetComponent<SpriteRenderer>();
        groundFloorContainer.SetContainerCapacityText();
        SetElevatorWOrkerCapacityText();
        timesUpdated = 0;
        maxSpeedUpgrades = 4;
    }

    private void Update()
    {
        if (!shouldBeMoving)
        {
            return;
        }

        if (active)
        {
            transform.position = Vector2.MoveTowards(transform.position, loadingPositions[index].transform.position,
                walkingSpeed * Time.deltaTime);
        }

        if (active && index != 0 &&
            Vector2.Distance(transform.position, loadingPosition.transform.position) < 0.01f)
        {
            OnArrivedAtLoadingPosition();
        }

        if (active &&
            Vector2.Distance(transform.position, unloadingPosition.transform.position) < 0.01f)
        {
            OnArrivedAtUnloadingPosition();
        }
    }

    private void OnMouseDown()
    {
        if (!active)
        {
            OnWorkerClicked();
        }
    }

    public override void OnArrivedAtLoadingPosition()
    {
        spriteR.sprite = busyIcon;
        StartCoroutine(LoadCapacity());
    }

    public override void OnArrivedAtUnloadingPosition()
    {
        if (!hasManager)
        {
            shouldBeMoving = false;
        }

        if (currentLoad == 0)
        {
            index = 1;
            if (!hasManager)
            {
                active = false;
            }
        }
        else if (active && currentLoad > 0)
        {
            spriteR.sprite = busyIcon;
        }

        StartCoroutine(UnloadCapacity());
    }

    private IEnumerator LoadCapacity()
    {
        yield return new WaitForSeconds(timeToLoad);

        var spaceLeftInElevator = capacity - currentLoad;

        if (spaceLeftInElevator == 0)
        {
            Debug.LogWarning("Elevator cant load.");
            isFullyLoaded = true;
        }

        else if (spaceLeftInElevator >= loadingPositions[index].CurrentCapacity)
        {
            currentLoad += loadingPositions[index].CurrentCapacity;
            loadingPositions[index].SetNewContainerCapacity(0);
            loadingPositions[index].SetContainerCapacityText();
            SetElevatorWOrkerCapacityText();
        }

        else if (spaceLeftInElevator < loadingPositions[index].CurrentCapacity)
        {
            int loadingPosCap = loadingPositions[index].CurrentCapacity;
            loadingPositions[index].SetNewContainerCapacity(loadingPosCap - spaceLeftInElevator);
            loadingPositions[index].SetContainerCapacityText();
            currentLoad = capacity;
            isFullyLoaded = true;
            SetElevatorWOrkerCapacityText();
        }

        if (index + 1 < loadingPositions.Count)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        loadingPosition = loadingPositions[index];
        spriteR.sprite = workerIcon;

        StopAllCoroutines();
    }

    private IEnumerator UnloadCapacity()
    {
        yield return new WaitForSeconds(timeToUnload);
        var spaceLeftInGroundFloorContainer = groundFloorContainer.maxCapacity - groundFloorContainer.CurrentCapacity;

        if (currentLoad <= spaceLeftInGroundFloorContainer)
        {
            int loadingPosCap = loadingPositions[index].CurrentCapacity;
            groundFloorContainer.SetNewContainerCapacity(loadingPosCap += currentLoad);
            currentLoad = 0;
            groundFloorContainer.SetContainerCapacityText();
            SetElevatorWOrkerCapacityText();
            isFullyLoaded = false;
        }

        if (currentLoad > spaceLeftInGroundFloorContainer)
        {
            currentLoad -= spaceLeftInGroundFloorContainer;
            groundFloorContainer.SetNewContainerCapacity(groundFloorContainer.maxCapacity);
            groundFloorContainer.SetContainerCapacityText();
            SetElevatorWOrkerCapacityText();
            isFullyLoaded = false;
        }

        if (spaceLeftInGroundFloorContainer == 0)
        {
            Debug.LogWarning("Elevator Worker cant unload");
        }

        if (!hasManager)
        {
            active = false;
        }

        spriteR.sprite = workerIcon;
        index = 1;
        loadingPosition = loadingPositions[index];
        StopAllCoroutines();
    }

    public override void OnWorkerClicked()
    {
        foreach (var loadingPos in loadingPositions)
        {
            if (loadingPos.CurrentCapacity <= 0 || loadingPositions[0] == loadingPos)
            {
                continue;
            }
            active = true;
            shouldBeMoving = true;
            loadingPosition = loadingPositions[index];
            break;
        }
    }

    public void SetElevatorWOrkerCapacityText()
    {
        transform.GetComponentInChildren<TextMesh>().text = currentLoad + "/" + capacity;
    }

    public void AddNewLoadingPositions(MineContainer newShaftElevatorShaft)
    {
        loadingPositions.Add(newShaftElevatorShaft);
    }

    public void UpgradeAdded()
    {
        timesUpdated++;
    }
}