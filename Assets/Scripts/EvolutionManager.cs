using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scenes.Scripts
{
    public class EvolutionManager : MonoBehaviour
    {
        private const string POSSIBLE_CELL_VALUES = "o ";

        private int populationCounter;
        public Scorer scorer = new AliveScorer();

        public StateVisualizer stateVisualizer;
        public World world;
        public WorldInitializer worldInitializer;

        private List<WorldScore> worldScores = new List<WorldScore>();

        private void Start()
        {
            world.endWorldStream.Subscribe(message =>
            {
                var (initialWorld, endWorld) = message;

                populationCounter += 1;

                var score = scorer.CalculateScore(endWorld);
                Debug.Log("Final score: " + score);
                worldScores.Add(new WorldScore(initialWorld, score));

                VisualizeBestWorld();

                if (populationCounter == 10)
                {
                    populationCounter = 0;
                    foreach (var worldMap in GenerateMutatedWorlds()) worldInitializer.worldStates.Enqueue(worldMap);
                }
            });
        }

        private void VisualizeBestWorld()
        {
            var bestWorldSoFar = worldScores
                .OrderByDescending(item => item.score)
                .First()
                .encodedWorld;
            stateVisualizer.Visualize(bestWorldSoFar);
        }

        private IEnumerable<EncodedWorld> GenerateMutatedWorlds()
        {
            return worldScores
                .OrderByDescending(item => item.score)
                .Take(2)
                .Select(worldScore =>
                    Enumerable.Repeat(worldScore, 5)
                        .Select(worldToMutate => Mutate(worldToMutate.encodedWorld))
                        .ToList())
                .SelectMany(i => i);
        }


        private EncodedWorld Mutate(EncodedWorld worldToMutate)
        {
            return new EncodedWorld(new StringBuilder(worldToMutate.code)
            {
                [Random.Range(0, worldToMutate.code.Length)] = RandomElement(POSSIBLE_CELL_VALUES)
            }.ToString());
        }

        private char RandomElement(string s)
        {
            return s[Random.Range(0, s.Length)];
        }
    }
}