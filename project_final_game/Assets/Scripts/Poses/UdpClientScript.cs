using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpClientScript : MonoBehaviour
{
    private UdpClient udpClient;
    private IPEndPoint serverEndPoint;
    private IPEndPoint clientEndPoint;
    private bool isReceiving = true;
    private string host = "127.0.0.1";
    private int sendPort = 9999;
    private int receivePort = 9998;
     public LandmarkManager landmarkManager;

    void Awake()
    {
        try
        {
            udpClient = new UdpClient(receivePort);
            serverEndPoint = new IPEndPoint(IPAddress.Parse(host), sendPort);
            clientEndPoint = new IPEndPoint(IPAddress.Any, receivePort);
            isReceiving = true;
            StartReceiving();
            
            Debug.Log("Cliente UDP configurado.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error configurando UDP en Awake: {e.Message}");
        }
    }

    public void StartRecording()
    {
        try
        {
            string tokenUser = PlayerPrefs.GetString("ACCESS_TOKEN");
            string message = $"start {tokenUser}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverEndPoint);
            Debug.Log("Señal 'start' enviada al servidor.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error enviando señal 'start': {e.Message}");
        }
    }

    public void PauseRecording()
    {
        try
        {
            string message = "pause";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverEndPoint);
            Debug.Log("Señal 'pause' enviada al servidor.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error enviando señal 'pause': {e.Message}");
        }
    }

    public void ResumeRecording()
    {
        try
        {
            string message = "resume";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverEndPoint);
            Debug.Log("Señal 'resume' enviada al servidor.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error enviando señal 'resume': {e.Message}");
        }
    }

    public void StopRecording()
    {
        try
        {
            string message = "stop";
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverEndPoint);
            Debug.Log("Señal 'stop' enviada al servidor.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error enviando señal 'stop': {e.Message}");
        }
    }

    private async void StartReceiving()
    {
        await System.Threading.Tasks.Task.Run(() =>
        {
            while (isReceiving)
            {
                try
                {
                    byte[] data = udpClient.Receive(ref clientEndPoint);
                    string receivedData = Encoding.UTF8.GetString(data);
                    landmarkManager?.UpdateLandmarksFromJson(receivedData);
                }
                catch (SocketException se) when (se.SocketErrorCode == SocketError.Interrupted)
                {
                    break;
                }
                catch (Exception e)
                {

                    Debug.LogError($"Error recibiendo datos: {e.Message}");
                }
            }
        });
    }

    private void OnApplicationQuit()
    {
        isReceiving = false;
        if (udpClient != null)
            udpClient.Close();
    }
}
