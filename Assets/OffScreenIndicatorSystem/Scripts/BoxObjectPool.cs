using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxObjectPool : MonoBehaviour
{
    public static BoxObjectPool Current;

    [Tooltip("Assign the box prefab.")] public Indicator pooledObject;
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
            Indicator box = Instantiate(pooledObject, transform, false);
            box.Activate(false);
            _pooledObjects.Add(box);
        }
    }

    // Gets pooled objects from the pool.
    public Indicator GetPooledObject()
    {
        foreach (Indicator t in _pooledObjects.Where(t => !t.Active))
            return t;

        if (!willGrow) return null;

        Indicator box = Instantiate(pooledObject, transform, false);
        box.Activate(false);
        _pooledObjects.Add(box);
        return box;
    }

    // Deactivate all the objects in the pool.
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator box in _pooledObjects) box.Activate(false);
    }
}