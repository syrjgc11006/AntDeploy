﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AntDeployWinform.Winform
{
    public partial class SelectProject : Form
    {
        public SelectProject(List<string> projectList = null)
        {
            InitializeComponent();
            Assembly assembly = typeof(Deploy).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("AntDeployWinform.Resources.Logo1.ico"))
            {
                if (stream != null) this.Icon = new Icon(stream);
            }

            if (projectList != null)
            {
                foreach (var item in projectList)
                {
                    if (File.Exists(item))
                    {
                        var fileInfo = new FileInfo(item);
                        this.listBox_project.Items.Add(fileInfo.Name + "<==>" + item);
                    }

                    if (Directory.Exists(item))
                    {
                        this.listBox_project.Items.Add("[Folder Deploy]<==>" + item);
                    }

                }
            }

            SelectProjectPath = string.Empty;
        }

        public string SelectProjectPath { get; set; }

        private void btn_select_folder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var folder = fbd.SelectedPath;
                    this.SelectProjectPath = folder;
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
        private void btn_select_project_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Choose Project";
            fdlg.Filter = "(.csproj)|*.csproj|(.vsproj)|*.vsproj";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                if (!fdlg.FileName.ToLower().EndsWith(".csproj") && !fdlg.FileName.ToLower().EndsWith(".vbproj"))
                {
                    MessageBox.Show("Err:must be csproj file！");
                    return;
                }

                this.SelectProjectPath = fdlg.FileName;

                this.DialogResult = DialogResult.OK;

            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void listBox_project_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectItem = this.listBox_project.SelectedItem as string;
            if (string.IsNullOrEmpty(selectItem)) return;
            var file = selectItem.Split(new string[] { "<==>" }, StringSplitOptions.None);
            if (file.Length != 2) return;
            var path = file[1];
            if (File.Exists(path))
            {
                this.SelectProjectPath = path;
                this.DialogResult = DialogResult.OK;
            }

            if (Directory.Exists(path))
            {
                this.SelectProjectPath = path;
                this.DialogResult = DialogResult.OK;
            }
        }

    }
}
