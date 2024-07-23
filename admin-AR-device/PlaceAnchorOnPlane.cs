using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]

    public class PlaceAnchorOnPlane : MonoBehaviour
    {
        [SerializeField]
        GameObject m_Prefab;

        public GameObject prefab
        {
            get => m_Prefab;
            set => m_Prefab = value;
        }

        public GameObject _camera;

        private Dictionary<string, string> AnchorDict = new Dictionary<string, string>();

        public void RemoveAllAnchors()
        {
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
        }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
        }

        void SetAnchorText(ARAnchor anchor, string text)
        {
            var canvasTextManager = anchor.GetComponent<CanvasTextManager>();
            if (canvasTextManager)
            {
                canvasTextManager.text = text;
            }
        }

        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    Logger.Log("Creating anchor attachment.");
                    var oldPrefab = m_AnchorManager.anchorPrefab;
                    m_AnchorManager.anchorPrefab = prefab;
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    m_AnchorManager.anchorPrefab = oldPrefab;
                    return anchor;
                }
            }

            // Otherwise, just create a regular anchor at the hit pose
            Logger.Log("Creating regular anchor.");

            // Note: the anchor can be anywhere in the scene hierarchy
            var gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);

            // Make sure the new GameObject has an ARAnchor component
            anchor = gameObject.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = gameObject.AddComponent<ARAnchor>();
            }
            return anchor;
        }

        void Update()
        {
        }

        public void PlaceAnchorA()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "A");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorB()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "B");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorC()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "C");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorD()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "D");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorE()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "E");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorF()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "F");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorG()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "G");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorH()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "H");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorI()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "I");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void PlaceAnchorJ()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (m_RaycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor
                var anchor = CreateAnchor(hit);
                if (anchor)
                {
                    // Remember the anchor so we can remove it later.
                    m_Anchors.Add(anchor);
                    // Add anchor to dictionary
                    AnchorDict.Add(anchor.name, "J");
                }
                else
                {
                    Logger.Log("Error creating anchor");
                }
            }
        }

        public void SaveAnchorDict()
        {
            //write anchor dictionary to file
            string filepath_dict = Application.persistentDataPath + "/anchordict.txt";
            using (StreamWriter sw = new StreamWriter(filepath_dict))
            {
                foreach (KeyValuePair<string, string> a in AnchorDict)
                {
                    sw.WriteLine(a.Key + "," + a.Value);
                }
            }
            StartCoroutine(UploadAnchorDict());
        }

        IEnumerator UploadAnchorDict()
        {
            if (!File.Exists(Application.persistentDataPath + "/anchordict.txt"))
            {
                Debug.Log("anchorDict doesn't exist");
            }

            WWWForm form = new WWWForm();
            form.AddBinaryData("anchordict", File.ReadAllBytes(Application.persistentDataPath + "/anchordict.txt"), "anchordict.txt", "text/csv");
            string url_anchordict = "http://" + (insert the IP address of your server) + ":8000/anchordict";
            UnityWebRequest www = UnityWebRequest.Post(url_anchordict, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Upload complete!");
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
