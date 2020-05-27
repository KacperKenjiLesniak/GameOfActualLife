using System.Collections.Generic;

namespace Scenes.Scripts
{
    public class VonNeumannNeighbourhood : Neighbourhood
    {
        public List<Cell> neighbours(Cell cell, WorldMap worldMap)
        {
            var coordsX = cell.coords.x;
            var coordsZ = cell.coords.z;

            return new List<Cell>
                {
                    worldMap.GetCell(coordsX - 1, coordsZ),
                    worldMap.GetCell(coordsX, coordsZ - 1),
                    worldMap.GetCell(coordsX, coordsZ + 1),
                    worldMap.GetCell(coordsX + 1, coordsZ),
                }
                .FindAll(c => c.coords.x != -1);
        }
    }
}