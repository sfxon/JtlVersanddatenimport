using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Versanddatenimport
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ValidateAssembly validateAssembly = new ValidateAssembly();

            if (!validateAssembly.IsValid) {
                //Das hier tritt auf, wenn die JTL-Wawi Extern nicht gefunden werden konnte.
                MessageBox.Show("Es wurde keine Installation der JTL-Wawi gefunden- oder die JTLwawiExtern.dll liegt im falschen Verzeichnis. Das Programm wird beendet.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
