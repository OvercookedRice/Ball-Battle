using System.Collections;
using System.Linq;
using UnityEngine;

// This class is referenced from https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html 

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMesh : MonoBehaviour
{
    void Start()
    {
        Vector3 original_position = transform.position;
        Quaternion original_rotation = transform.rotation;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        meshFilters = meshFilters.Where(x => x.sharedMesh != null).ToArray();

        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);

        transform.position = original_position;
        transform.rotation = original_rotation;
    }
}
