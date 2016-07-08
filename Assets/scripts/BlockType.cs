using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class BlockType
{
	public String name = "?";
	public int textureRight = 0, textureDown = 0;

	public bool transparent = false;
	public bool seperateCollisionMesh = false;

	public Vector2 UVCoord() {
		Vector2 uv = new Vector2 (textureRight, textureDown * 2);

		float percent = 64f / 1024f;
		uv *= percent;
		uv.y = 1 - uv.y;

		return uv;
	}

}

