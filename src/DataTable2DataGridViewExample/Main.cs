// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Main.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The main form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataTable2DataGridViewExample;

/// <summary>
/// The main form.
/// </summary>
public partial class Main : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Handles the form load event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event args.</param>
    private void FormLoad(object sender, EventArgs e)
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
        {
            dt.Columns[i].AllowDBNull = false;
        }

        // Make First Name + Last Name require uniqueness.
        var uniqueCols = new List<DataColumn>();
        var firstNameColumn = dt.Columns["First Name"];
        var lastNameColumn = dt.Columns["Last Name"];

        if (firstNameColumn is not null)
        {
            uniqueCols.Add(firstNameColumn);
        }

        if (lastNameColumn is not null)
        {
            uniqueCols.Add(lastNameColumn);
        }

        dt.Constraints.Add(new UniqueConstraint(uniqueCols.ToArray()));

        // Add items to the table.
        dt.Rows.Add("Rod", "Stephens", "Nerd", 10000);
        dt.Rows.Add("Sergio", "Aragones", "Cartoonist", 20000);
        dt.Rows.Add("Eoin", "Colfer", "Author", 30000);
        dt.Rows.Add("Terry", "Pratchett", "Author", 40000);

        // Make the DataGridView use the DataTable as its data source.
        this.dataGridView1.DataSource = dt;
    }
}
