namespace Scenes.Scripts.Rules
{
    public class BasicRules : Rules
    {
        private Neighbourhood neighbourhood = new MooreNeighbourhood();
        private int tooBeBornNeighbours;
        private int tooLittleNeighbours;
        private int tooMuchNeighbours;

        public BasicRules(int tooLittleNeighbours, int tooMuchNeighbours, int tooBeBornNeighbours)
        {
            this.tooLittleNeighbours = tooLittleNeighbours;
            this.tooMuchNeighbours = tooMuchNeighbours;
            this.tooBeBornNeighbours = tooBeBornNeighbours;
        }

        public Cell.State CalculateNextState(Cell cell, WorldMap worldMap)
        {
            var numberOfNeighbours = neighbourhood.neighbours(cell, worldMap)
                .FindAll(n => n.state == Cell.State.ALIVE)
                .Count;
            switch (cell.state)
            {
                case Cell.State.ALIVE
                    when numberOfNeighbours <= tooLittleNeighbours || numberOfNeighbours >= tooMuchNeighbours:
                    return Cell.State.DEAD;
                case Cell.State.DEAD when numberOfNeighbours == tooBeBornNeighbours:
                    return Cell.State.ALIVE;
                default:
                    return cell.state;
            }
        }
    }
}