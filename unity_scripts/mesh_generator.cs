using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class mesh_generator : MonoBehaviour {
    float width = 1;
    float height = 1;

    public bool autoUpdate;

    public Material mat;

    // Use this for initialization
    void Start()
    {
        generate_fractal();

    }

    public void generate_mesh()
    {
        generate_fractal();
    }

    public void printTriangle(Triangle t)
    {
        Debug.Log(t.pt1);
        Debug.Log(t.pt2);
        Debug.Log(t.pt3);
        Debug.Log("");
    }

    public class Triangle
    {
        public Vector3 pt1;
        public Vector3 pt2;
        public Vector3 pt3;

        public Triangle(Vector3 pt1, Vector3 pt2, Vector3 pt3)
        {
            this.pt1 = pt1;
            this.pt2 = pt2;
            this.pt3 = pt3;
        }
    }

    // ((x1,y1), (x2,y2))
    // ((x2, y2), (x1, y1))
    public class Pair
    {
        public Pair(Vector3 pt1, Vector3 pt2)
        {
            this.pt1 = pt1;
            this.pt2 = pt2;
        }

        public Vector3 pt1;
        public Vector3 pt2;

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Pair);
        }

        public bool Equals(Pair other)
        {
            // If parameter is null, return false.
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            bool compareOne = this.pt1 == other.pt1 && this.pt2 == other.pt2;
            bool compareTwo = this.pt1 == other.pt2 && this.pt2 == other.pt1;
            return compareOne || compareTwo;
        }

        public static bool operator ==(Pair lhs, Pair rhs)
        {
            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null))
            {
                Debug.Log("null comparison");
            }

            bool compareOne = lhs.pt1 == rhs.pt1 && lhs.pt2 == rhs.pt2;
            bool compareTwo = lhs.pt1 == rhs.pt2 && lhs.pt2 == rhs.pt1;
            return compareOne || compareTwo;
        }

        public static bool operator !=(Pair lhs, Pair rhs)
        {
            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null))
            {
                Debug.Log("null comparison");
            }

            bool compareOne = lhs.pt1 == rhs.pt1 && lhs.pt2 == rhs.pt2;
            bool compareTwo = lhs.pt1 == rhs.pt2 && lhs.pt2 == rhs.pt1;
            return !compareOne && !compareTwo;
        }

        public override int GetHashCode()
        {
            int hash_number = 31;
            int h1 = pt1.GetHashCode();
            int h2 = pt2.GetHashCode();
            return hash_number * (h1 + h2);
        }
    }

    public void clearDict()
    {
        pt_dict = new Dictionary<Pair, Vector3>();
    }

    Dictionary<Pair, Vector3> pt_dict = new Dictionary<Pair, Vector3>();
    public void generate_fractal()
    {
        clearDict();

        Vector3 input_pt1 = new Vector3(0, 0, 0);
        Vector3 input_pt2 = new Vector3(10, 20, 0);
        Vector3 input_pt3 = new Vector3(20, 0, 0);
        input_pts(new Pair(input_pt1, input_pt2));
        input_pts(new Pair(input_pt1, input_pt3));
        input_pts(new Pair(input_pt2, input_pt3));
        Triangle[] input = new Triangle[] { new Triangle(input_pt1, input_pt2, input_pt3) };

        Triangle[] output = fractal(input,4);
        for (int i = 0; i < output.Length; i++)
            printTriangle(output[i]);

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[output.Length*3];
        int vertex_index = 0;
        for(int i = 0; i < output.Length; i++)
        {
            vertices[vertex_index] = output[i].pt1;
            vertices[vertex_index + 1] = output[i].pt2;
            vertices[vertex_index + 2] = output[i].pt3;
            vertex_index += 3;
        }

        int[] triangles = new int[output.Length * 3];
        for(int i = 0; i < vertices.Length; i++)
            triangles[i] = i;

        int[] triangles_double = new int[output.Length * 3 * 2];
        for (int i = 0; i < vertices.Length; i++)
            triangles_double[i] = i;
        for(int i = 0; i < vertices.Length; i+=3)
        {
            triangles_double[i+vertices.Length] = i+1;
            triangles_double[i + vertices.Length + 1] = i;
            triangles_double[i + vertices.Length + 2] = i+2;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles_double;
        
        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<MeshRenderer>().material = mat;
    }

    public bool is_updated(Pair pt_pair)
    {
        Vector3 midpt = pt_dict[pt_pair];
        float midx = (pt_pair.pt1.x + pt_pair.pt2.x) / 2;
        float midy = (pt_pair.pt1.y + pt_pair.pt2.y) / 2;
        float midz = (pt_pair.pt1.z + pt_pair.pt2.z) / 2;
        Vector3 mid = new Vector3(midx, midy, midz);

        return !(midpt == mid);
    }

    // scale 2.8
    public void update_midpt(Pair pt_pair)
    {
        Vector3 midpt = pt_dict[pt_pair];
        float length = Vector3.Distance(pt_pair.pt1, pt_pair.pt2);
        float max_disp = length / (2.8f * Mathf.Sqrt(2));
        float y_disp = UnityEngine.Random.Range(-max_disp, max_disp);
        float z_disp = UnityEngine.Random.Range(0, max_disp);
        
        pt_dict[pt_pair] = new Vector3(midpt.x, midpt.y + y_disp, midpt.z + z_disp); 
    }

    public void input_midpt(Pair pt_pair, Vector3 midpt)
    {
        if (!pt_dict.ContainsKey(pt_pair))
            pt_dict[pt_pair] = midpt;
        else
        {
            pt_dict.Remove(pt_pair);
            pt_dict[pt_pair] = midpt;
        }
    }

    public void input_pts(Pair pt_pair)
    {
        float midx = (pt_pair.pt1.x + pt_pair.pt2.x) / 2;
        float midy = (pt_pair.pt1.y + pt_pair.pt2.y) / 2;
        float midz = (pt_pair.pt1.z + pt_pair.pt2.z) / 2;

        Vector3 midpt = new Vector3(midx, midy, midz);
        input_midpt(pt_pair, midpt);
    }

    public void input_triangle(Triangle t)
    {
        Vector3 pt1 = t.pt1;
        Vector3 pt2 = t.pt2;
        Vector3 pt3 = t.pt3;

        Pair pair12 = new Pair(pt1, pt2);
        Pair pair13 = new Pair(pt1, pt3);
        Pair pair23 = new Pair(pt2, pt3);

        input_pts(pair12);
        input_pts(pair13);
        input_pts(pair23);
    }

    public Triangle[] update_triangle(Triangle t)
    {
        Vector3 pt1 = t.pt1;
        Vector3 pt2 = t.pt2;
        Vector3 pt3 = t.pt3;

        Pair pair12 = new Pair(pt1, pt2);
        Pair pair13 = new Pair(pt1, pt3);
        Pair pair23 = new Pair(pt2, pt3);

        if (!is_updated(pair12))
            update_midpt(pair12);
        if (!is_updated(pair13))
            update_midpt(pair13);
        if (!is_updated(pair23))
            update_midpt(pair23);

        Triangle t1 = new Triangle(pt1, pt_dict[pair12], pt_dict[pair13]);
        Triangle t2 = new Triangle(pt2, pt_dict[pair12], pt_dict[pair23]);
        Triangle t3 = new Triangle(pt3, pt_dict[pair13], pt_dict[pair23]);
        Triangle t4 = new Triangle(pt_dict[pair12], pt_dict[pair13], pt_dict[pair23]);

        input_triangle(t1);
        input_triangle(t2);
        input_triangle(t3);
        input_triangle(t4);

        return new Triangle[] { t1, t2, t3, t4 };
    }

    public Triangle[] fractal(Triangle[] input, int iteration)
    {
        if (iteration == 0)
            return input;

        Triangle[] output = new Triangle[input.Length*4];
        int output_index = 0;
        for (int i = 0; i < input.Length; i++)
        {
            Triangle[] next = update_triangle(input[i]);
            for (int j = 0; j < next.Length; j++)
            {
                output[output_index] = next[j];
                output_index++;
            }
        }

        return fractal(output, iteration - 1);
    }
}
