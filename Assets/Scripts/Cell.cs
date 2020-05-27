using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Scripts;
using UnityEngine;

public class Cell 
{
    public enum State {DEAD, ALIVE}
    
    public Cell( Coords coords, State state, Transform cellObject)
    {
        this.coords = coords;
        this.state = state;
        this.cellObject = cellObject;
        
        nextState = state;
    }

    public Cell(Coords coords, Transform cellObject)
    {
        this.coords = coords;
        this.cellObject = cellObject;
    }

    public State state { get; set; }
    
    public State nextState { get; set; }
    
    public Coords coords{
        get;
        set;
    }
    public Transform cellObject { get; set; }

    public void makeDead()
    {
        nextState = State.DEAD;
    }

    public void makeAlive()
    {
        nextState = State.ALIVE;
    }

    public void proceedToNextState()
    {
        state = nextState;
        switch (state)
        {
            case State.ALIVE:
                cellObject.gameObject.SetActive(true);
                break;
            case State.DEAD:
                cellObject.gameObject.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}
