using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class XRSetup : MonoBehaviour
{
    public TrackingOriginModeFlags TrackingOriginMode = TrackingOriginModeFlags.Floor;

    public static bool IsVrSetup { get; private set; } = false;

    private bool FirstRun = true;
    private List<XRInputSubsystem> XrInputSubsystems = new List<XRInputSubsystem>();

    void Start()
    {
        XrInputSubsystems.Clear();
        SubsystemManager.GetInstances(XrInputSubsystems);

        Debug.Log("XRINPUT SUBSYSTEM COUNT (TOTAL):   " + XrInputSubsystems.Count);
        Debug.Log("XRINPUT SUBSYSTEM COUNT (RUNNING): " + XrInputSubsystems.Where(xris => xris.running).Count());

        if (XrInputSubsystems.Any())
        {
            FirstRun = true;
            StartCoroutine(Coroutine());
        }
        else
        {
            IsVrSetup = false;
        }
    }

    IEnumerator Coroutine()
    {
        yield return null;
        yield return null;

        float delay = 0;
        while (true)
        {
            var currentTrackingSpace = GetTrackingSpaceType();

            if (FirstRun || !currentTrackingSpace.HasValue || currentTrackingSpace.Value != TrackingOriginMode)
            {
                Debug.LogWarning("CURRENT TRACKING SPACE TYPE: " + currentTrackingSpace);
                SetTrackingSpaceType(TrackingOriginMode);
                FirstRun = false;
            }

            IncreaseDelay(ref delay);
            foreach (var d in Delay(delay)) { yield return d; }
        }
    }

    TrackingOriginModeFlags? GetTrackingSpaceType()
    {
        try
        {
            bool any = false;
            TrackingOriginModeFlags flags = 0;
            foreach (var xri in XrInputSubsystems.Where(xris => xris.running))
            {
                flags |= xri.GetTrackingOriginMode();
                any = true;
            }

            if (any)
            {
                return flags;
            }
            else
            {
                IsVrSetup = false;
                Debug.LogError("UNABLE TO DETERMINE CURRENT TRACKING SPACE TYPE (0 SUBSYSTEMS).");
                return null;
            }
        }
        catch (Exception ex)
        {
            IsVrSetup = false;
            Debug.LogError("UNABLE TO DETERMINE CURRENT TRACKING SPACE TYPE: " + ex.ToString());
            return null;
        }
    }

    bool SetTrackingSpaceType(TrackingOriginModeFlags? flags)
    {
        if (flags == null) { return false; }

        try
        {
            bool any = false;
            foreach (var xri in XrInputSubsystems)
            {
                any = true;
                if (!xri.TrySetTrackingOriginMode(flags.Value))
                {
                    Debug.LogError("UNABLE TO SET TRACKING SPACE TYPE (NO EXCEPTION).");
                    IsVrSetup = false;
                    return false;
                }
            }

            if (any)
            {
                IsVrSetup = true;
                return true;
            }
            else
            {
                Debug.LogError("UNABLE TO SET TRACKING SPACE TYPE (0 SUBSYSTEMS).");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("UNABLE TO SET TRACKING SPACE TYPE: " + ex.ToString());
        }

        IsVrSetup = false;
        return false;
    }

    private IEnumerable Delay(float delay)
    {
        if (delay < 30)
        {
            for (int i = 0; i <= delay; i++)
            {
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(delay / 1000f);
        }
    }

    private void IncreaseDelay(ref float delay)
    {
        if (delay < 30)
        {
            delay++;
        }
        else
        {
            delay *= 2f;
        }

        if (IsVrSetup)
        {
            if (delay > 5000) { delay = 5000; }
        }
        else
        {
            if (delay > 1000) { delay = 1000; }
        }
    }
}