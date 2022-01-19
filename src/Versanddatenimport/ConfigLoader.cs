using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Versanddatenimport
{
    class ConfigLoader
    {
        /****
         * Konfiguration aus einer Daten laden.
         * */
        public Config loadConfig(bool show_errors)
        {
            if (!System.IO.File.Exists(@"config.json"))
            {
                if (show_errors)
                {
                    MessageBox.Show("Konnte die Konfigurations-Datei (config.json) nicht lesen. Existiert die Datei? Ist sie mit korrekten Rechten versehen? Ggf. neue Konfiguration über den Menüpunkt \"Einstellungen\" erstellen.", "Fehler beim Lesen der Konfigurationsdatei");
                }
                return null;
            }

            //Load text
            String text = System.IO.File.ReadAllText(@"config.json");

            Config config = (Config)JsonSerializer.Deserialize<Config>(text);

            return config;
        }
    }
}
