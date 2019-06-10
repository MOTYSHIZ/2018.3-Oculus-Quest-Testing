using UnityEngine;
using Bolt;

public class SnapZoneEventBridge : MonoBehaviour
{
    public void SnappedCustomEventTrigger(object arg1)
    {
        CustomEvent.Trigger(gameObject, "snapped", new object[1] { arg1 });
    }

    public void UnsnappedCustomEventTrigger(object arg1)
    {
        CustomEvent.Trigger(gameObject, "unsnapped", new object[1] { arg1 });
    }
}
