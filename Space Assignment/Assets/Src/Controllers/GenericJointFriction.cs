using Assets.Src.Evolution;
using Assets.Src.ModuleSystem;
using UnityEngine;

public class GenericJointFriction : GeneticConfigurableMonobehaviour
{

    
    public float Friction = 0.4f;


    public Joint _hinge;
    public Rigidbody _thisBody;
    public Rigidbody _connectedBody;

	// Use this for initialization
	void Start () {
        _hinge = GetComponent<Joint>();
        _connectedBody = _hinge.connectedBody;

        _thisBody = GetComponent<Rigidbody>();

      
    }
	
	
	void FixedUpdate () {
        if(_hinge != null)
        {
            var parentAngularV = _connectedBody.angularVelocity;
            var ownAngularV = _thisBody.angularVelocity;
            
            
            var worldTorque = Friction * (ownAngularV - parentAngularV);

            _thisBody.AddTorque(-worldTorque);
            _connectedBody.AddTorque(worldTorque);
        }
    }

    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    {
        Friction = genomeWrapper.GetScaledNumber(Friction*2);
        return genomeWrapper;
    }
}
