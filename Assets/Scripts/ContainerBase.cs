using UnityEngine;

public abstract class ContainerBase : MonoBehaviour
{
    [SerializeField] private int maxCapacity;
    public int CurrentCapacity { get; private set; }
    public bool isFullyLoaded = false;

    protected internal void SetContainerCapacityText()
    {
        transform.GetComponentInChildren<TextMesh>().text = (CurrentCapacity + "/" + maxCapacity);
    }

    public int GetMaxCapacity()
    {
        return maxCapacity;
    }

    protected internal void SetNewContainerCapacity(int cap)
    {
        CurrentCapacity = cap;
    }

    protected internal void SetNewMaxCapacity(int cap)
    {
        maxCapacity = cap;
    }
}
