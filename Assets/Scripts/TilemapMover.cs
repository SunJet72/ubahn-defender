using System.Linq;
using Fusion;                    // You’re already using Fusion
using UnityEngine;
using UnityEngine.EventSystems;

public class GroundLooper : NetworkBehaviour
{
    [SerializeField] private Transform[] segments;   // assign the 2‑3 chunks here
    [SerializeField] private float segmentWidth = 100; // in world units
    // [SerializeField] private float movementSpeed;     // reference to your existing script
    // [SerializeField] private Vector2 movementDir;

    public override void FixedUpdateNetwork()
    {
        // 1. Move every chunk with the train’s current speed
        // foreach (var s in segments)
        // {
        //     s.position += (Vector3)(movementSpeed * Runner.DeltaTime* movementDir);
        // }

        // 2. When a chunk is completely behind the camera, recycle it to the front
        foreach (var s in segments)
        {
            if (s.position.y <= -1.5*segmentWidth)
            {
                float upMost = segments.Max(t => t.position.y);
                s.position = new Vector3(s.position.x, upMost + segmentWidth, s.position.z);
            }
        }
    }
}
