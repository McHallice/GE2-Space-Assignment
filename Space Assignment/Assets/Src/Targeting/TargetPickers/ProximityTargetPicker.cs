using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Src.Targeting.TargetPickers
{

    public class ProximityTargetPicker : GeneticallyConfigurableTargetPicker
    {
        public Transform SourceObject;

       
        public bool KullInvalidTargets = true;

        public override IEnumerable<PotentialTarget> FilterTargets(IEnumerable<PotentialTarget> potentialTargets)
        {
            potentialTargets = potentialTargets.Select(t => AddScoreForDifference(t));
            if (KullInvalidTargets && potentialTargets.Any(t => t.IsValidForCurrentPicker))
            {
                return potentialTargets.Where(t => t.IsValidForCurrentPicker);
            }
            return potentialTargets;
        }

        private PotentialTarget AddScoreForDifference(PotentialTarget target)
        {
            var dist = target.DistanceToTurret(SourceObject);
            target.Score = target.Score - (dist * Multiplier);
            if(dist < Threshold)
            {
                target.IsValidForCurrentPicker = true;
                target.Score += FlatBoost;
            } else
            {
                target.IsValidForCurrentPicker = false;
            }
            return target;
        }
    }
}
