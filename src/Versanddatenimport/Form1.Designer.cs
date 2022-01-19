
namespace Versanddatenimport
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonTestConnection = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.labelWorkerStatus = new System.Windows.Forms.Label();
            this.buttonStartWorker = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWorkerOutput = new System.Windows.Forms.TextBox();
            this.buttonSingleImport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTestConnection
            // 
            this.buttonTestConnection.Location = new System.Drawing.Point(353, 249);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new System.Drawing.Size(154, 30);
            this.buttonTestConnection.TabIndex = 0;
            this.buttonTestConnection.Text = "Verbindung testen";
            this.buttonTestConnection.UseVisualStyleBackColor = true;
            this.buttonTestConnection.Click += new System.EventHandler(this.buttonTestConnection_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem4});
            this.menuItem1.Text = "Datei";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
            this.menuItem4.Text = "Beenden";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3});
            this.menuItem2.Text = "Bearbeiten";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Optionen";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Status:";
            // 
            // labelWorkerStatus
            // 
            this.labelWorkerStatus.AutoSize = true;
            this.labelWorkerStatus.Location = new System.Drawing.Point(169, 31);
            this.labelWorkerStatus.Name = "labelWorkerStatus";
            this.labelWorkerStatus.Size = new System.Drawing.Size(113, 13);
            this.labelWorkerStatus.TabIndex = 2;
            this.labelWorkerStatus.Text = "Offline (nicht gestartet)";
            // 
            // buttonStartWorker
            // 
            this.buttonStartWorker.Location = new System.Drawing.Point(6, 26);
            this.buttonStartWorker.Name = "buttonStartWorker";
            this.buttonStartWorker.Size = new System.Drawing.Size(111, 23);
            this.buttonStartWorker.TabIndex = 3;
            this.buttonStartWorker.Text = "Worker starten";
            this.buttonStartWorker.UseVisualStyleBackColor = true;
            this.buttonStartWorker.Click += new System.EventHandler(this.buttonStartWorker_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxWorkerOutput);
            this.groupBox1.Controls.Add(this.buttonStartWorker);
            this.groupBox1.Controls.Add(this.labelWorkerStatus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 230);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Versanddaten-Worker";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Informationen zum laufenden Worker:";
            // 
            // textBoxWorkerOutput
            // 
            this.textBoxWorkerOutput.Location = new System.Drawing.Point(7, 82);
            this.textBoxWorkerOutput.Multiline = true;
            this.textBoxWorkerOutput.Name = "textBoxWorkerOutput";
            this.textBoxWorkerOutput.ReadOnly = true;
            this.textBoxWorkerOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxWorkerOutput.Size = new System.Drawing.Size(481, 142);
            this.textBoxWorkerOutput.TabIndex = 4;
            // 
            // buttonSingleImport
            // 
            this.buttonSingleImport.Location = new System.Drawing.Point(13, 249);
            this.buttonSingleImport.Name = "buttonSingleImport";
            this.buttonSingleImport.Size = new System.Drawing.Size(117, 30);
            this.buttonSingleImport.TabIndex = 5;
            this.buttonSingleImport.Text = "Einzelimport";
            this.buttonSingleImport.UseVisualStyleBackColor = true;
            this.buttonSingleImport.Click += new System.EventHandler(this.buttonSingleImport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 292);
            this.Controls.Add(this.buttonSingleImport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonTestConnection);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Versanddaten-Import by Mindfav";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTestConnection;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelWorkerStatus;
        private System.Windows.Forms.Button buttonStartWorker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWorkerOutput;
        private System.Windows.Forms.Button buttonSingleImport;
    }
}

