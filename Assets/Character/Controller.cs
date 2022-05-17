using UnityEngine;

public abstract class Chain<T> : MonoBehaviour
{
    public Chain<T> NextLink { get; set; }
    //public abstract void Commit(InputManager.InputState control);
    public virtual void Process(ref T data)
    {
        NextLink?.Process(ref data);
    }
}

public abstract class Controller : Chain<InputManager.InputState> { }