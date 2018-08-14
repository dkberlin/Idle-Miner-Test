using UnityEngine;

public abstract class ContainerBase : MonoBehaviour
{
    public int CurrentCapacity { get; protected set; }
    public int maxCapacity { get; protected set; }
    public bool isFullyLoaded = false;

    protected internal void SetContainerCapacityText()
    {
        transform.GetComponentInChildren<TextMesh>().text = (CurrentCapacity + "/" + maxCapacity);
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
