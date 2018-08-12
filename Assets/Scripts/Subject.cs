using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject
{
    List<Observer> observers = new List<Observer>();
    private List<Action> _actions;

    public void Notify()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            observers[i].OnNotify();
        }
    }

    public void RegisterAction(Action actionToRegister)
    {
        if (_actions == null)
        {
            _actions = new List<Action>();
        }

        _actions.Add(actionToRegister);
    }

    public void DeregisterAction(Action action)
    {
        if (_actions.Contains(action))
        {
            _actions.Remove(action);
        }
    }

    private void CallRegisteredActions()
    {
        if(_actions != null)
        {
            foreach (var action in _actions)
            {
                action();
            }
        }
    }

    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    public void RemobeObserver(Observer observer)
    {
        observers.Remove(observer);
    }

}
