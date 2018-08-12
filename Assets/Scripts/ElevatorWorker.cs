using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorWorker : WorkerBase
{
    public List<MineContainer> loadingPositions;
    public ContainerBase groundFloorContainer;
    //public event Action OnArrivedAtLoadingPoint;
    //public event Action OnArrivedAtUnloadingPoint;

    //public ContainerBase currentLoadingPosition
    //{
    //    get { return loadingPosition; }
    //}
    //public int spaceLeftInElevator
    //{
    //    get { return capacity - currentLoad; }
    //}
    //public int currentIndex
    //{
    //    get { return index; }
    //    set { index = value; }
    //}
    //public int amountOfFloors
    //{
    //    get { return loadingPositions.Count; }
    //}
    //public int currentLoadInElevator
    //{
    //    get { return currentLoad; }
    //    set { currentLoad = value; }
    //}
    [SerializeField]
    private Manager manager;

    private int index = 1;
    private SpriteRenderer spriteR;

    private bool shouldBeMoving;

    private void Start()
    {
        //worker.onClick.AddListener(OnWorkerClicked);
        loadingPosition = loadingPositions[index];
        spriteR = GetComponent<SpriteRenderer>();
        groundFloorContainer.SetContainerCapacityText();
        SetElevatorWOrkerCapacityText();
        manager.OnManagerBought += HandleManagerBought;
        manager.OnManagerActivated += HandleManagerActivated;
    }

    private void HandleManagerActivated()
    {
        throw new NotImplementedException();
    }

    private void HandleManagerBought()
    {
        hasManager = true;
        active = true;
    }

    private void OnMouseDown()
    {
        OnWorkerClicked();
    }

    public override void OnArrivedAtLoadingPosition()
    {
        spriteR.sprite = busyIcon;
        StartCoroutine(LoadCapacity());
    }

    public override void OnArrivedAtUnloadingPosition()
    {
        shouldBeMoving = false;
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

        loadingPosition = loadingPositions[index];
    }

    IEnumerator LoadCapacity()
    {
        yield return new WaitForSeconds(timeToLoad);

        int spaceLeftInElevator = capacity - currentLoad;

        if (spaceLeftInElevator == 0)
        {
            Debug.LogWarning("Elevator cant load.");
            isFullyLoaded = true;
        }

        else if (spaceLeftInElevator >= loadingPositions[index].currentCapacity)
        {
            currentLoad += loadingPositions[index].currentCapacity;
            loadingPositions[index].currentCapacity = 0;
            loadingPositions[index].SetContainerCapacityText();
            SetElevatorWOrkerCapacityText();
        }

        else if (spaceLeftInElevator < loadingPositions[index].currentCapacity)
        {
            loadingPositions[index].currentCapacity -= spaceLeftInElevator;
            loadingPositions[index].SetContainerCapacityText();
            currentLoad = capacity;
            isFullyLoaded = true;
            SetElevatorWOrkerCapacityText();
        }


        if (index+1 < loadingPositions.Count)
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

    IEnumerator UnloadCapacity()
    {
        yield return new WaitForSeconds(timeToUnload);
        int spaceLeftInGroundFloorContainer = groundFloorContainer.maxCapacity - groundFloorContainer.currentCapacity;

        if (currentLoad <= spaceLeftInGroundFloorContainer)
        {
            groundFloorContainer.currentCapacity += currentLoad;
            currentLoad = 0;
            groundFloorContainer.SetContainerCapacityText();
            SetElevatorWOrkerCapacityText();
            isFullyLoaded = false;
        }

        if (currentLoad > spaceLeftInGroundFloorContainer)
        {
            currentLoad -= spaceLeftInGroundFloorContainer;
            groundFloorContainer.currentCapacity = groundFloorContainer.maxCapacity;
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
        StopAllCoroutines();
    }

    public override void OnWorkerClicked()
    {
        foreach (var loadingPos in loadingPositions)
        {
            if (loadingPos.currentCapacity > 0 && loadingPositions[0] != loadingPos)
            {
                active = true;
                shouldBeMoving = true;
                loadingPosition = loadingPositions[index];
                break;
            }
        }
    }

    public void SetElevatorWOrkerCapacityText()
    {
        transform.GetComponentInChildren<TextMesh>().text = (currentLoad + "/" + capacity);
    }

    private void Update()
    {
        if (shouldBeMoving)
        {
            if (active)
            {
                transform.position = Vector2.MoveTowards(transform.position, loadingPositions[index].transform.position, walkingSpeed * Time.deltaTime);
            }

            if (active && index != 0 && Vector2.Distance(transform.position, loadingPosition.transform.position) < 0.01f)
            {
                OnArrivedAtLoadingPosition();
            }

            if (currentLoad > 0 && active && Vector2.Distance(transform.position, unloadingPosition.transform.position) < 0.01f)
            {
                OnArrivedAtUnloadingPosition();
            }
        }
    }
}
