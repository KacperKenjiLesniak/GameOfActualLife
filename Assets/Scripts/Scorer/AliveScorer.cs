using System.Linq;

namespace Scenes.Scripts
{
    public class AliveScorer : Scorer
    {
        public int CalculateScore(WorldMap worldMap)
        {
            return worldMap
                .GetCells()
                .Cast<Cell>()
                .Count(c => c.state == Cell.State.ALIVE);
        }
    }
}