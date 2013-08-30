using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class LoadXmlData : MonoBehaviour {
	
	public TextAsset songListXml;
	
	private List<Dictionary<string,string>> songs = new List<Dictionary<string,string>>();
 	private Dictionary<string,string> obj;
	
	void Start(){
		GetXml();
	}
	
	public void GetXml(){
		XmlDocument xmlDoc = new XmlDocument(); // xmlDoc is the new xml document.
		xmlDoc.LoadXml(songListXml.text); // load the file.
		XmlNodeList songsList = xmlDoc.GetElementsByTagName("list"); // array of the level nodes.
		foreach (XmlNode songInfo in songsList) {
			XmlNodeList content = songInfo.ChildNodes;
   			obj = new Dictionary<string,string>(); // Create a object(Dictionary) to collect the both nodes inside the level node and then put into levels[] array.
			
			foreach (XmlNode subcontent in content) {
				if(subcontent.Name == "trackno") {
					obj.Add("trackno", subcontent.InnerXml);
				} else if(subcontent.Name == "title") {
					obj.Add("title", subcontent.InnerXml);
				} else if(subcontent.Name == "singer") {
					obj.Add("singer", subcontent.InnerXml);
				} else if(subcontent.Name == "category") {
					obj.Add("category", subcontent.InnerXml);
				} else if(subcontent.Name == "duration") {
					obj.Add("duration", subcontent.InnerXml);
				} else if(subcontent.Name == "file") {
					obj.Add("file", subcontent.InnerXml);
				} else if(subcontent.Name == "lyrics") {
					obj.Add("lyrics", subcontent.InnerXml);
				} else if(subcontent.Name == "coveralbum") {
					obj.Add("coveralbum", subcontent.InnerXml);
				}
			}
			songs.Add(obj);
		}
	}
}
