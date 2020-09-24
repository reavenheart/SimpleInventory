using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private string authKey = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";
        
        [ContextMenu("SendTestPostRequest")]
        private void SendTestRequest()
        {
            WWWForm form = new WWWForm();
            StartCoroutine(SendPOSTRequest(form));
        }

        public void SendAddItemIntoBagRequest(string id)
        {
            WWWForm form = new WWWForm();
            form.AddField("event", "addToBag");
            form.AddField("item", id);
            StartCoroutine(SendPOSTRequest(form));
        }
        
        public void SendRemoveItemfromBagRequest(string id)
        {
            WWWForm form = new WWWForm();
            form.AddField("event", "removeFromBag");
            form.AddField("item", id);
            StartCoroutine(SendPOSTRequest(form));
        }
        
        private IEnumerator SendPOSTRequest(WWWForm form)
        {
            using (UnityWebRequest www = UnityWebRequest.Post("https://dev3r02.elysium.today/inventory/status", form))
            {
                www.SetRequestHeader("auth", authKey);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                }
            }
        }
    }
}