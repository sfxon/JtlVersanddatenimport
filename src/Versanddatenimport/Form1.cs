using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JTLwawiExtern;
using LumenWorks.Framework.IO.Csv;
using Microsoft.VisualBasic.FileIO;

namespace Versanddatenimport
{
    public partial class Form1 : Form
    {
        private CJTLwawiExtern _wawiExtern;
        private bool bWorkerRunning = false;
        static System.Windows.Forms.Timer worker = null;
        private Config workerConfig = null;
        private String lastError = "";
        private List<String[]> csvData = null;
        private const int expectedCsvColumns = 3;
        private String logFolderDate = "";

        public Form1()
        {
            InitializeComponent();

            //Event Handler setzen, für den Load Event.
            this.Shown += new System.EventHandler(this.Form1_Shown);
        }

        // Event Handler: wird ausgeführt, wenn das Fenster angezeigt wird.
        // Startet bei gewählter Option den Worker automatisch.
        private void Form1_Shown(object sender, EventArgs e)
        {
            if(!initConfig())
            {
                return;
            }

            if(workerConfig.autostartWorker)
            {
                buttonStartWorker.PerformClick();
            }
        }

        private void buttonTestConnection_Click(object sender, EventArgs e)
        {
            //Verbindungsdaten laden.
            ConfigLoader configLoader = new ConfigLoader();
            Config myConfig = null;

            bool show_errors = true;
            myConfig = configLoader.loadConfig(show_errors);

            if (null == myConfig)
            {
                //Keine Fehlerausgabe, weil der Fehler bereits ausgegeben wurde.
                return;
            }


            this._wawiExtern = new CJTLwawiExtern();

            string error = "";

            int status = this._wawiExtern.JTL_TesteDatenbankverbindung(
                myConfig.DatabaseServer,    //Server
                myConfig.DatabaseName,      //DB-Name
                myConfig.DatabaseUser,      //DB-User
                myConfig.DatabasePassword,  //DB-Password
                out error                   //Fehler werden in diese Variable ausgegeben.
            );

            if (status == 0)
            {
                MessageBox.Show("Es konnte keine Verbindung zur Datenbank hergestellt werden. \n\nFehlerdetails: \n" + error, "Datenbank-Verbindungsfehler");
                return;
            }

            MessageBox.Show("Datenbank-Verbindung erfolgreich.");
        }

        //Menü: Datei -> Optionen
        private void menuItem3_Click(object sender, EventArgs e)
        {
            Optionen o = new Optionen();
            o.ShowDialog(this);
        }

        //Menü: Datei -> Beenden
        private void menuItem4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void buttonStartWorker_Click(object sender, EventArgs e)
        {
            if (bWorkerRunning)
            {
                stopWorker();
            }
            else
            {
                startWorker();
            }
        }

        private void stopWorker()
        {
            if (worker != null)
            {
                worker.Stop();
            }

            buttonTestConnection.Enabled = true;        //Datenbank-Test Button deaktivieren
            buttonSingleImport.Enabled = true;          //Button für einelnen Import aktivieren.
            menuItem3.Enabled = true;                   //Datenbank-Setup deaktivieren
            buttonStartWorker.Text = "Worker starten";  //Text auf dem Button anpassen.
            bWorkerRunning = false;
            labelWorkerStatus.Text = "Offline (nicht gestartet)";


        }

        private void startWorker()
        {
            buttonTestConnection.Enabled = false;       //Datenbank-Test Button aktivieren
            buttonSingleImport.Enabled = false;         //Button für einelnen Import aktivieren.
            menuItem3.Enabled = false;                  //Datenbank-Setup aktivieren
            buttonStartWorker.Text = "Stoppen";         //Text auf dem Button anpassen.
            bWorkerRunning = true;
            labelWorkerStatus.Text = "Worker läuft";

            if (!initVersanddatenImport())
            {
                return;
            }

            textBoxWorkerOutput.Text = "Worker wird gestartet..\r\n";
            textBoxWorkerOutput.Text += "Verarbeitungszeit von: " + workerConfig.timeFrom + " Uhr\r\n";
            textBoxWorkerOutput.Text += "Verarbeitungszeit bis: " + workerConfig.timeTo + " Uhr\r\n";
            textBoxWorkerOutput.Text += "Verarbeitungs-Interval in Minuten: " + workerConfig.TimeIntervalInMinutes + "\r\n";

            worker = new System.Windows.Forms.Timer();
            worker.Tick += new EventHandler(workerProcess);     //Event Handler setzen
            worker.Interval = workerConfig.TimeIntervalInMinutes * 1000 * 60;                             //Zeit Interval setzen. Ist in Millisekunden angegeben. Deswegen die Sekunden mal eintausend.
            worker.Start();                                     //Worker starten
        }

        private bool initVersanddatenImport()
        {
            //Init Worker -> Loads config and stuff.
            if (!initConfig())
            {
                textBoxWorkerOutput.Text = "Import konnte nicht gestartet werden, weil die Konfiguration nicht geladen werden konnte.";
                return false;
            }

            return true;
        }

        private bool initConfig()
        {
            bool show_errors = false;

            ConfigLoader cfl = new ConfigLoader();
            workerConfig = cfl.loadConfig(show_errors);

            if (workerConfig == null)
            {
                stopWorker();
                return false;
            }

            return true;
        }

        private void workerProcess(Object myObject, EventArgs myEventArgs)
        {
            DateTime tmpDate = DateTime.Now;
            textBoxWorkerOutput.Text += tmpDate.ToString("yyyy-MM-dd HH:mm:ss") + " - Starte Verarbeitung.\r\n";

            worker.Stop();

            // CSV-Dateien mit Versanddaten aufteilen, wenn es komplexe CSV-Dateien gibt.
            if (!prepareInputFiles())
            {
                // Wenn es keine CSV-Dateien zum Aufteilen gab,
                // Daten importieren.
                doVersanddatenImport();
            }
            
            worker.Start();
        }

        private void buttonSingleImport_Click(object sender, EventArgs e)
        {
            DateTime tmpDate = DateTime.Now;
            textBoxWorkerOutput.Text += tmpDate.ToString("yyyy-MM-dd HH:mm:ss") + " - Starte Verarbeitung.\r\n";

            // CSV-Dateien mit Versanddaten aufteilen, wenn es komplexe CSV-Dateien gibt.
            if (prepareInputFiles())
            {
                return;
            }

            // Wenn es keine CSV-Dateien zum Aufteilen gab,
            // Daten importieren.
            if (!initVersanddatenImport())
            {
                return;
            }

            doVersanddatenImport();
        }

        private void doVersanddatenImport()
        {
            //Log Folder anlegen.
            DateTime tmpDate = DateTime.Now;
            logFolderDate = tmpDate.ToString("yyyy-MM-dd");
            String logFolder = workerConfig.folderLog + "/" + logFolderDate;
            FileStream logfile = null;
            StreamWriter logfileWriter = null;

            try
            {
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
            } catch (Exception e)
            {
                textBoxWorkerOutput.Text += "Konnte den Ordner für das Log-File nicht anlegen: " + logFolder + "; Verarbeitung wurde gestoppt." + e.Message + "\r\n";
                return;
            }

            //Check if work-folder is empty.. (if not - we do not allow another work attempt)
            if(!isWorkingDirectoryEmpty())
            {
                textBoxWorkerOutput.Text += "Es besteht ein nicht abgeschlossener Import. Bitte prüfen Sie den worker-Ordner. Verarbeitung wurde gestoppt." + "\r\n";
                return;
            }
            
            //Check folder for new file
            String nextFile = getNextFilename();

            if("" == nextFile)
            {
                textBoxWorkerOutput.Text += "Keine Datei zum Importieren vorhanden. Beende Durchlauf.\r\n";
                return;
            }

            FileInfo fi = new FileInfo(nextFile);
            string pureFilename = fi.Name;

            textBoxWorkerOutput.Text += "Bearbeite Importdatei: \r\n";
            textBoxWorkerOutput.Text += nextFile;
            textBoxWorkerOutput.Text += "\r\n";

            //Versuche Logdatei anzulegen.
            try
            {
                logfile = File.Create(logFolder + "/" + pureFilename);
                logfileWriter = new StreamWriter(logfile);

            } catch(Exception e)
            {
                textBoxWorkerOutput.Text += "Konnte Logdatei nicht erstellen. Verareitung gestoppt." + e.Message + "\r\n";
                return;
            }

            //Move file to working directory..
            if (!Directory.Exists("worker"))
            {
                try
                {
                    Directory.CreateDirectory("worker");
                } catch(Exception e)
                {
                    textBoxWorkerOutput.Text += "Konnte Arbeitsverzeichnis (/worker) weder finden noch erstellen. Verareitung gestoppt." + e.Message + "\r\n";
                    logfileWriter.WriteLine("Konnte Arbeitsverzeichnis (/worker) weder finden noch erstellen. Verareitung gestoppt." + e.Message);
                    logfileWriter.Close();
                    return;
                }
            }

            try
            {
                File.Move(nextFile, "worker/" + pureFilename);
            } catch(Exception e)
            {
                textBoxWorkerOutput.Text += "Konnte CSV Datei nicht in Arbeitsverzeichnis verschieben. Verarbeitung gestoppt." + "\r\n";
                logfileWriter.WriteLine("Konnte CSV Datei nicht in Arbeitsverzeichnis verschieben.Verarbeitung gestoppt.");
                logfileWriter.Close();
                return;
            }

            nextFile = "worker/" + pureFilename;        //Hiermit weiterarbeiten.

            //Datei-Inhalte überprüfen.
            if (false == loadFile(nextFile))
            {
                textBoxWorkerOutput.Text += lastError + "\r\n";
                logfileWriter.WriteLine(lastError);
                logfileWriter.Close();
                return;
            }
            
            if (csvData.Count == 0)
            {
                //MessageBox.Show("Die Datei enthält keine Daten.");
                textBoxWorkerOutput.Text += lastError + "\r\n";
                logfileWriter.WriteLine(lastError);
                logfileWriter.Close();
                return;
            }

            //Zeilen importieren..
            if (false == importTableRows(logfileWriter))
            {
                textBoxWorkerOutput.Text += lastError + "\r\n";
                logfileWriter.WriteLine(lastError);
                logfileWriter.Close();
                return;
            }

            //Eingelesene Datei an Zielort verschieben.
            textBoxWorkerOutput.Text += "Verschiebe bearbeitete Datei an Zielort: " + workerConfig.folderArchive + "/" + pureFilename + "\r\n";

            try
            {
                File.Move(nextFile, workerConfig.folderArchive + "/" + pureFilename);
            } catch(Exception e)
            {
                Random rand = new Random();
                File.Move(nextFile, workerConfig.folderArchive + "/" + pureFilename + tmpDate.ToString("yyyy-MM-dd-HH-mm-ss-") + rand.Next());
            }

            textBoxWorkerOutput.Text += "Datei erfolgreich verschoben. Beende Durchlauf" + "\r\n";
            logfileWriter.WriteLine("Datei erfolgreich verschoben. Beende Durchlauf");
            logfileWriter.Close();
        }

        private String getNextFilename()
        {
            DirectoryInfo d = new DirectoryInfo(workerConfig.folderIncomingFiles);
            FileInfo[] files = d.GetFiles("*.csv");

            for (int i = 0; i < files.Length; i++)
            {
                return files[i].FullName;
            }

            return "";
        }

        private String getNextFilenameToSplit()
        {
            DirectoryInfo d = new DirectoryInfo(workerConfig.folderReceivedFilesForSplit);
            FileInfo[] files = d.GetFiles("*.csv");

            for (int i = 0; i < files.Length; i++)
            {
                return files[i].FullName;
            }

            return "";
        }

        private bool isWorkingDirectoryEmpty()
        {
            if(!Directory.Exists("worker"))
            {
                return true;
            }
            
            DirectoryInfo d = new DirectoryInfo("worker");
            FileInfo[] files = d.GetFiles("*.csv");

            if(files.Length > 0) 
            {
                return false;
            }

            return true;
        }

        private bool loadFile(String filename)
        {
            //Prüfe, ob die Datei existiert.
            if (!File.Exists(filename))
            {
                return false;
            }

            //Lese Datei und prüfe den Inhalt..
            csvData = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                
                while (!parser.EndOfData)
                {          
                    //Process row
                    string[] fields = parser.ReadFields();
                    csvData.Add(fields);

                }
            }

            return true;
        }

        private bool importTableRows(StreamWriter logfileWriter)
        {
            logfileWriter.WriteLine("Öffne JTLWawiConnection");

            this._wawiExtern = new CJTLwawiExtern();
            var VersanddatenImporter = this._wawiExtern.VersanddatenImporter(
                workerConfig.DatabaseServer,
                workerConfig.DatabaseName,
                workerConfig.DatabaseUser,
                workerConfig.DatabasePassword,
                workerConfig.JtlUserId
            );

            logfileWriter.WriteLine("WawiConnection erfolgreich erstellt");

            for (int i = 0; i < csvData.Count; i++)
            {
                logfileWriter.WriteLine("Importiere Datensatz: " + csvData[i][0].ToString() + ";" + csvData[i][1].ToString() + ";" + csvData[i][2].ToString());

                importRow(
                    VersanddatenImporter,
                    csvData[i][0].ToString(),
                    DateTime.Parse(csvData[i][2].ToString()),
                    csvData[i][1].ToString(),
                    ""
                );
            }

            logfileWriter.WriteLine("Verarbeitung aller Zeilen beendet");

            return true;
        }


        private bool importRow(
                JTLwawiExtern.VersanddatenImport.VersanddatenImporter VersanddatenImporter,  
                String Paketnummer, 
                DateTime Versanddatum, 
                String TrackingId, 
                String VersandInfo)
        {
            //Einzelne Versandinfo erstellen
            Versandinformation i = new Versandinformation();

            /* Id kann folgendes sein:
             * P1234 => 1234 = kVersand = InternePaketnummer
             * V1234 => 1234 = kVersand = InternePaketnummer
             * L1234 => 1234 = kLieferschein = InterneLieferscheinnummern
             * AU12345-001 => Lieferscheinnummer (muss mit -XXX enden)
             * AU12345-001$1234 => Interne Paketnummer 1234 vom Lieferschein AU12345-001
             */
            
            i.Id = Paketnummer;
            i.Versanddatum =Versanddatum;
            i.TrackingId = TrackingId;
            i.VersandInfo = VersandInfo;

            //Der Importer von JTL hat leider keine Möglichkeit, einen Fehler zu melden.
            //Wir müssen uns also darauf verlassen, dass der Import erfolgreich durchläuft..
            VersanddatenImporter.Add(i.Id, i.Versanddatum, i.TrackingId, i.VersandInfo);
            VersanddatenImporter.Apply();

            return true;
        }

        private bool prepareInputFiles()
        {
            textBoxWorkerOutput.Text += "Prüfe, ob eine komplexe Import Datei zum Aufteilen vorliegt.\r\n";

            // Prüfe, ob eine CSV-Datei vorhanden ist.
            String nextFile = getNextFilenameToSplit();

            if ("" == nextFile)
            {
                textBoxWorkerOutput.Text += "Keine Datei zum Aufteilen vorhanden.\r\n";
                return false;
            }

            // Wenn eine Datei gefunden wurde, wird diese aufgeteilt:
            textBoxWorkerOutput.Text += "Teile Datei " + nextFile + " auf.\r\n";

            splitFile(nextFile);

            return true;
        }

        private void splitFile(String filename)
        {
            //Datei-Inhalte überprüfen.
            if (false == loadFile(filename))
            {
                textBoxWorkerOutput.Text += lastError + "\r\n";
                return;
            }

            if (csvData.Count == 0)
            {
                textBoxWorkerOutput.Text += lastError + "\r\n";
                return;
            }

            //Zeilen importieren..
            //if (false == importTableRows(logfileWriter))
            splitTableRows(filename);

            String destinationFilename = "splittedLog/" + Path.GetFileName(filename);
            File.Delete(destinationFilename); // Datei löschen, damit das nicht fehlschlägt, falls die Datei schon existiert.
            File.Move(filename, destinationFilename);
        }

        private void splitTableRows(String filename) 
        {
            for (int i = 0; i < csvData.Count; i++)
            {
                // Dateiname für die neue Datei erzeugen.
                String newFilename = Path.GetFileName(filename) + i + ".csv";
                newFilename = workerConfig.folderIncomingFiles + "/" + newFilename;
                
                // Filestream erstellen.
                FileStream newFileStream = File.Create(newFilename);
                StreamWriter newFileStreamWriter = new StreamWriter(newFileStream);

                newFileStreamWriter.Write(csvData[i][0].ToString() + ";" + csvData[i][1].ToString() + ";" + csvData[i][2].ToString());

                newFileStreamWriter.Close();
                newFileStream.Close();

                textBoxWorkerOutput.Text += "Neue Datei: " + newFilename + "\r\n";
            }
        }
    }
}
