namespace Scenes.Scripts
{
    public class WorldScore
    {
        public WorldScore(EncodedWorld encodedWorld, int score)
        {
            this.encodedWorld = encodedWorld;
            this.score = score;
        }

        public EncodedWorld encodedWorld { get; }
        public int score { get; }
    }
}