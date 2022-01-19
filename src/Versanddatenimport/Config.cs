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
        public String DatabaseServer { get; set; }      //Server-Instanz (For example: DESKTOP-123234\\SQLEXPRESS)
        public String DatabaseName { get; set; }        //Datenbank (Default for jtl is "eazybusiness")
        public String DatabaseUser { get; set; }        //User of database (for example: sa)
        public String DatabasePassword { get; set; }    //Passwort (default for jtl sa user is: sa04jT14 - holy moly - we never forget that legendary year 2014)
        public int JtlUserId { get; set; }              //Festgelegt in der Datenbank in Tabelle tBenutzer, Feld: kBenutzer

        public String timeFrom { get; set; }            //Zeit ab (Tageszeit, ab welcher der Worker läuft)
        public String timeTo { get; set; }              //Zeit bis (Tageszeit, bis zu der der Worker aufhört zu laufen)

        //ACHTUNG! Das Intervall läuft immer - Worker wird nur nicht ausgeführt, wenn er nicht innerhalb der oben angegebenen Zeitspanne liegt.
        public int TimeIntervalInMinutes { get; set; }  //Intervall in Minuten - alle x Minuten wird der Job gestartet.

        public String folderIncomingFiles { get; set; }

        public String folderArchive { get; set; }

        public String folderLog { get; set; }
    }
}
