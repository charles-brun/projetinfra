using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameEnter : MonoBehaviour
{
    public GameObject netMan;
    public kcp2k.KcpTransport transport;
    public bool IAmTheServer;
    public MyServer thisServer;
    public Text TextGUI;
    // Start is called before the first frame update
    void Start()
    {
        transport = netMan.GetComponent<kcp2k.KcpTransport>();
        if (transport == null)
        {
            Debug.Log("NO TRANSPORT");
        }
        else
        {
            if (IAmTheServer)
            {
                // le chemin du fichier
                string filePath = Application.persistentDataPath + "/AllServerPorts.txt";

                // INITIALISATION DU PORT
                int newPort;
                if (File.Exists(filePath))
                {
                    // ON LIST TOUT LES PORTS EXISTANT
                    string[] AllPortsStr = File.ReadAllLines(filePath);
                    // ON PRENDS LE DERNIER PORT
                    int LastPort = StrToInt(AllPortsStr[AllPortsStr.Length - 1]);
                    // ON A DONC LE NOUVEAU PORT
                    newPort = LastPort + 1;
                    // ON LE MET EN STRING POUR POUVOIR L'ENREGISTRER DANS LE FICHIER
                    string newPortStr = newPort.ToString();

                    string cc = "TRY PORT + " + newPort;
                    // ON CREE UN NOUVEAU ARRAY AVEC UNE CAPACITE PLUS GRANDE
                    string[] NewAllPortsStr = new string[AllPortsStr.Length + 1];
                    for (int i = 0; i < NewAllPortsStr.Length; i++)
                    {
                        if (i < NewAllPortsStr.Length - 1)
                        {
                            // ON REMET LES VIEUX PORTS
                            NewAllPortsStr[i] = AllPortsStr[i];
                        } else
                        {
                            // ON MET LE NOUVEAU PORT 
                            NewAllPortsStr[i] = newPortStr;
                        }
                    }
                    // EN ENREGISTRE LE TOUT
                    File.WriteAllLines(filePath, NewAllPortsStr);
                }
                else
                {
                    // ON COMMENCE AVEC LE PORT 7778 DANS LE CAS OU C'EST LE PREMIER SERVEUR
                    string[] AllPorts = { "7778" };
                    File.WriteAllLines(filePath, AllPorts);
                    newPort = 7778;
                    
                }
                transport.Port = (ushort)newPort;

                netMan.GetComponent<Mirror.NetworkManager>().StartServer();
                GameManager.Instance.MyPort = newPort;
                TextGUI.enabled = true;
                TextGUI.text = "SERVEUR PORT IS " + newPort.ToString();
                Debug.Log("SERVEUR PORT IS " + newPort.ToString());
                
                string ServerStatePath = Application.persistentDataPath + "/" + newPort.ToString() + "_" + Environment.MachineName.ToString() +  ".state";
                File.WriteAllText(ServerStatePath, "T");
            }
            else
            {
                // ------------------  LOAD PORT FROM JSON FILE  ------------------
                string filePath = Application.persistentDataPath + "/TargetPort.txt";
                string PortToUseStr = File.ReadAllText(filePath);

                int PortToUseInt = StrToInt(PortToUseStr);

                ushort PortToUse = (ushort)PortToUseInt;
                transport.Port = PortToUse;
                netMan.GetComponent<Mirror.NetworkManager>().networkAddress = "35.181.61.44";
                netMan.GetComponent<Mirror.NetworkManager>().StartClient();
                TextGUI.enabled = true;
                TextGUI.text = "SERVEUR PORT IS " + PortToUseStr;
            }
           
        }
    }

    int StrToInt(string strToUse)
    {
        int nbtemp;
        bool isParsable = Int32.TryParse(strToUse, out nbtemp);

        if (isParsable)
        {
            return nbtemp;

        }
        else
        {
            return -123456;
        }
    }
}
