using UnityEngine;
using System.Collections;
using System;

public class SocketDispatch : MonoBehaviour {

	public string address = "ws://127.0.0.1:4567";
	public GameObject testobj;

	WebSocket w;
	Google.Protobuf.VRCom.Update updateMsg;

	Vector3 newpos = new Vector3();

	// Use this for initialization
	IEnumerator Start () {
		w = new WebSocket(new Uri(address));
		yield return StartCoroutine(w.Connect());
		w.SendString ("{ \"username\":\"" + SystemInfo.deviceUniqueIdentifier + "\"}" );
		while (true)
		{
			byte[] msg = w.Recv();
			if (msg != null)
			{
				updateMsg = Google.Protobuf.VRCom.Update.Parser.ParseFrom (msg);
				Google.Protobuf.VRCom.Update.VrmsgOneofCase msgType = updateMsg.VrmsgCase;
				switch (msgType) {
				case Google.Protobuf.VRCom.Update.VrmsgOneofCase.Mocap:
					//Debug.Log ("Received Mocap");
					handleMocap (updateMsg.Mocap);
					break;
				case Google.Protobuf.VRCom.Update.VrmsgOneofCase.Hydra:
					Debug.Log ("Received Hydra");
					break;
				default:
					Debug.Log ("Received an unknown or empty message");
					break;
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
		testobj.transform.position = newpos;
	}

	void handleMocap(Google.Protobuf.VRCom.Mocap mocap) {
		newpos.Set (mocap.Subjects [0].Pos.X, mocap.Subjects [0].Pos.Y, mocap.Subjects [0].Pos.Z);
	}

	void OnApplicationQuit() {
		Debug.Log (w.m_Messages.Count);
		w.Close ();
	}
}
