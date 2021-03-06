using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using Microsoft.DeepZoomTools;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.InteropServices;

/*

Copyright (c) 2014, Tomasz Neugebauer
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the author nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

 * OpenSeadragon license: OpenSeadragon-license.txt
 * DeepZoomTools.dll license:  DeepZoomToolsDLL-license.txt
 * PHP license: php-license.txt
 * Civetweb license: civetweb-license.txt
 * jQuery license: jQuery-MIT-license.txt
 * BioJS license: biojs-apache-license.txt
 */


namespace DDV
{
	public class Form1 : System.Windows.Forms.Form
    {
        public string[] layoutStyles = new string[] {"Full Height Columns (Original)", "Tiled Layout"};

        private IContainer components;
		private System.Windows.Forms.OpenFileDialog fDlgSourceSequence;
        private Random RandomClass = new Random();
		private System.Windows.Forms.Button btnBrowseSelectFASTA;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblDataLength;
		private static string m_strSourceFile;
        private static string m_strSourceBitmapFile;
        public static bool ready_for_deep_zoom;
        private System.Windows.Forms.Label lblSequenceName;
        private SaveFileDialog saveDialog;
        public ProgressBar progressBar1;
        private Label lblProgressText;
        private Label lblDNAViewer;
		public string read;
        public string EndOfSequence = "";
        public string refseq = "";
        public string gi = "";
        public string DDVseqID = "";
        public string m_strFinalDestinationFolder = "";

        public string sequenceName = "";

        //BitmapData glbl_bmd;
        //Bitmap glbl_b;
        private RichTextBox resultLogTextBox;
        private Label label5;
        private TextBox txtGI;
        private Button btnDownloadFASTA;
        private Label lblRefSeq;
        private Label label8;
        private Label lblSourceBitmapFilename;
        private Process m_prcCivetweb;
        private FolderBrowserDialog fDlgFinalDestination;
        private Button btnFinalDestinationFolder;
        private GroupBox groupBox3;
        private Label lblOutputPath;
        private Button btnGeneratedIntefaces;
        private LinkLabel lnkLatestInterface;
        private Label label2;
        private TextBox textBoxTileSize;
        private TextBox txtBoxY;
        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolTip toolTip1;
        private Label label7;
        private CheckBox chckIncludeDensity;
        private OpenFileDialog dlgImageFileSet;
        private RichTextBox txtBoxFASTAStats;
        private ToolStripMenuItem helpToolStripMenuItem;
        private Label label10;
        private Label label11;
        private Label label14;
        private Label label13;
        private Label label12;
        private GroupBox groupBox4;
        private Label label9;
        private Label label6;
        private Label label4;
        private Label label3;
        private Label label15;
        private Label lblSourceSequence;
        private TextBox txtBoxColumnWidth;
        private Label label16;
        private ComboBox layoutSelector;
        private Label label17;
        private GroupBox groupBox2;
        private Label label20;
        private TextBox txtBoxSequenceNameOverride;
        private ComboBox outputNaming;
        private Label label19;
        private Button btnUploadInterface;
        private Button button_generate_viz;
        private Button button_generate_python_viz;
        private Label label18;
        private OpenFileDialog fDlgPythonInterpreter;
        private Button button_run_deepzoom;


        protected const string _newline = "\r\n";

   		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			m_strSourceFile="";
            m_strSourceBitmapFile = "";
            ready_for_deep_zoom = false;

            // Modify ui element attributes after initialization
            this.layoutSelector.DataSource = layoutStyles;
            this.layoutSelector.SelectedIndex = TILED_LAYOUT;
            this.txtBoxColumnWidth.Text = columnWidthInNucleotides.ToString();
            this.outputNaming.SelectedIndex = 1;

            button_generate_viz.Enabled = false;
            button_generate_python_viz.Enabled = false;
            button_run_deepzoom.Enabled = false;
            checkEnvironment();
            launchCivetweb();
            SetFinalDestinationFolder(@Directory.GetCurrentDirectory() + "\\output\\");
            btnGeneratedIntefaces.Enabled = true;
            
          
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            
             if (m_prcCivetweb != null)
                {
                    if (!m_prcCivetweb.HasExited)
                    {
                        killCivetweb();
                    }
                }
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

           
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.fDlgSourceSequence = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseSelectFASTA = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtBoxSequenceNameOverride = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDownloadFASTA = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGI = new System.Windows.Forms.TextBox();
            this.lblSourceSequence = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSourceBitmapFilename = new System.Windows.Forms.Label();
            this.lblDataLength = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxFASTAStats = new System.Windows.Forms.RichTextBox();
            this.lblRefSeq = new System.Windows.Forms.Label();
            this.lblSequenceName = new System.Windows.Forms.Label();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.lblDNAViewer = new System.Windows.Forms.Label();
            this.resultLogTextBox = new System.Windows.Forms.RichTextBox();
            this.fDlgFinalDestination = new System.Windows.Forms.FolderBrowserDialog();
            this.btnFinalDestinationFolder = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnUploadInterface = new System.Windows.Forms.Button();
            this.lnkLatestInterface = new System.Windows.Forms.LinkLabel();
            this.btnGeneratedIntefaces = new System.Windows.Forms.Button();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.chckIncludeDensity = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTileSize = new System.Windows.Forms.TextBox();
            this.txtBoxY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dlgImageFileSet = new System.Windows.Forms.OpenFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_run_deepzoom = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.button_generate_python_viz = new System.Windows.Forms.Button();
            this.button_generate_viz = new System.Windows.Forms.Button();
            this.outputNaming = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.layoutSelector = new System.Windows.Forms.ComboBox();
            this.txtBoxColumnWidth = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fDlgPythonInterpreter = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowseSelectFASTA
            // 
            this.btnBrowseSelectFASTA.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnBrowseSelectFASTA.Location = new System.Drawing.Point(149, 75);
            this.btnBrowseSelectFASTA.Name = "btnBrowseSelectFASTA";
            this.btnBrowseSelectFASTA.Size = new System.Drawing.Size(179, 24);
            this.btnBrowseSelectFASTA.TabIndex = 5;
            this.btnBrowseSelectFASTA.Text = "Browse/Select Local FASTA File";
            this.btnBrowseSelectFASTA.UseVisualStyleBackColor = false;
            this.btnBrowseSelectFASTA.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.txtBoxSequenceNameOverride);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnDownloadFASTA);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtGI);
            this.groupBox1.Controls.Add(this.lblSourceSequence);
            this.groupBox1.Controls.Add(this.btnBrowseSelectFASTA);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(512, 224);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gene Sequence Source File";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(47, 153);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(152, 13);
            this.label20.TabIndex = 43;
            this.label20.Text = "Sequence Name Override";
            // 
            // txtBoxSequenceNameOverride
            // 
            this.txtBoxSequenceNameOverride.Location = new System.Drawing.Point(50, 169);
            this.txtBoxSequenceNameOverride.Name = "txtBoxSequenceNameOverride";
            this.txtBoxSequenceNameOverride.Size = new System.Drawing.Size(277, 20);
            this.txtBoxSequenceNameOverride.TabIndex = 42;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(44, 102);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 13);
            this.label15.TabIndex = 41;
            this.label15.Text = "Sequence File:";
            this.label15.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(8, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 25);
            this.label11.TabIndex = 40;
            this.label11.Text = "1)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(230, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 13);
            this.label10.TabIndex = 39;
            this.label10.Text = "OR";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label7.Location = new System.Drawing.Point(44, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(391, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Enter GI and download FASTA file from NIH or browse for local source FASTA file";
            // 
            // btnDownloadFASTA
            // 
            this.btnDownloadFASTA.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDownloadFASTA.Location = new System.Drawing.Point(259, 32);
            this.btnDownloadFASTA.Name = "btnDownloadFASTA";
            this.btnDownloadFASTA.Size = new System.Drawing.Size(176, 23);
            this.btnDownloadFASTA.TabIndex = 37;
            this.btnDownloadFASTA.Text = "Download FASTA File From NIH";
            this.btnDownloadFASTA.UseVisualStyleBackColor = false;
            this.btnDownloadFASTA.Click += new System.EventHandler(this.button13_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(8, 153);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 25);
            this.label12.TabIndex = 41;
            this.label12.Text = "2)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "NIH GI:";
            // 
            // txtGI
            // 
            this.txtGI.Location = new System.Drawing.Point(93, 34);
            this.txtGI.Name = "txtGI";
            this.txtGI.Size = new System.Drawing.Size(160, 20);
            this.txtGI.TabIndex = 35;
            // 
            // lblSourceSequence
            // 
            this.lblSourceSequence.Location = new System.Drawing.Point(122, 102);
            this.lblSourceSequence.Name = "lblSourceSequence";
            this.lblSourceSequence.Size = new System.Drawing.Size(370, 54);
            this.lblSourceSequence.TabIndex = 6;
            this.lblSourceSequence.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Image file:";
            this.label8.Visible = false;
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // lblSourceBitmapFilename
            // 
            this.lblSourceBitmapFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSourceBitmapFilename.Location = new System.Drawing.Point(79, 132);
            this.lblSourceBitmapFilename.Name = "lblSourceBitmapFilename";
            this.lblSourceBitmapFilename.Size = new System.Drawing.Size(381, 43);
            this.lblSourceBitmapFilename.TabIndex = 38;
            this.lblSourceBitmapFilename.Visible = false;
            // 
            // lblDataLength
            // 
            this.lblDataLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDataLength.Location = new System.Drawing.Point(190, 134);
            this.lblDataLength.Name = "lblDataLength";
            this.lblDataLength.Size = new System.Drawing.Size(255, 56);
            this.lblDataLength.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 193);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "FASTA Stats:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(176, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Number of Base Pairs/Data Length:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "RefSeq:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Sequence Name:";
            // 
            // txtBoxFASTAStats
            // 
            this.txtBoxFASTAStats.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxFASTAStats.BackColor = System.Drawing.SystemColors.Control;
            this.txtBoxFASTAStats.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBoxFASTAStats.Location = new System.Drawing.Point(85, 193);
            this.txtBoxFASTAStats.Name = "txtBoxFASTAStats";
            this.txtBoxFASTAStats.ReadOnly = true;
            this.txtBoxFASTAStats.Size = new System.Drawing.Size(365, 307);
            this.txtBoxFASTAStats.TabIndex = 35;
            this.txtBoxFASTAStats.Text = "";
            // 
            // lblRefSeq
            // 
            this.lblRefSeq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRefSeq.Location = new System.Drawing.Point(60, 75);
            this.lblRefSeq.Name = "lblRefSeq";
            this.lblRefSeq.Size = new System.Drawing.Size(385, 37);
            this.lblRefSeq.TabIndex = 36;
            // 
            // lblSequenceName
            // 
            this.lblSequenceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSequenceName.Location = new System.Drawing.Point(102, 16);
            this.lblSequenceName.Name = "lblSequenceName";
            this.lblSequenceName.Size = new System.Drawing.Size(343, 48);
            this.lblSequenceName.TabIndex = 16;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(66, 515);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(458, 23);
            this.progressBar1.TabIndex = 21;
            // 
            // lblProgressText
            // 
            this.lblProgressText.AutoSize = true;
            this.lblProgressText.Location = new System.Drawing.Point(32, 544);
            this.lblProgressText.Name = "lblProgressText";
            this.lblProgressText.Size = new System.Drawing.Size(28, 13);
            this.lblProgressText.TabIndex = 24;
            this.lblProgressText.Text = "Log:";
            // 
            // lblDNAViewer
            // 
            this.lblDNAViewer.Location = new System.Drawing.Point(9, 515);
            this.lblDNAViewer.Name = "lblDNAViewer";
            this.lblDNAViewer.Size = new System.Drawing.Size(51, 15);
            this.lblDNAViewer.TabIndex = 25;
            this.lblDNAViewer.Text = "Progress:";
            // 
            // resultLogTextBox
            // 
            this.resultLogTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.resultLogTextBox.Location = new System.Drawing.Point(66, 544);
            this.resultLogTextBox.Name = "resultLogTextBox";
            this.resultLogTextBox.ReadOnly = true;
            this.resultLogTextBox.Size = new System.Drawing.Size(458, 173);
            this.resultLogTextBox.TabIndex = 29;
            this.resultLogTextBox.Text = "";
            this.resultLogTextBox.TextChanged += new System.EventHandler(this.resultLogTextBox_TextChanged);
            // 
            // btnFinalDestinationFolder
            // 
            this.btnFinalDestinationFolder.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnFinalDestinationFolder.Location = new System.Drawing.Point(11, 86);
            this.btnFinalDestinationFolder.Name = "btnFinalDestinationFolder";
            this.btnFinalDestinationFolder.Size = new System.Drawing.Size(122, 27);
            this.btnFinalDestinationFolder.TabIndex = 32;
            this.btnFinalDestinationFolder.Text = "Browse Output Folder";
            this.btnFinalDestinationFolder.UseVisualStyleBackColor = false;
            this.btnFinalDestinationFolder.Click += new System.EventHandler(this.btnFinalDestinationFolder_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnUploadInterface);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblSourceBitmapFilename);
            this.groupBox3.Controls.Add(this.lnkLatestInterface);
            this.groupBox3.Controls.Add(this.btnGeneratedIntefaces);
            this.groupBox3.Controls.Add(this.lblOutputPath);
            this.groupBox3.Controls.Add(this.btnFinalDestinationFolder);
            this.groupBox3.Location = new System.Drawing.Point(530, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(466, 178);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output";
            // 
            // btnUploadInterface
            // 
            this.btnUploadInterface.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnUploadInterface.Enabled = false;
            this.btnUploadInterface.Location = new System.Drawing.Point(11, 53);
            this.btnUploadInterface.Name = "btnUploadInterface";
            this.btnUploadInterface.Size = new System.Drawing.Size(151, 27);
            this.btnUploadInterface.TabIndex = 43;
            this.btnUploadInterface.Text = "Upload Latest Interface";
            this.btnUploadInterface.UseVisualStyleBackColor = false;
            this.btnUploadInterface.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // lnkLatestInterface
            // 
            this.lnkLatestInterface.AutoSize = true;
            this.lnkLatestInterface.Enabled = false;
            this.lnkLatestInterface.Location = new System.Drawing.Point(213, 19);
            this.lnkLatestInterface.MaximumSize = new System.Drawing.Size(245, 25);
            this.lnkLatestInterface.Name = "lnkLatestInterface";
            this.lnkLatestInterface.Size = new System.Drawing.Size(110, 13);
            this.lnkLatestInterface.TabIndex = 42;
            this.lnkLatestInterface.TabStop = true;
            this.lnkLatestInterface.Text = "Open Latest Interface";
            this.lnkLatestInterface.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLatestInterface_LinkClicked);
            // 
            // btnGeneratedIntefaces
            // 
            this.btnGeneratedIntefaces.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGeneratedIntefaces.Enabled = false;
            this.btnGeneratedIntefaces.Location = new System.Drawing.Point(11, 19);
            this.btnGeneratedIntefaces.Name = "btnGeneratedIntefaces";
            this.btnGeneratedIntefaces.Size = new System.Drawing.Size(196, 28);
            this.btnGeneratedIntefaces.TabIndex = 41;
            this.btnGeneratedIntefaces.Text = "Open Previous Generated Interfaces";
            this.btnGeneratedIntefaces.UseVisualStyleBackColor = false;
            this.btnGeneratedIntefaces.Click += new System.EventHandler(this.btnGeneratedIntefaces_Click);
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOutputPath.Location = new System.Drawing.Point(139, 83);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(321, 49);
            this.lblOutputPath.TabIndex = 40;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(6, 161);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 25);
            this.label14.TabIndex = 43;
            this.label14.Text = "4)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(8, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 25);
            this.label13.TabIndex = 42;
            this.label13.Text = "3)";
            // 
            // chckIncludeDensity
            // 
            this.chckIncludeDensity.AutoSize = true;
            this.chckIncludeDensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chckIncludeDensity.Location = new System.Drawing.Point(50, 72);
            this.chckIncludeDensity.Name = "chckIncludeDensity";
            this.chckIncludeDensity.Size = new System.Drawing.Size(161, 17);
            this.chckIncludeDensity.TabIndex = 40;
            this.chckIncludeDensity.Text = "Include Density Service";
            this.chckIncludeDensity.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(45, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Tile Size 1-1048";
            // 
            // textBoxTileSize
            // 
            this.textBoxTileSize.Location = new System.Drawing.Point(48, 177);
            this.textBoxTileSize.Name = "textBoxTileSize";
            this.textBoxTileSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxTileSize.TabIndex = 31;
            this.textBoxTileSize.Text = "256";
            this.toolTip1.SetToolTip(this.textBoxTileSize, "Recommended: 144 for bacteria, 256 for human");
            // 
            // txtBoxY
            // 
            this.txtBoxY.Location = new System.Drawing.Point(217, 45);
            this.txtBoxY.Name = "txtBoxY";
            this.txtBoxY.Size = new System.Drawing.Size(100, 20);
            this.txtBoxY.TabIndex = 30;
            this.txtBoxY.Text = "3000";
            this.toolTip1.SetToolTip(this.txtBoxY, "Recommended: 3000 for bacteria, 20000 for human");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(214, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Column Height";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSize = true;
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox4.Controls.Add(this.button_run_deepzoom);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.button_generate_python_viz);
            this.groupBox4.Controls.Add(this.button_generate_viz);
            this.groupBox4.Controls.Add(this.outputNaming);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.layoutSelector);
            this.groupBox4.Controls.Add(this.txtBoxColumnWidth);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.chckIncludeDensity);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.textBoxTileSize);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.txtBoxY);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(12, 257);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(512, 252);
            this.groupBox4.TabIndex = 41;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Generate DNA Visualization";
            // 
            // button_run_deepzoom
            // 
            this.button_run_deepzoom.BackColor = System.Drawing.Color.SkyBlue;
            this.button_run_deepzoom.Enabled = false;
            this.button_run_deepzoom.Location = new System.Drawing.Point(430, 203);
            this.button_run_deepzoom.Name = "button_run_deepzoom";
            this.button_run_deepzoom.Size = new System.Drawing.Size(76, 30);
            this.button_run_deepzoom.TabIndex = 55;
            this.button_run_deepzoom.Text = "DeepZoom";
            this.button_run_deepzoom.UseVisualStyleBackColor = false;
            this.button_run_deepzoom.Click += new System.EventHandler(this.button_generate_python_viz_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(204, 212);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 13);
            this.label18.TabIndex = 54;
            this.label18.Text = "OR";
            // 
            // button_generate_python_viz
            // 
            this.button_generate_python_viz.BackColor = System.Drawing.Color.SkyBlue;
            this.button_generate_python_viz.Enabled = false;
            this.button_generate_python_viz.Location = new System.Drawing.Point(233, 203);
            this.button_generate_python_viz.Name = "button_generate_python_viz";
            this.button_generate_python_viz.Size = new System.Drawing.Size(197, 30);
            this.button_generate_python_viz.TabIndex = 53;
            this.button_generate_python_viz.Text = "Generate Visualization in Python";
            this.button_generate_python_viz.UseVisualStyleBackColor = false;
            this.button_generate_python_viz.Click += new System.EventHandler(this.button_generate_python_viz_Click);
            // 
            // button_generate_viz
            // 
            this.button_generate_viz.BackColor = System.Drawing.Color.LightSkyBlue;
            this.button_generate_viz.Enabled = false;
            this.button_generate_viz.Location = new System.Drawing.Point(47, 203);
            this.button_generate_viz.Name = "button_generate_viz";
            this.button_generate_viz.Size = new System.Drawing.Size(151, 30);
            this.button_generate_viz.TabIndex = 52;
            this.button_generate_viz.Text = "Generate Visualization";
            this.button_generate_viz.UseVisualStyleBackColor = false;
            this.button_generate_viz.Click += new System.EventHandler(this.button_generate_viz_Click);
            // 
            // outputNaming
            // 
            this.outputNaming.FormattingEnabled = true;
            this.outputNaming.Items.AddRange(new object[] {
            "GI",
            "Name"});
            this.outputNaming.Location = new System.Drawing.Point(48, 120);
            this.outputNaming.Name = "outputNaming";
            this.outputNaming.Size = new System.Drawing.Size(121, 21);
            this.outputNaming.TabIndex = 51;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(45, 104);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(91, 13);
            this.label19.TabIndex = 50;
            this.label19.Text = "Output Naming";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(47, 29);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(45, 13);
            this.label17.TabIndex = 47;
            this.label17.Text = "Layout";
            // 
            // layoutSelector
            // 
            this.layoutSelector.FormattingEnabled = true;
            this.layoutSelector.Location = new System.Drawing.Point(50, 45);
            this.layoutSelector.Name = "layoutSelector";
            this.layoutSelector.Size = new System.Drawing.Size(161, 21);
            this.layoutSelector.TabIndex = 46;
            this.layoutSelector.SelectedIndexChanged += new System.EventHandler(this.layoutSelector_SelectedIndexChanged);
            // 
            // txtBoxColumnWidth
            // 
            this.txtBoxColumnWidth.Enabled = false;
            this.txtBoxColumnWidth.Location = new System.Drawing.Point(333, 46);
            this.txtBoxColumnWidth.Name = "txtBoxColumnWidth";
            this.txtBoxColumnWidth.Size = new System.Drawing.Size(100, 20);
            this.txtBoxColumnWidth.TabIndex = 45;
            this.txtBoxColumnWidth.Text = "100";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(330, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(85, 13);
            this.label16.TabIndex = 44;
            this.label16.Text = "Column Width";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtBoxFASTAStats);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblDataLength);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.lblSequenceName);
            this.groupBox2.Controls.Add(this.lblRefSeq);
            this.groupBox2.Location = new System.Drawing.Point(530, 211);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(466, 506);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sequence Properties";
            // 
            // fDlgPythonInterpreter
            // 
            this.fDlgPythonInterpreter.Filter = "Python Interpreter|python.exe";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblDNAViewer);
            this.Controls.Add(this.lblProgressText);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.resultLogTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "DNA Data Visualization Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}


		 private static void checkEnvironment(){

             string strDotNetVersion = Environment.Version.ToString();
             int iDotNetVersion = Environment.Version.Major;

             if (iDotNetVersion < 4)
             {
                 MessageBox.Show("Error: Attempt to run with .NET Framework "+strDotNetVersion+" \nThis application requires .NET Framework Version 4 or higher. \nTo run this software, please donwload and install .NET Framework 4 at http://www.microsoft.com/en-ca/download/details.aspx?id=17851");
             }

        }

		

		private void button2_Click_1(object sender, System.EventArgs e)
		{
			DialogResult dr = fDlgSourceSequence.ShowDialog();

			if (dr == DialogResult.OK)
			{
                if (fDlgSourceSequence.FileName == "") { 
                    MessageBox.Show("Please select source file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    return; 
                }
                
                //copy the file over to the output folder as "sequence.fasta"
                string strDestination = @Directory.GetCurrentDirectory() + "\\output\\sequence.fasta";
                //check if file already in correct place, if not, move it into output folder
                if (!(fDlgSourceSequence.FileName == strDestination))
                {
                    //if file exists, delete it
                    if (File.Exists(strDestination))
                    {
                        File.Delete(strDestination);
                    }
                    //if output Directory not there, make it
                    if (!(Directory.Exists(@Directory.GetCurrentDirectory() + "\\output")))
                    {
                        Directory.CreateDirectory(@Directory.GetCurrentDirectory() + "\\output");
                    }
                    //copy the file
                    File.Copy(fDlgSourceSequence.FileName, strDestination);
                    MessageBoxClear();
                    MessageBoxShow("Copied selected sequence to output folder.");
                }
                lblSourceSequence.Text = fDlgSourceSequence.FileName + " Selected";
                CleanInterfaceNewSequence();
                txtBoxSequenceNameOverride.Text = Path.GetFileNameWithoutExtension(fDlgSourceSequence.FileName);
                fDlgSourceSequence.FileName = strDestination;
                SetSourceSequence(fDlgSourceSequence.FileName);
                txtGI.Text = "";
                BitmapClear();
                
			}
		}


        public int columnWidthInNucleotides = 100;
        public const int FULL_COLUMN_LAYOUT = 0;
        public const int TILED_LAYOUT = 1;
        public long sequenceLength = 0;
        public bool multipart_file = false;
        public Dictionary<char, long> letter_counts = new Dictionary<char, long>() {
             {'A', 0}, {'G', 0}, {'T', 0}, {'C', 0}, {'N', 0}, {'R', 0}, {'Y', 0}, {'S', 0}, {'W', 0},
             {'K', 0}, {'M', 0}, {'B', 0}, {'D', 0}, {'H', 0}, {'V', 0}
        };

        public int ipT = 0;
        public int ipA = 0;
        public int ipG = 0;
        public int ipC = 0;
        
        public int ipR = 0;
        public int ipY = 0;
        public int ipS = 0;
        public int ipW = 0;
        public int ipK = 0;
        public int ipM = 0;
        public int ipB = 0;
        public int ipD = 0;
        public int ipH = 0;
        public int ipV = 0;
        public int ipN = 0;
        public int ipUnknown = 0;

        private long populateInfo(bool take_shortcuts)
        {
            if (m_strSourceFile == "") { 
                MessageBox.Show("Please select source file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                return (0); 
            }

            //check if FASTA file exists
            if (!File.Exists(m_strSourceFile))
            {
                MessageBox.Show("Could not locate FASTA file to process, please specify source file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return (0); 
            }
          
            StreamReader streamFASTAFile = File.OpenText(m_strSourceFile);

            int i = 0;
            bool bOnce = false;
            if (!take_shortcuts)
            {
                sequenceLength = 0;
            }else{
                multipart_file = true;
            }
            int iActualLineLength = 0;

            while (((read = streamFASTAFile.ReadLine()) != null) && (!take_shortcuts || !bOnce))
            {
                if (read == "")
                { //skip 
                }
                else
                {
                    if (read.Substring(0, 1) == ">")
                    {
                        if (i > 1) { //we've now hit the second entry in this fasta file
                            multipart_file = true;
                        }
                        else
                        {
                            if (txtBoxSequenceNameOverride.Text == "")
                            {
                                sequenceName = read;
                                sequenceName = sequenceName.Substring(1, sequenceName.Length - 1);
                            }
                            else
                            {
                                sequenceName = txtBoxSequenceNameOverride.Text;
                            }
                            lblSequenceName.Text = sequenceName;
                            lblSequenceName.Refresh();
                            //ref|NC_007414.1|
                            //[^\]
                            refseq = Regex.Match(sequenceName, @"ref\|(.*?)\|").Groups[1].Value;
                            if (refseq == "") { 
                                MessageBoxShow("Refseq not found in sequence file.");
                                lblRefSeq.Text = "False";
                            }
                            else { 
                               //string refseqAcc = Regex.Match(refseq, @"^([^\.]*)\.").Groups[1].Value;
                               lblRefSeq.Text = refseq;
                               MessageBoxShow("Refseq present in sequence data. ");
                            }
                            gi = Regex.Match(sequenceName, @"gi\|(.*?)\|").Groups[1].Value;
                            if (gi == "") {
                                DDVseqID="DDV"+DateTime.Now.ToString("yyyyMMddHHmmss");
                                MessageBoxShow("GI not found in sequence file.  Generated the following DDV ID for this sequence: "+DDVseqID);

                            }
                        }

                    }

                    else
                    {
                        read = read.ToUpper();

                        //for accounting:
                        if (take_shortcuts) { 
                            letter_counts = new Dictionary<char, long>() //just make sure legend shows up 
                            {
                                    {'A', 1}, {'G', 1}, {'T', 1}, {'C', 1}, {'N', 1}, {'R', 0}, {'Y', 0}, {'S', 0}, {'W', 0},
                                    {'K', 0}, {'M', 0}, {'B', 0}, {'D', 0}, {'H', 0}, {'V', 0}
                            };
                        }
                        else
                        {
                            CountOccurencesOfChar(read);
                        }
                        if (!bOnce) { 
                            iActualLineLength = read.Length; 
                            bOnce = true; 
                        }

                        sequenceLength += read.Trim().Length;
                       
                        i++;
                    }
                }
            }


            lblDataLength.Text = sequenceLength.ToString();
            lblDataLength.Text = lblDataLength.Text + " | Line Length: " + iActualLineLength.ToString();
            lblDataLength.Refresh();

            MessageBoxShow("Retrieved basic sequence properties from FASTA.");
            streamFASTAFile.Close();
            return sequenceLength;

        }


        private void MessageBoxClear()
        {
            resultLogTextBox.Text = "";
            resultLogTextBox.Refresh();
        }

        private void MessageBoxShow(string strMessage)
        {
            //Log progress on the interface generator.
            resultLogTextBox.Text += strMessage + "\n";
            resultLogTextBox.Refresh();
        }

        private void SaveBMP(ref Bitmap bmp, string strPath) // now 'ref' parameter
        {
            try
            {
                bmp.Save(strPath, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {
                Bitmap bitmap = (Bitmap) bmp.Clone();
                bmp.Dispose();
                bitmap.Save(strPath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }


        private void InitializeMakeBitmap()
        {

            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            EndOfSequence = "";

            string strFastaStats = "";
            letter_counts = new Dictionary<char, long>()
            {
                 {'A', 0}, {'G', 0}, {'T', 0}, {'C', 0}, {'N', 0}, {'R', 0}, {'Y', 0}, {'S', 0}, {'W', 0},
                 {'K', 0}, {'M', 0}, {'B', 0}, {'D', 0}, {'H', 0}, {'V', 0}
            };

            ipT = 0; ipA = 0; ipG = 0; ipC = 0; ipR = 0; ipY = 0; ipS = 0; ipW = 0; ipK = 0; ipM = 0; ipB = 0;
            ipD = 0; ipH = 0; ipV = 0; ipN = 0;

            lblProgressText.Text = "";
            lblProgressText.Refresh();
            progressBar1.Value = 0;
            progressBar1.Update();
            progressBar1.Refresh();
            long counter = 0;
            if (m_strSourceFile == "") { MessageBox.Show("Please select source file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (!File.Exists(m_strSourceFile)) { MessageBox.Show("Could not find source FASTA file"+m_strSourceFile+". Please select source file", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            FileInfo TheFile = new FileInfo(m_strSourceFile);
            sequenceLength = TheFile.Length;

            sequenceLength = populateInfo(false); // sequenceLength > 300000000);

            if (sequenceLength == 0)
            {
                MessageBoxShow("Error.  Invalid FASTA file.  ");
                return;
            }

            int y = 0;
            try
            {
                y = Convert.ToInt32(txtBoxY.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Please put only numbers in Image height");
            }

            try
            {
                columnWidthInNucleotides = Convert.ToInt32(txtBoxColumnWidth.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Please put only numbers in Image height");
            }
            
            //int x = (((sequenceLength / y) / 60 * 2)) + (sequenceLength / y) + 64 * 2;
            //int x = 100+((((sequenceLength / (y/intMagnification)) / (columnWidthInNucleotides*intMagnification)) * 4) + (sequenceLength / (y/intMagnification)) + ((columnWidthInNucleotides+4)*intMagnification)) * intMagnification;

            int iPaddingBetweenColumns = 2;
            int iColumnWidth = columnWidthInNucleotides + iPaddingBetweenColumns;
            int iNucleotidesPerColumn = columnWidthInNucleotides * y;
            int numColumns = (int)Math.Ceiling((double)sequenceLength / iNucleotidesPerColumn);
            int x = (numColumns * iColumnWidth) - iPaddingBetweenColumns; //last column has no padding.

            if (layoutSelector.SelectedIndex == TILED_LAYOUT)  // New layout added by Josiah Seaman
            {
                DDVLayoutManager layout = new DDVLayoutManager();
                int[] xy = layout.max_dimensions(sequenceLength, multipart_file); //xy point of last pixel, gives us the largest boundaries
                columnWidthInNucleotides = layout.levels[0].modulo; //override user input to ensure consistency.  Set these values in code
                x = xy[0];
                y = xy[1];
            }
            else
            {
                MessageBoxShow("iColumnWidth: " + columnWidthInNucleotides);
                MessageBoxShow("iNucleotidesPerColumn: " + iNucleotidesPerColumn);
                MessageBoxShow("numColumns: " + numColumns);
                MessageBoxShow("x: " + x);
            }

            // Bitmap B = new Bitmap(x, y);
            string strMessage = "Initializing PNG width=" + x + " height=" + y;
            MessageBoxShow(strMessage);
            Bitmap b;
            try
            {
                b = new Bitmap(x, y, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                MessageBoxShow("Actual PNG width=" + b.Width + " height=" + b.Height);
            }
            catch (ArgumentException e)
            {
                MessageBoxShow(e.ToString());
                MessageBoxShow("Image is too large to allocate");
                return;
            }
            utils.SetMyPalette(ref b);
            BitmapData bmd;
            try
            {
                bmd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            }
            catch (Exception e)
            {
                MessageBoxShow(e.ToString());
                MessageBoxShow("Could not lock image section");
                return;
            }


            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    utils.UnsafeSetPixel(i, j, (byte)0, ref bmd);
                }
            }
            
            MessageBoxShow("PNG Initialized");

            // Lets get the length so that when we are reading we know
            // when we have hit a "milestone" and to update the progress bar.
            FileInfo fileSize = new FileInfo(m_strSourceFile);
            long size = fileSize.Length;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = (int)size;

            BackgroundWorker workerBMPPainter = new BackgroundWorker();
            DDVLayoutManager tile_layout = new DDVLayoutManager();
            workerBMPPainter.WorkerReportsProgress = true;
            workerBMPPainter.WorkerSupportsCancellation = true;
            workerBMPPainter.ProgressChanged += (u, args) =>
            {
                try
                {
                    progressBar1.Value = args.ProgressPercentage;
                }
                catch (ArgumentOutOfRangeException e) { }
            };
            workerBMPPainter.DoWork += (u, args) =>
            {
                BackgroundWorker worker = u as BackgroundWorker;
                int selectedIndex = (int)args.Argument;
                StreamReader streamFASTAFile = File.OpenText(m_strSourceFile);
                worker.ReportProgress(1);

                if (selectedIndex == TILED_LAYOUT)  // 1 is the Tiled option
                {
                    //reassigns Bitmap b to a new object
                    b = tile_layout.process_file(streamFASTAFile, worker, b, bmd, multipart_file);
                }
                if (selectedIndex == FULL_COLUMN_LAYOUT)  // 0 is the Full Height Columns (Original) option
                {
                    counter = full_column_layout(counter, iColumnWidth, worker, streamFASTAFile, b, bmd);
                }


                streamFASTAFile.Close(); //close the sequence file

                //----------------------------end of convert data to pixels--------------------------

            };
            workerBMPPainter.RunWorkerAsync(layoutSelector.SelectedIndex);
            workerBMPPainter.RunWorkerCompleted += (u, args) =>
            {
                MessageBoxShow("Completed mapping DNA to pixels.  Saving result...");

                string strResultFileName = "";
                if (outputNaming.SelectedIndex == 0)  // 0 is GI naming
                {
                    if (gi != "")
                    {
                        strResultFileName = gi + ".png";
                    }
                    else
                    {
                        strResultFileName = DDVseqID + ".png";
                    }
                }
                else if (outputNaming.SelectedIndex == 1 && txtBoxSequenceNameOverride.Text != "")  // 1 is Name naming
                {
                    sequenceName = txtBoxSequenceNameOverride.Text.Replace('.', '_');
                    strResultFileName = sequenceName + ".png";  
                }

                //if file exists, delete
                if (File.Exists(strResultFileName))
                {
                    MessageBoxShow("Removing temp version of PNG file.");
                    File.Delete(strResultFileName);
                }
                if (File.Exists("embed.html"))
                {
                    MessageBoxShow("Removing temp version of PNG file.");
                    File.Delete("embed.html");
                }
                try
                {
                    b.Save(strResultFileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    throw new System.Exception("Error.  Could not save PNG file.  ");
                }
                b.Dispose();
                MessageBoxShow("File saved " + strResultFileName);

                string sourceFile = strResultFileName;
                string destinationFile = TheFile.DirectoryName + Path.DirectorySeparatorChar + strResultFileName;
                MoveWithReplace(sourceFile, destinationFile);
                MessageBoxShow("File moved to -> " + destinationFile);
                BitmapSet(destinationFile);


                if (counter != 0) { MessageBoxShow("Error: Number of Times Max Width Reached:" + counter.ToString()); }
                
                int ipTotal = ipA + ipT + ipG + ipC + ipR + ipY + ipS + ipW + ipK + ipM + ipB + ipD + ipH + ipV + ipN;
                strFastaStats = "FASTA Stats for " + sequenceName +
                //"\nUnknown:" + iUnknown + " processed: " + ipUnknown +
                "\niTotal:" + sequenceLength + " ipTotal: " + ipTotal +
                "\n---------------------------------------\n";
                txtBoxFASTAStats.Text = strFastaStats;
                strFastaStats += resultLogTextBox.Text;
               

                FileInfo f = new FileInfo(m_strFinalDestinationFolder + "//sequence.fasta");
                long direct_data_file_length = f.Length;

                //embed.html
                strResultFileName = "embed.html";
                FileInfo t = new FileInfo(strResultFileName);
                StreamWriter Tex = t.CreateText();
                StringWriter wr = new StringWriter();
                string embedHTML = @"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
<title>DNA Data Visualization : " + sequenceName + @"</title>
<script src='../../openseadragon.js' type='text/javascript'></script>
<script type='text/javascript' src='../../jquery-1.7.min.js'></script>
<script src='../../openseadragon-scalebar.js' type='text/javascript'></script>

<script type='text/javascript'>
	        var originalImageWidth= " + x + @";
            var originalImageHeight= " + y + @";
            var ColumnPadding = " + iPaddingBetweenColumns + @";
            var columnWidthInNucleotides = " + columnWidthInNucleotides + @";
            var layoutSelector = " + layoutSelector.SelectedIndex + @";
            var layout_levels = " + tile_layout.ToString() + @";
            var ContigSpacingJSON = " + tile_layout.ContigSpacingJSON() + @";
            var multipart_file = " + multipart_file.ToString().ToLower() + @";
            var includeDensity = " + chckIncludeDensity.Checked.ToString().ToLower() + @";

            var usa='refseq_fetch:" + refseq + @"';          
            var ipTotal = " + ipTotal + @";
            var direct_data_file='sequence.fasta';
            var direct_data_file_length=" + direct_data_file_length + @";
            var sbegin='1';
            var send=" + ipTotal.ToString() + @";
            var output_dir = '../../';
            </script>
<script src='../../nucleotideNumber.js' type='text/javascript'></script>
<script src='../../nucleicDensity.js' type='text/javascript'></script>";
                embedHTML = embedHTML + @"<link rel='stylesheet' type='text/css' href='../../seadragon.css' />
    <!-- BIOJS css -->
	<script language='JavaScript' type='text/javascript' src='../../Biojs.js'></script>
	<!-- component code -->
	<script language='JavaScript' type='text/javascript' src='../../Biojs.Sequence.js'></script>
</head>

<body>
<h2 class='mainTitle'>Data Visualization - DNA</h2>
<span style='float:left;'>Menu:&nbsp;</span>
	<ul class='selectChromosome'>
	<li><a href='../'>Select Visualization</a></li>
	 </ul>
<h2 class='mainTitle'><strong>" + sequenceName +
                            @"</strong> 
 </h2>

<div id='container' class='chromosome-container' data-chr-source=''>
</div>

<p class='legendHeading'><strong>Legend:</strong><br /></p><div style='margin-left:50px;'>";
                if (letter_counts['A'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-A.png' />"; }
                if (letter_counts['T'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-T.png' />"; }
                //TODO: Add legend for U = Uracil
                if (letter_counts['G'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-G.png' />"; }
                if (letter_counts['C'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-C.png' />"; }
                if (letter_counts['R'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-R.png' />"; }
                if (letter_counts['Y'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-Y.png' />"; }
                if (letter_counts['S'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-S.png' />"; }
                if (letter_counts['W'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-W.png' />"; }
                if (letter_counts['K'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-K.png' />"; }
                if (letter_counts['M'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-M.png' />"; }
                if (letter_counts['B'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-B.png' />"; }
                if (letter_counts['D'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-D.png' />"; }
                if (letter_counts['H'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-H.png' />"; }
                if (letter_counts['V'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-V.png' />"; }
                if (letter_counts['N'] != 0) { embedHTML = embedHTML + @"<img src='../../LEGEND-N.png' />"; }
                embedHTML = embedHTML + @"<img src='../../LEGEND-bg.png' /></div>

<script type='text/javascript'>
	        outputTable();
            if (includeDensity) { outputDensityUI();}
</script>

<div class='legend-details'>
<h3>Data Source:</h3>
<a href='sequence.fasta'>Download FASTA file</a>
<br />";


                if (gi != "")
                {
                    embedHTML = embedHTML + @"
NCBI (gi): <a href='http://www.ncbi.nlm.nih.gov/nuccore/" + gi + @"'>http://www.ncbi.nlm.nih.gov/nuccore/" + gi + @"</a><br />";
                }
                else
                    {
                        embedHTML = embedHTML + @"
Custom/local sequence (DDV seq ID): "+DDVseqID+"<br />";
                    } 

                embedHTML = embedHTML + @"

<h3>Notes</h3>
This DNA data visualization interface was generated with <a href='https://github.com/photomedia/DDV'>DDV</a><br />Date Visualization Created:" + DateTime.Now.ToString("d/MM/yyyy") + @"
<script type='text/javascript'>
	        otherCredits();
</script>
</div>
</body>

</html>
            ";
                Tex.Write(embedHTML);
                MessageBoxShow("File generated " + strResultFileName);
                wr.Close();
                Tex.Close();

                //move file
                sourceFile = strResultFileName;
                destinationFile = TheFile.DirectoryName + Path.DirectorySeparatorChar + strResultFileName;
                MoveWithReplace(sourceFile, destinationFile);
                MessageBoxShow("File moved to -> " + destinationFile);

                //end of embed.html
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;

                MessageBoxShow("Completed.");
                MessageBoxShow("Image and interface files generated. Processing Deep Zoom...");
                //enable deepzoomprocessing
                button_generate_viz.Enabled = true;
                if (layoutSelector.SelectedIndex == 1)
                {
                    button_generate_python_viz.Enabled = true;
                    button_run_deepzoom.Enabled = true;
                }

                
            };
           
        }

        private long full_column_layout(long counter, int iColumnWidth, BackgroundWorker worker, StreamReader streamFASTAFile, Bitmap b, BitmapData bmd)
        {
            //----------------------------convert data to pixels---------------------------------
            int boundX = b.Width;
            int boundY = b.Height;
            int nucleotidesInThisLine = 0;

            counter = 0;
            bool end = false;
            string firstLetter = null;

            int x_pointer = 0;
            int y_pointer = 0;
            int lineBeginning = x_pointer;
            int progress = 0;

            while (((read = streamFASTAFile.ReadLine()) != null) && !end)
            {
                if (read == "")
                { //skip 
                }
                else
                {
                    progress += (int)read.Length;
                    worker.ReportProgress(progress);

                    firstLetter = read.Substring(0, 1);

                    if (firstLetter == ">")
                    {

                    }

                    else
                    {
                        read = utils.CleanInputFile(read);
                        read = utils.ConvertToDigits(read);


                        //------------Classic Long column Layout--------------/
                        for (int c = 0; c < read.Length; c++)
                        {
                            utils.Write1BaseToBMP(read[c], ref b, x_pointer, y_pointer, ref bmd);
                            x_pointer++; //increment one pixel size to the right
                            nucleotidesInThisLine += 1;

                            if (nucleotidesInThisLine >= columnWidthInNucleotides) // carriage return
                            {
                                nucleotidesInThisLine = 0;
                                x_pointer = lineBeginning;
                                y_pointer++;

                                if (y_pointer >= boundY)
                                {
                                    x_pointer += iColumnWidth;
                                    lineBeginning = x_pointer;
                                    y_pointer = 0;
                                }

                                if (x_pointer >= boundX)
                                {
                                    counter++;
                                    end = true;
                                    //this would be an unexpected error, throw an exception
                                    throw new System.Exception("Unexpected error while converting data.  Attempt to paint a pixel outside of image bounds. Please review the parameters and ensure the data is in FASTA format.");
                                }
                            }
                        }
                    }
                }
            }
            b.UnlockBits(bmd);
            bmd = null;
            return counter;
        }

        //Generates Bitmap from Data
        private void generate_image_and_interface(object sender, EventArgs e)
        {
            if (File.Exists(m_strFinalDestinationFolder + "\\embed.html"))
            {
                File.Delete(m_strFinalDestinationFolder + "\\embed.html");
            }
            foreach (string filePath in Directory.GetFiles(m_strFinalDestinationFolder))
            {
                if (filePath.EndsWith(".png"))
                {
                    File.Delete(filePath);
                }
            }

            try
            {
                InitializeMakeBitmap();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Path is a null reference.");
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("The caller does not have the required permission.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("ArgumentException Error has occurred.  ");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Error.  IO Exception has occurred.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Processing failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
            }
            

        }

       

        public void CountOccurencesOfChar(string instance)
        {
            foreach (char curChar in instance)
            {
                letter_counts[curChar]++;
            }
        }

        public static void CopyDirectory(string source, string destination)
        {
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                destination += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            var entries = Directory.GetFileSystemEntries(source);
            foreach (var e in entries)
            {
                if (Directory.Exists(e))
                {
                    CopyDirectory(e, destination + Path.GetFileName(e));
                }
                else
                {
                    File.Copy(e, destination + Path.GetFileName(e), true);
                }
            }
        }

        public static void MoveWithReplace(string sourceFileName, string destFileName)
        {
            //check if the source is not the same as destination
            if (sourceFileName != destFileName)
            {
                //first, delete target file if exists, as File.Move() does not support overwrite
                if (File.Exists(destFileName))
                {
                    File.Delete(destFileName);
                }

                File.Move(sourceFileName, destFileName);
            }

        }

        public static void CopyFileWithReplaceIfNewer(string sourceFileName, string destFileName)
        {
            //check if the source is not the same as destination
            if (sourceFileName != destFileName)
            {
                FileInfo file = new FileInfo(sourceFileName);
                FileInfo destFile = new FileInfo(destFileName);
                if (destFile.Exists)
                {
                    if (file.LastWriteTime > destFile.LastWriteTime)
                    {
                        //MOVE File with Overwrite since it is newer
                        File.Copy(sourceFileName, destFileName, true);
                    }
                }
                else
                {
                    //DestFile does not exist, so just move it there
                    File.Copy(sourceFileName, destFileName);
                }
            }

        }

        public static void CopyFileNoReplace(string sourceFileName, string destFileName)
        {
            if (!File.Exists(destFileName))
            {
                File.Copy(sourceFileName, destFileName, false);
            } 
        }

        public static void MoveDirectory(string source, string destination)
        {
            CopyDirectory(source, destination);
            Directory.Delete(source, true);
        }

        //Process Bitmap with DeepZoomTools.dll
        //calls on the DeepZoomTools.dll to generate the deepzoom tiles
        //Tile size default is 256, but user can change it using txtTileSize
        //After it is complete, it moves all of the files into proper subfolders
        //renames "embed.html" file to index.html
        private void process_deep_zoom(object sender, EventArgs e)
        {
            BackgroundWorker waitForImage = new BackgroundWorker();
            waitForImage.DoWork += (u, args) =>
            {
                while (!ready_for_deep_zoom)
                {
                    System.Threading.Thread.Sleep(5000);
                    //MessageBoxShow(".");//TODO: remove this line
                }

            };
            waitForImage.RunWorkerAsync();
            waitForImage.RunWorkerCompleted += (u, args) =>
            {

                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 40;
                progressBar1.Value += 1;

                //move source image into source folder
                // Remove path from the file name.
                string fName = System.IO.Path.GetFileName(m_strSourceBitmapFile);
                string fNameNoExtension = System.IO.Path.GetFileNameWithoutExtension(m_strSourceBitmapFile);
                string fPathName = System.IO.Path.GetDirectoryName(m_strSourceBitmapFile);
                //string finalDestinationPath = m_strFinalDestinationFolder; 
                string fImageDestinationPath = @Directory.GetCurrentDirectory() + "\\source\\"; ;
                string destinationFile = fImageDestinationPath + fName;
                if (!Directory.Exists(fImageDestinationPath))
                {
                    MessageBoxShow("Source folder does not exist, creating");
                    Directory.CreateDirectory(fImageDestinationPath);
                }
                MessageBoxShow("Moving " + fName + " from " + fPathName + " to " + destinationFile);
                // To move source image to source folder:
                if (!File.Exists(m_strSourceBitmapFile)) { MessageBox.Show("Error: could not locate source image file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                MoveWithReplace(m_strSourceBitmapFile, destinationFile);

                //default tile size is 256, but check interface for user entered value
                int txtTileSize = 256;
                try
                {
                    txtTileSize = Convert.ToInt32(textBoxTileSize.Text);

                }
                catch (Exception)
                {
                    MessageBox.Show("Please put only numbers (1-1048) in Tile size");
                }

                //Deep Zoom processing
                MessageBoxShow("Attempting to initialize DeepZoomTools.dll.");

                try
                {

                    //This runs Deep Zoom Tools as a separate thread in backgroundworker
                    List<object> DZparams = new List<object>();
                    DZparams.Add(destinationFile);
                    DZparams.Add(fNameNoExtension);
                    DZparams.Add(txtTileSize);

                    BackgroundWorker bwDZ = new BackgroundWorker();
                    bwDZ.DoWork += new DoWorkEventHandler(DeepZoomDLL_doWork);
                    bwDZ.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CopyFilesIntoOutputFolder);
                    bwDZ.RunWorkerAsync(DZparams);


                    /* this would run deep zoom tools.dll in the same thread.
                    ImageCreator imageCreator = new ImageCreator();
                    //CollectionCreator collectionCreator = new CollectionCreator();
                    imageCreator.TileSize = txtTileSize;
                    imageCreator.TileOverlap = 1;
                    imageCreator.TileFormat = Microsoft.DeepZoomTools.ImageFormat.Png;
                    imageCreator.Create("source\\" + fName, "output");
                    Microsoft.DeepZoomTools.Image img = new Microsoft.DeepZoomTools.Image(fName);
                    */

                    /* this runs DZConvert.exe as a process 
                    Process compiler = new Process();
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.FileName = "Dzconvert.exe";
                    compiler.StartInfo.Arguments = "source\\" + fName + " output /tf:png /ts:"+txtTileSize+" /ov:1";
                    compiler.StartInfo.UseShellExecute = false;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    compiler.StartInfo.RedirectStandardError = true;
                    MessageBoxShow("Calling function: " + compiler.StartInfo.FileName + compiler.StartInfo.Arguments);
                    compiler.Start();
                    MessageBoxShow(compiler.StandardOutput.ReadToEnd());
                    MessageBoxShow(compiler.StandardError.ReadToEnd());
                    compiler.WaitForExit();
                    */

                    progressBar1.Value += 2;
                    progressBar1.Update();
                    progressBar1.Refresh();

                    MessageBoxShow("Processing with DeepZoomTools.dll, please wait...");

                    // Set cursor as default arrow
                    Cursor.Current = Cursors.Default;

                }
                catch (ArgumentNullException)
                {
                    MessageBox.Show("Path is a null reference.");
                }
                catch (System.Security.SecurityException)
                {
                    MessageBox.Show("The caller does not have the " +
                        "required permission.");
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("ArgumentException Error has occurred.  ");
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Error.  IO Exception has occurred.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Processing failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
                }
            };
        }

        private void CopyFilesIntoOutputFolder(object sender, EventArgs e)
        {
            string fName = System.IO.Path.GetFileName(m_strSourceBitmapFile);
            string fNameNoExtension = System.IO.Path.GetFileNameWithoutExtension(m_strSourceBitmapFile);
            string fPathName = System.IO.Path.GetDirectoryName(m_strSourceBitmapFile);
            string finalDestinationPath = m_strFinalDestinationFolder;

            progressBar1.Value += 2;
            progressBar1.Update();
            progressBar1.Refresh();

            MessageBoxShow("Finished Processing with DeepZoomTools.dll.  Preparing final folders.");

            try
            {
            //moving generated folders
                string strSource=@Directory.GetCurrentDirectory() + "\\output\\" + fNameNoExtension + "_files";
                string strDestination = @Directory.GetCurrentDirectory() + "\\output\\"+fNameNoExtension+"\\GeneratedImages\\dzc_output_files";
                MessageBoxShow("Moving " + strSource + " to " + strDestination);
                MoveDirectory(strSource, strDestination);

                incrementProgressbar();

                //moving generated xml file
                strSource = @Directory.GetCurrentDirectory() + "\\output\\" + fNameNoExtension + ".xml";
                strDestination = @Directory.GetCurrentDirectory() + "\\output\\" + fNameNoExtension + "\\GeneratedImages\\dzc_output.xml";
                MessageBoxShow("Moving " + strSource + " to " + strDestination);
                //File.Move(strSource, strDestination);
                MoveWithReplace(strSource, strDestination);

                incrementProgressbar();

                // To move generated "embed" file to output folder, rename it to index.html
                strSource = fPathName+"\\embed.html";
                strDestination = @Directory.GetCurrentDirectory() + "\\output\\" + fNameNoExtension + "\\index.html";
                MessageBoxShow("Moving " + strSource + " to " + strDestination);
                MoveWithReplace(strSource, strDestination);

                incrementProgressbar();


                //moving generated folders 
                strSource = @Directory.GetCurrentDirectory() + "\\output\\" + fNameNoExtension;
                if (outputNaming.SelectedIndex == 0)  // 0 is GI naming
                {
                    if (gi != "") { strDestination = finalDestinationPath + "dnadata\\nuccore" + gi; }
                    else { strDestination = finalDestinationPath + "dnadata\\" + DDVseqID; }
                }
                else if (outputNaming.SelectedIndex == 1)  // 1 is Name naming
                {
                    strDestination = finalDestinationPath + "dnadata\\" + sequenceName;
                }
                MessageBoxShow("Moving Results" + strSource + " to " + strDestination);
                MoveDirectory(strSource, strDestination);

                incrementProgressbar();

                //moving [img] folder into place
                strSource = @Directory.GetCurrentDirectory() + "\\img";
                if (outputNaming.SelectedIndex == 0)  // 0 is GI naming
                {
                    if (gi != "") { strDestination = finalDestinationPath + "dnadata\\nuccore" + gi + "\\img"; }
                    else { strDestination = finalDestinationPath + "dnadata\\" + DDVseqID + "\\img"; }
                }
                else if (outputNaming.SelectedIndex == 1)  // 1 is Name naming
                {
                    strDestination = finalDestinationPath + "dnadata\\" + sequenceName + "\\img";
                }
                MessageBoxShow("Copying images" + strSource + " to " + strDestination);
                if (!(Directory.Exists(strDestination)))
                {
                    CopyDirectory(strSource, strDestination);
                    MessageBoxShow("Done.");
                }
                else
                {
                    MessageBoxShow("Folder already exists, skipping");
                }
                incrementProgressbar();

                //copy shared files into output folder
                string[] files = {"openseadragon.min.js","openseadragon.js","openseadragon.js.map",
                "openseadragon-scalebar.js","openseadragon.min.js.map","Biojs.js","Biojs.Sequence.js"};
                foreach(string file in files)
                {
                    copy_source_to_output_folder(finalDestinationPath, file);
                }

                // To move donwloaded sequence file to final folder
                strSource = fPathName + "\\sequence.fasta";
                if (File.Exists(strSource))
                {
                    if (outputNaming.SelectedIndex == 0)  // 0 is GI naming
                    {
                        if (gi != "") { strDestination = finalDestinationPath + "dnadata\\nuccore" + gi + "\\sequence.fasta"; }
                        else { strDestination = finalDestinationPath + "dnadata\\" + DDVseqID + "\\sequence.fasta"; }
                    }
                    else if (outputNaming.SelectedIndex == 1)  // 1 is Name naming
                    {
                        strDestination = finalDestinationPath + "dnadata\\" + sequenceName + "\\sequence.fasta";
                    }
                    MessageBoxShow("Copying " + strSource + " to " + strDestination);
                    File.Copy(strSource, strDestination, true);
                }

                // To move donwloaded sequence-info.xml to final folder
                strSource = fPathName + "\\sequence-info.xml";
                if (File.Exists(strSource))
                {
                    if (outputNaming.SelectedIndex == 0)  // 0 is GI naming
                    {
                        if (gi != "") { strDestination = finalDestinationPath + "dnadata\\nuccore" + gi + "\\sequence-info.xml"; }
                        else { strDestination = finalDestinationPath + "dnadata\\" + DDVseqID + "\\sequence-info.xml"; }
                    }
                    else if (outputNaming.SelectedIndex == 1)  // 1 is Name naming
                    {
                        strDestination = finalDestinationPath + "dnadata\\" + sequenceName + "\\sequence-info.xml";
                    }
                    MessageBoxShow("Moving " + strSource + " to " + strDestination);
                    File.Copy(strSource, strDestination, true);
                    File.Delete(strSource);
                }

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\seadragon.css"; ;
                strDestination = finalDestinationPath + "seadragon.css";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileWithReplaceIfNewer(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\nucleotideNumber.js";
                strDestination = finalDestinationPath + "nucleotideNumber.js";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileWithReplaceIfNewer(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\jquery-1.7.min.js";
                strDestination = finalDestinationPath + "jquery-1.7.min.js";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\nucleicDensity.js";
                strDestination = finalDestinationPath + "nucleicDensity.js";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\d3.v3.js";
                strDestination = finalDestinationPath + "d3.v3.js";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\loading.gif";
                strDestination = finalDestinationPath + "loading.gif";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-A.png";
                strDestination = finalDestinationPath + "LEGEND-A.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-B.png";
                strDestination = finalDestinationPath + "LEGEND-B.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-bg.png";
                strDestination = finalDestinationPath + "LEGEND-bg.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-C.png";
                strDestination = finalDestinationPath + "LEGEND-C.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-D.png";
                strDestination = finalDestinationPath + "LEGEND-D.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-G.png";
                strDestination = finalDestinationPath + "LEGEND-G.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-H.png";
                strDestination = finalDestinationPath + "LEGEND-H.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-K.png";
                strDestination = finalDestinationPath + "LEGEND-K.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-M.png";
                strDestination = finalDestinationPath + "LEGEND-M.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-N.png";
                strDestination = finalDestinationPath + "LEGEND-N.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-R.png";
                strDestination = finalDestinationPath + "LEGEND-R.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-S.png";
                strDestination = finalDestinationPath + "LEGEND-S.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-T.png";
                strDestination = finalDestinationPath + "LEGEND-T.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-V.png";
                strDestination = finalDestinationPath + "LEGEND-V.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-W.png";
                strDestination = finalDestinationPath + "LEGEND-W.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\LEGEND-Y.png";
                strDestination = finalDestinationPath + "LEGEND-Y.png";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                //copy shared files into output folder
                strSource = @Directory.GetCurrentDirectory() + "\\density.php";
                strDestination = finalDestinationPath + "density.php";
                MessageBoxShow("Copying " + strSource + " to " + strDestination);
                CopyFileNoReplace(strSource, strDestination);
                incrementProgressbar();

                MessageBoxShow("Completed. ");

                //Enable link to Generated Interfaces
                btnGeneratedIntefaces.Enabled = true;
                

                //Go to current result
                string strResultURL = "";
                if (gi != "")
                {
                    strResultURL = "http://localhost:1818/dnadata/nuccore" + fNameNoExtension + "/index.html";
                }
                else
                {
                    strResultURL = "http://localhost:1818/dnadata/" + fNameNoExtension + "/index.html";
     
                }
                MessageBoxShow("Opening Result "+strResultURL);
                lnkLatestInterface.Text = strResultURL;
                lnkLatestInterface.Enabled = true;
                btnUploadInterface.Enabled = true;
                try
                {
                    System.Diagnostics.Process.Start(strResultURL);
                }
                catch
                    (
                     System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (System.Exception other)
                {
                    MessageBox.Show(other.Message);
                }

                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
                
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Path is a null reference.");
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("The caller does not have the " +
                    "required permission.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("ArgumentException Error has occurred.  ");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Error.  IO Exception has occurred.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Processing failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
            }

        }

        private void copy_source_to_output_folder(string finalDestinationPath, string filename)
        {
            string strSource = @Directory.GetCurrentDirectory() + "\\" + filename ;
            string strDestination = finalDestinationPath + filename;
            MessageBoxShow("Copying scripts" + strSource + " to " + strDestination);
            CopyFileWithReplaceIfNewer(strSource, strDestination);
            incrementProgressbar();
        }

        private void incrementProgressbar()
        {
            progressBar1.Value += 1;
            progressBar1.Update();
            progressBar1.Refresh();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Download FASTA file from NIH
            if (txtGI.Text == "")
            {
                MessageBox.Show("Please enter valid GI or select local data file.");
                return;
            }
            MessageBoxClear();
             // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;
            WebClient Client = new WebClient();
            
            string strDownload = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=nucleotide&rettype=fasta&id=" + txtGI.Text;
            string strDownloadInfo = "http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=nucleotide&rettype=docsum&id=" + txtGI.Text;;
            string strDestinationInfo = @Directory.GetCurrentDirectory() + "\\output\\sequence-info.xml";
            string strDestination = @Directory.GetCurrentDirectory() + "\\output\\sequence.fasta";
            //if file exists, delete it
            if (File.Exists(strDestinationInfo))
            {
                File.Delete(strDestinationInfo);
                MessageBoxShow("Deleting cached sequence info...");
            }
            //if file exists, delete it
            if (File.Exists(strDestination)){
                File.Delete(strDestination);
                MessageBoxShow("Deleting cached sequence...");
            }
            //if output Directory not there, make it
            if (!(Directory.Exists(@Directory.GetCurrentDirectory() + "\\output"))){
                Directory.CreateDirectory(@Directory.GetCurrentDirectory() + "\\output");
            }
            MessageBoxShow("Attempting to download file...please wait");
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            button_generate_viz.Enabled = true;
            if (layoutSelector.SelectedIndex == 1) {
                button_generate_python_viz.Enabled = true;
                button_run_deepzoom.Enabled = true;
            }
            try
            {
                progressBar1.Value = 2;
                MessageBoxShow("Attempting download of docsummary.");
                WebClient client = new WebClient();
                client.DownloadFile(strDownloadInfo, strDestinationInfo);
                MessageBoxShow("Downloaded docsummary:");
                string strDocSummary = File.ReadAllText(strDestinationInfo);
                MessageBoxShow(strDocSummary);
                MessageBoxShow("Downloading FASTA sequence. Please wait...");
                int bytesLength = 0;
                XmlReaderSettings xmlreadersettings = new XmlReaderSettings();
                xmlreadersettings.DtdProcessing = DtdProcessing.Ignore;
                using (XmlReader reader = XmlReader.Create(new StringReader(strDocSummary), xmlreadersettings)) 
                {
                    string s = "";
                    while (s != "Length")
                    {
                        reader.ReadToFollowing("Item");
                        reader.MoveToAttribute("Name");
                        s = reader.Value;
                    }
                    reader.MoveToContent();
                    bytesLength = reader.ReadElementContentAsInt();
                    //MessageBoxShow(bytesLength.ToString());
                }
                progressBar1.Maximum = bytesLength+1000;
	            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
	            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                // Starts the download
                client.DownloadFileAsync(new Uri(strDownload), strDestination);	 
                // Reset UI
                btnDownloadFASTA.Enabled = false;
                DisableGenerateUI();
            }
            catch (Exception ex)
            {
                if (strDownload == "")
                {
                    MessageBox.Show("Please enter valid URL or select local data file");
                }
                else
                {
                    MessageBox.Show("Downloading failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
                }

            }
            
            
        }

        void DisableGenerateUI()
        {
            button_generate_viz.Enabled = false;
            button_generate_python_viz.Enabled = false;
            button_run_deepzoom.Enabled = false;
            ready_for_deep_zoom = false;
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if ((int)e.BytesReceived < progressBar1.Maximum)
            {
                progressBar1.Value = (int)e.BytesReceived;
                progressBar1.Update();
                progressBar1.Refresh();
            }
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBoxShow("Download Completed.");
            btnDownloadFASTA.Enabled = true;
            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
            //Clear variables
            BitmapClear();
            string strDestination = @Directory.GetCurrentDirectory() + "\\output\\sequence.fasta";
            SetSourceSequence(strDestination);
            CleanInterfaceNewSequence();
            lblSourceSequence.Text = txtGI.Text + " Downloaded.";
        }

        private void SetSourceSequence(string strDestination)
        {
            m_strSourceFile = strDestination;
        }

        private void CleanInterfaceNewSequence()
        {
            lblDataLength.Text = "";
            lblSequenceName.Text = "";
            txtBoxFASTAStats.Text = "";
            lblRefSeq.Text = "";
            progressBar1.Value = 0;
            //btnGenerateImage.Enabled = false;
            //btnProcessBitmapDeepZoom.Enabled = false;
            txtBoxSequenceNameOverride.Text = "";

            button_generate_viz.Enabled = true;
            if (layoutSelector.SelectedIndex == 1)
            {
                button_generate_python_viz.Enabled = true;
                button_run_deepzoom.Enabled = true;
            }
            txtBoxSequenceNameOverride.Enabled = true;
            label15.Visible = true;
            lblSourceSequence.Visible = true;
        }

        private void BitmapClear()
        {
            m_strSourceBitmapFile = "";
            lblSourceBitmapFilename.Text = "";
            lblSourceBitmapFilename.Visible = false;
            label8.Visible = false;
            //CLEAR functions associated with bitmap
            //btnProcessBitmapDeepZoom.Enabled = false;
        }

        private void BitmapSet(string destinationFile)
        {
            m_strSourceBitmapFile = destinationFile;
            lblSourceBitmapFilename.Text = destinationFile;
            lblSourceBitmapFilename.Visible = true;
            label8.Visible = true;
            MessageBoxShow("Image file set to -> " + destinationFile + " for processing.");
            ready_for_deep_zoom = true;
        }

        private void read_sequence(object sender, EventArgs e)
        {
            // Set cursor as wait 
            Cursor.Current = Cursors.WaitCursor;
            long length = populateInfo(false);
            if (length == 0)
            {
                //btnGenerateImage.Enabled = false;
                MessageBoxShow("Cannot generate image from this FASTA file.");
            }
            else
            {
                //btnGenerateImage.Enabled = true;
                txtBoxSequenceNameOverride.Enabled = false;
            }
            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

     
       
        private void launchCivetweb(){


            //Run Civetweb for localhost to be created in output folder
            //for nucleic_density applications
            try
            {
            //make sure output folder exists, if it doesn't, create it
            //initialize Civetweb is set to use this location as default localhost
            if (!Directory.Exists("output")) { Directory.CreateDirectory("output"); }

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(Civetweb_doWork);
            bw.RunWorkerAsync();
            

            MessageBoxShow("Launching Civetweb");
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Path is a null reference.");
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("The caller does not have the " +
                    "required permission.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("ArgumentException Error has occurred.  ");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Error.  IO Exception has occurred.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Processing failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
            }

        }

        private void DeepZoomDLL_doWork(object sender, DoWorkEventArgs e)
        {

                List <object> genericlist = e.Argument as List <object>;
                string destinationFile = (string) genericlist[0];
                string fNameNoExtension = (string) genericlist[1];
                int txtTileSize = (int) genericlist[2];

                //MessageBox.Show(fNameNoExtension);

                ImageCreator ic = new ImageCreator();
                ic.TileSize = txtTileSize;
                ic.TileOverlap = 1;
                ic.TileFormat = Microsoft.DeepZoomTools.ImageFormat.Png;
                //ic.UseOptimizations = true;
                ic.Create(destinationFile, "output\\" + fNameNoExtension);

               
        }

        private void Civetweb_doWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                m_prcCivetweb = new Process();
                m_prcCivetweb.StartInfo.CreateNoWindow = true;
                m_prcCivetweb.StartInfo.FileName = "civetweb.exe";
                m_prcCivetweb.StartInfo.UseShellExecute = false;
                //m_prcCivetweb.StartInfo.RedirectStandardOutput = true;
                //m_prcCivetweb.StartInfo.RedirectStandardError = true;
                MessageBoxShow("Calling function: " + m_prcCivetweb.StartInfo.FileName + m_prcCivetweb.StartInfo.Arguments);
                m_prcCivetweb.Start();
                m_prcCivetweb.WaitForExit();
            
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("Error: The caller does not have the required permission.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Error: ArgumentException Error has occurred.  ");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Error.  IO Exception has occurred.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Processing failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
            }
               
           
        }

        private void killCivetweb()
        {
            
            try
            {
                m_prcCivetweb.Kill();
                m_prcCivetweb.Dispose();

             }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Path is a null reference.");
            }
            catch (System.Security.SecurityException)
            {
                MessageBox.Show("The caller does not have the " +
                    "required permission.");
            }
            catch (ArgumentException)
            {
                MessageBox.Show("ArgumentException Error has occurred.  ");
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Error.  IO Exception has occurred.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Processing failed with the following error message: " + Environment.NewLine + Environment.NewLine + ex.Message, ex.GetType().Name);
            }
        }

        private void btnFinalDestinationFolder_Click(object sender, EventArgs e)
        {
            if (m_strFinalDestinationFolder != "")
            {
                Process.Start(m_strFinalDestinationFolder);
            }
            else
            {
                MessageBox.Show("Output folder not set.");
            }
            /*
            DialogResult dr = fDlgFinalDestination.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (fDlgFinalDestination.SelectedPath == "")
                {
                    MessageBox.Show("Please select destination folder.  The generated interface files will be placed under this folder.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SetFinalDestinationFolder(fDlgFinalDestination.SelectedPath);
            }
            */


        }

        private void SetFinalDestinationFolder(string path)
        {
            m_strFinalDestinationFolder = path;
            lblOutputPath.Text = m_strFinalDestinationFolder;
        }

        private void btnGeneratedIntefaces_Click(object sender, EventArgs e)
        {
            //Go to current result
            string strResultURL = "http://localhost:1818/dnadata/";
            MessageBoxShow("Opening Result " + strResultURL);
            try
            {
                System.Diagnostics.Process.Start(strResultURL);
            }
            catch
                (
                 System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void lnkLatestInterface_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Go to current result
            string strResultURL = lnkLatestInterface.Text;
            MessageBoxShow("Opening Result " + strResultURL);
            try
            {
                System.Diagnostics.Process.Start(strResultURL);
            }
            catch
                (
                 System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            DialogResult dr = dlgImageFileSet.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (dlgImageFileSet.FileName != ""){
                    BitmapSet(dlgImageFileSet.FileName);
                    //btnProcessBitmapDeepZoom.Enabled=true;
                }
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Go to Help 
            string strHelpURL = "http://www.photomedia.ca/DDV/download/#help";
            MessageBoxShow("Opening Help " + strHelpURL);
            try
            {
                System.Diagnostics.Process.Start(strHelpURL);
            }
            catch
                (
                 System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void resultLogTextBox_TextChanged(object sender, EventArgs e)
        {
            resultLogTextBox.SelectionStart = resultLogTextBox.Text.Length;
            resultLogTextBox.ScrollToCaret();
        }

        private void layoutSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (layoutSelector.SelectedIndex == 0)  // 0 is Full Height Columns (Original)
            {
                txtBoxColumnWidth.Enabled = true;
                txtBoxY.Enabled = true;
                button_generate_python_viz.Enabled = false;
                button_run_deepzoom.Enabled = false;
            }
            else if (layoutSelector.SelectedIndex == 1)  // 1 is Tiled
            {
                txtBoxColumnWidth.Enabled = false; //edit these numbers directly in DDVLayoutManager.cs
                txtBoxY.Enabled = false;
                if (button_generate_viz.Enabled)
                {
                    button_generate_python_viz.Enabled = true;
                    button_run_deepzoom.Enabled = true;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string strDestination = "";
            string folder = "";

            if (outputNaming.SelectedIndex == 0)  // 0 is GI naming
            {
                if (gi != "") 
                { 
                    strDestination = m_strFinalDestinationFolder + "dnadata\\nuccore" + gi;
                    folder = "nuccore" + gi;
                }
                else 
                { 
                    strDestination = m_strFinalDestinationFolder + "dnadata\\" + DDVseqID;
                    folder = DDVseqID;
                }
            }
            else if (outputNaming.SelectedIndex == 1)  // 1 is Name naming
            {
                strDestination = m_strFinalDestinationFolder + "dnadata\\" + sequenceName;
                folder = sequenceName;
            }

            if (strDestination != "")
            {
                Form3 frm = new Form3(strDestination, folder);
                frm.Show();
            }
        }

        private void button_generate_viz_Click(object sender, EventArgs e)
        {
            //read_sequence(sender, e); //redundant
            generate_image_and_interface(sender, e);
            process_deep_zoom(sender, e);
        }

        private void run_external_processing(object sender, EventArgs e, bool image_done)
        {
            lblProgressText.Text = "";
            lblProgressText.Refresh();
            progressBar1.Value = 0;
            progressBar1.Update();
            progressBar1.Refresh();

            FileInfo TheFile = new FileInfo(m_strSourceFile);
            sequenceLength = TheFile.Length;
            sequenceLength = populateInfo(false); // sequenceLength > 300000000);

            string large_images_py = @Directory.GetCurrentDirectory() + "\\LargeImages.py";

            string sequenceName = "";
            if (outputNaming.SelectedIndex == 0)
            {
                if (gi != "")
                {
                    sequenceName = gi;
                }
                else
                {
                    sequenceName = DDVseqID;
                }
            }
            else if (outputNaming.SelectedIndex == 1 && txtBoxSequenceNameOverride.Text != "")
            {
                sequenceName = txtBoxSequenceNameOverride.Text.Replace('.', '_');
            }
            string strResultFileName = sequenceName + ".png";
            if (!image_done)
            {
                if (File.Exists(strResultFileName))
                {
                    MessageBoxShow("Removing temp version of PNG file.");
                    File.Delete(strResultFileName);
                }
                if (File.Exists("embed.html"))
                {
                    MessageBoxShow("Removing temp version of embed html file.");
                    File.Delete("embed.html");
                }
            }

            progressBar1.Value = 10;
            progressBar1.Update();
            progressBar1.Refresh();
            if (!image_done)
            {
                MessageBoxShow("Sending job to Python...");
                ProcessStartInfo python = new ProcessStartInfo();
                python.FileName = fDlgPythonInterpreter.FileName;
                python.Arguments = string.Format("{0} {1} {2} {3}", large_images_py, m_strSourceFile, @Directory.GetCurrentDirectory(), strResultFileName);
                python.UseShellExecute = false;
                python.RedirectStandardOutput = true;

                using (Process python_process = Process.Start(python))
                {
                    using (StreamReader reader = python_process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        MessageBoxShow(result);
                    }
                }
                MessageBoxShow("Done creating PNG and embed HTML files.");
            }
            progressBar1.Value = 40;
            progressBar1.Update();
            progressBar1.Refresh();

            string pngDestination = TheFile.DirectoryName + Path.DirectorySeparatorChar + strResultFileName;
            string htmlDestination = TheFile.DirectoryName + Path.DirectorySeparatorChar + "embed.html";
            MoveWithReplace(strResultFileName, pngDestination);
            MoveWithReplace("embed.html", htmlDestination);
            CopyFileWithReplaceIfNewer(m_strSourceFile, m_strFinalDestinationFolder + "//sequence.fasta");
            BitmapSet(pngDestination);

            progressBar1.Value = 70;
            progressBar1.Update();
            progressBar1.Refresh();

            MessageBoxShow("Starting Deep Zoom Processing...");
            process_deep_zoom(sender, e);

            progressBar1.Value = 100;
            progressBar1.Update();
            progressBar1.Refresh();
        }

        private void button_generate_python_viz_Click(object sender, EventArgs e)
        {
            if (m_strSourceFile == "") { MessageBox.Show("Please select source file.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            if (!File.Exists(m_strSourceFile)) { MessageBox.Show("Could not find source FASTA file: " + m_strSourceFile + ". Please select source file", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            Button button_sender = (Button)sender;
            if (button_sender.Name == "button_generate_python_viz")
            {
                MessageBox.Show("Please select the 'Python.exe' associated with your VirtualEnvironment setup for this project.", "Select Virtual Environment", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult dr = fDlgPythonInterpreter.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    if (fDlgPythonInterpreter.FileName == "")
                    {
                        MessageBox.Show("A proper Python Interpreter was not selected!", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    run_external_processing(sender, e, false);
                }
                else
                {
                    MessageBox.Show("Not processing as a Python Interpreter was not selected!", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                try
                {
                    MessageBox.Show("Before running DeepZoom on an already generated image, please make sure that the generated PNG is in the same folder as DDV.exe and that it is named the same as the FASTA file you selected or the same name as the name Override you selected.\n\nYou also need to have an already generated embed.html in the same folder as well.\n\nThere is currently no error handling on this, so missing the proper files will crash the program.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    run_external_processing(sender, e, true);
                }
                catch
                {
                    MessageBoxShow("Could not find the proper image and/or embed files! Processing aborted...");
                }
            }
        }
	}
}

            