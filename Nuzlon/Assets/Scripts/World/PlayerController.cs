using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed, _encounterRate;
    [SerializeField]
    private LayerMask _solidObjectLayer, _encounterLayer;

    private bool _isMoving;
    private Vector2 _input;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(!_isMoving)
        {
            //getting input
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");

            //(hopefully remove diagonal movement
            if (Mathf.Abs( Input.GetAxis("Horizontal")) > Mathf.Abs( Input.GetAxis("Vertical")))
            {
                _input.y = 0;
            }else //if(Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(Input.GetAxis("Vertical")))
            {
                _input.x = 0;
            }

            if (_input!=Vector2.zero)
            {
                _animator.SetFloat("MoveX", _input.x);
                _animator.SetFloat("MoveY", _input.y);

                //setting target tile for movement
                Vector2 targetPos = transform.position;
                targetPos.x += _input.x;
                targetPos.y += _input.y;

                if(IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        _animator.SetBool("IsMoving", _isMoving);
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        //bool stops double input
        _isMoving = true;

        //moving coroutine
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _moveSpeed * Time.deltaTime);
            yield return null;
        }
        //setting things straight with the small numbers
        transform.position = targetPos;

        //ready for new input
        _isMoving = false;

        CheckForEncounters();
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.3f, _encounterLayer)!=null)
        {
            if(Random.Range(0,100)<_encounterRate)
            {
                Debug.Log("RANDOM ENCOUNTER");
            }
        }
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.3f, _solidObjectLayer))
        {
            return false;
        }
        return true;
    }
}
