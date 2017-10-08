using UnityEngine;

namespace Es.InkPainter.Sample
{
	public class MousePainter : MonoBehaviour
	{
		public Transform bullet;
		public Transform bulletStartPosition;
		public float bulletSpeed = 2.0f;

		/// <summary>
		/// Types of methods used to paint.
		/// </summary>
		[System.Serializable]
		private enum UseMethodType
		{
			RaycastHitInfo,
			WorldPoint,
			NearestSurfacePoint,
			DirectUV,
		}

		[SerializeField]
		private Brush brush;

		[SerializeField]
		private UseMethodType useMethodType = UseMethodType.RaycastHitInfo;

		private bool mouseDown = false;
		private void Update()
		{
			if(Input.GetMouseButton(0) && !mouseDown)
			{
				mouseDown = true;
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				bool success = true;
				RaycastHit hitInfo;
				if(Physics.Raycast(ray, out hitInfo))
				{
					var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
					if (paintObject != null) {
						Transform b = (Transform) Instantiate (bullet, bulletStartPosition.position, Quaternion.identity);
						Vector3 vector = b.position - hitInfo.point;
						vector.Normalize ();
						vector = vector * -bulletSpeed;
						b.GetComponent<Rigidbody> ().velocity = vector;
					}
					if(!success)
						Debug.LogError("Failed to paint.");
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				mouseDown = false;
			}
		}

		/*
		public Vector3 velocityToDest(Vector3 start, Vector3 end, float angle) {
			float x = start.x - end.x;
			float y = start.y - end.y;
			float z = start.z - end.z;
			float v = Mathf.Sqrt ((x*x*(-9.81))/((x*Mathf.Sin(2*angle))-(2*y*Mathf.Cos(angle))));
			return new Vector3 (v*Mathf.Cos(angle)*(x/));
		}*/

		public void OnGUI()
		{
			if(GUILayout.Button("Reset"))
			{
				foreach(var canvas in FindObjectsOfType<InkCanvas>())
					canvas.ResetPaint();
			}
		}
	}
}