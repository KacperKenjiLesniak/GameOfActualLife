namespace Scenes.Scripts.Rules
{
    public interface Rules
    {
        Cell.State CalculateNextState(Cell cell, WorldMap worldMap);
    }
}