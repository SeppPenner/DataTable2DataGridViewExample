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
        this.Text = $@"{Application.ProductName} {Application.ProductVersion}";

        // Make the DataGridView use the DataTable as its data source.
        this.dataGridView1.DataSource = PeopleTableFactory.CreatePeopleTable();
    }
}
