using UnityEngine;
using System.Collections;

public class MapChunk : MonoBehaviour {
	public Map parent;
	public int chunkX, chunkY;
	private bool dirty = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (dirty) {
			Generate();
			dirty = false;
		}
	}

	public void Position() {
		this.transform.position = new Vector3 (chunkX * 32 * 2.5f, 0, chunkY * 32 * 2.5f);
	}

	public void Generate()	{
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
	}

	private const float TILE_SIZE = 2.5f;

	private static void Tri(MeshBuilder meshBuilder)
	{
		int baseIndex = meshBuilder.Vertices.Count;
		meshBuilder.AddTriangle (baseIndex, baseIndex + 1, baseIndex + 2);
		meshBuilder.AddTriangle (baseIndex, baseIndex + 2, baseIndex + 3);
	}

	private static void BuildTile(MeshBuilder meshBuilder, Vector3 offset, char tile)
	{



		if (tile != '@') {
			//Index of vertex 0 for this tile.
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);

		} else {
			//Top layer
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);

			//0-1 side
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);

			//1-2 side
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 0.0f));
			meshBuilder.Normals.Add (Vector3.up);


			//2-3 side
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, TILE_SIZE) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);


			//3-0 side
			Tri(meshBuilder);
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 0.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (0.0f, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (1.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);
			
			meshBuilder.Vertices.Add (new Vector3 (TILE_SIZE, 1.0f, 0.0f) + offset);
			meshBuilder.UVs.Add (new Vector2 (0.0f, 1.0f));
			meshBuilder.Normals.Add (Vector3.up);

		}

		
	}
}
