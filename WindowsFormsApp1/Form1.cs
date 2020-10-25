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
            cl.Tag = new Cell(cl, CellPosition, "false");
            cl.Value = "false";
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
                    if (caster != cell)
                    {
                        UpdateSingleCellValue(cell);
                    }
                }
            }
        }
        private void UpdateSingleCellValue(DataGridViewCell cell_)
        {
            Parser pars = new Parser();
            Cell cell = (Cell)cell_.Tag;
            cell_.Value = pars.Result(cell.Expression);
            
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
                cell.Expression = "false";
                cell.Value = "false";
                dgvCell.Value = "false";
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
            dgvCell.Value = pars.Result(cell.Expression).ToString();
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
            MessageBox.Show("ви повинні писати всі операції та значення через пробіл, ви маєте самі слідкувати за тим, що ви пишете, программа не буде нічого вгадувати і дороблювати за вас! операції: + - * / mod div not or and = > <");
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
            value_ = "false";
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
            string cellPosition = cell.Position;
            if(cellPosition.Equals("")||invokerPosition.Equals(CurrentCell.Position))
            {
                return false;
            }
            if(cellPosition.Equals(CurrentCell.Position))
            {
                return true;
            }
            return HasInnerRecursion(cell, invokerPosition);
        }
        private bool HasInnerRecursion(Cell cell,string invokerPosition)
        {
            List<Cell> refs = cell.CellRef;
            for (int i = refs.Count - 1; i>=0;--i)
            {
                if(refs[i].Position.Equals(""))
                {
                    return false;
                }
                if(refs[i].Position.Equals(CurrentCell.Position)|| HasReferenceRecursion(refs[i],invokerPosition))
                {
                    return true;
                }
            }
            return false;
        }
    }
    
    class Parser
    {

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
        public void Transform_to_RPN(string input)
        {
            lenght = 0;
            Regex rx_numbers = new Regex(@"[0-9]+");
            Regex rx_cells = new Regex(@"R[0-9]+A[0-9]+");
            Regex rx_opers = new Regex(@"[-,+,/,*,mod,div,or,and,=,>,<]");
            List<string> stack_ = new List<string>();
            int i = 0;

            foreach (string c in Split_(input))
            {
                if (rx_numbers.IsMatch(c)&& !rx_cells.IsMatch(c))
                {
                    RPN_.Add(c);
                    ++lenght;
                }
                if (rx_cells.IsMatch(c))
                {
                    Cell cell = CellManager.Instance.GetCell(c);
                    
                    
                    RPN_.Add(cell.Value);
                    ++lenght;
                    
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
                if (rx_opers.IsMatch(c)&&c!="not")
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

            for (int e = 0; e < i&&stack_.Count!=0; e++)
            {
                RPN_.Add(stack_[e]);
                ++lenght;
            }
            

        }
        public bool Result(string input)
        {
            Transform_to_RPN(input);
            List<string> stack_ = new List<string>();
            Regex rx_opers = new Regex(@"[-,+,/,*,mod,div,or,and,=,>,<,not]");
            int i = 0;
            if(lenght == 0)
            {
                return false;
            }
            try
            {
                while (i < lenght)
                {
                    if (!rx_opers.IsMatch(RPN_[i])||RPN_[i]=="True"||RPN_[i]=="false")
                    {
                        stack_.Insert(0, RPN_[i]);

                    }

                    if (rx_opers.IsMatch(RPN_[i]))
                    {
                        long result;
                        bool result0;
                        if (RPN_[i] == "+")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a + b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "-")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a - b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "*")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a * b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "/")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a / b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "mod")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a % b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == "div")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result = a / b - a % b;
                            stack_.Insert(0, result.ToString());
                        }
                        if (RPN_[i] == ">")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a > b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "<")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
                            stack_.RemoveRange(0, 2);
                            result0 = a < b;
                            stack_.Insert(0, result0.ToString());
                        }
                        if (RPN_[i] == "=")
                        {
                            long b = Convert.ToInt64(stack_[0]);
                            long a = Convert.ToInt64(stack_[1]);
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
                return false;
            }
            if (stack_.Count != 0)
            {
                if (stack_[0] == "0")
                {
                    return false;
                }
                return Convert.ToBoolean(stack_[0]);
            }
            return false;


        }
    }
}
