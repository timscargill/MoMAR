using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]

    public class MotionLog : MonoBehaviour
    {
        [SerializeField]
        ARPlaneManager m_ARPlaneManager;

        public GameObject _camera;
        public Camera _cam;
        private List<string> poses = new List<string>();
        private List<Ray> rays = new List<Ray>();
        private List<Vector3> relativeEnvHits = new List<Vector3>();
        private List<string> motionLogOutput = new List<string>();
        public Text log;
        private int frame_count = 0;
        private string savePath;
        private string savePath_timing;
        public Text viewer_output;

        void Awake()
        {
            m_ARPlaneManager = GetComponent<ARPlaneManager>();
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        void Start()
        {
            device_id = SystemInfo.deviceUniqueIdentifier;
            savePath = Application.persistentDataPath + "/motionlog.txt";
        }

        void Update()
        {
            // Once content is loaded start logging
            if (RenderAnchorContent.anchorContent_loaded == true && Quiz.quiz_finished == false)
            {
                //Get device pose
                var positionString = _camera.transform.position.x.ToString("F2") + "," + _camera.transform.position.y.ToString("F2") + "," + _camera.transform.position.z.ToString("F2");
                var rotationString = _camera.transform.rotation.x.ToString("F2") + "," + _camera.transform.rotation.y.ToString("F2") + "," + _camera.transform.rotation.z.ToString("F2") + "," + _camera.transform.rotation.w.ToString("F2");
                var pose = positionString + "," + rotationString;

                ////Get environment hitpoints
                //Here we get 12 environment hitpoints using 12 raycasts distibuted across the device screen
                int numRays = 12;
                int horizontal_increment = _cam.pixelWidth / 6;
                int vertical_increment = _cam.pixelHeight / 8;
                //Create list of the 12 rays
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment, vertical_increment, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 3, vertical_increment, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 5, vertical_increment, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment, vertical_increment * 3, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 3, vertical_increment * 3, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 5, vertical_increment * 3, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment, vertical_increment * 5, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 3, vertical_increment * 5, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 5, vertical_increment * 5, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment, vertical_increment * 5, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 3, vertical_increment * 5, 0)));
                rays.Add(_cam.ScreenPointToRay(new Vector3(horizontal_increment * 5, vertical_increment * 5, 0)));

                //Get environment hitpoints for each ray
                for (var i = 0; i < rays.Count; i++)
                {
                    if ((m_RaycastManager.Raycast(rays[i], s_Hits, TrackableType.PlaneWithinPolygon)))
                    {
                        //Add relative position
                        relativeEnvHits.Add(RenderAnchorContent.anchorPositionA - s_Hits[0].pose.position);
                    }
                }
                rays.Clear();

                //Concatenate hits into a string
                int numHits = relativeEnvHits.Count;
                string relativeEnvHitsString = "";
                for (var i = 0; i < numHits; i++)
                {
                    relativeEnvHitsString += (relativeEnvHits[i].x.ToString("F1") + ' ' + relativeEnvHits[i].y.ToString("F1") + ' ' + relativeEnvHits[i].z.ToString("F1"));
                    relativeEnvHitsString += ",";
                }
                //Pad to take account of missing hits
                for (var i = 0; i < numRays - numHits; i++)
                {
                    relativeEnvHitsString += ",";
                }
                relativeEnvHits.Clear();

                //Get inertial data
                string gyroData = Input.gyro.rotationRate.x.ToString("F3") + "," + Input.gyro.rotationRate.y.ToString("F3") + "," + Input.gyro.rotationRate.z.ToString("F3");

                //Get object hit by ray
                string object_viewed = viewer_output.text;

                //Write data to motion log
                motionLogOutput.Add(device_id + "," + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:fff") + "," + pose + "," + relativeEnvHitsString + gyroData + "," + object_viewed);
                frame_count += 1;
            }

            if (frame_count == 120)
            {
                File.WriteAllLines(savePath, motionLogOutput);
                StartCoroutine(UploadMotionLog());
                motionLogOutput.Clear();
                frame_count = 0;
            }
        }

        IEnumerator UploadMotionLog()
        {
            //Check motion log exists
            if (!File.Exists(Application.persistentDataPath + "/motionlog.txt"))
            {
                log.text = "motionLog doesn't exist";
            }

            WWWForm form = new WWWForm();
            form.AddBinaryData("motionlog", File.ReadAllBytes(Application.persistentDataPath + "/motionlog.txt"), "motionlog.txt", "text/csv");
            string url_motionlog = "http://" + (insert the IP address of your server) + ":8000/motionlog";
            UnityWebRequest www = UnityWebRequest.Post(url_motionlog, form);
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
        ARRaycastManager m_RaycastManager;
    }
}
