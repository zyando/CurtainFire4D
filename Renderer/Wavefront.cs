using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VecMath;

namespace CurtainFireMaker.Renderer
{
    public class Wavefront
    {
        public Vector3[] Vertices { get; }
        public Vector3[] Normals { get; }
        public Vector2[] Texcoords { get; }
        public int[] Indices { get; }

        public Wavefront(StreamReader reader)
        {
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var texcoords = new List<Vector2>();

            var vertexIndices = new Dictionary<(int, int, int), int>();
            var faces = new List<int>();

            while (reader.Peek() >= 0)
            {
                string line = reader.ReadLine();

                string[] tokens = line.Split(' ');
                switch (tokens[0])
                {
                    case "v":
                        vertices.Add(ParseVec3(tokens));
                        break;

                    case "vn":
                        normals.Add(ParseVec3(tokens));
                        break;

                    case "vt":
                        texcoords.Add(ParseVec2(tokens));
                        break;

                    case "f":
                        foreach (var token in tokens.Skip(1))
                        {
                            var index = ParseFace(token);
                            int faceIndex;

                            if (!vertexIndices.ContainsKey(index))
                            {
                                faceIndex = vertexIndices[index] = vertexIndices.Count;
                            }
                            else
                            {
                                faceIndex = vertexIndices[index];
                            }
                            faces.Add(faceIndex);
                        }
                        break;
                }
            }

            Indices = faces.ToArray();

            Vertices = new Vector3[vertexIndices.Count];
            Normals = new Vector3[vertexIndices.Count];
            Texcoords = new Vector2[vertexIndices.Count];

            foreach (var pair in vertexIndices)
            {
                Vertices[pair.Value] = vertices[pair.Key.Item1];
                Normals[pair.Value] = normals[pair.Key.Item3];
                Texcoords[pair.Value] = pair.Key.Item2 != -1 ? texcoords[pair.Key.Item2] : Vector2.Zero;
            }

            Vector3 ParseVec3(string[] tokens)
            {
                return new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3]));
            }

            Vector2 ParseVec2(string[] tokens)
            {
                return new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2]));
            }

            (int, int, int) ParseFace(string token)
            {
                var splited = token.Split('/').Select(s => s != "" ? s : "0").ToArray();
                return (int.Parse(splited[0]) - 1, int.Parse(splited[1]) - 1, int.Parse(splited[2]) - 1);
            }
        }

        public void Render()
        {

        }
    }
}
