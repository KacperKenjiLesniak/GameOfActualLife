namespace Scenes.Scripts
{
    public class WorldMap
    {
        public int worldSize { get; }
        private Cell[,] worldMap;

        public WorldMap(int worldSize)
        {
            this.worldSize = worldSize;
            
            worldMap = new Cell[worldSize, worldSize];
        }
        
        public Cell GetCell(int x, int z)
        {
            if (x >= 0 && x < worldSize && z >= 0 && z < worldSize)
            {
                return worldMap[x, z];
            }
            return new Cell(new Coords(-1, -1), null);
        }
        
        public void SetCell(Cell cell)
        {
            if (cell.coords.x >= 0 && cell.coords.x < worldSize && cell.coords.z >= 0 && cell.coords.z < worldSize)
            {
                worldMap[cell.coords.x, cell.coords.z] = cell;
            }
        }

        public Cell[,] GetCells()
        {
            return worldMap;
        }

        public void GoToNextState()
        {
            foreach (var cell in worldMap)
            {
                cell.proceedToNextState();
            }
        }
    }
}