using System;
using System.Data;
using System.Windows.Forms;

namespace DataTable2DataGridViewExample
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Make the DataTable object.
            var dt = new DataTable("People");

            // Add columns to the DataTable.
            dt.Columns.Add("First Name", typeof(string));
            dt.Columns.Add("Last Name", typeof(string));
            dt.Columns.Add("Occupation", typeof(string));
            dt.Columns.Add("Salary", typeof(int));

            // Make all columns required.
            for (var i = 0; i < dt.Columns.Count; i++)
                dt.Columns[i].AllowDBNull = false;

            // Make First Name + Last Name require uniqueness.
            DataColumn[] uniqueCols =
            {
                dt.Columns["First Name"],
                dt.Columns["Last Name"]
            };
            dt.Constraints.Add(new UniqueConstraint(uniqueCols));

            // Add items to the table.
            dt.Rows.Add("Rod", "Stephens", "Nerd", 10000);
            dt.Rows.Add("Sergio", "Aragones", "Cartoonist", 20000);
            dt.Rows.Add("Eoin", "Colfer", "Author", 30000);
            dt.Rows.Add("Terry", "Pratchett", "Author", 40000);

            // Make the DataGridView use the DataTable as its data source.
            dataGridView1.DataSource = dt;
        }
    }
}