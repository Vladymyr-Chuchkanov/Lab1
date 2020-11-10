//using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        private const int M_columns = 10;
        private const int M_rows = 10;
        private const int M_HeaderWidth = 75;
        private string currentPath_ = "";
        

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            InitializeDataGridView();
            InitializeAllCells();
            CellManager.Instance.DataGridView = dataGridView1;
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnCount = M_columns;
            dataGridView1.RowCount = M_rows;
            FillHeaders();
            dataGridView1.AutoResizeRows();
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.RowHeadersWidth = M_HeaderWidth;

        }
        private void FillHeaders()
        {
            foreach(DataGridViewColumn cl in dataGridView1.Columns)
            {
                cl.HeaderText = "A" + (cl.Index + 1);
                cl.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewRow rw in dataGridView1.Rows)
            {
                rw.HeaderCell.Value = "R" + (rw.Index + 1);

            }
        }
        private void InitializeAllCells()
        {
            foreach (DataGridViewRow rw in dataGridView1.Rows)
            {
                foreach(DataGridViewCell cl in rw.Cells)
                {
                    InitializeSingleCell(rw, cl);
                }
            }

        }
        private void InitializeSingleCell(DataGridViewRow rw, DataGridViewCell cl)
        {
            string CellPosition = "R" + (rw.Index + 1).ToString() + "A" + (cl.ColumnIndex + 1).ToString();
            cl.Tag = new Cell(cl, CellPosition, "");
            cl.Value = "";
        }
        private void UpdateCellValues()
        {
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                foreach(DataGridViewCell cell in row.Cells)
                {
                    UpdateSingleCellValue(cell);
                }
            }
        }
        private void UpdateCellValues(DataGridViewCell caster)
        {                       
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //if (caster != cell)
                    //{
                        UpdateSingleCellValue(cell);
                    //}
                }
            }
        }
        private void UpdateSingleCellValue(DataGridViewCell cell_)
        {
            Parser pars = new Parser();
            Cell cell = (Cell)cell_.Tag;
            cell_.Value = pars.Result(cell,cell.Expression);
            cell.Value = pars.Result(cell, cell.Expression);


            bool check = CellManager.Instance.HasReferenceRecursion(cell,cell.Position);
            if(check)
            {
                cell.Value = "RECURSION!";
                MessageBox.Show("Рекурсія в " + cell.Position);
                
            }

        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex == -1||e.ColumnIndex == -1)
            {
                return;
            }
            Cell cell = (Cell)dataGridView1[e.ColumnIndex, e.RowIndex].Tag;
            DataGridViewCell dgvCell = cell.Parent;
            if(!dgvCell.ReadOnly)
            {
                dataGridView1.BeginEdit(true);
            }
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Cell cell = (Cell)dataGridView1[e.ColumnIndex, e.RowIndex].Tag;
            CellManager.Instance.CurrentCell = cell;
            DataGridViewCell dgvCell = cell.Parent;
            dgvCell.Value = cell.Expression;
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Cell cell = (Cell)dataGridView1[e.ColumnIndex, e.RowIndex].Tag;
            DataGridViewCell dgvCell = cell.Parent;
            if(dgvCell.Value == null)
            {
                cell.Expression = "";
                cell.Value = "";
                dgvCell.Value = "";
            }
            ClearRemovedRefs(cell);
            UpdateCellExpressions(cell, dgvCell);
        }
        private void ClearRemovedRefs(Cell cell)
        {
            List<Cell> removedCells = new List<Cell>();
            foreach(Cell refCell in cell.CellRef)
            {
                if(!cell.Expression.Contains(refCell.Position))
                {
                    removedCells.Add(refCell);
                }
            }
            foreach (Cell refCell in removedCells)
            {
                cell.CellRef.Remove(refCell);
            }
        }
        private void UpdateCellExpressions(Cell cell, DataGridViewCell dgvCell)
        {
            cell.Expression = dgvCell.Value.ToString();
            Parser pars = new Parser();                      
            dgvCell.Value = pars.Result(cell,cell.Expression).ToString();
            cell.Value = Convert.ToString(dgvCell.Value);


            UpdateCellValues(dgvCell);
        }
        private void SaveDataGridView(string filePath)
        {
            currentPath_ = filePath;
            dataGridView1.EndEdit();
            DataTable table = new DataTable("0");
            ForgeDataTable(table);
            table.WriteXml(filePath);
        }
        private void ForgeDataTable(DataTable table)
        {
            foreach(DataGridViewColumn dgvCol in dataGridView1.Columns)
            {
                table.Columns.Add(dgvCol.Index.ToString());
            }
            foreach( DataGridViewRow dgvRow in dataGridView1.Rows)
            {
                DataRow dataRow = table.NewRow();
                foreach(DataColumn col in table.Columns)
                {
                    Cell cell = (Cell)dgvRow.Cells[Int32.Parse(col.ColumnName)].Tag;
                    dataRow[col.ColumnName] = cell.Expression;
                }
                table.Rows.Add(dataRow);
            }
        }
        private bool SaveDataGridView(string filePath, string dummy)
        {
            if(filePath!="")
            {
                SaveDataGridView(filePath);
                return true;
            }
            else if(saveFileDialog1.ShowDialog()== DialogResult.OK)
            {
                SaveDataGridView(saveFileDialog1.FileName);
                return true;
            }
            return false;
        }
        private void LoadDataGridView(string filePath)
        {
            currentPath_ = filePath;
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(filePath);
            DataTable table = dataSet.Tables[0];
            dataGridView1.ColumnCount = table.Columns.Count;
            dataGridView1.RowCount = table.Rows.Count;
            foreach(DataGridViewRow dgvRow in dataGridView1.Rows)
            {
                foreach(DataGridViewCell dgvCell in dgvRow.Cells)
                {
                    string CellPosition = "R" + (dgvRow.Index + 1).ToString() + "A" + (dgvCell.ColumnIndex + 1).ToString();
                    string Expression = table.Rows[dgvCell.RowIndex][dgvCell.ColumnIndex].ToString();
                    dgvCell.Tag = new Cell(dgvCell, CellPosition, Expression);
                }
            }
            UpdateCellValues();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CellManager.Instance.CurrentCell = new Cell();
                LoadDataGridView(openFileDialog1.FileName);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDataGridView(currentPath_, "");
        }

        private void ShowInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ви повинні писати всі операції та значення через пробіл, ви маєте самі слідкувати за тим, що ви пишете, программа не буде нічого вгадувати і дороблювати за вас! операції:" + '\n'+
                " +:  5 + 3 = 8" + '\n' 
                + " -: 5 - 3 = 2 " + '\n' 
                + "*: 5 * 3 = 15" + '\n' 
                + " /: 5 / 3 = 1,66666.." + '\n' 
                + " mod: 5 mod 3 = 2" + '\n' 
                + " div: 5 div 3 = 0.6666.." 
                + '\n' + " not: not true = false" + '\n' 
                + " or: true or false = true" + '\n' 
                + " and: true and false = true" + '\n' 
                + " =: 5 = 3  false" + '\n' 
                + " >: 5 > 3 = true" + '\n'
                + " <: 5 < 3 = false");
        }
        private void AddRow()
        {
            dataGridView1.Rows.Add(new DataGridViewRow());
            FillHeaders();
            DataGridViewRow addedRow = dataGridView1.Rows[dataGridView1.RowCount - 1];
            addedRow.DefaultCellStyle.Font = new Font(DefaultFont.Name, DefaultFont.Size, GraphicsUnit.Point);
            foreach(DataGridViewCell cell in addedRow.Cells)
            {
                InitializeSingleCell(addedRow, cell);
            }
        }
        private void AddColumn()
        {
            dataGridView1.Columns.Add(new DataGridViewColumn(dataGridView1.Rows[0].Cells[0]));
            FillHeaders();
            foreach(DataGridViewRow dgvRow in dataGridView1.Rows)
            {
                InitializeSingleCell(dgvRow, dgvRow.Cells[dataGridView1.ColumnCount - 1]);
            }
        }
        private void RowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRow();
        }
        private void ColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddColumn();
        }
        private void DeleteRow()
        {
            if(dataGridView1.RowCount == 1)
            {
                MessageBox.Show("ви не можете видалити останній ряд!");
                return;
            }
            if(DeleteRowHasRefs())
            {
                MessageBox.Show("спочатку видаліть усі посилання на клітини останнього рядку !");
                return;
            }
            int lastRow = dataGridView1.RowCount - 1;
            dataGridView1.Rows.RemoveAt(lastRow);
        }
        private bool DeleteRowHasRefs()
        {
            List<string> firstArr = new List<string>();
            
            int lastRow = dataGridView1.RowCount - 1;
            foreach(DataGridViewCell dgvCell in dataGridView1.Rows[lastRow].Cells)
            {
                Cell cell = (Cell)dgvCell.Tag;
                firstArr.Add(cell.Position);
            }
            return FindDeletedRowsRefs(firstArr, lastRow);
            
        }
        private bool FindDeletedRowsRefs(List<string> arr, int lastRow)
        {
            for(int i = 0;i<lastRow;++i)
            {
                foreach(DataGridViewCell dgvCell in dataGridView1.Rows[i].Cells)
                {
                    Cell cell = (Cell)dgvCell.Tag;
                    List<Cell> refs = cell.CellRef;
                    for(int j = refs.Count - 1;j>=0;--j)
                    {
                        if(arr.Contains(refs[j].Position))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private void DeleteColumn()
        {
            if (dataGridView1.RowCount == 1)
            {
                MessageBox.Show("ви не можете видалити останній ряд!");
                return;
            }
            if (DeleteColHasRefs())
            {
                MessageBox.Show("спочатку видаліть усі посилання на клітини останнього рядку!");
                return;
            }
            int lastCol = dataGridView1.ColumnCount - 1;
            dataGridView1.Columns.RemoveAt(lastCol);
        }
        private bool DeleteColHasRefs()
        {
            List<string> firstArr = new List<string>();

            int lastCol = dataGridView1.ColumnCount - 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                Cell cell = (Cell)row.Cells[lastCol].Tag;
                firstArr.Add(cell.Position);
            }
            return FindDeletedColsRefs(firstArr, lastCol);

        }
        private bool FindDeletedColsRefs(List<string> arr, int lastCol)
        {
            for (int i = 0; i < lastCol; ++i)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    Cell cell = (Cell)row.Cells[i].Tag;
                    List<Cell> refs = cell.CellRef;
                    for (int j = refs.Count - 1; j >= 0; --j)
                    {
                        if (arr.Contains(refs[j].Position))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void RowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("ви дійсно хочете видалити рядок?", "Стій!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                DeleteRow();
            }

        }

        private void ColumnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteColumn();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("а зберегти!?","Стій!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if(res == DialogResult.Yes)
            {
                if(!SaveDataGridView(currentPath_,""))
                {
                    e.Cancel = true;
                }
            }
            else if(res == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
    class Cell
    {
        private DataGridViewCell parent_;
        private string value_;
        private string expression_;
        private string position_;
        public string Value
        {
            get { return value_; }
            set { value_ = value; }
        }
        public string Expression
        {
            get { return expression_; }
            set { expression_ = value; }
        }
        public DataGridViewCell Parent
        {
            get { return parent_; }
        }
        public string Position
        {
            get { return position_; }
        }
        public List<Cell> CellRef
        {
            get; set;
        }
        public Cell(DataGridViewCell parent, string pos, string exp)
        {
            parent_ = parent;
            position_ = pos;
            expression_ = exp;
            value_ = "";
            CellRef = new List<Cell>();
            CellRef.Add(new Cell());
        }
        public Cell()
        {
            position_ = "";
        }
        
    }
    class CellManager
    {
        private DataGridView dataGridView_;
        private static CellManager instance_;
        public Cell CurrentCell
        {
            get; set;
        }
        public DataGridView DataGridView
        {
            set { dataGridView_ = value; }
        }
        public static CellManager Instance
        {
            get
            {
                if (instance_ == null)
                {
                    instance_ = new CellManager();
                }
                return instance_;
            }
        }
        private CellManager() { }
        public  Cell GetCell(string cellPosition)
        {
            
            var matches = new Regex(@"^R(?<row>\d+)A(?<col>\d+)$").Matches(cellPosition);
            int row = Int32.Parse(matches[0].Groups["row"].Value) - 1;
            int col = Int32.Parse(matches[0].Groups["col"].Value) - 1;
            try
            {
                return (Cell)dataGridView_[col, row].Tag;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
      
        public Cell GetCell(DataGridViewCell dataGridViewCell)
        {
            return (Cell)dataGridViewCell.Tag;
        }
        public bool HasReferenceRecursion(Cell cell, string invokerPosition)
        {
            
            if(cell.CellRef==null)
            {
                return false;
            }
            foreach(Cell cell1 in cell.CellRef)
            {
                if(cell1.Position==invokerPosition)
                {
                    return true;
                }
                if (HasReferenceRecursion(cell1, invokerPosition))
                {
                    return true;
                }
            }
            return false;
        }
       
    }
    
    class Parser
    {
        public List<Cell> CellRef0 = new List<Cell>();
        
        private int lenght;
        private List<string> RPN_ = new List<string>();
        public string[] Split_(string s)
        {
            string[] g = s.Split();
            return g;
        }
        public int GetPriority(string s)
        {
            switch (s)
            {

                case "div":
                case "mod":
                case "*":
                case "/":
                    return 1;
                case "+":
                case "-":
                    return 2;
                case ">":
                case "<":
                case "=":

                    return 3;
                case "or":
                case "and":
                    return 4;


            }
            return 5;
        }
        public void Transform_to_RPN(Cell cell_, string input)
        {
            
            lenght = 0;
            Regex rx_numbers = new Regex(@"[0-9]+");
            Regex rx_neg_numbers = new Regex(@"-[0-9]+");
            Regex rx_cells = new Regex(@"R[0-9]+A[0-9]+");
            Regex rx_opers = new Regex(@"[-,+,/,*,mod,div,or,and,=,>,<]");
            List<string> stack_ = new List<string>();
            int i = 0;
            try
            {
                cell_.CellRef = new List<Cell>();
                foreach (string c in Split_(input))
                {
                    if (rx_numbers.IsMatch(c) && !rx_cells.IsMatch(c)&& !rx_neg_numbers.IsMatch(c))
                    {
                        RPN_.Add(c);
                        ++lenght;
                    }
                    if(rx_neg_numbers.IsMatch(c))
                    {
                        string c1 = "";
                        foreach(char b in c)
                        {
                            if(b != '-')
                            {
                                c1 += b;
                            }
                        }
                        RPN_.Add("0");
                        RPN_.Add(c1);
                        RPN_.Add("-");
                        lenght += 3;
                    }
                    if (rx_cells.IsMatch(c))
                    {
                        Cell cell = CellManager.Instance.GetCell(c);
                        if (!cell_.CellRef.Contains(cell))
                        {
                            cell_.CellRef.Add(cell);                            
                        }
                        string c1 = "";
                        if (rx_neg_numbers.IsMatch(cell.Value))
                        {
                            foreach (char b in cell.Value)
                            {
                                if (b != '-')
                                {
                                    c1 += b;
                                }
                            }
                            RPN_.Add("0");
                            RPN_.Add(c1);
                            RPN_.Add("-");
                            lenght += 3;
                        }
                        else
                        {
                            RPN_.Add(cell.Value);
                            ++lenght;
                        }

                    }
                    if (c == "not")
                    {
                        ++i;
                        stack_.Insert(0, c);
                    }

                    if (c == "(")
                    {
                        ++i;
                        stack_.Insert(0, c);
                    }
                    if (c == ")")
                    {
                        while (i != 0 && stack_[0] != "(")
                        {
                            RPN_.Add(stack_[0]);
                            ++lenght;
                            stack_.Remove(stack_[0]);
                            --i;
                        }
                        stack_.Remove("(");
                        --i;
                    }
                    if (rx_opers.IsMatch(c) && c != "not")
                    {
                        if (i > 0)
                        {
                            while (i > 0 && (stack_[0] == "not" || GetPriority(stack_[0]) <= GetPriority(c)))
                            {
                                RPN_.Add(stack_[0]);
                                ++lenght;
                                stack_.Remove(stack_[0]);
                                --i;
                            }
                        }
                        stack_.Insert(0, c);
                        ++i;
                    }

                }

                for (int e = 0; e < i && stack_.Count != 0; e++)
                {
                    RPN_.Add(stack_[e]);
                    ++lenght;
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show("вітаю! у вас помилка!");
                MessageBox.Show(e.Message);
                
            }


        }
        public string Result(Cell cell,string input)
        {
            Transform_to_RPN(cell,input);
            List<string> stack_ = new List<string>();
            Regex rx_opers = new Regex(@"[-,+,/,*,mod,div,or,and,=,>,<,not]");
            Regex rx_fl = new Regex(@"[0-9]+,[0-9]+");
            int i = 0;
            if(lenght == 0)
            {
                return "";
            }
            try
            {
                while (i < lenght)
                {
                    if (!rx_opers.IsMatch(RPN_[i])||RPN_[i]=="True"||RPN_[i]=="False")
                    {
                        stack_.Insert(0, RPN_[i]);

                    }
                    if(rx_fl.IsMatch(RPN_[i]))
                    {
                        stack_.Insert(0, RPN_[i]);
                    }
                    if (rx_opers.IsMatch(RPN_[i]))
                    {
                        double result;
                        bool result0;
                        if (RPN_[i] == "+")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a + b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "-")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a - b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "*")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a * b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "/")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a / b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "mod")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a % b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "div")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a / b - (a - (a%b))/b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == ">")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a > b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "<")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a < b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "=")
                        {
                            double b = Convert.ToDouble(stack_[0]);
                            double a = Convert.ToDouble(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a == b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "or")
                        {
                            bool b = Convert.ToBoolean(stack_[0]);
                            bool a = Convert.ToBoolean(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a | b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "and")
                        {
                            bool b = Convert.ToBoolean(stack_[0]);
                            bool a = Convert.ToBoolean(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a & b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "not")
                        {
                            bool a = Convert.ToBoolean(stack_[0]);
                            stack_.Remove(stack_[0]);
                            result0 = !a;
                            stack_.Insert(0, result0.ToString());
                        }

                    }
                    ++i;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("вітаю! у вас помилка!");
                MessageBox.Show(e.Message);
                return "error!!!!!";
            }
            if (stack_.Count != 0)
            {
                                              
                return stack_[0];                               
            }
            return "";


        }
    }
}
