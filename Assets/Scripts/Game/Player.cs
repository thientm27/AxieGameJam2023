using UnityEngine;
using Spine;
using Spine.Unity;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private const float timeMoveUp = 0.5f;
    [SerializeField] private SkeletonAnimation model;
    [SerializeField] private Transform goTransform;
    [SerializeField] private float maxHeight = 10f;
    [SerializeField] private float minHeight = -10f;
    [SerializeField] private float speed = 18f;
    [SerializeField] private Vector2 limitWidth;

    // Cache
    private bool isUp = false;
    private bool canAttack = true;
    private Vector2 position;
    private float downSpeed = 2f;
    private void HitMonster()
    {
        canAttack = false;
        Up();
    }
    private void Update()
    {
        if (isUp == true) return;
        if(goTransform.position.y <= minHeight)
        {
            isUp = true;
            Up();
            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position = goTransform.position;
            position.x -= speed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, limitWidth.x, limitWidth.y);
            goTransform.position = position;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            position = goTransform.position;
            position.x += speed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, limitWidth.x, limitWidth.y);
            goTransform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow))
        {

        }
        position = goTransform.position;
        position.y -= downSpeed * Time.deltaTime;
        goTransform.position = position;
    }
    private void Up()
    {
        goTransform.DOMoveY(maxHeight, timeMoveUp).SetEase(Ease.Linear).OnComplete(() =>
        {
            isUp = false;
            canAttack = true;
        });
    }
}
