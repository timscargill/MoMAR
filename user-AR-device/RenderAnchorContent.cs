using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]

    public class RenderAnchorContent : MonoBehaviour
    {
        private Dictionary<string, string> savedAnchorDict = new Dictionary<string, string>();

        //declare the GameObjects to render and any Colliders you want to use here

        private int frame_count = 0;
        private bool anchorDict_requested = false;
        private bool anchorDict_downloaded = false;
        private bool anchorDict_matched = false;

        public static bool anchorContent_loaded;
        
        //declare Vector3s to store your anchor positions here

        public GameObject _camera;
        public Text viewer_output;

        void Awake()
        {
            m_AnchorManager = GetComponent<ARAnchorManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //check if world map has been downloaded, dictionary has been downloaded, and anchors matches found
            frame_count += 1;
            if (frame_count == 5)
            {
                if ((ARWorldMapController.worldMap_downloaded == true) && (anchorDict_requested == false))
                {
                    viewer_output.text = "Move the phone slowly";
                    StartCoroutine(DownloadAnchorDict());
                }

                if ((ARWorldMapController.worldMap_deserialized == true) && (anchorDict_downloaded == true) && (anchorDict_matched == false))
                {
                    RenderContent();
                }
                frame_count = 0;
            }
        }

        IEnumerator DownloadAnchorDict()
        {
            anchorDict_requested = true;
            string url_anchordict = "http://" + (insert the IP address of your server) + ":8000/anchordict-train.txt";
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
                    //get comma separated txt file into dictionary
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

        void RenderContent()
        {
            //loop through available anchors and render content if in dictionary
            foreach (ARAnchor anchor in m_AnchorManager.trackables)
            {
                if (savedAnchorDict.ContainsKey(anchor.name))
                {
                    anchorDict_matched = true;
                    var anchor_name = savedAnchorDict[anchor.name];
                    
                    //Instantiate your GameObjects and Colliders relative to each anchor here

                    anchorContent_loaded = true;
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
