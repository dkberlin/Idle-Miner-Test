using UnityEngine;

public abstract class WorkerBase : MonoBehaviour
{
    [SerializeField] protected Sprite workerIcon;
    [SerializeField] protected Sprite busyIcon;
    [SerializeField] protected ContainerBase loadingPosition;
    [SerializeField] protected ContainerBase unloadingPosition;

    public bool active;
    public bool hasManager = false;
    public int capacity;
    public float timeToLoad;
    public float timeToUnload;
    public float walkingSpeed;

    protected int currentLoad;
    protected bool isFullyLoaded = false;

    public abstract void OnWorkerClicked();
    public abstract void OnArrivedAtLoadingPosition();
    public abstract void OnArrivedAtUnloadingPosition();
}