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
            this.WindowState = FormWindowState.Maximized;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            LeftCombo.Items.Add("C:\\");
            RightCombo.Items.Add("C:\\");
            label1.Text = "";
            label2.Text = "";

        }

        private void Form1_Load(object sender, EventArgs e)
        { }
        private void LeftCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadAll("C:\\", LeftFiles, LeftDirs, label1);
        }
        private void RightCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadAll("C:\\", RightFiles, RightDirs, label2);
        }
        private void LeftDirs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label1, LeftDirs, LeftFiles);
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
        }
        private void RightDirs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label2, RightDirs, RightFiles);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }
        private void LeftFiles_MouseClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label1, LeftDirs, LeftFiles);
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
        }
        private void RightFiles_MouseClick(object sender, MouseEventArgs e)
        {
            GetReadyToNextAction(label2, RightDirs, RightFiles);
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
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
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
        
        
        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "To start work: Select folder " + '\n' +
                "Open File: Double clik on right/left files" + '\n' +
                "Open Directory: Double clik on right/left directories" + '\n' +
                "Move to upper directory: press button above right/left directories" + '\n' +
                "Open File: Double clik on right/left files" + '\n' + 
                "Create new/rename file: click on edit button(new file or dir when selected no dir or file)" + '\n'+'\n' +
                "If you want find .html files you can put words(or not) from file you want to box near the button." + '\n' +
                "files will be searched in current directory" + '\n' + '\n' +
                "if you want to remove extra spaces, Tabs and same lines from your file: press 'processe' " + '\n' +
                "if you need not not processed file: check box near button" + '\n' + '\n' +
                "if you want to transport all files, that are in left files to another place: use 'transport->'"
                );
        }

        private void DeleteBttLeft_Click(object sender, EventArgs e)
        {
            DeleteFileOrDir(LeftFiles, LeftDirs,label1);
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }

        private void DeleteBttRight_Click(object sender, EventArgs e)
        {
            DeleteFileOrDir(RightFiles, RightDirs,label2);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CleanSpaces();
        }

        private void EditLeftBtt_Click(object sender, EventArgs e)
        {
            if(LeftDirs.SelectedItem==null&&LeftFiles.SelectedItem==null)
            {
                AddFileOrDir(label1);
            }
            else
            {
                EditName(label1,LeftFiles,LeftDirs);
            }
        }        
        private void EditRightBtt_Click(object sender, EventArgs e)
        {
            if (RightDirs.SelectedItem == null && RightFiles.SelectedItem == null)
            {
                AddFileOrDir(label2);
            }
            else
            {
                EditName(label2, RightFiles, RightDirs);
            }
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
            try
            {
                if (Path.GetExtension(path) != "")
                {
                    DialogResult res = MessageBox.Show("Open as txt?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);                    
                    if (res == DialogResult.Yes)
                    {
                        var p =Process.Start("notepad.exe", path);
                        p.Kill();
                    }
                    else
                    {
                        var p =Process.Start( path);
                        p.Kill();
                    }                    
                    GoUpDir(bt);                    
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
            catch { }
        }
        public void GetReadyToNextAction(Button bt, ListBox boxDirs, ListBox boxFiles)
        {
            if (bt.Text[bt.Text.Length - 1] != '\\')
            {
                if (boxDirs.SelectedItem != null)
                {
                    bt.Text = bt.Text + "\\" + boxDirs.SelectedItem.ToString();
                }
                if (boxFiles.SelectedItem != null)
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
            if (LeftFiles.SelectedItem == null&&LeftDirs.SelectedItem==null) {
                for (int i = LeftFiles.Items.Count; i > 0; --i)
                {
                    string FromPath = LeftFiles.Items[i - 1].ToString();
                    FileInfo file;
                    if (FromPath.Contains(".html"))
                    {
                        file = new FileInfo(FromPath);
                    }
                    else
                    {
                        file = new FileInfo(label1.Text + "\\" + FromPath);
                    }
                    if (DestPath != "")
                    {
                        file.CopyTo(DestPath + "\\" + file.Name, true);
                        file.Delete();
                    }
                    else
                    {
                        MessageBox.Show("You have not chosen destination!");
                        return;
                    }
                }
                LeftFiles.Items.Clear();
            }
            else if(LeftFiles.SelectedItem != null)
            {
                string pas = label1.Text + "\\" + LeftFiles.SelectedItem.ToString();
                string to = label2.Text+ "\\" + LeftFiles.SelectedItem.ToString();
                File.Copy(pas, to, true);
            }  
            else if (LeftDirs.SelectedItem != null)
            {
                string pas = label1.Text + "\\" + LeftDirs.SelectedItem.ToString();
                string to = label2.Text + "\\" + LeftDirs.SelectedItem.ToString();
                Directory.Move(pas, to);
            }
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
            LoadAll(DestPath, RightFiles, RightDirs, label2);
        }
        public void Processe()
        {
            try
            {
                string FromPath = label1.Text + "\\" + LeftFiles.SelectedItem.ToString();
                string DestPath = label1.Text + "\\" + "processed" + LeftFiles.SelectedItem.ToString();
                FileInfo file1 = new FileInfo(FromPath);
                FileInfo file2 = new FileInfo(DestPath);
                FilePars pars = new FilePars();
                pars.CleanRows(FromPath, DestPath);
                if (checkDeleteProcesse.Checked)
                {
                    file1.Delete();
                }
            }
            catch { }
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }
        void DClearDir(DirectoryInfo dir)
        {
            foreach(FileInfo file in dir.GetFiles())
            {
                file.Delete();
            }
            foreach(DirectoryInfo dr in dir.GetDirectories())
            {
                DClearDir(dr);
            }
            dir.Delete();
        }
        public void DeleteFileOrDir(ListBox boxFiles, ListBox boxDirs, Button label)
        {
            string FromPath = "";
            if (boxDirs.SelectedItem != null)
            {
                FromPath = label.Text + "\\" + boxDirs.SelectedItem.ToString();
                boxDirs.Items.Remove(boxDirs.SelectedItem);
                DirectoryInfo dir = new DirectoryInfo(FromPath);
                DialogResult res = MessageBox.Show("Do you want delete directory?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes) { DClearDir(dir); }
            }
            if (boxFiles.SelectedItem != null)
            {
                FromPath = label.Text + "\\" + boxFiles.SelectedItem.ToString();
                boxFiles.Items.Remove(boxFiles.SelectedItem);
                FileInfo file = new FileInfo(FromPath);
                DialogResult res = MessageBox.Show("Do you want delete file?", "Warning!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes) { file.Delete(); }
            }
            else { return; }
        }
        public void CleanSpaces()
        {
            try
            {
                string FromPath = label1.Text + "\\" + LeftFiles.SelectedItem.ToString();
                FileInfo file1 = new FileInfo(FromPath);
                FilePars pars = new FilePars();
                pars.CleanSpaces(FromPath);
            }
            catch { }
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }
        public void AddFileOrDir(Button bt)
        {
            DialogResult res = MessageBox.Show("Do you want to create new Dir?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.No)
            {
                DialogResult res1 = MessageBox.Show("Do you want to create new File?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res1 == DialogResult.No)
                {
                    return;
                }
                else
                {
                    string path = bt.Text + "\\newfile.txt";                    
                    FileInfo file = new FileInfo(path);
                    if(file.Exists)
                    {
                        DialogResult res2 = MessageBox.Show("Do you want to replace file with same name?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(res2 == DialogResult.Yes)
                        {
                            file.Delete();
                            file.Create();
                        }
                        else{return;}
                    }
                    else { file.Create(); }
                }
            }
            else if (res == DialogResult.Yes)
            {
                DirectoryInfo dir = new DirectoryInfo(bt.Text + "\\newdir");
                dir.Create();
            }
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
        }
        public void EditName(Button bt, ListBox boxFiles, ListBox boxDirs)
        {
            Form2 testDialog = new Form2();
            testDialog.label1.Text = bt.Text;
            testDialog.ShowDialog(this);
            string name = "";

            if (testDialog.button1.Text == "renamed")
            {
                name = testDialog.textBox1.Text;
            }
            if (boxDirs.SelectedItem != null)
            {
                DirectoryInfo dir = new DirectoryInfo(bt.Text + "\\" + boxDirs.SelectedItem.ToString());
                DirectoryInfo dir0 = new DirectoryInfo(bt.Text + "\\" + name);
                DirectoryInfo dir2 = new DirectoryInfo(bt.Text + "\\" + name);
                DialogResult res=DialogResult.No;
                if (boxDirs.Items.ToString().Contains(name))
                {
                    res = MessageBox.Show("Do you want to replace object with same name?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else { dir.MoveTo(bt.Text + "\\" + name); }
                if (res == DialogResult.Yes)
                {
                    DClearDir(dir2);                    
                    dir0.Create();
                    DClearDir(dir);
                }                
            }
            if (boxFiles.SelectedItem != null)
            {
                string pathFrom = bt.Text + "\\" + boxFiles.SelectedItem.ToString();
                DialogResult res = DialogResult.No;
                string topath = "";
                if (bt.Text[bt.Text.Length - 1] == '\\') { topath = bt.Text; }
                else { topath = bt.Text + "\\"; }
                DirectoryInfo dir = new DirectoryInfo(bt.Text);
                int i = 0;
                foreach(FileInfo f1 in dir.GetFiles())
                {
                    if (name == f1.ToString())
                    {
                        i = 1;break;
                    }
                }
                if (i==1)
                {
                    res = MessageBox.Show("Do you want to replace object with same name?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else
                {
                    File.Move(pathFrom, topath + name);
                }
                if (res == DialogResult.Yes)
                {                
                    File.Replace(pathFrom, topath + name,"C:\\1\\0.txt",true);                
                }                
            }
            testDialog.Dispose();
            LoadAll(label1.Text, LeftFiles, LeftDirs, label1);
            LoadAll(label2.Text, RightFiles, RightDirs, label2);
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
        public void CleanRows(string path1, string path2)
        {
            StreamReader sr = new StreamReader(path1);
            StreamWriter sw = new StreamWriter(path2);
            List<string> lines = new List<string>();
            string line;
            while((line = sr.ReadLine())!=null)
            {                
                if (!lines.Contains(line))
                {
                    lines.Add(line);
                }
            }
            sr.Dispose();
            foreach(string l in lines)
            {
                sw.WriteLine(l);
            }
            sw.Dispose();
        }
        public void CleanSpaces(string path1)
        {
            StreamReader sr = new StreamReader(path1);
            List<string> lines = new List<string>();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string line0 = "";
                string[] x = line.Split();
                foreach(string i in x)
                {
                    if(i!="")
                    {
                        line0 += i+" ";
                    }
                }
                lines.Add(line0);
            }
            sr.Dispose();
            StreamWriter sw = new StreamWriter(path1,false);
            foreach(string i in lines)
            {
                sw.WriteLine(i);
            }
            sw.Dispose();
        }
    }
}
