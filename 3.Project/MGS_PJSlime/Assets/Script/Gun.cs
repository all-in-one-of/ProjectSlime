
using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	float a = 0.3f;
	Vector2 op;

	void Start(){
		op = transform.localPosition;
	}

	void Update (){
		if(transform.localPosition.y < 0){
			transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + 13 * Time.deltaTime);
		}else{
			transform.localPosition = op;
		}
			
	}
}
