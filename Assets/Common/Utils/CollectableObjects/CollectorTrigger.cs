using Collectables.View;
using UnityEngine;

public class CollectorTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _radius;
    [SerializeField] private bool _drawDebug;
    
    private Collider[] _collidersBuffer = new Collider[5];
    private float _scanInterval;
    private const int ScanFrequency = 30;
    private float _scanTimer;

    private void Start()
    {
        _scanInterval = 1.0f / ScanFrequency;
    }

    public void Update()
    {
        _scanTimer -= Time.deltaTime;
        if (_scanTimer < 0)
        {
            _scanTimer += _scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            _radius,
            _collidersBuffer,
            _layerMask,
            QueryTriggerInteraction.Ignore);

        if (count < 1)
        {
            return;
        }

        foreach (var col in _collidersBuffer)
        {
            if (!col)
            {
                continue;
            }

            if (col.TryGetComponent<ICollectableView>(out var collectable))
            {
                collectable.Collect();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_drawDebug)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}