using UnityEngine;
using System.Collections;

public class CubeMotion : MonoBehaviour {

	Vector3 newpos = new Vector3();
	Quaternion newrot = new Quaternion();

	// Use this for initialization
	void Start () {
		SocketDispatch.On (Google.Protobuf.VRCom.Update.VrmsgOneofCase.Mocap, handleMocap);
		SocketDispatch.On (Google.Protobuf.VRCom.Update.VrmsgOneofCase.Hydra, handleHydra);
	}
		
	// Update is called once per frame
	void Update () {
	}

	void handleMocap(Google.Protobuf.VRCom.Update msg) {
		newpos.Set (msg.Mocap.Subjects [0].Pos.X, msg.Mocap.Subjects [0].Pos.Y, msg.Mocap.Subjects [0].Pos.Z);
		transform.position = newpos;
	}

	void handleHydra(Google.Protobuf.VRCom.Update msg) {
		if (msg.Hydra.CtrlNum == 0) {
			newrot.Set (msg.Hydra.Rot.X, msg.Hydra.Rot.Y, msg.Hydra.Rot.Z, msg.Hydra.Rot.W); 
			transform.rotation = newrot;
		}
	}
}
