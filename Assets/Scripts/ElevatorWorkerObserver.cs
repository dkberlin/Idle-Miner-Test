//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ElevatorWorkerObserver : MonoBehaviour
//{
//    public ElevatorWorker elevatorGuy;

//    private void Start()
//    {
//        elevatorGuy.OnArrivedAtLoadingPoint += HandleArrivalAtLoadingPoint;
//        elevatorGuy.OnArrivedAtUnloadingPoint += HandleArrivalAtUnloadingPoint;
//    }

//    private void HandleArrivalAtUnloadingPoint()
//    {
//        if (!elevatorGuy.active)
//        {
//            return;
//        }
//        else
//        {
//            elevatorGuy.currentIndex = 1;
//        }
//    }

//    private void HandleArrivalAtLoadingPoint()
//    {
//        ContainerBase currentLoadingPosition = elevatorGuy.currentLoadingPosition;
//        int spaceLeftInElevator = elevatorGuy.spaceLeftInElevator;

//        if (currentLoadingPosition.currentCapacity == 0)
//        {
//            SetNextGoalForElevator(elevatorGuy.currentIndex);
//            return;
//        }

//        if (spaceLeftInElevator >= currentLoadingPosition.currentCapacity)
//        {
//            elevatorGuy.currentLoadInElevator = currentLoadingPosition.currentCapacity;
//            return;
//        }

//        if (spaceLeftInElevator < currentLoadingPosition.currentCapacity)
//        {
//            currentLoadingPosition.currentCapacity -= spaceLeftInElevator;
//            elevatorGuy.currentLoadInElevator = elevatorGuy.capacity;
//            return;
//        }

//        if (spaceLeftInElevator == 0)
//        {
//            elevatorGuy.currentIndex = 0;
//        }
//    }

//    private void SetNextGoalForElevator(int currentIndex)
//    {
//        if (currentIndex +1 < elevatorGuy.amountOfFloors)
//        {
//            int newIndex = currentIndex++;
//            elevatorGuy.currentIndex = newIndex;
//        }
//        else
//        {
//            elevatorGuy.currentIndex = 0;
//        }
//    }
//}
