using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrain : MonoBehaviour {

	public int seed = 0;

	public int chunkSize = 20;
	public int viewSize = 40;

	public Chunk chunkFab;

	public List<Chunk> chunks = new List<Chunk>();

	// Use this for initialization
	void Start () {
		if (seed == 0) {
			seed = Mathf.FloorToInt (Random.value * int.MaxValue / 100);
		}

		chunks.Add((Chunk)Instantiate (chunkFab));

	}
	
	// Update is called once per frame
	void Update () {
		CreateChunksIfWeNeedTo ();
	}

	public bool ChunkExists(float x, float z) {
		for (int a = 0; a < chunks.Count; a++) {
			if( (x < chunks[a].transform.position.x) ||
				(z < chunks[a].transform.position.z) ||
				(x >= chunks[a].transform.position.x + chunkSize) ||
				(z >= chunks[a].transform.position.z + chunkSize))
				continue;
			return true;
		}
		return false;
	}

	void CreateChunksIfWeNeedTo() {
		for (float x = transform.position.x - viewSize; x <= transform.position.x + viewSize; x+=chunkSize) {
			for (float z = transform.position.z - viewSize; z <= transform.position.z + viewSize; z+=chunkSize) {

				if(ChunkExists(x, z))continue;

				int chunkX = Mathf.FloorToInt (x / chunkSize) * chunkSize;
				int chunkZ = Mathf.FloorToInt (z / chunkSize) * chunkSize;

				chunks.Add ((Chunk) Instantiate(chunkFab, new Vector3(chunkX, 0, chunkZ), Quaternion.identity));
			}
		}
	}

	public Chunk GetChunk(int x, int y, int z) {

		for (int a = 0; a < chunks.Count; a++) {
			if( (x < chunks[a].transform.position.x) ||
				(z < chunks[a].transform.position.z) ||
				(x >= chunks[a].transform.position.x + chunkSize) ||
				(z >= chunks[a].transform.position.z + chunkSize))
				continue;
			return chunks[a];
		}
		return null;

	}
}
