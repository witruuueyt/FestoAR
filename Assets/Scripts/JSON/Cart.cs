using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class Cart : MonoBehaviour
{
    public List<string> CurrentCartData = new List<string>();

    public CartJSON[] currentCartObjectArray;

    public string listInfo;

    public TMP_Text info;

    public string requestURL;

    public NodeCheck nodeCheck;
    public TMP_Text onoText;
    private string currentCarrierID;


    public void ReceieveData(string CurrentCartStringPHPMany)
    {
        string newCurrentCartStringPHPMany = fixJson(CurrentCartStringPHPMany);

        Debug.LogWarning(newCurrentCartStringPHPMany);

        currentCartObjectArray = JsonHelper.FromJson<CartJSON>(newCurrentCartStringPHPMany);

        CurrentCartData.Clear();
        listInfo = "";

        for (int i = 0; i < currentCartObjectArray.Length; i++)
        {
            Debug.LogWarning("ONo:" + currentCartObjectArray[i].ONo + ", Cart Number:" + currentCartObjectArray[i].CarrierID);

            CurrentCartData.Add("ONo:" + currentCartObjectArray[i].ONo + ", Cart Number:" + currentCartObjectArray[i].CarrierID);
        }

        foreach (var listMember in CurrentCartData)
        {
            listInfo += listMember.ToString() + "\n" + "\n";
        }

        info.text = listInfo;
        Debug.LogError(info.text);
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    public void GetRequestPublic()
    {
        StartCoroutine(GetRequest("http://172.21.0.90/SQLData.php?Command=cart"));     //calls coroutine and sets string
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    ReceieveData(webRequest.downloadHandler.text);
                    Debug.LogError("Current Orders Success");

                    break;
            }
        }
    }



    //    private void update()
    //    {
    //        currentCarrierID = currentCartObjectArray[i].ONo;
    //    }
}
