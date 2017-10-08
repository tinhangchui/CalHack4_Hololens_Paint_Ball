using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{

    public Transform bullet;
    public Transform bulletStartPosition;
    public float bulletSpeed = 20.0f;

    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

	public Transform testObject;
	private Vector3 navigationStartPosition;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
		recognizer.NavigationStartedEvent += (InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay) => 
		{
			navigationStartPosition = testObject.position;
		};
		recognizer.NavigationUpdatedEvent += (InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay) => 
		{
			testObject.position = navigationStartPosition + normalizedOffset * 10f;
		};
		recognizer.NavigationCompletedEvent += (InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay) => 
		{
			testObject.position = navigationStartPosition;
		};
		recognizer.NavigationCanceledEvent += (InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay) => 
		{
			testObject.position = navigationStartPosition;
		};
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject != null)
            {   
                Transform b = (Transform)Instantiate(bullet, bulletStartPosition.position, Quaternion.identity);
                Vector3 vector = ray.direction * bulletSpeed;
                b.GetComponent<Rigidbody>().velocity = vector;
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