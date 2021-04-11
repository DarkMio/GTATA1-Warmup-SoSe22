using UnityEngine;
using static Unity.Mathematics.math;

namespace Scripts
{

    /// <summary>
    /// Helper class for sprite/bounds like intersections
    /// </summary>
    public static class Intersection
    {
        /// <summary>
        /// Extracts the correct bounds of the top-most tile on a TileRow
        /// </summary>
        private static Bounds RetrieveBounds(TileRow tile)
        {
            return tile.TopSprite.bounds;
        }

        /// <summary>
        /// Extracts the correct bounds from a CharacterController
        /// </summary>
        private static Bounds RetrieveBounds(RunCharacterController character)
        {
            var characterExtents = character.CharacterSprite.bounds.extents;
            return new Bounds(character.Transform.position, characterExtents);
        }
        
        /// <summary>
        /// Returns the percentage of an intersection of the character sprite with a tile.
        /// </summary>
        public static float NormalizedIntersectionAmount(RunCharacterController character, TileRow tile)
        {
            // get the bounds
            var tileBounds = RetrieveBounds(tile);
            var characterBounds = RetrieveBounds(character);

            // calculate the x/y overlap
            var xOverlap = max(0, min(tileBounds.max.x, characterBounds.max.x) - max(tileBounds.min.x, characterBounds.min.x));
            var yOverlap = max(0, min(tileBounds.max.y, characterBounds.max.y) - max(tileBounds.min.y, characterBounds.min.y));
            // an overlap is also just a rectangle - so calculate the area occupied 
            var overlapArea = xOverlap * yOverlap;
            // similarly calculate the area of the character occupied
            var characterArea = characterBounds.size.x * characterBounds.size.y;
            // return the fraction of how much overlap occurs on the character sprite
            return overlapArea / characterArea;
        }
    }
}