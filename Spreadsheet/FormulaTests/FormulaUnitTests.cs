// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Author: Kyle Maher & CS3500 Staff
// Date: September 27, 2026

namespace FormulaTests;

using Formula;

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    ///

// --- Tests for Rule 1, One Token Rule

    // --- Tests for empty formula with no tokens throws exception
    [TestMethod]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( string.Empty ) );
    }
    
    // --- Tests for empty formula with just spaces still throws exception
    [TestMethod]
    public void FormulaConstructor_TestOnlySpaces_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "     " ) );
    }
    
    // --- Tests for only one token making sure it's valid
    [TestMethod]
    public void FormulaConstructor_TestSingleNumberToken_Valid( )
    {
        _ = new Formula( "0" );
    }

    // --- Tests single variable token alone
    [TestMethod]
    public void FormulaConstructor_TestSingleVariableToken_Valid( )
    {
        _ = new Formula( "z1" );
    }

    // --- Tests single token in parentheses
    [TestMethod]
    public void FormulaConstructor_TestSingleTokenInParens_Valid( )
    {
        _ = new Formula( "(5)" );
    }

    // --- Tests single decimal token alone
    [TestMethod]
    public void FormulaConstructor_TestSingleDecimalToken_Valid( )
    {
        _ = new Formula( "3.14" );
    }

    // --- Tests single scientific notation token alone
    [TestMethod]
    public void FormulaConstructor_TestSingleScientificNotationToken_Valid( )
    {
        _ = new Formula( "1e5" );
    }
    
// --- Tests for Rule 2, Valid Token Rule
    // --- Tests for invalid character and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestIllegalCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "$" ) );
    }
    
    // --- Tests for invalid operator character and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOnlyPlusCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "+" ) );
    }
    
    // --- Tests for invalid operator character and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOnlyMinusCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "-" ) );
    }
    
    // --- Tests for invalid operator character and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOnlyMultiplyCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "*" ) );
    }
    
    // --- Tests for invalid divide character and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOnlyDivideCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "/" ) );
    }
    
    // --- Tests for incorrect order variable and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestIncorrectOrderVariableCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "1z" ) );
    }
    
    // --- Tests for invalid Parenthesis character and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOnlyParenCharacter_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "()" ) );
    }
    
    // --- Tests for valid expression use
    [TestMethod]
    public void FormulaConstructor_TestOnlyPlusCharacter_Valid( )
    {
        _ = new Formula( "0+0" );
    }
    
    // --- Tests for valid expression use
    [TestMethod]
    public void FormulaConstructor_TestOnlyMinusCharacter_Valid( )
    {
        _ = new Formula( "0-0" );
    }
    
    // --- Tests for valid expression use
    [TestMethod]
    public void FormulaConstructor_TestOnlyDivideCharacter_Valid( )
    {
        _ = new Formula( "0/0" );
    }
    
    // --- Tests for valid expression use
    [TestMethod]
    public void FormulaConstructor_TestOnlyMultiplyCharacter_Valid( )
    {
        _ = new Formula( "0*0" );
    }
    
    // --- Tests for valid expression use
    [TestMethod]
    public void FormulaConstructor_TestOnlyParenthesisCharacter_Valid( )
    {
        _ = new Formula( "(0)" );
    }
    
    // --- Tests for valid variable use
    [TestMethod]
    public void FormulaConstructor_TestValidVariableCharacter_Valid( )
    {
        _ = new Formula( "z1" );
    }
    
    // --- Tests for scientific notation lowercase e
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationLowercaseE_Valid( )
    {
        _ = new Formula( "1e9" );
    }

    // --- Tests for scientific notation with uppercase E
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationUppercase_Valid( )
    {
        _ = new Formula( "1E9" );
    }
    
    // --- Tests for scientific notation with negative Exponent
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationNegativeExponent_Valid( )
    {
        _ = new Formula( "1E-9" );
    }
    
// --- Tests for Rule 3, Closing Parenthesis Rule
    // --- Tests for closing parentheses before opening and make sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestClosingBeforeOpening_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( ")0(" ) );
    }

    // --- Tests for extra closing Parenthesis mid input and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestExtraClosingParenMidFormula_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "(0))+0" ) );
    }

    // --- Tests for valid expression with nested parenthesis
    [TestMethod]
    public void FormulaConstructor_TestNestedParensProperOrder_Valid( )
    {
        _ = new Formula( "((0+0)*0)" );
    }


// --- Tests for Rule 4, Balanced Parentheses Rule
    // --- Tests for more opening Parenthesis than closing and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestMoreOpenThanClose_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "((0+0)" ) );
    }

    // --- Tests for more closing Parenthesis than opening and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestMoreCloseThanOpen_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "(0+0))" ) );
    }

    // --- Tests for valid expression with multiple parenthesis
    [TestMethod]
    public void FormulaConstructor_TestDeeplyNestedBalancedParens_Valid( )
    {
        _ = new Formula( "(((0)))" );
    }

// --- Tests for Rule 5, First Token Rule
    // --- Tests for formula starting with closing Parenthesis and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenClosingParen_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( ")0" ) );
    }

    // --- Tests for valid expression with token
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid( )
    {
        _ = new Formula( "0+0" );
    }

    // --- Tests for valid variable with expression
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenVariable_Valid( )
    {
        _ = new Formula( "z1+0" );
    }

    // --- Tests for valid Parenthesis with expression
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenOpeningParen_Valid( )
    {
        _ = new Formula( "(0+0)" );
    }


// --- Tests for Rule 6, Last Token Rule
    // --- Tests for formula ending with operator and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestLastTokenOperator_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "0+" ) );
    }

    // --- Tests for formula ending with opening Parenthesis and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestLastTokenOpeningParen_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "0+(" ) );
    }

    // --- Tests for valid variable and it's valid
    [TestMethod]
    public void FormulaConstructor_TestLastTokenVariable_Valid( )
    {
        _ = new Formula( "0+z1" );
    }
// --- Tests for Rule 7, Parenthesis/Operator Following Rule
    // --- Tests for invalid operator following opening Parenthesis and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOperatorAfterOpeningParen_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "(+0)" ) );
    }

    // --- Tests for invalid operator following another operator and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestOperatorAfterOperator_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "0++0" ) );
    }

    // --- Tests for valid expression use with parenthesis
    [TestMethod]
    public void FormulaConstructor_TestNumberAfterOpeningParen_Valid( )
    {
        _ = new Formula( "(0+0)" );
    }

    // --- Tests for valid expression in different order
    [TestMethod]
    public void FormulaConstructor_TestOpeningParenAfterOperator_Valid( )
    {
        _ = new Formula( "0+(0+0)" );
    }
    
// --- Tests for Rule 8, Extra Following Rule
    // --- Tests for number immediately followed by another number and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestNumberFollowedByNumber_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "0 0" ) );
    }

    // --- Tests for closing Parenthesis immediately followed by opening Parenthesis and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestClosingParenFollowedByOpeningParen_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "(0+0)(0+0)" ) );
    }

    // --- Tests for variable immediately followed by number and makes sure it throws exception
    [TestMethod]
    public void FormulaConstructor_TestVariableFollowedByNumber_Invalid( )
    {
        Assert.Throws<FormulaFormatException>( () => _ = new Formula( "z1 0" ) );
    }
    // --- Tests for valid expression following rule implementation
    [TestMethod]
    public void FormulaConstructor_TestClosingParenFollowedByOperator_Valid( )
    {
        _ = new Formula( "(0+0)+0" );
    }

// --- Tests for Equals and Operators (==, !=)

    // --- Tests basic formula check against itself
    [TestMethod]
    public void FormulaEquals_TestBasicCheck_Valid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        Assert.IsTrue( f1.Equals( f1 ) );
    }

    // --- Tests for unequal formulas
    [TestMethod]
    public void FormulaEquals_TestDifferentFormulas_Valid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        Formula f2 = new Formula( "z1 - 5" );
        Assert.IsFalse( f1.Equals( f2 ) );
        Assert.IsFalse( f1 == f2 );
        Assert.IsTrue( f1 != f2 );
    }

    // --- Tests for null comparisons
    [TestMethod]
    public void FormulaEquals_TestNullComparison_Valid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        Formula? f2 = null;
        Assert.IsFalse( f1.Equals( f2 ) );
        Assert.IsFalse( f1 == f2 );
        Assert.IsTrue( f1 != f2 );
    }

    // --- Tests for non-formula object comparison
    [TestMethod]
    public void FormulaEquals_TestStringObjectComparison_Valid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        string f2 = "Z1+5";
        Assert.IsFalse( f1.Equals( f2 ) );
    }

// --- Tests for GetHashCode

    // --- Tests for matching hash codes for equal formulas
    [TestMethod]
    public void FormulaGetHashCode_TestEqualFormulasSameHashCode_Valid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        Formula f2 = new Formula( "Z1 + 5" );
        Assert.AreEqual( f1.GetHashCode( ), f2.GetHashCode( ) );
    }

    // --- Tests for hashcode consistency
    [TestMethod]
    public void FormulaGetHashCode_TestSameObjectHashCode_Valid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        Assert.AreEqual( f1.GetHashCode( ), f1.GetHashCode( ) );
    }

// --- Tests for Evaluate Method

    // --- Tests evaluating a single standalone number
    [TestMethod]
    public void FormulaEvaluate_TestSingleNumber_Valid( )
    {
        Formula f1 = new Formula( "42" );
        object result = f1.Evaluate( v => 0 );
        Assert.AreEqual( 42.0, (double)result, 1e-9 );
    }

    // --- Tests evaluating a single variable alone
    [TestMethod]
    public void FormulaEvaluate_TestSingleVariable_Valid( )
    {
        Formula f1 = new Formula( "x1" );
        object result = f1.Evaluate( v => v == "X1" ? 42.0 : throw new ArgumentException( ) );
        Assert.AreEqual( 42.0, (double)result, 1e-9 );
    }

    // --- Tests simple addition evaluation
    [TestMethod]
    public void FormulaEvaluate_TestSimpleAddition_Valid( )
    {
        Formula f1 = new Formula( "5 + 5" );
        object result = f1.Evaluate( v => 0 );
        Assert.AreEqual( 10.0, (double)result, 1e-9 );
    }

    // --- Tests operator precedence evaluation
    [TestMethod]
    public void FormulaEvaluate_TestPrecedence_Valid( )
    {
        Formula f1 = new Formula( "2 + 3 * 5" );
        object result = f1.Evaluate( v => 0 );
        Assert.AreEqual( 17.0, (double)result, 1e-9 );
    }

    // --- Tests parentheses precedence evaluation
    [TestMethod]
    public void FormulaEvaluate_TestParentheses_Valid( )
    {
        Formula f1 = new Formula( "(2 + 3) * 5" );
        object result = f1.Evaluate( v => 0 );
        Assert.AreEqual( 25.0, (double)result, 1e-9 );
    }

    // --- Tests variable lookup evaluation
    [TestMethod]
    public void FormulaEvaluate_TestVariableLookup_Valid( )
    {
        Formula f1 = new Formula( "z1 * 2 + a1" );
        object result = f1.Evaluate( v => v == "Z1" ? 4 : (v == "A1" ? 2 : throw new ArgumentException( )) );
        Assert.AreEqual( 10.0, (double)result, 1e-9 );
    }

    // --- Tests division by zero returns FormulaError
    [TestMethod]
    public void FormulaEvaluate_TestDivisionByZero_Invalid( )
    {
        Formula f1 = new Formula( "5 / 0" );
        object result = f1.Evaluate( v => 0 );
        Assert.IsInstanceOfType( result, typeof( FormulaError ) );
    }

    // --- Tests undefined variable lookup returns FormulaError
    [TestMethod]
    public void FormulaEvaluate_TestUndefinedVariable_Invalid( )
    {
        Formula f1 = new Formula( "z1 + 5" );
        object result = f1.Evaluate( v => throw new ArgumentException( "Undefined variable" ) );
        Assert.IsInstanceOfType( result, typeof( FormulaError ) );
    }
}