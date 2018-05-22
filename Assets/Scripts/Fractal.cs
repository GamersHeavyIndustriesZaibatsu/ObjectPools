using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;
    public int maxDepth;
    private int depth;
    public float childScale;
    public float waitSeconds;

    public Color innerColor1;
    public Color outerColor1;
    public Color innerColor2;
    public Color outerColor2;
    public Color edgeColor1;
    public Color edgeColor2;

    private static Vector3[] childDirections =
    {
        Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private Material[,] materials;

    private void InitializeMaterials()
    {
        materials = new Material[maxDepth + 1, 2];
        for (int i = 0; i <= maxDepth; i++)
        {
            float t = i / (maxDepth - 1f);
            t *= t;

            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(innerColor1, outerColor1, t);
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(innerColor2, outerColor2, t);
        }
        materials[maxDepth, 0].color = edgeColor1;
        materials[maxDepth, 1].color = edgeColor2;
    }

    private void Start()
    {
        //new WaitForSeconds(60f);

        if(materials == null)
        {
            InitializeMaterials();
        }

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        GetComponent<MeshRenderer>().material = materials[depth, Random.Range(0,2)];

        if (depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    private IEnumerator CreateChildren()
    {
        waitSeconds = Random.Range(0.01f, 1f);

        for (int i = 0; i < childDirections.Length; i++)
        {
            yield return new WaitForSeconds(waitSeconds);
            //yield return new WaitForSeconds(0.1f);
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }

    }

    private void Initialize(Fractal parent, int childIndex)
    {
        mesh = parent.mesh;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
        //waitSeconds = parent.waitSeconds / 2f;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }
}
