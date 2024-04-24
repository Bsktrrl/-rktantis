using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingUpdates : MonoBehaviour
{
    public Light mainLight;
    public Light secondaryLight;
    public GameObject player;
    float fogDarkness;

    void Start()
    {
        StartCoroutine(UpdateLighting());
    }

    IEnumerator UpdateLighting()
    {
        yield return new WaitForSeconds(0.1f);
        
        //Sun and moon
        secondaryLight.transform.forward = -mainLight.transform.forward;
        mainLight.intensity = Mathf.Clamp(mainLight.transform.forward.y, -1, -0.05f) * -1;
        
        //Fog
        fogDarkness = (1 - mainLight.transform.forward.y) * 0.5f;
        RenderSettings.fogColor = new Color(1 * fogDarkness, 0.8156863f * fogDarkness, 0.6078432f * fogDarkness, 1);

        //Intensity of ambient light
        RenderSettings.ambientIntensity = Mathf.Pow(Mathf.Clamp(mainLight.transform.forward.y, 0, 1) + 1, 2f);

        if (Vector3.Dot(Vector3.Normalize(player.transform.position), mainLight.transform.forward) <= 0.9f)
        {
            DynamicGI.UpdateEnvironment();
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(UpdateLighting());
    }
}