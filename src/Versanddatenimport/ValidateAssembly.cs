using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Win32;

namespace Versanddatenimport
{
    public class ValidateAssembly
    {

        private string _wawiPath;
        public bool IsValid { get; private set; }

        public ValidateAssembly()
        {
            this.IsValid = false;
            ValidateAndLoadAssemblys();
        }

        private void ValidateAndLoadAssemblys()
        {
            _wawiPath = FindInstallLocation();
            if (!string.IsNullOrWhiteSpace(this._wawiPath) && ValidExternDllVersion())
            {
                this.IsValid = true;
                AppDomain.CurrentDomain.AssemblyResolve += LoadAssemblys;
            }
            else
            {
                this.IsValid = false;
            }
        }

        /// <summary>
        /// Sucht in der Registry nach einem Uninstall-Subkey für die Wawi
        /// </summary>
        /// <param name="baseKey">Basis-Registry-Zweig (für 32bit oder 64bit-Systeme)</param>
        /// <returns>Der Pfad zur Installation der Wawi</returns>
        private string FindUninstallSubkey(string baseKey)
        {
            var oKey = Registry.LocalMachine.OpenSubKey(baseKey);
            if (oKey == null) return null;
            return
                oKey.GetSubKeyNames()
                    .Select(oKey.OpenSubKey)
                    .Where(oSubKey => string.Equals("JTL-Wawi", oSubKey.GetValue("DisplayName")))
                    .Select(oSubKey => Convert.ToString(oSubKey.GetValue("InstallLocation")))
                    .FirstOrDefault();
        }

        /// <summary>
        /// Sucht der Registry (sowohl 32bit als auch 64bit) nach dem Installationsort für die Wawi
        /// </summary>
        /// <returns>Wawi-Installationsort</returns>
        private string FindInstallLocation()
        {
            var cLocation = FindUninstallSubkey(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"); //32bit
            if (!string.IsNullOrEmpty(cLocation))
                return cLocation;

            return FindUninstallSubkey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall"); //64bit
        }

        private bool ValidExternDllVersion()
        {
            string externDllPath = Path.Combine(_wawiPath, "JTLwawiExtern.dll");
            Version externDllVersion = new Version(FileVersionInfo.GetVersionInfo(externDllPath).FileVersion);
            Version eingebundeneExternDllVersion = new Version(1, 5, 33, 0); //Eingebette Version manuell pflegen
            if (externDllVersion < eingebundeneExternDllVersion)
                return false;
            return true;
        }

        private Assembly LoadAssemblys(object sender, ResolveEventArgs args)
        {
            string folderPath = Path.GetDirectoryName(this._wawiPath);
            string assemblyPath = Path.Combine(folderPath, string.Format("{0}.dll", new AssemblyName(args.Name).Name));
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}
