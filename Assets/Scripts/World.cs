using System;
using System.Collections.Generic;
using System.Linq;
using Scenes.Scripts;
using Scenes.Scripts.Rules;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class World : MonoBehaviour
{
    public WorldConfig config;

    public IObservable<Tuple<EncodedWorld, EncodedWorld>> endWorldStream;
    private EncodedWorld initialState;
    private int iteration;

    private DateTimeOffset lastUpdate;
    private Queue<Tuple<EncodedWorld, EncodedWorld>> messagesToBeSent = new Queue<Tuple<EncodedWorld, EncodedWorld>>();
    public Rules rules;
    public WorldInitializer worldInitializer;
    public WorldMap worldMap;

    private void Awake()
    {
        rules = new BasicRules(config.tooLittleNeighbours, config.tooMuchNeighbours, config.tooBeBornNeighbours);

        InitializeWorldState();

        this.UpdateAsObservable()
            .Timestamp()
            .Where(x => x.Timestamp > lastUpdate.AddSeconds(config.refreshRate))
            .Subscribe(x =>
                {
                    iteration += 1;
                    RefreshWorld();
                    lastUpdate = x.Timestamp;
                }
            );

        this.UpdateAsObservable()
            .Where(_ => iteration == 10)
            .Subscribe(x =>
            {
                PrepareEndWorldMessage();
                ResetWorld();
                InitializeWorldState();
            });

        endWorldStream = this.UpdateAsObservable()
            .Where(_ => messagesToBeSent.Any())
            .Select(_ => messagesToBeSent.Dequeue());
    }

    private void PrepareEndWorldMessage()
    {
        messagesToBeSent.Enqueue(
            new Tuple<EncodedWorld, EncodedWorld>(new EncodedWorld(initialState.code),
                new EncodedWorld(worldMap)));
    }

    public void InitializeWorldState()
    {
        worldMap = worldInitializer.GetInitialWorldState();
        initialState = new EncodedWorld(worldMap);
    }

    public void ResetWorld()
    {
        iteration = 0;
        foreach (var cell in worldMap.GetCells())
        {
            Destroy(cell.cellObject.gameObject);
            worldMap.SetCell(new Cell(cell.coords, Cell.State.DEAD, null));
        }
    }

    private void RefreshWorld()
    {
        foreach (var cell in worldMap.GetCells()) cell.nextState = rules.CalculateNextState(cell, worldMap);

        worldMap.GoToNextState();
    }
}