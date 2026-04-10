using System;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    [SerializeField]
    private LayerMask sensingLayers;
    private HashSet<Collider2D> sensedObjects;

    public event Action<Collider2D> OnStartSense;
    public event Action<Collider2D> OnStopSense;

    public bool sensing => sensedObjects.Count > 0;

    private void Start()
    {
        sensedObjects = new();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (InMask(other, sensingLayers))
        {
            sensedObjects.Add(other);
            OnStartSense?.Invoke(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        sensedObjects.Remove(other);
        OnStopSense?.Invoke(other);
    }

    private static bool InMask(Collider2D collider, LayerMask mask)
    {
        return mask == (mask | 1 << collider.gameObject.layer);
    }
}