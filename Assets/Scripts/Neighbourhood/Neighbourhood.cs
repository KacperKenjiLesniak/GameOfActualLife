using System.Collections.Generic;

namespace Scenes.Scripts
{
    public interface Neighbourhood
    {
        List<Cell> neighbours(Cell cell, WorldMap worldMap);
    }
}