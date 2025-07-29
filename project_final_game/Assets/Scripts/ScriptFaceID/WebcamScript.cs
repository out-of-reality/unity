using UnityEngine;
using UnityEngine.UI;

public class WebcamScript : MonoBehaviour
{
    private WebCamTexture webcam;
    public RawImage img;

    public WebCamTexture WebcamTexture => webcam;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0)
        {
            webcam = new WebCamTexture(devices[0].name);
            img.texture = webcam;
            img.material.mainTexture = webcam;
            webcam.Play();
        }
        else
        {
            Debug.LogError("No camera found");
        }
    }

    public void StopCamera()
    {
        if (webcam != null && webcam.isPlaying)
        {
            webcam.Stop();
        }
    }
}
