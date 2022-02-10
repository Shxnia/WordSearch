using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GemMine.EasyEasing;

namespace GemMine.WordSearch {

	// if the user guesses one word,
	// this gameobject will be instantiated
	// it show the word floating up

	public class textSlider : MonoBehaviour {

		// Basically, this is a color and position tween
		Color textColor;

		// Use this for initialization
		void Start () {
			// get the color and set the alpha value
			textColor = this.GetComponent<Text> ().color;
			textColor.a = 1f;
			this.GetComponent<Text> ().color = textColor;

			// do the tween
			easyEasing.Vector2To (this.gameObject,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", GetComponent<RectTransform> ().anchoredPosition,
					"to", GetComponent<RectTransform> ().anchoredPosition + new Vector2(0,300),
					"duration", 2f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "moveTextUpdate",
					"onCompleteTarget", this.gameObject,
					"onComplete", "moveTextComplete"));
		}


		//
		// public void moveTextUpdate(Vector2 position)
		//
		// This method is called during the tween's update
		//

		public void moveTextUpdate(Vector2 position) {
			this.GetComponent<RectTransform> ().anchoredPosition = position;
			if (textColor.a > 0)
				textColor.a -= 0.01f;
			this.GetComponent<Text> ().color = textColor;
		}


		//
		// public void moveTextComplete() 
		//
		// This method is called when the tween is complete
		// it destroys the object
		//

		public void moveTextComplete() {
			Destroy (this.gameObject, 2f);
		}
	}
}
