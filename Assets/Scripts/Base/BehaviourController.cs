using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class BehaviourController : MonoBehaviour {

    public float speedMoveto;
    public float timeMoveUpdate;
    public Action _CallBackEndMove = null;
	
    protected void DetectCollistion()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.zero);
        if (hit.collider != null)
        {
            OnCollision(hit.collider);
        }
    }
    private void OnCompleMoveTo()
    {
        if (_CallBackEndMove != null) _CallBackEndMove();
    }

    protected abstract void OnCollision(Collider2D hit);
}
