using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.AccessControl;

[RequireComponent (typeof(Terrain))]
public class PlayerIO : MonoBehaviour {
	
	public byte activeBlockType = 1;
	public Transform retAdd, retDel;

	public Terrain terrain;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		terrain = GetComponent<Terrain>();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray (transform.position + transform.forward / 2, transform.forward);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 8f)) {
			Vector3 p = hit.point - hit.normal / 2;
			retDel.position = new Vector3 (Mathf.Floor(p.x), Mathf.Floor(p.y), Mathf.Ceil(p.z));
			p = hit.point + hit.normal / 2;
			retAdd.position = new Vector3 (Mathf.Floor(p.x), Mathf.Floor(p.y), Mathf.Ceil(p.z));
			Control ();
		} 
		else {
			retAdd.position = new Vector3 (0, -100, 0);
			retDel.position = new Vector3 (0, -100, 0);
		}

	}

	protected void Control(){
		if (Input.GetMouseButtonDown (0)) {
			int x = Mathf.RoundToInt (retAdd.position.x);
			int y = Mathf.RoundToInt (retAdd.position.y);
			int z = Mathf.RoundToInt (retAdd.position.z);
			//chunk.SetBrick (x, y, z, activeBlockType);
			terrain.GetChunk(x, y, z).SetBrick (x, y, z, activeBlockType);
		}
		if (Input.GetMouseButtonDown (1)) {
			int x = Mathf.RoundToInt (retDel.position.x);
			int y = Mathf.RoundToInt (retDel.position.y);
			int z = Mathf.RoundToInt (retDel.position.z);
			//chunk.SetBrick (x, y, z, 0);
			terrain.GetChunk(x, y, z).SetBrick (x, y, z, 0);
		}

		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		if(wheel > 0){
			activeBlockType++;
			if (activeBlockType > 4)
				activeBlockType = 1;
		}
		else if (wheel < 0){
			activeBlockType--;
			if (activeBlockType < 1)
				activeBlockType = 4;
		}
	}
}
