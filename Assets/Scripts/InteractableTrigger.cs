using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour {
    public enum InteractionFunction {
        SetNotKinematic
    }

    [System.Serializable]
    public struct Interaction {
        public GameObject target;
        public InteractionFunction action;
    }

    public Interaction[] interactions;

    private bool isTriggered = false;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.CompareTag("Player") || other.CompareTag("Pullable")) && !isTriggered) {
            isTriggered = true;
            animator.SetBool("Triggered", isTriggered);

            foreach (Interaction interaction in interactions) {
                switch (interaction.action) {
                    case InteractionFunction.SetNotKinematic:
                        SetNotKinematic(interaction.target);
                        break;
                }
            }
        }
    }

    private void SetNotKinematic(GameObject target) {
        target.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
