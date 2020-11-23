using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Lab2_OOP_Books
{
    
    public partial class Form1 : Form
    {
        public const int len_const = 5;
        public Form1()
        {
            InitializeComponent();
            LoadAllData();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            Transform();
        }
        public void Transform()
        {
            XslCompiledTransform xsl0 = new XslCompiledTransform();
            xsl0.Load("result.xsl");
            xsl0.Transform("XMLFile_Books.xml", "Books_HTML.html");
        }
        public void LoadAllData()
        {
            XmlDocument load = new XmlDocument();
            load.Load("XMLFile_Books.xml");
            XmlElement elem = load.DocumentElement;
            XmlNodeList lst = elem.SelectNodes("Book");
            for (int i = 0; i < lst.Count; ++i)
            {
                XmlNode node = lst.Item(i);
                FillComboBox(node);
            }
        }
        private void FillComboBox(XmlNode node)
        {
            if (!comboBoxAuthor.Items.Contains(node.SelectSingleNode("@Author").Value))
            {
                comboBoxAuthor.Items.Add(node.SelectSingleNode("@Author").Value);
            }
            if (!comboBoxName.Items.Contains(node.SelectSingleNode("@Name").Value))
            {
                comboBoxName.Items.Add(node.SelectSingleNode("@Name").Value);
            }
            if (!comboBoxCycle.Items.Contains(node.SelectSingleNode("@Cycle").Value))
            {
                comboBoxCycle.Items.Add(node.SelectSingleNode("@Cycle").Value);
            }
            if (!comboBoxGenre.Items.Contains(node.SelectSingleNode("@Genre").Value))
            {
                comboBoxGenre.Items.Add(node.SelectSingleNode("@Genre").Value);
            }
            if (!comboBoxPublishYear.Items.Contains(node.SelectSingleNode("@PublishYear").Value))
            {
                comboBoxPublishYear.Items.Add(node.SelectSingleNode("@PublishYear").Value);
            }
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void Search()
        {
            Books bks_search = new Books();
            if(checkBoxAuthor.Checked)
            {
                bks_search.Author = comboBoxAuthor.SelectedItem.ToString();
            }
            if (checkBoxName.Checked)
            {
                bks_search.Name = comboBoxName.SelectedItem.ToString();
            }
            if (checkBoxCycle.Checked)
            {
                bks_search.Cycle = comboBoxCycle.SelectedItem.ToString();
            }
            if(checkBoxGenre.Checked)
            {
                bks_search.Genre = comboBoxGenre.SelectedItem.ToString();
            }
            if(checkBoxPublishYear.Checked)
            {
                bks_search.PublishYear = comboBoxPublishYear.SelectedItem.ToString();
            }
            Analyzer0 analyzer = new DOM();            
            if (radioButtonSAX.Checked)
            {
                analyzer = new SAX();
            }
            if (radioButtonLINQ.Checked)
            {
                analyzer = new LINQ();
            }
            List < Books > result = analyzer.Result(bks_search);
            foreach(Books i in result)
            {
                richTextBox.Text += "Автор: " + i.Author + '\n';
                richTextBox.Text += "Название: " + i.Name + '\n';
                richTextBox.Text += "Цикл: " + i.Cycle + '\n';
                richTextBox.Text += "Жанр: " + i.Genre + '\n';
                richTextBox.Text += "Год издания: " + i.PublishYear + '\n';
                richTextBox.Text += '\n';
            }

        }
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            richTextBox.Text = "";
            comboBoxAuthor.Text = "";
            comboBoxCycle.Text = "";
            comboBoxGenre.Text = "";
            comboBoxName.Text = "";
            comboBoxPublishYear.Text = "";
            checkBoxAuthor.Checked = false;
            checkBoxCycle.Checked = false;
            checkBoxGenre.Checked = false;
            checkBoxName.Checked = false;
            checkBoxPublishYear.Checked = false;
        }
    }
    public class Books
    {
        public Books()
        { }
        public Books(string a,string b, string c, string d, string e)
        {
            this.Author = a;
            this.Name = b;
            this.Cycle = c;
            this.Genre = d;
            this.PublishYear = e;
        }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Cycle { get; set; }
        public string Genre { get; set; }
        public string PublishYear { get; set; }

    }
    interface Analyzer0
    {
        List<Books> Result(Books bks_search);
    }
    public class DOM:Analyzer0
    {
        public List<Books> Result(Books bks_search)
        {
            List<Books> lst_to_return = new List<Books>();
            XmlDocument load = new XmlDocument();
            load.Load(@"XMLFile_Books.xml");
            XmlElement elem = load.DocumentElement;
            XmlNodeList lst = elem.SelectNodes("Book");
            for (int i = 0; i < lst.Count; ++i)
            {
                XmlNode node = lst.Item(i);
                
                string a = node.SelectSingleNode("@Author").Value;
                string b = node.SelectSingleNode("@Name").Value;
                string c = node.SelectSingleNode("@Cycle").Value;
                string d = node.SelectSingleNode("@Genre").Value;
                string e = node.SelectSingleNode("@PublishYear").Value;
                if ((a ==bks_search.Author || bks_search.Author==null)&&
                   (b == bks_search.Name || bks_search.Name == null) &&
                   (c == bks_search.Cycle || bks_search.Cycle == null) &&
                   (d == bks_search.Genre || bks_search.Genre == null) &&
                   (e == bks_search.PublishYear || bks_search.PublishYear == null))
                {
                    Books bk = new Books(a,b,c,d,e);
                    lst_to_return.Add(bk);
                }
            }


            return lst_to_return;
        }
    }
    public class SAX : Analyzer0
    {
        public List<Books> Result(Books bks_search)
        {
            List<Books> lst_to_return = new List<Books>();
            var reader = new XmlTextReader(@"XMLFile_Books.xml");
            while(reader.Read())
            {
                if(reader.HasAttributes)
                {
                    string[] arr = new string[Form1.len_const];
                    for(int i = 0; reader.MoveToNextAttribute();++i)
                    {
                        arr[i] = reader.Value;
                    }
                    if ((arr[0] == bks_search.Author || bks_search.Author == null) &&
                   (arr[1] == bks_search.Name || bks_search.Name == null) &&
                   (arr[2] == bks_search.Cycle || bks_search.Cycle == null) &&
                   (arr[3] == bks_search.Genre || bks_search.Genre == null) &&
                   (arr[4] == bks_search.PublishYear || bks_search.PublishYear == null))
                    {
                        Books bk = new Books(arr[0], arr[1], arr[2], arr[3], arr[4]);
                        lst_to_return.Add(bk);
                    }
                }


            }
            return lst_to_return;
        }
    }
    public class LINQ : Analyzer0
    {
        public List<Books> Result(Books bks_search)
        {
            List<Books> lst_to_return = new List<Books>();
            var load = XDocument.Load(@"XMLFile_Books.xml");
            var result = from obj in load.Descendants("Book")
                         where
                         ((obj.Attribute("Author").Value.Equals(bks_search.Author) || bks_search.Author == null) &&
                         (obj.Attribute("Name").Value.Equals(bks_search.Name) || bks_search.Name == null) &&
                         (obj.Attribute("Cycle").Value.Equals(bks_search.Cycle) || bks_search.Cycle == null) &&
                         (obj.Attribute("Genre").Value.Equals(bks_search.Genre) || bks_search.Genre == null) &&
                         (obj.Attribute("PublishYear").Value.Equals(bks_search.PublishYear) || bks_search.PublishYear == null))
                         select new
                         {
                             a = obj.Attribute("Author").Value,
                             b = obj.Attribute("Name").Value,
                             c = obj.Attribute("Cycle").Value,
                             d = obj.Attribute("Genre").Value,
                             e = obj.Attribute("PublishYear").Value
                         };
            foreach(var res in result)
            {
                Books a = new Books(res.a, res.b, res.c, res.d, res.e);
                lst_to_return.Add(a);
            }
            return lst_to_return;
        }
    }
}
