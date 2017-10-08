using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Es.InkPainter.Sample
{
    public class GazeGestureManager : MonoBehaviour
    {

        public Transform bullet;
        public Transform bulletStartPosition;
        public float bulletSpeed = 20.0f;

        public static GazeGestureManager Instance { get; private set; }

        static Color[] colors = { Color.white, Color.red, Color.blue, Color.green };
        int currColor = 0;

        // Represents the hologram that is currently being gazed at.
        public GameObject FocusedObject { get; private set; }

        GestureRecognizer recognizer;

        // Use this for initialization
        void Awake()
        {
            Instance = this;

            // Set up a GestureRecognizer to detect Select gestures.
            recognizer = new GestureRecognizer();
            recognizer.TappedEvent += (source, tapCount, ray) =>
            {
                // Send an OnSelect message to the focused object and its ancestors.
                if (FocusedObject != null)
                {
                    if (FocusedObject.tag.Equals("ColorChangeButton"))
                    {
                        currColor = (currColor + 1) % colors.Length;
                        FocusedObject.GetComponent<Renderer>().material.color = colors[currColor];
                    }
                    else
                    {
                        Transform b = (Transform)Instantiate(bullet, bulletStartPosition.position, Quaternion.identity);
                        b.GetComponent<Renderer>().material.color = colors[currColor];
                        b.GetComponent<Bullet>().brush.Color = colors[currColor];
                        Vector3 vector = ray.direction * bulletSpeed;
                        b.GetComponent<Rigidbody>().velocity = vector;
                    }
                }
            };
            recognizer.StartCapturingGestures();
        }

        // Update is called once per frame
        void Update()
        {
            // Figure out which hologram is focused this frame.
            GameObject oldFocusObject = FocusedObject;

            // Do a raycast into the world based on the user's
            // head position and orientation.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                // If the raycast hit a hologram, use that as the focused object.
                FocusedObject = hitInfo.collider.gameObject;
            }
            else
            {
                // If the raycast did not hit a hologram, clear the focused object.
                FocusedObject = null;
            }

            // If the focused object changed this frame,
            // start detecting fresh gestures again.
            if (FocusedObject != oldFocusObject)
            {
                recognizer.CancelGestures();
                recognizer.StartCapturingGestures();
            }
        }
    }
}