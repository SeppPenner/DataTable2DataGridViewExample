// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PeopleTableFactoryTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="PeopleTableFactory" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataTable2DataGridViewExample.Tests;

/// <summary>
/// A class to test the <see cref="PeopleTableFactory"/> class.
/// </summary>
[TestClass]
public class PeopleTableFactoryTests
{
    /// <summary>
    /// Checks whether the table is named and carries the four columns of the example in their original order.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableReturnsTheFourColumns()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        Assert.AreEqual("People", table.TableName);
        Assert.AreEqual(4, table.Columns.Count);
        Assert.AreEqual("First Name", table.Columns[0].ColumnName);
        Assert.AreEqual("Last Name", table.Columns[1].ColumnName);
        Assert.AreEqual("Occupation", table.Columns[2].ColumnName);
        Assert.AreEqual("Salary", table.Columns[3].ColumnName);
        Assert.AreEqual(typeof(string), table.Columns[0].DataType);
        Assert.AreEqual(typeof(string), table.Columns[1].DataType);
        Assert.AreEqual(typeof(string), table.Columns[2].DataType);
        Assert.AreEqual(typeof(int), table.Columns[3].DataType);
    }

    /// <summary>
    /// Checks whether every column is required, which is what the loop over the columns does.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableMakesEveryColumnRequired()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        foreach (DataColumn column in table.Columns)
        {
            Assert.IsFalse(column.AllowDBNull, $"The column {column.ColumnName} should not allow null values.");
        }
    }

    /// <summary>
    /// Checks whether the <see cref="UniqueConstraint"/> spans both name columns and no other column. A constraint
    /// over only one of them would silently allow two people with the same last name to be rejected.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableConstrainsBothNameColumnsTogether()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        Assert.AreEqual(1, table.Constraints.Count);
        var constraint = table.Constraints[0] as UniqueConstraint;
        Assert.IsNotNull(constraint);
        Assert.AreEqual(2, constraint.Columns.Length);
        Assert.AreEqual("First Name", constraint.Columns[0].ColumnName);
        Assert.AreEqual("Last Name", constraint.Columns[1].ColumnName);
    }

    /// <summary>
    /// Checks whether the four sample rows are added with their values.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableReturnsTheFourSampleRows()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        Assert.AreEqual(4, table.Rows.Count);
        Assert.AreEqual("Rod", table.Rows[0]["First Name"]);
        Assert.AreEqual("Stephens", table.Rows[0]["Last Name"]);
        Assert.AreEqual("Nerd", table.Rows[0]["Occupation"]);
        Assert.AreEqual(10000, table.Rows[0]["Salary"]);
        Assert.AreEqual("Sergio", table.Rows[1]["First Name"]);
        Assert.AreEqual("Eoin", table.Rows[2]["First Name"]);
        Assert.AreEqual("Terry", table.Rows[3]["First Name"]);
        Assert.AreEqual(40000, table.Rows[3]["Salary"]);
    }

    /// <summary>
    /// Checks whether every call returns its own table, so that two forms could not share one instance by accident.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableReturnsANewTableOnEveryCall()
    {
        var firstTable = PeopleTableFactory.CreatePeopleTable();
        var secondTable = PeopleTableFactory.CreatePeopleTable();

        Assert.AreNotSame(firstTable, secondTable);
    }

    /// <summary>
    /// Checks whether a row without a value is rejected. This is the consequence of the required columns that shows
    /// up in the running application as an error dialog of the <see cref="DataGridView"/>.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableRejectsARowWithAMissingValue()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        Assert.ThrowsExactly<NoNullAllowedException>(() => table.Rows.Add(null, "Tester", "Author", 50000));
    }

    /// <summary>
    /// Checks whether a second person with the same first and last name is rejected by the
    /// <see cref="UniqueConstraint"/>.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableRejectsADuplicateName()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        Assert.ThrowsExactly<ConstraintException>(() => table.Rows.Add("Rod", "Stephens", "Author", 50000));
    }

    /// <summary>
    /// Checks whether a person with the same last name but a different first name is accepted, which is the point of
    /// a constraint over both columns instead of one per column.
    /// </summary>
    [TestMethod]
    public void CreatePeopleTableAcceptsASharedLastName()
    {
        var table = PeopleTableFactory.CreatePeopleTable();

        table.Rows.Add("Rhianna", "Pratchett", "Author", 50000);

        Assert.AreEqual(5, table.Rows.Count);
    }

    /// <summary>
    /// Checks what the example is about: the table assigned to <see cref="DataGridView.DataSource"/> shows up as
    /// columns and rows of the grid. The <see cref="BindingContext"/> is set by hand because the grid has no parent
    /// form here, without it the binding would be deferred until the grid gets one.
    /// </summary>
    [TestMethod]
    public void TheTableFillsTheDataGridViewWhenItIsUsedAsDataSource()
    {
        using var dataGridView = new DataGridView
        {
            BindingContext = new BindingContext()
        };

        dataGridView.DataSource = PeopleTableFactory.CreatePeopleTable();

        Assert.AreEqual(4, dataGridView.Columns.Count);
        Assert.AreEqual("First Name", dataGridView.Columns[0].HeaderText);
        Assert.AreEqual("Salary", dataGridView.Columns[3].HeaderText);

        // Four rows of data plus the empty row the grid offers for new entries.
        Assert.AreEqual(5, dataGridView.Rows.Count);
        Assert.AreEqual("Rod", dataGridView.Rows[0].Cells[0].Value);
        Assert.AreEqual(40000, dataGridView.Rows[3].Cells[3].Value);
    }
}
