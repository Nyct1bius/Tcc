using UnityEngine;

[RequireComponent(typeof(Rigidbody))]    
public class BuoyancyObject : MonoBehaviour
{
    [SerializeField] private Transform[] floatPoints;
    [SerializeField] private float underWaterDrag = 3f;
    [SerializeField] private float underWaterAngularDrag = 1f;

    [SerializeField] private float airDrag = 0f;
    [SerializeField] private float airAngularDrag = 0.05f;


    [SerializeField] private float floatingPower = 15f;

    private bool underWater = false;
    private int floatPointsUnderWater = 0;

    Rigidbody _body;

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        for(int i = 0; i < floatPoints.Length; i++)
        {
            float waterLevel = OceanManager.Instance.GetWavesHeight(floatPoints[i].position);   
            float difference = floatPoints[i].position.y - waterLevel;

            if (difference < 0f)
            {
                _body.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floatPoints[i].position, ForceMode.Force);
                floatPointsUnderWater++;
                if (!underWater)
                {
                    underWater = true;
                    _body.linearDamping = underWaterDrag;
                    _body.angularDamping = underWaterAngularDrag;
                }
            }
        }
       
        if(underWater && floatPointsUnderWater == 0)
        {
            underWater = false;
            _body.linearDamping = airDrag;
            _body.angularDamping = airAngularDrag;
        }

    }
}
