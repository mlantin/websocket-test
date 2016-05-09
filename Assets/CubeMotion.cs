using UnityEngine;
using System.Collections;

public class CubeMotion : MonoBehaviour {

	Vector3 newpos = new Vector3();
	Quaternion newrot = new Quaternion();

	// Use this for initialization
	void Start () {
		SocketDispatch.On ("test", handleMocap);
		SocketDispatch.On (Google.Protobuf.VRCom.Update.VrmsgOneofCase.Hydra, handleHydra);
	}
		
	// Update is called once per frame
	void Update () {
	}

	void handleMocap(Google.Protobuf.VRCom.MocapSubject msg) {
		newpos.Set (msg.Pos.X, msg.Pos.Y, msg.Pos.Z);
		transform.position = newpos;
	}

	void handleHydra(Google.Protobuf.VRCom.Update msg) {
		if (msg.Hydra.CtrlNum == 0) {
			newrot.Set (msg.Hydra.Rot.X, msg.Hydra.Rot.Y, msg.Hydra.Rot.Z, msg.Hydra.Rot.W); 
			transform.rotation = newrot;
		}
	}
}
