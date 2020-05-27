using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    private const int WORLD_SIZE = 100;

    public WorldMap worldMap = new WorldMap(WORLD_SIZE);

    public Transform cellPrefab;

    public Neighbourhood neighbourhood = new VonNeumannNeighbourhood();
    
    void Start()
    {
        for (var x = 0; x < WORLD_SIZE; x++)
        {
            for (var z = 0; z < WORLD_SIZE; z++)
            {
                worldMap.SetCell(InstantiateCell(x, z), x, z);
            }
        }
    }

    float elapsed;

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed %= 1f;
            RefreshWorld();
        }
    }

    private Cell InstantiateCell(int x, int z)
    {
        var randomState = (Cell.State)Random.Range(0, 2);
        
        var cellObject = Instantiate(cellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
        cellObject.gameObject.SetActive(Cell.State.ALIVE.Equals(randomState));
        
        return new Cell(new Coords(x, z), randomState, cellObject);
    }

    private void RefreshWorld()
    {
        foreach (var cell in worldMap.GetCells())
        {
            var numberOfNeighbours = neighbourhood.neighbours(cell, worldMap)
                .FindAll(n => n.state == Cell.State.ALIVE)
                .Count;
            if (cell.state == Cell.State.ALIVE)
            {
                if (numberOfNeighbours < 2 || numberOfNeighbours > 3)
                {
                    cell.makeDead();
                }
            }
            else if (cell.state == Cell.State.DEAD)
            {
                if (numberOfNeighbours == 3)
                {
                    cell.makeAlive();
                }
            }
        }

        foreach (var cell in worldMap.GetCells())
        {
            cell.proceedToNextState();
        }
    }
    
}
