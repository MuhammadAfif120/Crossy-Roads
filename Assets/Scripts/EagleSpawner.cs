using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{

    [SerializeField] CursedEagle cursedEagle;
    [SerializeField] Chicken chicken;
    [SerializeField] float initialTimer = 10;

    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = initialTimer;
        cursedEagle.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0 && cursedEagle.gameObject.activeInHierarchy == false)
        {
            cursedEagle.gameObject.SetActive(true);
            cursedEagle.transform.position = chicken.transform.position + new Vector3(0, 0, 13);
            chicken.setMoveable(false);
        }

        timer -= Time.deltaTime;
    }

    public void ResetTimer()
    {
        timer = initialTimer;
    }
}
