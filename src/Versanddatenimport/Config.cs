using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Versanddatenimport
{
    class Config
    {
        // Server-Instanz (For example: DESKTOP-123234\\SQLEXPRESS)
        public String DatabaseServer { get; set; }

        // Datenbank (Default for jtl is "eazybusiness")
        public String DatabaseName { get; set; }

        // User of database (for example: sa)
        public String DatabaseUser { get; set; }

        // Passwort (default for jtl sa user is: sa04jT14 - holy moly - we never forget that legendary year 2014)
        public String DatabasePassword { get; set; }

        // Festgelegt in der Datenbank in Tabelle tBenutzer, Feld: kBenutzer
        public int JtlUserId { get; set; }

        // Zeit ab (Tageszeit, ab welcher der Worker läuft)
        public String timeFrom { get; set; }

        //Zeit bis (Tageszeit, bis zu der der Worker aufhört zu laufen)
        public String timeTo { get; set; }

        // Intervall in Minuten - alle x Minuten wird der Job gestartet.
        // Wenn der Worker gestartet ist, läuft das Intervall immer -
        // Worker führt nur keine Aktion aus, wenn er nicht
        // innerhalb der "aktiven" Zeitspanne liegt,
        // also zwischen timeFrom und timeTo.
        public int TimeIntervalInMinutes { get; set; }

        // Ordner für empfangene Dateien, die gesplittet werden sollen
        // Das ist der Ordner mit CSV-Dateien, die zeilenweise aufgesplittet werden sollen,
        // damit der Importer die Datensätze einzeln importieren kann.
        public String folderReceivedFilesForSplit { get; set; }

        // Ordner für eingehende Dateien
        // Das ist der Ordner, in den die gesplitteten Dateien verschoben wurden.
        // Diese werden von diesem Ordner aus in die Wawi importiert.
        public String folderIncomingFiles { get; set; }

        // Ordner für das Archiv.
        public String folderArchive { get; set; }

        // Ordner für Log-Dateien.
        public String folderLog { get; set; }

        // Wenn das auf true steht, wird der Worker automatisch gestartet,
        // sobald das Programm gestartet wird.
        // Dadurch kann das Programm in den Autostart des Rechners
        // eingebunden werden, und automatisch nach einem Neustart starten.
        public bool autostartWorker { get; set; }
    }
}
