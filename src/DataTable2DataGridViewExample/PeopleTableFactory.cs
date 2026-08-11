// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeopleTableFactory.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to create the example data table.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataTable2DataGridViewExample;

/// <summary>
/// A class to create the example data table.
/// </summary>
public static class PeopleTableFactory
{
    /// <summary>
    /// Creates the example <see cref="DataTable"/>, including its columns, its constraints and its sample rows.
    /// </summary>
    /// <returns>The example <see cref="DataTable"/>.</returns>
    public static DataTable CreatePeopleTable()
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

        return dt;
    }
}
