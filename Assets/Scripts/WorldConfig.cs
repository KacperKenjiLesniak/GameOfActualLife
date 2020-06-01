using UnityEngine;

namespace Scenes.Scripts
{
    public class WorldConfig : MonoBehaviour
    {
        public Transform cellPrefab;
        public float refreshRate = 0.2f;
        public int tooBeBornNeighbours = 3;
        public int tooLittleNeighbours = 1;
        public int tooMuchNeighbours = 4;
        public int worldSize = 20;
    }
}