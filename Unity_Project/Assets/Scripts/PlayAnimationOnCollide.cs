using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]

public class PlayAnimationOnCollide : MonoBehaviour {
	public delegate void AnimationPlayAction (string animationTrigger);
	public static event AnimationPlayAction OnAnimationPlay;

	Collider2D myCollider;
	Animator animator;

	public string AnimationTriggerName;

	// Use this for initialization
	void Start () {
		AssignReferences();
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		animator.SetTrigger(AnimationTriggerName);

		if (OnAnimationPlay != null) {
			OnAnimationPlay(AnimationTriggerName);
		}
	}

	void AssignReferences () {
		myCollider = GetComponent<Collider2D>();
		myCollider.isTrigger = true;

		animator = GetComponent<Animator>();
	}
}
