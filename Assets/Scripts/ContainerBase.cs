using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContainerBase : MonoBehaviour
{
    public int currentCapacity;
    public int maxCapacity;
    public bool isFullyLoaded = false;

    public void SetContainerCapacityText()
    {
        transform.GetComponentInChildren<TextMesh>().text = (currentCapacity + "/" + maxCapacity);
    }
}
