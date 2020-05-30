using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes.Scripts;
using Scenes.Scripts.Rules;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class World : MonoBehaviour
{
    private const int WORLD_SIZE = 20;
    private const float REFRESH_RATE = 0.2f;

    public WorldMap worldMap = new WorldMap(WORLD_SIZE);
    public Rules rules;

    public int tooLittleNeighbours = 1;
    public int tooMuchNeighbours = 4;
    public int tooBeBornNeighbours = 3;

    public StateVisualizer stateVisualizer;
    public WorldInitializer worldInitializer;
    public Scorer scorer = new AliveScorer();

    private DateTimeOffset lastUpdate;
    private int iteration;
    private List<WorldScore> worldScores = new List<WorldScore>();
    private EncodedWorld initialState;

    void Start()
    {
        rules = new BasicRules(tooLittleNeighbours, tooMuchNeighbours, tooBeBornNeighbours);

        InitializeRandomWorldState();

        this.UpdateAsObservable()
            .Timestamp()
            .Where(x => x.Timestamp > lastUpdate.AddSeconds(REFRESH_RATE))
            .Subscribe(x =>
                {
                    iteration += 1;
                    RefreshWorld();
                    lastUpdate = x.Timestamp;
                }
            );

        this.UpdateAsObservable()
            .Where(_ => iteration == 10)
            .Subscribe(_ =>
            {
                ResetWorld();
                InitializeRandomWorldState();
            });
    }

    public void InitializeRandomWorldState()
    {
        worldMap = worldInitializer.InitializeRandomWorldState(WORLD_SIZE);

        initialState = new EncodedWorld(worldMap);
    }

    public void ResetWorld()
    {
        iteration = 0;

        var score = scorer.CalculateScore(worldMap);
        Debug.Log("Final score: " + score);
        worldScores.Add(new WorldScore(initialState, score));

        foreach (var cell in worldMap.GetCells())
        {
            Destroy(cell.cellObject.gameObject);
            worldMap.SetCell(new Cell(cell.coords, Cell.State.DEAD, null));
        }

        stateVisualizer.Visualize(worldScores.OrderByDescending(item => item.score).First().encodedWorld);
    }

    private void RefreshWorld()
    {
        foreach (var cell in worldMap.GetCells())
        {
            cell.nextState = rules.CalculateNextState(cell, worldMap);
        }

        worldMap.GoToNextState();
    }

    void Update()
    {
    }
}