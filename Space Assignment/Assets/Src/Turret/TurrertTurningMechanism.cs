using Assets.Src.Evolution;
using Assets.Src.Interfaces;
using Assets.Src.ModuleSystem;
using Assets.Src.Targeting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrertTurningMechanism : GeneticConfigurableMonobehaviour
{
    private IKnowsCurrentTarget _targetChoosingMechanism;
    public Transform RestTarget;

    public Rigidbody TurnTable;
    public Rigidbody ElevationHub;
    private bool _active = true;

    private ITurretTurner _turner;

    private string InactiveTag = "Untagged";

    

    public float TurnTableMotorFoce = 30;
    public float TurnTableMotorSpeedMultiplier = 500;
    public float TurnTableMotorSpeedCap = 100;

    public float ElevationHubMotorFoce = 30;
    public float ElevationHubMotorSpeedMultiplier = 500;
    public float ElevationHubMotorSpeedCap = 100;

    // Use this for initialization
    void Start()
    {
        _targetChoosingMechanism = GetComponent<IKnowsCurrentTarget>();
        var speedKnower = GetComponent<IKnowsProjectileSpeed>();
        var projectileSpeed = speedKnower != null ? speedKnower.KnownProjectileSpeed : null;
        var rigidbody = GetComponent<Rigidbody>();

        _turner = new UnityTurretTurner(rigidbody, TurnTable, ElevationHub, RestTarget, projectileSpeed)
        {
            TurnTableTorque = TurnTableMotorFoce,
            TurnTableSpeedMultiplier = TurnTableMotorSpeedMultiplier,
            TurnTableSpeedCap = TurnTableMotorSpeedCap,
            EHTorque = ElevationHubMotorFoce,
            EHSpeedMultiplier = ElevationHubMotorSpeedMultiplier,
            EHSpeedCap = ElevationHubMotorSpeedCap
        };

        
        
    }

   
    void FixedUpdate()
    {
        ;
    }

    public void Deactivate()
    {
      
        _active = false;
        tag = InactiveTag;
        if(ElevationHub != null)
        {
            ElevationHub.tag = InactiveTag;
        }
        if (TurnTable != null)
        {
            TurnTable.tag = InactiveTag;
        }
    }

    public void DieNow()
    {
        Deactivate();
        DestroyJoint(ElevationHub);
        DestroyJoint(TurnTable);
    }

    private void DestroyJoint(Rigidbody jointedObject)
    {
        if (jointedObject != null)
        {
            var hinge = jointedObject.GetComponent<HingeJoint>();
            if (hinge != null)
            {
                GameObject.Destroy(hinge);
            }
            jointedObject.transform.parent = null;
        }
    }
    
    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    {
        TurnTableMotorFoce = genomeWrapper.GetScaledNumber(TurnTableMotorFoce);
        TurnTableMotorSpeedMultiplier = genomeWrapper.GetScaledNumber(TurnTableMotorSpeedMultiplier * 2);
        TurnTableMotorSpeedCap = genomeWrapper.GetScaledNumber(TurnTableMotorSpeedCap * 2);
        ElevationHubMotorFoce = genomeWrapper.GetScaledNumber(ElevationHubMotorFoce);
        ElevationHubMotorSpeedMultiplier = genomeWrapper.GetScaledNumber(ElevationHubMotorSpeedMultiplier * 2);
        ElevationHubMotorSpeedCap = genomeWrapper.GetScaledNumber(ElevationHubMotorSpeedCap * 2);
        
        return genomeWrapper;
    }
}
