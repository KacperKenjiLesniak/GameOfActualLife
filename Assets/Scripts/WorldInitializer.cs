using UnityEngine;

namespace Scenes.Scripts
{
    public class WorldInitializer : MonoBehaviour
    {

        public Transform cellPrefab;
        
        public WorldMap InitializeRandomWorldState(int worldSize)
        {
            WorldMap worldMap = new WorldMap(worldSize);
            for (var x = 0; x < worldSize; x++)
            {
                for (var z = 0; z < worldSize; z++)
                {
                    if (x >= worldSize / 4 && x < 3 * worldSize / 4 && z >= worldSize / 4 && z < 3 * worldSize / 4)
                    {
                        worldMap.SetCell(InstantiateCell(new Coords(x, z), (Cell.State) Random.Range(0, 2)));
                    }
                    else
                    {
                        worldMap.SetCell(InstantiateCell(new Coords(x, z), Cell.State.DEAD));
                    }
                }
            }

            return worldMap;
        }
        
        private Cell InstantiateCell(Coords coords, Cell.State state)
        {
            var cellObject = Instantiate(cellPrefab, new Vector3(coords.x, 0, coords.z), Quaternion.identity, transform);

            cellObject.gameObject.SetActive(Cell.State.ALIVE.Equals(state));

            return new Cell(coords, state, cellObject);
        }

    }
}