using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Threading;
namespace ModuleLoader_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void AddModuleRow(string name, string baseAddr, string size, string timestamp, string checksum, string dataAddr, string dataSize)
        {
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add(baseAddr);
            item.SubItems.Add(size);
            item.SubItems.Add(timestamp);
            item.SubItems.Add(checksum);
            item.SubItems.Add(dataAddr);
            item.SubItems.Add(dataSize);
            listView1.Items.Add(item);
        }

        void RefreshList()
        {
            try
            {
                bool proto = false;
                listView1.Items.Clear();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ModuleLoader.exe",
                    Arguments = "-S",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process proc = Process.Start(psi))
                {
                    string currentName = null;
                    string baseAddr = "", size = "", timestamp = "", checksum = "", dataAddr = "", dataSize = "";

                    string err = proc.StandardError?.ReadToEnd()?.Trim();
                    if (!string.IsNullOrEmpty(err)) { 
                        MessageBox.Show(this, err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else { 
                        while (!proc.StandardOutput.EndOfStream)
                        {
                            string line = proc.StandardOutput.ReadLine()?.Trim();
                            if (string.IsNullOrEmpty(line)) continue;
                            if (!line.Contains(":"))
                            {
                                if (currentName != null)
                                {
                                    if (currentName.ToLower() == "proto.xex" || currentName.ToLower() == "protov2.xex") proto = true;
                                    AddModuleRow(currentName, baseAddr, size, timestamp, checksum, dataAddr, dataSize);
                                }

                                currentName = line;
                                baseAddr = size = timestamp = checksum = dataAddr = dataSize = "";
                            }
                            else
                            {
                                string[] parts = line.Split(new[] { ':' }, 2);
                                if (parts.Length == 2)
                                {
                                    string key = parts[0].Trim();
                                    string value = parts[1].Trim();
                                    switch (key)
                                    {
                                        case "BaseAddress": baseAddr = value; break;
                                        case "Size": size = value; break;
                                        case "Timestamp": timestamp = value; break;
                                        case "Checksum": checksum = value; break;
                                        case "DataAddress": dataAddr = value; break;
                                        case "DataSize": dataSize = value; break;
                                    }
                                }
                            }
                        }
                    }
                    if (currentName != null)
                    {
                        if (currentName.ToLower() == "proto.xex" || currentName.ToLower() == "protov2.xex") proto = true;
                        AddModuleRow(currentName, baseAddr, size, timestamp, checksum, dataAddr, dataSize);
                    }
                    label2.ForeColor = (proto ? Color.Green : Color.Red);
                    label2.Text = (proto ? "Loaded" : "Not Loaded");
                    button4.Enabled = !proto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running ModuleLoader: {ex.Message}");
            }
        }

        void LoadModule(string name)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ModuleLoader.exe",
                    Arguments = "-l " + name,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proc = Process.Start(psi))
                {
                    
                    string output = proc.StandardOutput.ReadToEnd()?.Trim();
                    string err = proc.StandardError?.ReadToEnd()?.Trim();
                    if (!string.IsNullOrEmpty(err)) { if (name == "Hdd:\\Proto.xex") { LoadModule("Usb:\\Proto.xex"); } else { MessageBox.Show(this, err + (err.Contains("No such") ? Environment.NewLine + "File: " + name : ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
                    if (output.Contains("has been loaded")) { MessageBox.Show(this, output, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); RefreshList(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running ModuleLoader: {ex.Message}");
            }
        }

        void UnloadModule(string name)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "ModuleLoader.exe",
                    Arguments = "-u " + name,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd()?.Trim();
                    string err = proc.StandardError?.ReadToEnd()?.Trim();
                    if (!string.IsNullOrEmpty(err)) { MessageBox.Show(this, err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    if (output.Contains("has been unloaded")) { MessageBox.Show(this, output, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); RefreshList(); }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running ModuleLoader: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string result = Interaction.InputBox("Enter path of module to load:", "Load Module", "Usb:\\Proto.xex");
            if (!string.IsNullOrEmpty(result)) LoadModule(result);
            else MessageBox.Show(this, "You must enter a module to load. Example: HDD:\\Proto.xex", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadModule("Hdd:\\Proto.xex");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                MessageBox.Show(this, "You must select a module from the list to unload.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string item = listView1.SelectedItems[0].Text;
                if (item == "Proto.xex") { MessageBox.Show(this, "Unloading Proto.xex will crash your console.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                else UnloadModule(listView1.SelectedItems[0].Text);
            }
        }

    }
}
