using System.Collections.Generic;
using System.Linq;

namespace Assets.Src.Targeting.TargetPickers
{
  
    class MassTargetPicker : GeneticallyConfigurableTargetPicker
    {
        public bool KullInvalidTargets = true;

        public override IEnumerable<PotentialTarget> FilterTargets(IEnumerable<PotentialTarget> potentialTargets)
        {
    
            potentialTargets = potentialTargets.Select(t => {
                var rigidbody = t.Rigidbody;
                if(rigidbody != null)
                {
                    t.Score += Multiplier * rigidbody.mass;
                    if (rigidbody.mass > Threshold)
                    {
                        
                        t.IsValidForCurrentPicker = true;
                        t.Score += FlatBoost;
                    } else
                    {
                        t.IsValidForCurrentPicker = false;
                    }
                    return t;
                }
                else
                {
                    t.IsValidForCurrentPicker = false;
                    return t;
                }
            });

            
            if (KullInvalidTargets && potentialTargets.Any(t => t.IsValidForCurrentPicker))
            {
             
                return potentialTargets.Where(t => t.IsValidForCurrentPicker);
            }

            return potentialTargets;
        }
    }
}
