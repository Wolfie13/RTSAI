using UnityEngine;
using System;
using System.Collections.Generic;

public class MaterialSource : MonoBehaviour {
	public MatEntry[] materials;
	public TexEntry[] textures;

	public Material getMaterialByName(string name)
	{
		foreach (MatEntry m in materials)
		{
			if (name.Equals(m.name))
			{
				return m.material;
			}
		}
		
		return null;
	}
	
	[Serializable]
	public struct MatEntry
	{
		public string name;
		public Material material;
	}

	public Texture2D getTextureByName(string name)
	{
		foreach (TexEntry m in textures)
		{
			if (name.Equals(m.name))
			{
				return m.texture;
			}
		}
		
		return null;
	}
	
	[Serializable]
	public struct TexEntry
	{
		public string name;
		public Texture2D texture;
	}
}