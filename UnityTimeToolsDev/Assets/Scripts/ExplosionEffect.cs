using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{

    public GameObject particlesPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayParticlesAt(Vector3 position)
    {
        GameObject clone = Instantiate(particlesPrefab, position, Quaternion.identity);
        foreach (var child in clone.GetComponentsInChildren<ParticleSystem>())
        {
            child.Play();
        }

    }
}
