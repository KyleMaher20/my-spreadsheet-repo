// Co Author: Kyle Maher
// Date: 0/3/2026
namespace SpreadsheetsUnitTests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spreadsheets;
using Formula;

[TestClass]
public class SpreadsheetsUnitTests
{
    [TestMethod]
    public void RecalculateOrder( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        gridObject.SetCellContents( "Y1", new Formula( "Z0 * 2" ) );
        gridObject.SetCellContents( "X2", new Formula( "Y1 + 5" ) );

        IList<string> dependentSequence = gridObject.SetCellContents( "Z0", 10.0 );

        List<string> targetSequence = new List<string>( ) { "Z0", "Y1", "X2" };
        Assert.AreEqual( targetSequence.Count, dependentSequence.Count );

        for ( int z = 0; z < targetSequence.Count; z++ )
        {
            Assert.AreEqual( targetSequence[z], dependentSequence[z] );
        }
    }

    [TestMethod]
    public void InvalidNameCheck( )
    {
        Spreadsheet gridObject = new Spreadsheet( );

        try
        {
            gridObject.SetCellContents( "0Z", 42.0 );
            Assert.Fail( "Expected InvalidNameException was not thrown." );
        }
        catch ( InvalidNameException )
        {
        }
    }

    [TestMethod]
    public void StoreFormulaContent( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        Formula mathExpression = new Formula( "Y1 + X2 * 3" );

        gridObject.SetCellContents( "Z0", mathExpression );
        object parsedContent = gridObject.GetCellContents( "Z0" );

        Assert.IsInstanceOfType( parsedContent, typeof( Formula ) );
        Assert.AreEqual( mathExpression.ToString( ), parsedContent.ToString( ) );
    }

    [TestMethod]
    public void CircularRestore( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        gridObject.SetCellContents( "Z0", 5.0 );

        try
        {
            gridObject.SetCellContents( "Z0", new Formula( "Z0 + 1" ) );
            Assert.Fail( "Expected CircularException was not thrown." );
        }
        catch ( CircularException )
        {
        }

        object savedValue = gridObject.GetCellContents( "Z0" );
        Assert.AreEqual( 5.0, savedValue );
    }

    [TestMethod]
    public void GetContentsEmpty( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        object queriedValue = gridObject.GetCellContents( "Z0" );

        Assert.AreEqual( string.Empty, queriedValue );
    }

    [TestMethod]
    public void CircularDependency( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        gridObject.SetCellContents( "Z0", new Formula( "Y1 + 1" ) );

        try
        {
            gridObject.SetCellContents( "Y1", new Formula( "Z0 + 1" ) );
            Assert.Fail( "Expected CircularException was not thrown." );
        }
        catch ( CircularException )
        {
        }
    }

    [TestMethod]
    public void ClearCellContents( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        gridObject.SetCellContents( "Z0", "data" );
        Assert.AreEqual( 1, gridObject.GetNamesOfAllNonemptyCells( ).Count );

        gridObject.SetCellContents( "Z0", "" );

        ISet<string> populatedSet = gridObject.GetNamesOfAllNonemptyCells( );
        Assert.AreEqual( 0, populatedSet.Count );
        Assert.AreEqual( string.Empty, gridObject.GetCellContents( "Z0" ) );
    }

    [TestMethod]
    public void CaseInsensitive( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        gridObject.SetCellContents( "z0", "lowercase test" );

        object uppercaseValue = gridObject.GetCellContents( "Z0" );
        object lowercaseValue = gridObject.GetCellContents( "z0" );

        Assert.AreEqual( "lowercase test", uppercaseValue );
        Assert.AreEqual( "lowercase test", lowercaseValue );
    }

    [TestMethod]
    public void NullNameCheck( )
    {
        Spreadsheet gridObject = new Spreadsheet( );

        try
        {
            gridObject.GetCellContents( null! );
            Assert.Fail( "Expected InvalidNameException was not thrown." );
        }
        catch ( InvalidNameException )
        {
        }
    }

    [TestMethod]
    public void GetNonemptyCells( )
    {
        Spreadsheet gridObject = new Spreadsheet( );
        gridObject.SetCellContents( "Z0", 12.5 );
        gridObject.SetCellContents( "Y1", "hello" );
        gridObject.SetCellContents( "X2", new Formula( "Z0 + Y1" ) );

        ISet<string> populatedSet = gridObject.GetNamesOfAllNonemptyCells( );

        Assert.AreEqual( 3, populatedSet.Count );
        Assert.IsTrue( populatedSet.Contains( "Z0" ) );
        Assert.IsTrue( populatedSet.Contains( "Y1" ) );
        Assert.IsTrue( populatedSet.Contains( "X2" ) );
    }
}