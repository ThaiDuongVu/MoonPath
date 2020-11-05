using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class ArrowObjectPool : MonoBehaviour
{
    public static ArrowObjectPool Current;

    [Tooltip("Assign the arrow prefab.")] public Indicator pooledObject;
    [Tooltip("Initial pooled amount.")] public int pooledAmount = 1;

    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    private List<Indicator> _pooledObjects;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        _pooledObjects = new List<Indicator>();

        for (int i = 0; i < pooledAmount; i++)
        {
            Indicator arrow = Instantiate(pooledObject, transform, false);
            arrow.Activate(false);
            _pooledObjects.Add(arrow);
        }
    }

    // Gets pooled objects from the pool.
    public Indicator GetPooledObject()
    {
        foreach (Indicator t in _pooledObjects.Where(t => !t.Active)) return t;

        if (!willGrow) return null;

        Indicator arrow = Instantiate(pooledObject, transform, false);
        arrow.Activate(false);
        _pooledObjects.Add(arrow);
        return arrow;
    }

    // Deactivate all the objects in the pool.
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator arrow in _pooledObjects) arrow.Activate(false);
    }
}