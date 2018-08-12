﻿using System;
using System.Collections;
using UnityEngine;

public class OverdaysWorker : WorkerBase
{
    [SerializeField] private ContainerBase loadingContainer;

    [SerializeField] private Manager manager;

    private SpriteRenderer spriteR;

    public event Action<int> OnMoneyEarned;

    public int timesUpdated = 0;

    private ContainerBase[] allContainers;

    private void Start()
    {
        spriteR = GetComponent<SpriteRenderer>();
        allContainers = transform.parent.GetComponentsInChildren<ContainerBase>();

        foreach (var t in allContainers)
        {
            if (t.tag == "shaftContainerElement")
            {
                loadingPosition = t;
            }
            else
            {
                unloadingPosition = t;
            }
        }

        loadingContainer = loadingPosition;
        manager = transform.parent.GetComponentInChildren<Manager>();
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
        spriteR.sprite = busyIcon;
        StartCoroutine(UnloadCapacity());
    }

    private IEnumerator LoadCapacity()
    {
        yield return new WaitForSeconds(timeToLoad);

        var spaceLeft = capacity - currentLoad;

        if (spaceLeft >= loadingContainer.currentCapacity)
        {
            currentLoad = loadingContainer.currentCapacity;
            loadingContainer.currentCapacity = 0;
            loadingContainer.SetContainerCapacityText();
        }

        if (spaceLeft < loadingContainer.currentCapacity)
        {
            currentLoad = capacity;
            loadingContainer.currentCapacity -= spaceLeft;
            loadingContainer.SetContainerCapacityText();
        }

        if (currentLoad == capacity)
        {
            Debug.LogWarning("Overdays Worker cant load.");
        }

        Debug.LogWarning("Overdays worker has currently loaded " + currentLoad);
        isFullyLoaded = true;
        spriteR.sprite = workerIcon;

        StopAllCoroutines();
    }

    private IEnumerator UnloadCapacity()
    {
        yield return new WaitForSeconds(timeToUnload);
        if (OnMoneyEarned != null)
        {
            OnMoneyEarned(currentLoad);
            currentLoad = 0;
        }

        isFullyLoaded = false;

        if (!hasManager)
        {
            active = false;
        }

        spriteR.sprite = workerIcon;

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