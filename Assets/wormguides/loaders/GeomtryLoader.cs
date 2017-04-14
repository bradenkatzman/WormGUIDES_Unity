using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeometryLoader {

	private static string OBJ_EXT = ".obj";
	private static string VERTEX_LINE = "v ";
	private static string FACE_LINE = "f ";
	private static string SLASH = "\\";
	private static string T = "_t";
	private static string ASSETS = "Assets";

	/*
	 * 
	 */
	public static int getEffectiveStartTime(
		string resourcePath,
		int startTime,
		int endTime) {

		resourcePath = SLASH + resourcePath;

		string FilePath = Directory.GetCurrentDirectory () + ASSETS + SLASH + resourcePath;
		if (File.Exists (FilePath)) {
			// check for obj file with a time
			for (int time = startTime; time <= endTime; time++) {
				string url = Directory.GetCurrentDirectory() + SLASH + resourcePath + T + time.ToString() + OBJ_EXT;

				if (File.Exists(url)) {
					return time;
				}
			}
		}

		return Int32.MinValue;
	}

	public static GameObject loadObj(string resourcePath) {
		GameObject go = null;
		string filePath = Directory.GetCurrentDirectory() + SLASH + ASSETS + SLASH + resourcePath + OBJ_EXT;

		List<float[]> coords = new List<float[]> ();
		List<int[]> faces = new List<int[]> ();

		if (File.Exists (filePath)) {
			using (var fs = File.OpenRead (filePath))
			using (var reader = new StreamReader (fs)) {
				string line;
				string v;
				string f;
				string lineType;
				while (!reader.EndOfStream) {
					line = reader.ReadLine ();

					if (line.Length > 3) {
						lineType = line.Substring (0, 2);
						if (lineType.ToLower ().Equals (VERTEX_LINE.ToLower ())) {
							v = line.Substring (2).Trim ();
							float[] vertices = new float[3];
							int counter = 0;
							string[] tokens = v.Split (' ');
							for (int i = 0; i < tokens.Length; i++) {
								vertices [counter++] = float.Parse (tokens [i]);
							}

							// make sure good line
							if (counter == 3) {
								coords.Add (vertices);
							}
						} else if (lineType.ToLower ().Equals (FACE_LINE.ToLower ())) {
							f = line.Substring (2).Trim ();
							int[] faceCoords = new int[3];
							int counter = 0;
							string[] tokens = f.Split (' ');
							for (int i = 0; i < tokens.Length; i++) {
								faceCoords [counter++] = (int)Int32.Parse (tokens [i]);
							}

							// make sure good lin
							if (counter == 3) {
								faces.Add (faceCoords);
							}
						}
					}
				} // end while
					

				go = createMesh (coords, faces);
				MeshRenderer mr = go.AddComponent<MeshRenderer> ();
				mr.material = new Material (Shader.Find ("Diffuse"));
				mr.material.shader = Shader.Find ("Diffuse");
			} // end reader
		}

		return go;
	}

	private static GameObject createMesh(List<float[]> verts, List<int[]> faces) {
		GameObject go = new GameObject ();
		Mesh mesh = new Mesh ();
		MeshFilter mf = go.AddComponent<MeshFilter> ();
		mf.sharedMesh = mesh;
		int counter = 0;
		int texCounter = 0;
		float stripeSeparation = 1500;
		Vector3[] vertices = new Vector3[(verts.Count)];
		Vector2[] texVertices = new Vector2[(verts.Count)];
		foreach (float[] coord in verts) {
			if (coord.Length == 3) {
				float x, y, z;
				x = coord [0];
				y = coord [1];
				z = coord [2];
				Vector3 vec = new Vector3 (x, y, z);
				vertices [counter++] = vec;

				Vector2 texVert = new Vector2(
					0f,
					((float) coord[0] / stripeSeparation) * 200f);
				texVertices [texCounter++] = texVert;
			}
		}

		mesh.vertices = vertices;
		mesh.uv = texVertices;

		counter = 0;
		int[] triangles = new int[(faces.Count * 3) * 2];
		foreach (int[] face in faces) {
			for (int j = 0; j < 3; j++) {
				triangles[counter++] = face[j] - 1;
				triangles[counter++] = face[j] - 1;
			}
		}
		mesh.triangles = triangles;
		return go;
	}
}
