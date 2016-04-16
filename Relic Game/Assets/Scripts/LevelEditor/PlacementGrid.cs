using UnityEngine;
using Vexe.Runtime.Types;

namespace Assets.Scripts.LevelEditor
{
    public class PlacementGrid : BaseBehaviour
    {
        [AssignedInUnity]
        public Transform TopLeft;

        /// <summary>The number of grid units across, in <see cref="GridSize"/> increments.</summary>
        [AssignedInUnity]
        public int Width = 1;

        [AssignedInUnity]
        public int Height = 1;

        [AssignedInUnity]
        public int GridSize = 1;
    }
}