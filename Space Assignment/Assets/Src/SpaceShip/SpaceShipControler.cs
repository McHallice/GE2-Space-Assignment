using Assets.Src.Controllers;
using Assets.Src.Evolution;
using Assets.Src.Interfaces;

using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControler : AbstractDeactivatableController
{
    private IKnowsCurrentTarget _targetChoosingMechanism;

    public float ShootAngle = 30;
    public float TorqueMultiplier = 9;
    public int StartDelay = 2;

    public float SlowdownWeighting = 10;
    public float RadialSpeedThreshold = 10;
    public float MaxRange = 100;
    public float MinRange = 20;
    public float LocationAimWeighting = 1;
    public float MaxTangentialVelocity = 10;
    public float MinTangentialVelocity = 0;
    public float TangentialSpeedWeighting = 1;

   
    public Rigidbody Torquer;
    
    private readonly List<Rigidbody> _torquers = new List<Rigidbody>();

    public float AngularDragForTorquers = 20;

    private const float Fuel = Mathf.Infinity;
    private Rigidbody _thisSpaceship;

    

    public Transform VectorArrow;

   
    public void Start()
    {
        _thisSpaceship = GetComponent<Rigidbody>();
        _targetChoosingMechanism = GetComponent<IKnowsCurrentTarget>();

       
        if (Torquer != null)
        {
            _torquers.Add(Torquer);
        }
        Initialise();
    }

    private void Initialise()
    {
        

       
    }

    
    public void FixedUpdate()
    {
       
    }

    

    public void RegisterTorquer(Transform torquer)
    {
        _torquers.Add(torquer.GetComponent<Rigidbody>());
        Initialise();
        
    }

    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    {
        const float MaxVelociyTollerance = 100;
        const float DefaultVelociyTolleranceProportion = 0.1f;

        ShootAngle = genomeWrapper.GetScaledNumber(180);
        LocationAimWeighting = genomeWrapper.GetScaledNumber(2);
        SlowdownWeighting = genomeWrapper.GetScaledNumber(70);
        MaxRange = genomeWrapper.GetScaledNumber(5000, 0, 0.1f);
        MinRange = genomeWrapper.GetScaledNumber(1000, 0, 0.1f);
        MaxTangentialVelocity = genomeWrapper.GetScaledNumber(MaxVelociyTollerance, 0, DefaultVelociyTolleranceProportion);
        MinTangentialVelocity = genomeWrapper.GetScaledNumber(MaxVelociyTollerance, 0, DefaultVelociyTolleranceProportion);
        TangentialSpeedWeighting = genomeWrapper.GetScaledNumber(70);
        AngularDragForTorquers = genomeWrapper.GetScaledNumber(2, 0, 0.2f);
        RadialSpeedThreshold = genomeWrapper.GetScaledNumber(MaxVelociyTollerance, 0, DefaultVelociyTolleranceProportion);

        return genomeWrapper;
    }
}
