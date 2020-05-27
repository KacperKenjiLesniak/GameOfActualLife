using System.Collections;
using System.Collections.Generic;
using Scenes.Scripts;
using UnityEngine;

namespace Scenes.Scripts
{
    public class WorldMap
    {

        private int worldSize;
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
        
        public void SetCell(Cell cell, int x, int z)
        {
            if (x >= 0 && x < worldSize && z >= 0 && z < worldSize)
            {
                worldMap[x, z] = cell;
            }
        }

        public Cell[,] GetCells()
        {
            return worldMap;
        }
    }
}