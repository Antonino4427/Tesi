
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;
using TMPro;
using OpenAI;
using OpenAI.Edits;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

public class OutputGenerator : MonoBehaviour
{
    public string html;
    public GameObject camera;
    //public GameObject PhotoCaptureManager;
    public string sceneName;
    public GameObject simulationManager;
    public GameObject objectManagerVR;
    public int sceneCode;
    private List<string> buffer;
    private List<string> timestamps;
    private string description;
    private CameraManager photoCaptureManager;
    [SerializeField] private TextMeshPro TestoFinale;
    [SerializeField] private GameObject loadingCanvas;
    public string riformulato = string.Empty;
    public EditResponse result;
    private GameObject _soundManager;
    public AudioClip _notificationSound;


    public bool StoryboardGenerato = false;

    void Start()
    {
        timestamps = new List<string>();
        html = "";
        sceneCode = UnityEngine.Random.Range(100000, 999999);
        UnityEngine.Debug.Log("Scene code: " + sceneCode);
        photoCaptureManager = FindObjectOfType<CameraManager>();

        _soundManager = GameObject.Find("SoundManager");
    }

    

    public async void GenerateFile()
    {

        

        sceneName = objectManagerVR.GetComponent<ObjectManagerVR>().sceneName.GetComponent<TextMeshPro>().text;
        timestamps.Clear();
        //prende tutti gli screenshot count, esclude i doppioni e li ordina per sapere quali immagini mostrare

        buffer = GetComponent<PhraseGenerator>().ClearBuffer();

       
        foreach (string b in buffer)
        {
            timestamps.Add(b.Split('_')[0]);
        }
        timestamps = timestamps.Distinct().ToList().OrderBy(i => int.Parse(i)).ToList();
       


       // string basePath = Path.Combine(UnityEngine.Application.streamingAssetsPath, "storyboards/Storyboard.html");

        using (var writer = new StreamWriter(@"Assets\Valentina\Scripts\storyboards\Storyboard.html"))
        {
            html = "<!DOCTYPE html> <html> <body style = 'font-family: Courier New, monospace'> " +
                "<h1 contenteditable = \"false\" style = \"text-align:center\">" + sceneName + " Storyboard</h1> <p style = \"text-align:center\">Panels are editable! Click on them to make adjustments. </p> <p style = \"text-align:center\">Go to 'print' and select 'save as pdf' to download your storyboard!</p>";
            /*
            for (int i = 1; i < camera.GetComponent<CameraManager>().imgCount; i++)
            {
                html += "<h3 style = \"text-align:center\">Shot #1</h3>" +
                    "<div class= \"flex-container\"  style=\"display: flex; flex-direction: row; justify-content: center; margin: 10px; align-items: center\" >" +
                    "<div><img src=\"screenshots/"+sceneCode+"_img"+i+".png\" width=\"300\" height=\"150\" style=\"padding: 20px; margin: 10px\" > </div>" +
                    " <div> <p> Linea prova </p>  <p> Altra linea prova </p> </div> </div>" +
                    "<p style = \"text-align:center; margin-left: 200px; margin-right: 200px\">Descrizione paragrafo</p>";
            }
            html += "</body> </html>";
            */
            var j = 0;
            var tsps = "";
            var intervals = "";
            var t_old = "0";
            var inq = "";

            foreach (string t in timestamps)
            {
                tsps += t + ", ";

                //UnityEngine.Debug.Log("t: " + t + "; t_old: " + t_old);
                intervals += (Convert.ToInt32(t) - Convert.ToInt32(t_old)).ToString() + ", ";

                j++;
                //prende buffer e raggruppa laddove si presenta lo stesso timestamp (quello corrente)
                description = "";


                foreach (string b in buffer)
                {
                    if (b.Split('_')[0] == t)
                    {
                        description += b.Split('_')[1];
                    }
                }


               await Rephrase();





                html += "<h3 style = \"text-align:center; margin-top: 20px\">Shot #" + j + "</h3>" +
                    "<div class= \"flex-container\"  style=\"display: flex; flex-direction: row; justify-content: center; margin: 8px; align-items: center; background-color:#c0e9fa;\" >" +
                        "<div><img src=\"screenshots/" + sceneCode + "_img" + t + ".png\" width=\"550\" height=\"300\" style=\"padding: 20px; margin: 10px\" onerror=\"this.src = 'not_found.png'; \"> </div>" +
                        "<div contenteditable = \"true\">" +
                            "<p>Duration: " + photoCaptureManager.actionTimes[t] + " sec</p>" +
                            "<p> Focal length: " + photoCaptureManager.focalTable[t] + " mm</p>" +
                        //"<p>Shot type: " + inq + "</p>" +
                        "</div> " +
                    "</div>" +
                    "<p contenteditable = \"true\" style = \"text-align:center; width: '300'; margin-bottom: 10px\"> Original: " + description + "</p>" +
                    "<p contenteditable = \"true\" style = \"text-align:center; width: '300'; margin-bottom: 10px\"> AI edit: " + riformulato + "</p>";



                t_old = t;




            }
            

            

            //Animatic + Timer
            /*html += "<h2 style = \"text-align:center; margin-top: 50px\">" + sceneName + " Animatic</h3> <p style = \"text-align:center\">This video simply visualizes the panels in sequence considering timestamps. </p>";

            html += "<div style = 'text-align: center; margin-bottom: 10px'><button onclick='doImages(0)' style = 'background-color: #4CAF50; border:none; color: white'>PLAY</button></div>";

            //html += "<script>   let n = 0, timeouts = [" + tsps + "]; intervals = [" + intervals + "]; const doImages = (num) => { setTimeout(() => { document.querySelector('[data-changer]').setAttribute('src', 'screenshots/" + sceneCode + "_img'+timeouts[num]+'.png'); if (num++ < timeouts.length-1) doImages(num); }, intervals[num] * 1000) };          </script>";

            html += "<div style = 'text-align: center'> <img width = '700' height = '380' data-changer src = 'placeholder-video.png' /> </div>";

            //Timer
            html += "<div  style = 'text-align: center; margin-bottom: 50px; margin-top: 10px'><label id='minutes'>00</label>:<label id='seconds'>00</label></div>";

            html += "<script> let n = 0, play = false, timeouts = [" + tsps + "]; intervals = [" + intervals + "]; const doImages = (num) => { setTimeout(() => { document.querySelector('[data-changer]').setAttribute('src', 'screenshots/" + sceneCode + "_img'+timeouts[num]+'.png'); play = true; if (num++ < timeouts.length-1) doImages(num); }, intervals[num] * 1000) }; " +
                "var minutesLabel = document.getElementById('minutes'); var secondsLabel = document.getElementById('seconds'); var totalSeconds = 0; setInterval(setTime, 1000); function setTime() { if (play == true) { ++totalSeconds; secondsLabel.innerHTML = pad(totalSeconds % 60); minutesLabel.innerHTML = pad(parseInt(totalSeconds / 60)); }} function pad(val) { var valString = val + ''; if (valString.length < 2) { return '0' + valString; } else { return valString; } } </script>";

            html += "</body></html>";
            */
            
            SaveStoryboard(html);

            writer.WriteLine(html);

            html = "";
        }

        StoryboardGenerato = true;

        if (StoryboardGenerato)
        {
            loadingCanvas.SetActive(false);
            _soundManager.GetComponent<AudioSource>().PlayOneShot(_notificationSound);
            TestoFinale.text = "Storyboard created!\n\n" +
                "Remove your Oculus, you'll find\n" +
                "the storyboard on the PC.";

            StoryboardGenerato = false;
        }
        


        //Application.OpenURL("file:///C:/Users/Scarzello/Documents/Polito/Corsi%20LM/TESI/Progetto%20Unity/StoryboardMaker/Assets/Scripts/storyboards/Storyboard.html");
        Application.OpenURL(Path.GetFullPath(@"Assets\Valentina\Scripts\storyboards\Storyboard.html"));

    }

    public void SaveStoryboard(string html)
    {
        int index = 0;
        List<String> parent = new List<string>();

        Directory.CreateDirectory(Application.persistentDataPath + "/folder");
        Directory.CreateDirectory(Application.persistentDataPath + "/folder/screenshots");
        photoCaptureManager.images.ForEach((i) =>
        {
            
            string filePath = Application.persistentDataPath + "/folder/screenshots/" + sceneCode + "_img" + index + ".png";
            File.WriteAllBytes(filePath, i);

            index++;
        });

        byte[] fileData = System.Text.Encoding.UTF8.GetBytes(html);
       

        string filePath = Application.persistentDataPath + "/folder/Stroyboard.html";

        File.WriteAllBytes(filePath, fileData);

    }

   

    public async Task Rephrase()
    {

        string textToRerwrite = description; // Testo da riformulare
        string apiKey = "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"; // API key
        

        try
        {
            var openai = new OpenAIClient(apiKey);


            var request = new EditRequest(textToRerwrite, "Riformula il testo, unendo le frasi, quando possibile, e togliendo le ripetizioni consecutive. Lascialo in inglese, non modificare i nomi propri e non aggiungere informazioni inventate aggiuntive. Se non è presente alcun testo restituisci una stringa vuota");
            result = await openai.EditsEndpoint.CreateEditAsync(request);

            Debug.Log("request " + description);

            await Task.Delay(20000); // aspetta 20 s per non superare il limite 3 richieste al minuto della versione free

            Debug.Log("result " + result);

            riformulato = result.Choices[0].Text;

            // Sostituisci il testo originale con il testo riformulato
            //description = riformulato;
        }
        catch (Exception e)
        {
            Debug.LogError("Errore nella richiesta API: " + e.Message);
            
        }

       

    }

   
}

/*
 * Ogni screenshot viene salvato con un codice scena casuale all'inizio, e il timestamp corrispondente al time corrente alla fine. 
 * Dal buffer, che porta con sè il time per ogni elemento, vengono estrapolati i distinti time e si visualizza uno screenshot per ogni distinto time presente nel buffer.
 * Se non è stato fatto uno screenshot a quello specifico time, l'immagine non viene visualizzata nell'html, ma è un errore "utente" quindi è giusto mostrarlo. 
 * 
 * 
 * */


/*if (photoCaptureManager.focalTable[t].ToString() == "14")
                {
                    inq = "1";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "24")
                {
                    inq = "2";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "35")
                {
                    inq = "9";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "50")
                {
                    inq = "3";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "85")
                {
                    inq = "4";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "105")
                {
                    inq = "5";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "135")
                {
                    inq = "6";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "200")
                {
                    inq = "7";
                }
                else if (photoCaptureManager.focalTable[t].ToString() == "400")
                {
                    inq = "8";
                }*/