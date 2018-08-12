using UnityEngine;

public abstract class WorkerBase : MonoBehaviour
{
    public bool active;

    [SerializeField] protected Sprite busyIcon;

    public int capacity;

    protected int currentLoad;

    public bool hasManager = false;
    protected bool isFullyLoaded = false;

    [SerializeField] protected ContainerBase loadingPosition;

    public float timeToLoad;

    public float timeToUnload;

    [SerializeField] protected ContainerBase unloadingPosition;

    public float walkingSpeed;

    [SerializeField] protected Sprite workerIcon;

    public abstract void OnWorkerClicked();
    public abstract void OnArrivedAtLoadingPosition();
    public abstract void OnArrivedAtUnloadingPosition();
}