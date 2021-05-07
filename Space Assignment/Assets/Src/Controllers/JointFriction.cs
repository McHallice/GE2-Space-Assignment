using Assets.Src.Evolution;
using Assets.Src.Interfaces;
using Assets.Src.ModuleSystem;
using UnityEngine;

public class JointFriction : GeneticConfigurableMonobehaviour
{

    
    public float Friction = 0.4f;

    

    private HingeJoint _hinge;
    private Rigidbody _thisBody;
    private Rigidbody _connectedBody;
    private Vector3 _axis;  //local space

	// Use this for initialization
	void Start () {
        _hinge = GetComponent<HingeJoint>();
        _connectedBody = _hinge.connectedBody;
        _axis = _hinge.axis;

        _thisBody = GetComponent<Rigidbody>();

        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(_hinge != null)
        {
            var angularV = _hinge.velocity;
           
            var worldAxis = transform.TransformVector(_axis);
            var worldTorque = Friction * angularV * worldAxis;

            _thisBody.AddTorque(-worldTorque);
            if(_connectedBody != null)
                _connectedBody.AddTorque(worldTorque);
        }
    }

    protected override GenomeWrapper SubConfigure(GenomeWrapper genomeWrapper)
    {
        Friction = genomeWrapper.GetScaledNumber(Friction*2);
        return genomeWrapper;
    }
}
