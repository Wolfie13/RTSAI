﻿using UnityEngine;
using System.Collections;

public class MapChunk : MonoBehaviour {
    public const float TILE_SIZE = 2.5f;

	public void Generate(int chunkX, int chunkY, Map parent)	{
		MeshFilter filter = this.gameObject.GetComponent<MeshFilter> ();
		if (filter == null) {
			filter = this.gameObject.AddComponent<MeshFilter> ();
		}

		MeshRenderer rend = this.gameObject.GetComponent<MeshRenderer> ();
		if (rend == null) {
			rend = this.gameObject.AddComponent<MeshRenderer> ();
		}

		rend.material = parent.mapMaterial;

		MeshBuilder meshBuilder = new MeshBuilder();

		int startX = chunkX * 32;
		int startY = chunkY * 32;
		for (int i = startX; i < startX + 32; i++) {
			float xPos = TILE_SIZE * (i - startX);
			for (int j = startY; j < startY + 32; j++) {
				float yPos = TILE_SIZE * (j - startY);
				BuildTile(meshBuilder, new Vector3(xPos, 0, yPos), parent.getTile(i, j));
			}
		}

		this.GetComponent<MeshFilter> ().sharedMesh = meshBuilder.CreateMesh ();
		this.transform.position = new Vector3(chunkX * Map.ChunkSize * TILE_SIZE, 0, chunkY * Map.ChunkSize * TILE_SIZE);
	}

	

	private static void Tri(MeshBuilder meshBuilder)
	{
		int baseIndex = meshBuilder.Vertices.Count;
		meshBuilder.AddTriangle (baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle (baseIndex, baseIndex + 2, baseIndex + 3);
	}
	private static void BuildTile(MeshBuilder meshBuilder, Vector3 offset, char tile)
	{
		float[] grass = new float[] {0, 0.5f, 0.5f, 1};
		float[] stone = new float[] {0.5f, 0.5f, 1, 1};
        float[] tree = new float[] { 0, 0, 0.5f, 0.5f };

        if (Map.Terrain.Contains(tile))
        {
			//Index of vertex 0 for this tile.
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (grass[0], grass[1]));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (grass[0], grass[3]));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (grass[2], grass[3]));
			meshBuilder.Normals.Add (Vector3.up);

			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (grass[2], grass[1]));
			meshBuilder.Normals.Add (Vector3.up);

		}
        else if (Map.Trees.Contains(tile))
        {
            Tri(meshBuilder);
            meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + offset);
            meshBuilder.UVs.Add(new Vector2(tree[0], tree[1]));
            meshBuilder.Normals.Add(Vector3.up);

            meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, TILE_SIZE) + offset);
            meshBuilder.UVs.Add(new Vector2(tree[0], tree[3]));
            meshBuilder.Normals.Add(Vector3.up);

            meshBuilder.Vertices.Add(new Vector3(TILE_SIZE, 0.0f, TILE_SIZE) + offset);
            meshBuilder.UVs.Add(new Vector2(tree[2], tree[3]));
            meshBuilder.Normals.Add(Vector3.up);

            meshBuilder.Vertices.Add(new Vector3(TILE_SIZE, 0.0f, 0.0f) + offset);
            meshBuilder.UVs.Add(new Vector2(tree[2], tree[1]));
            meshBuilder.Normals.Add(Vector3.up);
        }
        
        
        else {
			//Top layer
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (stone[0], stone[1]));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (stone[0], stone[3]));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (stone[2], stone[3]));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (stone[2], stone[1]));
			meshBuilder.Normals.Add (Vector3.up);

		}

		
	}
}
