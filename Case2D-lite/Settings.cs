using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case2D.Common
{
    public static class Settings
    {
        public const float lengthUnitsPerMeter = 1.0f;
        public const int maxPolygonVertices = 8;
        /// The maximum number of contact points between two convex shapes. Do
        /// not change this value.
        public const int maxManifoldPoints = 2;

        /// This is used to fatten AABBs in the dynamic tree. This allows proxies
        /// to move by a small amount without triggering a tree adjustment.
        /// This is in meters.
        public const float aabbExtension = (0.1f * lengthUnitsPerMeter);

        /// This is used to fatten AABBs in the dynamic tree. This is used to predict
        /// the future position based on the current displacement.
        /// This is a dimensionless multiplier.
        public const float aabbMultiplier = 4.0f;

        /// A small length used as a collision and constraint tolerance. Usually it is
        /// chosen to be numerically significant, but visually insignificant. In meters.
        public const float linearSlop = (0.005f * lengthUnitsPerMeter);

        /// A small angle used as a collision and constraint tolerance. Usually it is
        /// chosen to be numerically significant, but visually insignificant.
        public const float angularSlop = (2.0f / 180.0f * (float)Math.PI);

        /// The radius of the polygon/edge shape skin. This should not be modified. Making
        /// this smaller means polygons will have an insufficient buffer for continuous collision.
        /// Making it larger may create artifacts for vertex collision.
        public const float polygonRadius = (2.0f * linearSlop);

        /// Maximum number of sub-steps per contact in continuous physics simulation.
        public const int maxSubSteps = 8;


        // Dynamics

        /// Maximum number of contacts to be handled to solve a TOI impact.
        public const int maxTOIContacts = 32;

        /// A velocity threshold for elastic collisions. Any collision with a relative linear
        /// velocity below this threshold will be treated as inelastic. Meters per second.
        public const float velocityThreshold = (1.0f * lengthUnitsPerMeter);

        /// The maximum linear position correction used when solving constraints. This helps to
        /// prevent overshoot. Meters.
        public const float maxLinearCorrection = (0.2f * lengthUnitsPerMeter);

        /// The maximum angular position correction used when solving constraints. This helps to
        /// prevent overshoot.
        public const float maxAngularCorrection = (8.0f / 180.0f * (float)Math.PI);

        /// The maximum linear translation of a body per step. This limit is very large and is used
        /// to prevent numerical problems. You shouldn't need to adjust this. Meters.
        public const float maxTranslation = (2.0f * lengthUnitsPerMeter);
        public const float maxTranslationSquared = (maxTranslation * maxTranslation);

        /// The maximum angular velocity of a body. This limit is very large and is used
        /// to prevent numerical problems. You shouldn't need to adjust this.
        public const float maxRotation = (0.5f * (float)Math.PI);
        public const float maxRotationSquared = (maxRotation * maxRotation);

        /// This scale factor controls how fast overlap is resolved. Ideally this would be 1 so
        /// that overlap is removed in one time step. However using values close to 1 often lead
        /// to overshoot.
        public const float baumgarte = 0.2f;
        public const float toiBaumgarte = 0.75f;


        // Sleep

        /// The time that a body must be still before it will go to sleep.
        public const float timeToSleep = 0.5f;

        /// A body cannot sleep if its linear velocity is above this tolerance.
        public const float linearSleepTolerance = (0.01f * lengthUnitsPerMeter);

        /// A body cannot sleep if its angular velocity is above this tolerance.
        public const float angularSleepTolerance = (2.0f / 180.0f * (float)Math.PI);
    }
}
