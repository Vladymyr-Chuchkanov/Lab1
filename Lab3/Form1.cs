using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
            LeftCombo.Items.Add("C:\\");
            RightCombo.Items.Add("C:\\");
            label1.Text = "";
            label2.Text = "";
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {}
        private void LeftCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadAll("C:\\", LeftFiles, LeftDirs,label1);
        }
        private void RightCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadAll("C:\\", RightFiles, RightDirs,label2);
        }
        private void LeftDirs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label1,LeftDirs,LeftFiles);
            LoadAll(label1.Text,LeftFiles,LeftDirs,label1);
        }
        private void RightDirs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label2,RightDirs,RightFiles);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }
        private void LeftFiles_MouseClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label1,LeftDirs,LeftFiles);
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
        }
        private void RightFiles_MouseClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label2,RightDirs,RightFiles);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }
        private void Label1_Click(object sender, EventArgs e)
        {
            GoUpDir(label1);
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
        }
        private void Label2_Click(object sender, EventArgs e)
        {
            GoUpDir(label2);
            LoadAll(label2.Text, RightFiles, RightDirs,label2);
        }
        private void SearchHtmlByWords_Click(object sender, EventArgs e)
        {
            FindFile();
        }
        private void TransportBtt_Click(object sender, EventArgs e)
        {
            Transport();            
        }
        private void ProcesseBtt_Click(object sender, EventArgs e)
        {
            Processe();
        }
        public void GoUpDir(Button bt)
        {
            if (bt.Text.Length != 0 && bt.Text[bt.Text.Length - 1] == '\\')
            {
                bt.Text = bt.Text.Remove(bt.Text.Length - 1, 1);
            }
            for (int i = bt.Text.Length - 1; bt.Text.Length != 0 && bt.Text[i] != '\\'; --i)
            {

                bt.Text = bt.Text.Remove(bt.Text.Length - 1, 1);
            }
            if (bt.Text.Length == 0)
            {
                bt.Text = "C:\\";
            }
        }
        public void LoadAll(string path, ListBox boxFiles, ListBox boxDirs, Button bt)
        {
            if (Path.GetExtension(path) != "")
            {
                Process.Start(path);
                return;
            }
            boxDirs.Items.Clear();
            boxFiles.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(path);
            bt.Text = path;
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo crrDir in dirs)
            {
                boxDirs.Items.Add(crrDir);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo crrFile in files)
            {
                boxFiles.Items.Add(crrFile);
            }
        }
        public void GetReadyToNextAction(Button bt, ListBox boxDirs, ListBox boxFiles)
        {
            if (bt.Text[bt.Text.Length - 1] != '\\')
            {
                if (boxDirs.SelectedItem != null)
                {
                    bt.Text = bt.Text + "\\" + boxDirs.SelectedItem.ToString();
                }
                else
                {
                    bt.Text = bt.Text + "\\" + boxFiles.SelectedItem.ToString();
                }
            }
            else
            {
                if (boxDirs.SelectedItem != null)
                {
                    bt.Text = bt.Text + boxDirs.SelectedItem.ToString();
                }
                else
                {
                    bt.Text = bt.Text + boxFiles.SelectedItem.ToString();
                }
            }
        }
        public void FindFile()
        {
            LeftDirs.Items.Clear();
            LeftFiles.Items.Clear();
            DirectoryInfo dir = new DirectoryInfo(label1.Text);
            FindFileRecurs(dir);
        }
        public void FindFileRecurs(DirectoryInfo dir)
        {
            if (dir.GetDirectories().Length != 0)
            {
                foreach (DirectoryInfo crrDir in dir.GetDirectories())
                {
                    if (crrDir.GetDirectories().Length != 0)
                    {
                        FindFileRecurs(crrDir);
                    }
                    FileInfo[] files = crrDir.GetFiles();
                    foreach (FileInfo crrFile in files)
                    {
                        if (crrFile.Extension == ".html")
                        {
                            FilePars pars = new FilePars();
                            if (pars.SearchByWords(crrFile.FullName, SearchHtmlTxt.Text))
                            {
                                LeftFiles.Items.Add(crrFile.FullName);
                            }
                        }
                    }
                }
            }
            FileInfo[] files1 = dir.GetFiles();
            foreach (FileInfo crrFile in files1)
            {
                if (crrFile.Extension == ".html")
                {
                    FilePars pars = new FilePars();
                    if (pars.SearchByWords(crrFile.FullName, SearchHtmlTxt.Text))
                    {
                        LeftFiles.Items.Add(crrFile.FullName);
                    }
                }
            }
        }

        public void Transport()
        {
            string DestPath = label2.Text;
            for (int i = LeftFiles.Items.Count; i > 0; --i)
            {
                string FromPath = LeftFiles.Items[i - 1].ToString();
                FileInfo file = new FileInfo(FromPath);

                if (DestPath != "")
                {
                    file.CopyTo(DestPath + "\\" + file.Name);
                    file.Delete();

                }
                else
                {
                    MessageBox.Show("You have not chosen destination!");
                    return;
                }
            }
            LeftFiles.Items.Clear();
            LoadAll(DestPath, RightFiles, RightDirs, label2);
        }

        public void Processe()
        {
            string FromPath = label1.Text + "\\" + LeftFiles.SelectedItem.ToString();
            string DestPath = label1.Text + "\\" + "processed" + LeftFiles.SelectedItem.ToString();
            FileInfo file1 = new FileInfo(FromPath);
            FileInfo file2 = new FileInfo(DestPath);
            FilePars pars = new FilePars();
            pars.ReworkFile(FromPath, DestPath);
            if (checkDeleteProcesse.Checked)
            {
                file1.Delete();
            }
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "To start work: Select folder " + '\n' +
                "Open File: Double clik on right/left files" +'\n'+
                "Open Directory: Double clik on right/left directories" + '\n'+
                "Move to upper directory: press button above right/left directories" + '\n' +
                "Open File: Double clik on right/left files" + '\n' + '\n' +
                "If you want find .html files you can put words(or not) from file you want to box near the button." + '\n' +
                "files will be searched in current directory" + '\n' + '\n' +
                "if you want to remove extra spaces, Tabs and same lines from your file: press 'processe' " + '\n' +
                "if you need not not processed file: check box near button" + '\n' + '\n' +
                "if you want to transport all files, that are in left files to another place: use 'transport->'"
                );
        }
    }
    public class FilePars
    {
        private List<string> htmlFile = new List<string>();
        public FilePars() { }
        public bool SearchByWords(string path, string words)
        {
            StreamReader sr = new StreamReader(path);
            string str = sr.ReadToEnd();
            if(str.Contains(words))
            {
                sr.Dispose();
                return true;
            }
            else
            {
                sr.Dispose();
                return false;
            }
            
        }
        public void ReworkFile(string path1, string path2)
        {
            StreamReader sr = new StreamReader(path1);
            StreamWriter sw = new StreamWriter(path2);
            List<string> lines = new List<string>();

            string line;
            while((line = sr.ReadLine())!=null)
            {
                string[] i = line.Split();                
                string line0="";
                foreach(string a in i)
                {
                    if (a != "")
                    {
                        line0 += a + " ";
                    }
                }
                if (!lines.Contains(line0))
                {
                    lines.Add(line0);
                }
            }
            sr.Dispose();
            foreach(string l in lines)
            {
                sw.WriteLine(l);
            }
            sw.Dispose();
        }
    }
}
