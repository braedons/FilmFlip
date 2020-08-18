using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
    private Vector3 prevPos = Vector3.zero;
    private Rigidbody2D target = null;
    public bool shouldMovePlayer = false;
    
    void Start() {
        
    }

    void LateUpdate() {
        if (target != null && shouldMovePlayer && IsNewPosition()) {
            Vector3 offset = transform.position - prevPos;
            target.MovePosition(target.transform.position + offset);
        }

        prevPos = transform.position;
    }

    private bool IsNewPosition() {
        Vector3 change = transform.position - prevPos;
        return change != Vector3.zero;//Mathf.Abs(change.x) > .01 || Mathf.Abs(change.y) > 0.1;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            // other.collider.transform.SetParent(transform);
            target = other.gameObject.GetComponent<Rigidbody2D>();
        }
    }
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            // other.collider.transform.SetParent(null);
            target = null;
        }
    }
}
