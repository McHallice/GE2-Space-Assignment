using Assets.Src.Controllers;
using Assets.Src.Evolution;
using Assets.Src.Interfaces;
using Assets.Src.ObjectManagement;
using Assets.Src.Targeting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetChoosingMechanism : AbstractDeactivatableController, IDeactivateableTargetKnower
{
    public ITargetDetector Detector;

    
    public bool ContinuallyCheckForTargets = false;

    public bool NeverRetarget = false;
    
   
    public float PollInterval = 0;
    private float _pollCountdonwn = 0;

   
    public Target CurrentTarget { get; private set; }
    public IEnumerable<Target> FilteredTargets { get; private set; }
   

    public IKnowsEnemyTags EnemyTagKnower;
    public CombinedTargetPicker TargetPicker;

    public bool IncludeNavigationTargets = false;

   
    void Start ()
    {
        if(TargetPicker == null)
        {
            Debug.LogError(name + " Has no target picker");
            _active = false;
            return;
        }
        if(Detector == null)
        {
            EnemyTagKnower = EnemyTagKnower ?? GetComponentInParent<IKnowsEnemyTags>();
            if(EnemyTagKnower == null)
            {
                Debug.LogWarning(name + " Could not find enemy tag source for target picker while configuring the detector.");
            } else
            {
                Detector = new RepositoryTargetDetector(EnemyTagKnower);
            }
        }
        FilteredTargets = new List<Target>();
    }
	
	
	void FixedUpdate () {
        if (_active)
        {
            var targetIsInvalid = CurrentTarget == null || CurrentTarget.Transform.IsInvalid();

            if (targetIsInvalid || (ContinuallyCheckForTargets && _pollCountdonwn <= 0))
            {
                
                if (Detector == null)
                {
                    Debug.LogWarning(name + " has no detector.");
                    return;
                }
                
                var allTargets = Detector.DetectTargets(IncludeNavigationTargets);
                var allTargetsList = allTargets.ToList();
                FilteredTargets = TargetPicker.FilterTargets(allTargets).OrderByDescending(t => t.Score).Select(t => t as Target);
                var filteredTargetsList = FilteredTargets.ToList();
                var bestTarget = FilteredTargets.FirstOrDefault();
                if(TargetHasChanged(bestTarget, CurrentTarget))
                {
                    
                    
                    CurrentTarget = bestTarget;
                }
                if (CurrentTarget != null && NeverRetarget)
                {
                    Deactivate();   
                }
                _pollCountdonwn = PollInterval;
            } else
            {
               
                _pollCountdonwn -= Time.fixedDeltaTime;
            }
        }
    }

    private bool TargetHasChanged(Target old, Target newTarget)
    {
        if(old == newTarget)
        {
           
            return false;
        }
        if(old == null && newTarget != null || old != null && newTarget == null)
        {
            return true;
        }
        return old.Transform != newTarget.Transform;
    }



    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    {
        PollInterval = genomeWrapper.GetScaledNumber(10, 1);

        return genomeWrapper;
    }
}
