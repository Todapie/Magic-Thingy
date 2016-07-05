using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshCollider))]

public class Chunk : MonoBehaviour {
	int seed;

	public float detailScale = 25f;
	public float heightScale = 20;

	public int width = 20;
	public byte[,,] map;

	protected Mesh mesh;

	protected List<Vector3> verts = new List<Vector3>();
	protected List<int> tris = new List<int>();
	protected List<Vector2> uv = new List<Vector2>();

	protected MeshCollider meshCollider;

	void Start () {
		seed = Camera.main.GetComponent<Terrain> ().seed;

		meshCollider = GetComponent<MeshCollider> ();
		map = new byte[width, width, width];

		int startX = Mathf.FloorToInt(transform.position.x);
		int startZ = Mathf.FloorToInt(transform.position.z);

		for (int x = startX; x < width + startX; x++) 
		{
			for (int z = startZ; z < width + startZ; z++)
			{
				int y = (int)(Mathf.PerlinNoise ((x + seed) / detailScale, (z + seed) / detailScale) * heightScale);
				setHeight (x - startX, z - startZ, y);
			}
		}

		mesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = mesh;

		Regenrate ();
	}
	
	public void setHeight(int x, int z, int height) {

		for (int i = 0; i <= height; i++) {
			map [x, i, z] = 1;
		}

	}

	void Update () {
	
	}

	public void DrawBrick(int x, int y, int z, byte block)
	{
		Vector3 start = new Vector3 (x, y, z);
		Vector3 offset1, offset2;

		if(isTransparent(x, y - 1, z))
		{
			offset1 = Vector3.left;
			offset2 = Vector3.back;
			DrawFace(start + Vector3.right, offset1, offset2, block);
		}

		if(isTransparent(x, y + 1, z))
		{
			offset1 = Vector3.right;
			offset2 = Vector3.back;
			DrawFace(start + Vector3.up, offset1, offset2, block);
		}

		if(isTransparent(x - 1, y, z))
		{
			offset1 = Vector3.up;
			offset2 = Vector3.back;
			DrawFace(start, offset1, offset2, block);
		}

		if(isTransparent(x + 1, y, z))
		{
			offset1 = Vector3.down;
			offset2 = Vector3.back;
			DrawFace(start + Vector3.right + Vector3.up, offset1, offset2, block);
		}

		if(isTransparent(x, y, z - 1))
		{
			offset1 = Vector3.left;
			offset2 = Vector3.up;
			DrawFace(start + Vector3.right + Vector3.back, offset1, offset2, block);
		}

		if(isTransparent(x, y, z + 1))
		{
			offset1 = Vector3.right;
			offset2 = Vector3.up;
			DrawFace(start, offset1, offset2, block);
		}
	}

	public void DrawFace(Vector3 start, Vector3 offset1, Vector3 offset2, byte block)
	{
		int index = verts.Count;

		verts.Add (start);
		verts.Add (start + offset1);
		verts.Add (start + offset2);
		verts.Add (start + offset1 + offset2);

		Vector2 uvBase;
		switch (block) {
		default:
			uvBase = new Vector2 (0.25f, 0.25f);
			break;
		case 2:
			uvBase = new Vector2 (0.75f, 0.75f);
			break;
		case 3:
			uvBase = new Vector2 (0.25f, 0.75f);
			break;
		case 4:
			uvBase = new Vector2 (0.75f, 0.25f);
			break;
		}

		if ((offset1 == Vector3.right) && (offset2 == Vector3.back)) {
			uv.Add (uvBase);
			uv.Add (uvBase + new Vector2(0.125f, 0));
			uv.Add (uvBase + new Vector2(0, 0.125f));
			uv.Add (uvBase + new Vector2(0.125f, 0.125f));
		}
		else {
			uv.Add (uvBase);
			uv.Add (uvBase + new Vector2(-0.125f, 0));
			uv.Add (uvBase + new Vector2(0, 0.125f));
			uv.Add (uvBase + new Vector2(-0.125f, 0.125f));
		}

		tris.Add (index + 0);
		tris.Add (index + 1);
		tris.Add (index + 2);
		tris.Add (index + 3);
		tris.Add (index + 2);
		tris.Add (index + 1);
	}

	public bool isTransparent(int x, int y, int z)
	{
		if((x < 0) || (y < 0) || (z < 0) || (x >= width) || (y >= width) || (z >= width))return true;
		return map [x, y, z] == 0;
	}

	public void Regenrate() 
	{
		verts.Clear ();
		tris.Clear ();
		uv.Clear ();
		mesh.triangles = tris.ToArray();

		for (int x = 0; x < width; x++) 
		{
			for (int y = 0; y < width; y++)
			{
				for (int z = 0; z < width; z++)
				{
					byte block = map [x, y, z];
					if (block == 0)continue;

					DrawBrick (x, y, z, block);
				}	
			}
		}

		mesh.vertices = verts.ToArray ();
		mesh.triangles = tris.ToArray ();
		mesh.uv = uv.ToArray ();
		mesh.RecalculateNormals ();

		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = mesh;
	}

	public void SetBrick(int x, int y, int z, byte block) {

		x -= Mathf.RoundToInt (transform.position.x);
		y -= Mathf.RoundToInt (transform.position.y);
		z -= Mathf.RoundToInt (transform.position.z);

		if((x < 0) || (y < 0) || (z < 0) || (x >= width) || (y >= width) || (z >= width))return;

		if(map[x, y, z] != block) {
			map [x, y, z] = block;
			Regenrate ();
		}
	}
}
