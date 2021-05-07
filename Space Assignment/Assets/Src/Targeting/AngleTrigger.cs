﻿using Assets.Src.Evolution;
using Assets.Src.Interfaces;
using Assets.Src.ModuleSystem;
using Assets.Src.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTrigger : GeneticConfigurableMonobehaviour, IFireControl
{
    public Rigidbody AimingObject;
    public float ShootAngle = 10;
    public bool AvoidFriendlyFire = true;
    public float FriendlyDetectionDistance = 20;
    public float MinFriendlyDetectionDistance = 0.5f;

    private IKnowsCurrentTarget _targetChoosingMechanism;
    private float? _projectileSpeed;

    // Use this for initialization
    void Start ()
    {
        var speedKnower = GetComponent<IKnowsProjectileSpeed>();
        _targetChoosingMechanism = GetComponent<IKnowsCurrentTarget>();
        _projectileSpeed = speedKnower != null ? speedKnower.KnownProjectileSpeed : null;
    }

    public bool ShouldShoot(Target target)
    {
        if (AvoidFriendlyFire)
        {
            RaycastHit hit;
            var ray = new Ray(AimingObject.position + (AimingObject.transform.forward * MinFriendlyDetectionDistance), AimingObject.transform.forward);
            if (Physics.Raycast(ray, out hit, FriendlyDetectionDistance, -1, QueryTriggerInteraction.Ignore))
            {
                
                if (hit.transform.tag == tag)
                {
                    
                    return false;
                }
            }
        }
        if (target != null)
        {
            var location = target.LocationInOthersSpace(AimingObject, _projectileSpeed);

            var angle = Vector3.Angle(location, Vector3.forward);

            return angle < ShootAngle;
        }
        return false;
    }

    public bool ShouldShoot()
    {
        return ShouldShoot(_targetChoosingMechanism.CurrentTarget);
    }

    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    {
        ShootAngle =genomeWrapper.GetScaledNumber(20, 0, 0.01f);
        
        return genomeWrapper;
    }
}
