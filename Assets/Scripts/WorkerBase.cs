using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WorkerBase : MonoBehaviour
{
    public float walkingSpeed;

    public bool hasManager = false;

    public int capacity;

    [SerializeField]
    protected ContainerBase loadingPosition;

    [SerializeField]
    protected ContainerBase unloadingPosition;

    public float timeToLoad;

    public float timeToUnload;

    [SerializeField]
    protected Sprite workerIcon;

    [SerializeField]
    protected Sprite busyIcon;

    protected int currentLoad;

    public bool active;
    protected bool isFullyLoaded = false;
    public abstract void OnWorkerClicked();
    public abstract void OnArrivedAtLoadingPosition();
    public abstract void OnArrivedAtUnloadingPosition();
}
