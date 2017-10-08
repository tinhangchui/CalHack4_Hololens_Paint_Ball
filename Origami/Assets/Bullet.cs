using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Es.InkPainter.Sample
{

	public class Bullet : MonoBehaviour {

		[SerializeField]
		public Brush brush;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnCollisionEnter(Collision col) {
			foreach (ContactPoint contact in col.contacts)
			{
				var paintObject = col.collider.GetComponent<InkCanvas>();
				paintObject.Paint (brush, contact.point);
				Destroy (gameObject);
				break;
			}
		}
	}

}