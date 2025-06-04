using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class GestureInputReceiver : MonoBehaviour
{
	UdpClient udpClient;
	int port = 5005;
	PlayerController player;

	void Start()
	{
		udpClient = new UdpClient(port);
		udpClient.Client.Blocking = false;  // non-blocking
		player = PlayerController.obj;
		Debug.Log("UDP listener started on port " + port);
	}

	void Update()
	{
		if (udpClient.Available > 0)
		{
			IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
			byte[] data = udpClient.Receive(ref remoteEP);
			string command = Encoding.UTF8.GetString(data).Trim().ToLower();

			Debug.Log("Received command: " + command);

			if (player == null)
			{
				Debug.LogError("PlayerController.obj is null!");
				return;
			}

			switch (command)
			{
				case "left":
					player.moveLeft();
					break;
				case "right":
					player.moveRight();
					break;
				case "jump":
					player.Jump();
					break;
				case "crouch":
					player.Crouch();
					break;
				default:
					Debug.Log("Unknown command: " + command);
					break;
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (udpClient != null)
			udpClient.Close();
	}
}
