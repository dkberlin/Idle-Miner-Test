using UnityEngine;

public abstract class WorkerBase : MonoBehaviour
{
    [SerializeField] protected Sprite workerIcon;
    [SerializeField] protected Sprite busyIcon;
    [SerializeField] protected ContainerBase loadingPosition;
    [SerializeField] protected ContainerBase unloadingPosition;

    public bool active;
    public bool hasManager = false;
    public int Capacity { get; protected set; }
    public float TimeToLoad { get; protected set; }
    public float TimeToUnload { get; protected set; }
    public float WalkingSpeed { get; protected set; }

    protected int currentLoad;
    protected bool isFullyLoaded = false;

    public abstract void OnWorkerClicked();
    public abstract void OnArrivedAtLoadingPosition();
    public abstract void OnArrivedAtUnloadingPosition();

    public void SetCap(int cap)
    {
        Capacity = cap;
    }

    public void SetTimeToLoad(float time)
    {
        TimeToLoad = time;
    }

    public void SetTimeToUnload(float time)
    {
        TimeToUnload = time;
    }

    public void SetWalkingSpeed(float speed)
    {
        WalkingSpeed = speed;
    }
}