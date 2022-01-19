﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;


namespace Versanddatenimport
{
    public partial class Optionen : Form
    {
        public Optionen()
        {
            InitializeComponent();

            if (!System.IO.File.Exists(@"config.json"))
            {
                return;
            }

            ConfigLoader configLoader = new ConfigLoader();
            Config config = null;

            bool showErrors = false;
            config =  configLoader.loadConfig(showErrors);

            if(null != config)
            {
                textBoxDatabaseServer.Text = config.DatabaseServer;
                textBoxDatabaseName.Text = config.DatabaseName;
                textBoxDatabaseUser.Text = config.DatabaseUser;
                textBoxDatabasePassword.Text = config.DatabasePassword;
                textBoxJtlUserId.Text = config.JtlUserId.ToString();
                datetimePickerTimeFrom.Value = DateTime.Parse(config.timeFrom);
                datetimePickerTimeTo.Value = DateTime.Parse(config.timeTo);
                textBoxWorkerTimeIntervalInMinutes.Text = config.TimeIntervalInMinutes.ToString();
                textBoxFolderIncomingFiles.Text = config.folderIncomingFiles;
                textBoxFolderArchive.Text = config.folderArchive;
                textBoxFolderLog.Text = config.folderLog;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config config = new Config();

            config.DatabaseServer = textBoxDatabaseServer.Text;
            config.DatabaseName = textBoxDatabaseName.Text;
            config.DatabaseUser = textBoxDatabaseUser.Text;
            config.DatabasePassword = textBoxDatabasePassword.Text;

            //DatenbankUserId abfraen
            int jtl_user_id = 0;
            
            if(false == int.TryParse(textBoxJtlUserId.Text, out jtl_user_id))
            {
                MessageBox.Show("Ungültiger Wert für JTL Benutzer ID eingegeben.", "Fehler");
                return;
            }

            if(jtl_user_id <= 0)
            {
                MessageBox.Show("Ungültiger Wert für JTL Benutzer ID eingegeben.", "Fehler");
                return;
            }

            config.JtlUserId = jtl_user_id;

            //Zeit von abfragen
            config.timeFrom = datetimePickerTimeFrom.Value.ToString("HH:mm");

            //Zeit bis abfragen
            config.timeTo = datetimePickerTimeTo.Value.ToString("HH:mm");

            //Interval abfragen
            //ACHTUNG! Das Intervall läuft immer - Worker wird nur nicht ausgeführt, wenn er nicht innerhalb der oben angegebenen Zeitspanne liegt.
            int interval = 0;

            if(false == int.TryParse(textBoxWorkerTimeIntervalInMinutes.Text, out interval))
            {
                MessageBox.Show("Ungültiger Wert für das Zeitspanne in Minuten. Bitte nur Ganzzahlen eingeben!");
                return;
            }

            if(interval <= 0)
            {
                MessageBox.Show("Ungültiger Wert für das Zeitspanne in Minuten. Bitte nur Ganzzahlen eingeben!");
                return;
            }

            config.TimeIntervalInMinutes = interval;

            
            //Json erstellen.
            var options = new JsonSerializerOptions
            {
                WriteIndented = true        //Pretty Print for json
            };

            config.folderIncomingFiles = textBoxFolderIncomingFiles.Text;
            config.folderArchive = textBoxFolderArchive.Text;
            config.folderLog = textBoxFolderLog.Text;

            String json = JsonSerializer.Serialize(config, options);
            System.IO.File.WriteAllText(@"config.json", json);

            this.Close();

            //JsonSerializer.Deserialize(json, ???);
        }

        private void buttonChooseFolderIncomingFiles_Click(object sender, EventArgs e)
        {
            if(folderBrowserIncomingFiles.ShowDialog() == DialogResult.OK)
            {
                textBoxFolderIncomingFiles.Text = folderBrowserIncomingFiles.SelectedPath;
            }
        }

        private void buttonChooseFolderArchive_Click(object sender, EventArgs e)
        {
            if (folderBrowserArchive.ShowDialog() == DialogResult.OK)
            {
                textBoxFolderArchive.Text = folderBrowserArchive.SelectedPath;
            }
        }

        private void buttonChooseFolderLog_Click(object sender, EventArgs e)
        {
            if(folderBrowserLog.ShowDialog() == DialogResult.OK)
            {
                textBoxFolderLog.Text = folderBrowserLog.SelectedPath;
            }
        }
    }
}