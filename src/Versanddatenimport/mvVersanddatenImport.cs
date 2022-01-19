using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JTLwawiExtern;

namespace Versanddatenimport
{
    public class mvVersanddatenImport
    {
        public static CJTLwawiExtern _wawiExtern;
        public String DatabaseServer { get; set; }      //Server-Instanz (For example: DESKTOP-123234\\SQLEXPRESS)
        public String DatabaseName { get; set; }        //Datenbank (Default for jtl is "eazybusiness")
        public String DatabaseUser { get; set; }        //User of database (for example: sa)
        public String DatabasePassword { get; set; }    //Passwort (default for jtl sa user is: sa04jT14 - holy moly - we never forget that legendary year 2014)
        public int DatabaseUserId { get; set; }      //Festgelegt in der Datenbank in Tabelle tBenutzer, Feld: kBenutzer

        public mvVersanddatenImport(String DatabaseServer, String DatabaseName, String DatabaseUser,  String DatabasePassword, int DatabaseUserId)
        {
            _wawiExtern = new CJTLwawiExtern();

            this.DatabaseServer = DatabaseServer;
            this.DatabaseName = DatabaseName;
            this.DatabaseUser = DatabaseUser;
            this.DatabasePassword = DatabasePassword;
            this.DatabaseUserId = DatabaseUserId;
        }

        /* Versendet einen Eintrag.
         * Id kann folgendes sein:
         * P1234 => 1234 = kVersand = InternePaketnummer
         * V1234 => 1234 = kVersand = InternePaketnummer
         * L1234 => 1234 = kLieferschein = InterneLieferscheinnummern
         * AU12345-001 => Lieferscheinnummer (muss mit -XXX enden)
         * AU12345-001$1234 => Interne Paketnummer 1234 vom Lieferschein AU12345-001
         * 
         * Versanddatum: Zeitstempel, wann versendet wurde.
         * 
         * TrackingId: TrackingId vom Dienstleister (DHL oder DPD o.ä.)
         * 
         * Versandinfo: Freitextfeld zur eigenen Verwendung.
         */
        public bool sendData(String Id, DateTime Versanddatum, String TrackingId, String Versandinfo)
        {
            try
            {
                var VersanddatenImporter = _wawiExtern.VersanddatenImporter(
                    DatabaseServer,
                    DatabaseName,
                    DatabaseUser,
                    DatabasePassword,
                    DatabaseUserId
                );

                VersanddatenImporter.Add(Id, Versanddatum, TrackingId, Versandinfo);
                VersanddatenImporter.Apply();
            }
            catch(Exception e)
            {
                MessageBox.Show(
                    "Verbindung zur JTL Datenbank ist fehlgeschlagen oder Übertragen der Daten fehlgeschlagen.\n" +
                    "Bitte Datenbankeinstellungen prüfen. \n\nÜbergebene Daten:" +
                    "\nId " + Id + 
                    "\nVersanddatum: " + Versanddatum + 
                    "\nTrackingId: " + TrackingId + 
                    "\nVersandinfo: " + Versandinfo + 
                    "\nFehlermeldung der Exception: " + e.Message
                );
                return false;
            }

            return true;
        }
    }
}
