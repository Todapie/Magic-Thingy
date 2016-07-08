using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockList : MonoBehaviour {

	public List<BlockType> blocks = new List<BlockType> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetCount() {
		return blocks.Count;
	}

	public BlockType GetBlock(String name) {
		for (int a = 0; a < blocks.Count; a++) {
			if (blocks [a].name == name)
				return blocks [a];
		}
		return blocks [0];
	}

	public BlockType GetBlock(byte num) {
		return blocks [num - 1];
	}

	public byte GetBlockByte(String name) {
		for (int a = 0; a < blocks.Count; a++) {
			if (blocks [a].name == name)
				return (byte)(a + 1);
		}
		return 1;
	}
}
