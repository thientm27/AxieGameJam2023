using UnityEngine;

public class Layer : MonoBehaviour
{
    private Transform goTf;
    private void Awake()
    {
        goTf = transform;
    }
    private void Update()
    {
        if(goTf.position.y <= -21f)
        {
            SimplePool.Despawn(gameObject);
        }
    }
}
