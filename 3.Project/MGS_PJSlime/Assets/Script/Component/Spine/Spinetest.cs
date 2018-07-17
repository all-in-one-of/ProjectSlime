using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Spinetest : MonoBehaviour {

	[SpineAnimation]
	public string idleAnim;

	[SpineAnimation]
	public string openAnim;

	SkeletonAnimation skeletonAnimation;

	// Use this for initialization
	void Start () {
		skeletonAnimation = GetComponent<SkeletonAnimation>();
		skeletonAnimation.state.SetAnimation(0, "open", true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
