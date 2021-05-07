using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Targeting.TargetPickers
{
    public class ApproachingTargetPicker : GeneticallyConfigurableTargetPicker
    {
        public Rigidbody SourceObject;
        
        public override IEnumerable<PotentialTarget> FilterTargets(IEnumerable<PotentialTarget> potentialTargets)
        {
            return potentialTargets.Select(t => AddScoreForDifference(t));
        }

        private PotentialTarget AddScoreForDifference(PotentialTarget target)
        {
            var targetVelocity = target.Rigidbody == null ? Vector3.zero : target.Rigidbody.velocity;

            var relativeVelocity = SourceObject.velocity - targetVelocity;

            var reletiveLocation = target.Transform.position - SourceObject.position;

            var approachAngle = Vector3.Angle(relativeVelocity, reletiveLocation);

            var angleComponent = (-approachAngle/90)+1; 

            angleComponent = (float)Math.Pow(angleComponent, 3); 

            var score = angleComponent * relativeVelocity.magnitude * Multiplier;

            target.Score += score;

            

            return target;
        }
    }
}
