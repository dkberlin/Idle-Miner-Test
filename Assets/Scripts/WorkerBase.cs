using UnityEngine;

public abstract class WorkerBase : MonoBehaviour
{
    [SerializeField] protected Sprite workerIcon;
    [SerializeField] protected Sprite busyIcon;
    [SerializeField] protected ContainerBase loadingPosition;
    [SerializeField] protected ContainerBase unloadingPosition;

    public bool active;
    public bool hasManager = false;

    [SerializeField] protected int capacity;
    [SerializeField] protected float timeToLoad;
    [SerializeField] protected float timeToUnload;
    [SerializeField] protected float walkingSpeed;

    protected int currentLoad;
    protected bool isFullyLoaded = false;

    public abstract void OnWorkerClicked();
    public abstract void OnArrivedAtLoadingPosition();
    public abstract void OnArrivedAtUnloadingPosition();

    #region GETTER/SETTER
    public int GetCapacity()
    {
        return capacity;
    }

    public float GetTimeToLoad()
    {
        return timeToLoad;
    }

    public float GetWalkingSpeed()
    {
        return walkingSpeed;
    }

    public float GetTimeToUnload()
    {
        return timeToUnload;
    }

    public void SetCap(int cap)
    {
        capacity = cap;
    }

    public void SetTimeToLoad(float time)
    {
        timeToLoad = time;
    }

    public void SetTimeToUnload(float time)
    {
        timeToUnload = time;
    }

    public void SetWalkingSpeed(float speed)
    {
        walkingSpeed = speed;
    }
    #endregion
}