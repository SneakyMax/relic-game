using UnityEngine;

namespace Assets.Scripts
{
    public static class MathHelpers
    {
        /// <summary>Gets the world space angle from the <see cref="from"/> position to the <see cref="to"/> position IGNORING Z.</summary>
        public static float AngleTo(this Vector3 from, Vector3 to)
        {
            var fromToTo = (Vector2) to - (Vector2) from;
            var direction = Mathf.Atan2(fromToTo.y, fromToTo.x);

            return direction;
        }

        public static Vector3 UnitVectorTo(this Vector3 from, Vector3 to)
        {
            var direction = AngleTo(from, to);
            return new Vector3(Mathf.Cos(direction), Mathf.Sin(direction), 0);
        }
    }
}