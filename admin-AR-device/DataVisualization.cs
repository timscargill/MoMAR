using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]

    public class DataVisualization : MonoBehaviour
    {
        private Dictionary<string, string> savedAnchorDict = new Dictionary<string, string>();
        private int frame_count = 0;
        private bool motionData_requested = false;
        private bool motionData_downloaded = false;
        private bool anchorDict_requested = false;
        private bool anchorDict_downloaded = false;
        private bool anchorDict_matched = false;
        public GameObject _heatmapNode;
        private List<float> x = new List<float>();
        private List<float> y = new List<float>();
        private List<float> z = new List<float>();
        private List<GameObject> mapNodes = new List<GameObject>();

        void Awake()
        {
            m_AnchorManager = GetComponent<ARAnchorManager>();
        }

        void Start()
        {
        }

        void Update()
        {
            //check if world map has been downloaded, dictionary has been downloaded, and anchors matches found
            frame_count += 1;
            if (frame_count == 5)
            {
                if ((ARWorldMapController.worldMap_downloaded == true) && (motionData_requested == false))
                {
                    StartCoroutine(DownloadMotionData());
                }
                if ((ARWorldMapController.worldMap_downloaded == true) && (motionData_downloaded == true) && (anchorDict_requested == false))
                {
                    StartCoroutine(DownloadAnchorDict());
                }
                if ((ARWorldMapController.worldMap_deserialized == true) && (anchorDict_downloaded == true) && (anchorDict_matched == false))
                {
                    RenderVisualization();
                }
                frame_count = 0;
            }
        }

        IEnumerator DownloadMotionData()
        {
            motionData_requested = true;
            string url_motiondata = "http://" + (insert the IP address of your server) + ":8000/motiondata.csv";
            using (UnityWebRequest www = UnityWebRequest.Get(url_motiondata))
            {
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string savePath = string.Format("{0}/{1}.csv", Application.persistentDataPath, "motionData");
                    File.WriteAllText(savePath, www.downloadHandler.text);

                    foreach (var l in File.ReadLines(Application.persistentDataPath + "/motionData.csv").Skip(1))
                    {
                        var lsplit = l.Split(',');
                        if (lsplit.Length > 2)
                        {
                            x.Add(float.Parse(lsplit[0]));
                            y.Add(float.Parse(lsplit[1]));
                            z.Add(float.Parse(lsplit[2]));
                        }
                    }
                    motionData_downloaded = true;
                }
            }
        }

        IEnumerator DownloadAnchorDict()
        {
            anchorDict_requested = true;
            string url_anchordict = "http://" + (insert the IP address of your server) + ":8000/anchordict.txt";
            using (UnityWebRequest www = UnityWebRequest.Get(url_anchordict))
            {
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string savePath = string.Format("{0}/{1}.txt", Application.persistentDataPath, "anchorDict");
                    File.WriteAllText(savePath, www.downloadHandler.text);
                    var lines = File.ReadAllLines(Application.persistentDataPath + "/anchorDict.txt");
                    //Convert comma separated .txt file to dictionary
                    foreach (var l in lines)
                    {
                        var lsplit = l.Split(',');
                        if (lsplit.Length > 1)
                        {
                            var newkey = lsplit[0];
                            var newval = lsplit[1];
                            savedAnchorDict[newkey] = newval;
                        }
                    }
                    anchorDict_downloaded = true;
                }
            }
        }

        void RenderVisualization()
        {
            //Basic rendering example shown here, customize this method according to your map requirements
            
            //Loop through available anchors and render motion map content if in dictionary
            foreach (ARAnchor anchor in m_AnchorManager.trackables)
            {
                if (savedAnchorDict.ContainsKey(anchor.name))
                {
                    anchorDict_matched = true;

                    var content_type = savedAnchorDict[anchor.name];
                    
                    //Duplicate the code inside the below if statement for each anchor being used
                    if (content_type == "A")
                    {
                        for (int i = 0; i < x.Count; i++)
                        {
                            GameObject node = Instantiate(_heatmapNode, anchor.transform.position - new Vector3(x[i], y[i], z[i]), Quaternion.Euler(0, 0, 0));
                            mapNodes.Add(node);
                        }
                    }
                }
                else
                {
                    Debug.Log("No anchor matches");
                }
            }
        }
        ARAnchorManager m_AnchorManager;
    }
}
