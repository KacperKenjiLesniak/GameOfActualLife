using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Scenes.Scripts
{
    public class EvolutionManager : MonoBehaviour
    {
        public StateVisualizer stateVisualizer;
        public World world;
        public Scorer scorer = new AliveScorer();

        private List<WorldScore> worldScores = new List<WorldScore>();

        void Start()
        {
            world.endWorldStream.Subscribe(x =>
            {
                var score = scorer.CalculateScore(x.Item2);
                Debug.Log("Final score: " + score);
                worldScores.Add(new WorldScore(x.Item1, score));
                stateVisualizer.Visualize(worldScores
                    .OrderByDescending(item => item.score)
                    .First()
                    .encodedWorld);
            });
        }
    }
}