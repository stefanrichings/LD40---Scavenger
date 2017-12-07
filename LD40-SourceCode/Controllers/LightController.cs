using System.Collections.Generic;
using UnityEngine;

public class LightController : UnitySingleton<LightController>
{
    public float lightIntensity = 5f;
    public float materialEmission = 1f;

    public List<Material> materials;

    List<Light> lights = new List<Light>();
    float initialIntensity;

    protected override void Awake ()
    {
        base.Awake();

        initialIntensity = lightIntensity;
        AddLights();

        GameController.OnCoreChange += (bool val) =>
        {
            if (val)
            {
                SubtractIntensity();
            }
        };
    }

    public void RemoveFromList(Light light)
    {
        lights.Remove(light);
    }

    public void SubtractIntensity()
    {
        lightIntensity -= (initialIntensity / GameController.Instance.NumberOfCores);
        materialEmission -= (1f / GameController.Instance.NumberOfCores);

        SetLightIntensity(lightIntensity);
    }

    public void SetLightIntensity(float intensity)
    {
        foreach (var light in lights)
        {
            if (light != null)
            {
                light.intensity = intensity;
            }
        }

        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.white, materialEmission));
        }
    }

    void AddLights()
    {
        foreach (var light in FindObjectsOfType<Light>())
        {
            lights.Add(light);
        }
        SetLightIntensity(lightIntensity);
    }
}
