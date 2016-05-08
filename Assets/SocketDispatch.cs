using UnityEngine;
using System.Collections;
using System;

public class SocketDispatch : MonoBehaviour {

	public delegate void VRMsgHandler(Google.Protobuf.VRCom.Update msg);
	static event VRMsgHandler OnMocapMsg;
	static event VRMsgHandler OnHydraMsg; 

	public string address = "ws://127.0.0.1:4567";
	WebSocket w;
	Google.Protobuf.VRCom.Update updateMsg;


	// Use this for initialization
	IEnumerator Start () {
		w = new WebSocket(new Uri(address));
		yield return StartCoroutine(w.Connect());
		w.SendString ("{ \"username\":\"" + SystemInfo.deviceUniqueIdentifier + "\"}" );
		while (true)
		{
			WebSocketSharp.MessageEventArgs msg = w.Recv();
			if (msg != null)
			{
				if (msg.Type == WebSocketSharp.Opcode.Binary) {
					updateMsg = Google.Protobuf.VRCom.Update.Parser.ParseFrom (msg.RawData);
					Google.Protobuf.VRCom.Update.VrmsgOneofCase msgType = updateMsg.VrmsgCase;
					switch (msgType) {
					case Google.Protobuf.VRCom.Update.VrmsgOneofCase.Mocap:
						if (OnMocapMsg != null)
							OnMocapMsg (updateMsg);
						break;
					case Google.Protobuf.VRCom.Update.VrmsgOneofCase.Hydra:
						if (OnHydraMsg != null)
							OnHydraMsg (updateMsg);
						break;
					default:
						Debug.Log ("Received an unknown or empty message");
						break;
					}
				}
			}
			if (w.error != null)
			{
				Debug.LogError ("Error: "+w.error);
				break;
			}
			yield return 0;
		}
		w.Close();
	}
	
	// Update is called once per frame
	void Update () {
	}
		
	public static void On(Google.Protobuf.VRCom.Update.VrmsgOneofCase type, VRMsgHandler handler) {
		switch(type) {
		case Google.Protobuf.VRCom.Update.VrmsgOneofCase.Mocap:
			OnMocapMsg += handler;
			break;
		case Google.Protobuf.VRCom.Update.VrmsgOneofCase.Hydra:
			OnHydraMsg += handler;
			break;
		default:
			break;
		}
	}

	void OnApplicationQuit() {
		Debug.Log (w.m_Messages.Count);
		w.Close ();
	}
}
