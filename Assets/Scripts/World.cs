using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes.Scripts;
using Scenes.Scripts.Rules;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
    private const int WORLD_SIZE = 20;
    private const float REFRESH_RATE = 0.2f;

    public WorldMap worldMap = new WorldMap(WORLD_SIZE);
    public Rules rules;
    
    public int tooLittleNeighbours = 1;
    public int tooMuchNeighbours = 4;
    public int tooBeBornNeighbours = 3;
    
    public Transform cellPrefab;
    public StateVisualizer stateVisualizer;
    
    private float elapsed;
    private int iteration;
    private List<WorldScore> worldScores = new List<WorldScore>();
    private EncodedWorld initialState;
    
    void Start()
    {
        rules = new BasicRules(tooLittleNeighbours, tooMuchNeighbours, tooBeBornNeighbours);

        InitializeRandomWorldState();
    }

    public void InitializeRandomWorldState()
    {
        for (var x = 0; x < WORLD_SIZE; x++)
        {
            for (var z = 0; z < WORLD_SIZE; z++)
            {
                worldMap.SetCell(InstantiateCell(new Coords(x, z), (Cell.State) Random.Range(0, 2)));
            }
        }

        initialState = new EncodedWorld(worldMap);
    }

    public void PurgeWorld()
    {
        worldScores.Add(new WorldScore(initialState, CalculateScore(worldMap)));
        iteration = 0;
        Debug.Log("Final score: " + CalculateScore(worldMap));
        foreach (var cell in worldMap.GetCells())
        {
            Destroy(cell.cellObject.gameObject);
            worldMap.SetCell(InstantiateCell(cell.coords, Cell.State.DEAD));
        }
        stateVisualizer.Visualize(worldScores.OrderByDescending(item => item.score).First().encodedWorld);
    }

    private int CalculateScore(WorldMap worldMap)
    {
        return worldMap
            .GetCells()
            .Cast<Cell>()
            .Count(c => c.state == Cell.State.ALIVE);
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= REFRESH_RATE)
        {
            if (iteration == 10)
            {
                PurgeWorld();
                InitializeRandomWorldState();
            }
            elapsed %= REFRESH_RATE;
            iteration += 1;
            RefreshWorld();
        }

    }

    private Cell InstantiateCell(Coords coords, Cell.State state)
    {
        var cellObject = Instantiate(cellPrefab, new Vector3(coords.x, 0, coords.z), Quaternion.identity, transform);
        
        cellObject.gameObject.SetActive(Cell.State.ALIVE.Equals(state));

        return new Cell(coords, state, cellObject);
    }

    private void RefreshWorld()
    {
        foreach (var cell in worldMap.GetCells())
        {
            cell.nextState = rules.CalculateNextState(cell, worldMap);
        }

        worldMap.GoToNextState();
    }
}