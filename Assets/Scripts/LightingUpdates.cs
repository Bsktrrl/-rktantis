using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingUpdates : MonoBehaviour
{
    public Light mainLight;
    public Light secondaryLight;

    void Start()
    {
        StartCoroutine(UpdateLighting());
    }

    IEnumerator UpdateLighting()
    {
        yield return new WaitForSeconds(0.1f);
        secondaryLight.transform.forward = -mainLight.transform.forward;
        DynamicGI.UpdateEnvironment();
        RenderSettings.fogColor = new Color(1, 0.8156863f, 0.6078432f) * ((1 - mainLight.transform.forward.y) * 0.5f);
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateLighting());
    }
}