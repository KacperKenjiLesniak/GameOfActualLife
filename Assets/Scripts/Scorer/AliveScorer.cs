using System.Linq;

namespace Scenes.Scripts
{
    public class AliveScorer : Scorer
    {
        public int CalculateScore(EncodedWorld encodedWorld)
        {
            return encodedWorld.code
                .Count(c => c == EncodedWorld.ALIVE);
        }
    }
}