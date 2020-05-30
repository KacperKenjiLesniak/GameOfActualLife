namespace Scenes.Scripts
{
    public class WorldScore
    {
        public EncodedWorld encodedWorld { get; }
        public int score { get; }

        public WorldScore(EncodedWorld encodedWorld, int score)
        {
            this.encodedWorld = encodedWorld;
            this.score = score;
        }
    }
}