using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using GemMine.EasyEasing;

namespace GemMine.WordSearch {

	// This script is attaches to the PanelAlert, to find in the UI Canvas
	// it realizes a red flashing (for the last ten seconds)

	public class alertPanel : MonoBehaviour {

		// the flashing consists of two pictures
		// one on the upper screen
		Image upperImage;
		// and one on the lower screen
		Image lowerImage;
		// just to make sure I do not overwrite the color
		Color imageColor;



		// Use this for initialization
		void Start () {
			// get a reference to the upper Image
			upperImage = transform.Find ("ImageAlert1").GetComponent<Image> ();
			// get a reference to the lower Image
			lowerImage = transform.Find ("ImageAlert2").GetComponent<Image> ();
			// set the starting color
			imageColor = new Color (1,1,1,1);
		}



		//
		// public void flash()
		//
		// This method gets called from the outside and flashes the images
		// via GemMine's tweening library
		//

		public void flash() {
			// set the original color
			upperImage.color = imageColor;
			lowerImage.color = imageColor;
			// activate the two pictures
			upperImage.gameObject.SetActive (true);
			lowerImage.gameObject.SetActive (true);

			// tween the color value (changing alpha)
			easyEasing.ColorTo (upperImage.gameObject,
				easyEasing.Params ("easeType", "easeOutQuad",
					"from", new Color(1,1,1,1),
					"to", new Color(1,1,1,0),
					"duration", 0.7f,
					"onUpdateTarget", this.gameObject,
					"onUpdate", "colorUpdate",
					"onCompleteTarget", this.gameObject,
					"onComplete", "colorUpdateComplete"));
		}


		//
		// public void colorUpdate(Color color) 
		//
		// This method is called during the tween updates
		// and sets the actual color
		//

		public void colorUpdate(Color color) {
			upperImage.color = color;
			lowerImage.color = color;
		}


		//
		// public void colorUpdateComplete()
		//
		// This method is called after the tween is complete
		// it deactivates the images and resets the color
		//

		public void colorUpdateComplete() {
			upperImage.gameObject.SetActive (false);
			lowerImage.gameObject.SetActive (false);
			upperImage.color = imageColor;
			lowerImage.color = imageColor;

		}
	}
}
