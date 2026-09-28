// <summary>
// <para>
// Author: Kyle Maher & CS3500 Staff
// Date: September 27, 2026
// </para>
// </summary>

namespace Formula;

using System.Text.RegularExpressions;

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormulaError"/> class.
    /// <para>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    /// Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
/// Any method meeting this type signature can be used for
/// looking up the value of a variable.
/// </summary>
/// <exception cref="ArgumentException">
/// If a variable name is provided that is not recognized by the implementing method,
/// then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
/// The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);

/// <summary>
/// <para>
/// This class represents formulas written in standard infix notation using standard precedence
/// rules. The allowed symbols are non-negative numbers written using double-precision
/// floating-point syntax; variables that consist of one or more letters followed by
/// one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
/// </para>
/// <para>
/// Spaces are significant only insofar that they delimit tokens. For example, "xy" is
/// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
/// and "x 23" consists of a variable "x" and a number "23". Otherwise, spaces are to be removed.
/// </para>
/// <para>
/// For Assignment Two, you are to implement the following functionality:
/// </para>
/// <list type="bullet">
/// <item>
/// Formula Constructor which checks the syntax of a formula.
/// </item>
/// <item>
/// Get Variables
/// </item>
/// <item>
/// ToString
/// </item>
/// </list>
/// </summary>
public class Formula
{
    /// <summary>
    /// All variables are letters followed by numbers. This pattern
    /// represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    /// Puts the orginal unpasred string into the constructor
    /// </summary>
    private readonly string rawFormulaString;

    /// <summary>
    ///Stores the string representation of the formula so it can run the O(1) time compexity of ToString()
    /// </summary>
    private readonly string normalizedFormulaString;

    // <copyright file="Formula_PS4.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>
// <authors>
// Solution by: Joe Zachary, Daniel Kopta, Jim de St. Germain, Travis Martin
// </authors>
// Add these methods to your Formula class.
    /// <summary>
    /// <para>
    /// Reports whether f1 == f2, using the notion of equality from the <see  cref="Equals"/> method.
    /// </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula? f1, Formula? f2)
    {
        if (ReferenceEquals(f1, f2) == true)
        {
            return true;
        }

        if (f1 is null || f2 is null)
        {
            return false;
        }

        return f1.Equals(f2);
    }

    /// <summary>
    /// <para>
    /// Reports whether f1 != f2, using the notion of equality from the <see  cref="Equals"/> method.
    /// </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula? f1, Formula? f2)
    {
        return !(f1 == f2);
    }

    /// <summary>
    /// <para>
    /// Determines if two formula objects represent the same formula.
    /// </para>
    /// <para>
    /// By definition, if the parameter is null or does not reference
    /// a Formula Object then return false.
    /// </para>
    /// <para>
    /// Two Formulas are considered equal if their canonical string representations
    /// (as defined by ToString) are equal.
    /// </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    /// True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj is Formula otherFormula)
        {
            if (this.ToString() == otherFormula.ToString())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// <para>
    /// Evaluates this Formula, using the lookup delegate to determine the values  of
    /// variables.
    /// </para>
    /// <remarks>
    /// When the lookup method is called, it will always be passed a normalized (capitalized)
    /// variable name. The lookup method will throw an ArgumentException if there is
    /// not a definition for that variable token.
    /// </remarks>
    /// <para>
    /// If no undefined variables or divisions by zero are encountered when evaluating
    /// this Formula, the numeric value of the formula is returned. Otherwise, a
    /// FormulaError is returned (with a meaningful explanation as the Reason property).
    /// </para>
    /// <para>
    /// This method should never throw an exception.
    /// </para>
    /// </summary>
    /// <param name="lookup">
    /// <para>
    /// Given a variable symbol as its parameter, lookup returns the variable's value
    /// (if it has one) or throws an ArgumentException (otherwise). This method will expect
    /// variable names to be normalized.
    /// </para>
    /// </param>
    /// <returns> Either a double or a FormulaError, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        Stack<double> valueStack = new Stack<double>();
        Stack<string> operatorStack = new Stack<string>();
        List<string> parsedTokens = GetTokens(this.rawFormulaString);

        for (int idx = 0; idx < parsedTokens.Count; idx++)
        {
            string element = parsedTokens[idx];
            bool isNumber = double.TryParse(element, out double numericValue);
            bool isVariable = IsVar(element);

            if (isNumber == true)
            {
                FormulaError? errorResult = ProcessValueToken(numericValue, valueStack, operatorStack);
                if (errorResult != null)
                {
                    return errorResult;
                }
            }
            else if (isVariable == true)
            {
                try
                {
                    double lookedUpValue = lookup(element.ToUpper());
                    FormulaError? errorResult = ProcessValueToken(lookedUpValue, valueStack, operatorStack);
                    if (errorResult != null)
                    {
                        return errorResult;
                    }
                }
                catch (ArgumentException)
                {
                    return new FormulaError($"Undefined variable token: {element}");
                }
            }
            else if (element == "+" || element == "-")
            {
                if (operatorStack.Count > 0)
                {
                    string topOp = operatorStack.Peek();
                    if (topOp == "+" || topOp == "-")
                    {
                        FormulaError? errorResult = ApplyTopOperator(valueStack, operatorStack);
                        if (errorResult != null)
                        {
                            return errorResult;
                        }
                    }
                }
                operatorStack.Push(element);
            }
            else if (element == "*" || element == "/" || element == "(")
            {
                operatorStack.Push(element);
            }
            else if (element == ")")
            {
                if (operatorStack.Count > 0)
                {
                    string topOp = operatorStack.Peek();
                    if (topOp == "+" || topOp == "-")
                    {
                        FormulaError? errorResult = ApplyTopOperator(valueStack, operatorStack);
                        if (errorResult != null)
                        {
                            return errorResult;
                        }
                    }
                }

                if (operatorStack.Count > 0 && operatorStack.Peek() == "(")
                {
                    operatorStack.Pop();
                }

                if (operatorStack.Count > 0)
                {
                    string topOp = operatorStack.Peek();
                    if (topOp == "*" || topOp == "/")
                    {
                        FormulaError? errorResult = ApplyTopOperator(valueStack, operatorStack);
                        if (errorResult != null)
                        {
                            return errorResult;
                        }
                    }
                }
            }
        }

        if (operatorStack.Count > 0)
        {
            FormulaError? errorResult = ApplyTopOperator(valueStack, operatorStack);
            if (errorResult != null)
            {
                return errorResult;
            }
        }

        return valueStack.Pop();
    }

    /// <summary>
    /// <para>
    /// Returns a hash code for this Formula. If f1.Equals(f2), then it must be the
    /// case that f1.GetHashCode() == f2.GetHashCode(). Ideally, the probability that two
    /// randomly-generated unequal Formulas have the same hash code should be miniscule.
    /// </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return this.normalizedFormulaString.GetHashCode();
    }

    /// <summary>
    /// Helper method for Evaluate to handle processing a single numeric value or looked-up variable.
    /// </summary>
    private static FormulaError? ProcessValueToken(double val, Stack<double> valueStack, Stack<string> operatorStack)
    {
        if (operatorStack.Count > 0)
        {
            string topOp = operatorStack.Peek();
            if (topOp == "*" || topOp == "/")
            {
                string op = operatorStack.Pop();
                double leftVal = valueStack.Pop();

                if (op == "*")
                {
                    valueStack.Push(leftVal * val);
                }
                else if (op == "/")
                {
                    if (val == 0.0)
                    {
                        return new FormulaError("Division by zero occurred.");
                    }
                    valueStack.Push(leftVal / val);
                }
            }
            else
            {
                valueStack.Push(val);
            }
        }
        else
        {
            valueStack.Push(val);
        }

        return null;
    }

    /// <summary>
    /// Helper method for Evaluate to pop an operator and two values, applying addition or subtraction.
    /// </summary>
    private static FormulaError? ApplyTopOperator(Stack<double> valueStack, Stack<string> operatorStack)
    {
        string op = operatorStack.Pop();
        double rightVal = valueStack.Pop();
        double leftVal = valueStack.Pop();

        if (op == "+")
        {
            valueStack.Push(leftVal + rightVal);
        }
        else if (op == "-")
        {
            valueStack.Push(leftVal - rightVal);
        }

        return null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Formula"/> class.
    /// <para>
    /// Creates a Formula from a string that consists of an infix expression written as
    /// described in the class comment. If the expression is syntactically incorrect,
    /// throws a FormulaFormatException with an explanatory Message. See the assignment
    /// specifications for the syntax rules you are to implement.
    /// </para>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        List<string> parsedTokens = GetTokens(formula);
        // Rule 1
        if (parsedTokens.Count == 0)
        {
            throw new FormulaFormatException("Formula string cannot be empty.");
        }

        // Rule 5
        string startToken = parsedTokens[0];
        bool startIsNum = double.TryParse(startToken, out _);
        bool startIsVar = IsVar(startToken);

        if (startIsNum == false && startIsVar == false && startToken != "(")
        {
            throw new FormulaFormatException("First token must be a number, variable, or '('.");
        }

        // Rule 6
        string endToken = parsedTokens[parsedTokens.Count - 1];
        bool endIsNum = double.TryParse(endToken, out _);
        bool endIsVar = IsVar(endToken);

        if (endIsNum == false && endIsVar == false && endToken != ")")
        {
            throw new FormulaFormatException("Last token must be a number, variable, or ')'.");
        }

        int leftParenCount = 0;
        int rightParenCount = 0;
        string canonicalBuilder = "";

        // Goes over the tokens to check if there correct 
        for (int idx = 0; idx < parsedTokens.Count; idx++)
        {
            string element = parsedTokens[idx];
            bool isNumber = double.TryParse(element, out double numericValue);
            bool isVariable = IsVar(element);
            bool isOperator = false;

            if (element == "+" || element == "-" || element == "*" || element == "/")
            {
                isOperator = true;
            }

            // Rule 2
            if (isNumber == false)
            {
                if (isVariable == false)
                {
                    if (isOperator == false)
                    {
                        if (element != "(" && element != ")")
                        {
                            throw new FormulaFormatException($"Invalid token found: {element}");
                        }
                    }
                }
            }

            // Rules 3 & 4
            if (element == "(")
            {
                leftParenCount++;
            }
            else if (element == ")")
            {
                rightParenCount++;
                if (rightParenCount > leftParenCount)
                {
                    throw new FormulaFormatException("Closing parenthesis count cannot exceed opening count.");
                }
            }

            // Rules 7 & 8
            if (idx + 1 < parsedTokens.Count)
            {
                string nextElement = parsedTokens[idx + 1];
                bool nextIsNum = double.TryParse(nextElement, out _);
                bool nextIsVar = IsVar(nextElement);

                // Rule 7
                if (element == "(" || isOperator == true)
                {
                    if (nextIsNum == false)
                    {
                        if (nextIsVar == false)
                        {
                            if (nextElement != "(")
                            {
                                throw new FormulaFormatException(
                                    "Operator or '(' must be followed by a number, variable, or '('.");
                            }
                        }
                    }
                }

                // Rule 8
                if (isNumber || isVariable || element == ")")
                {
                    bool nextIsOp = false;
                    if (nextElement == "+" || nextElement == "-" || nextElement == "*" || nextElement == "/")
                    {
                        nextIsOp = true;
                    }

                    if (!nextIsOp && nextElement != ")")
                    {
                        throw new FormulaFormatException(
                            "Number, variable, or ')' must be followed by an operator or ')'.");
                    }
                }
            }

            // Makes the token valid
            if (isNumber)
            {
                canonicalBuilder += numericValue.ToString();
            }
            else if (isVariable)
            {
                canonicalBuilder += element.ToUpper();
            }
            else
            {
                canonicalBuilder += element;
            }
        }

        // Rule 4: Balanced Parentheses Check
        if (leftParenCount != rightParenCount)
        {
            throw new FormulaFormatException("Total count of opening and closing parentheses must be equal.");
        }

        this.rawFormulaString = formula;
        this.normalizedFormulaString = canonicalBuilder;
    }

    /// <summary>
    /// <para>
    /// Returns a set of all the variables in the formula.
    /// </para>
    /// <remarks>
    /// Important: no variable may appear more than once in the returned set, even
    /// if it is used more than once in the Formula.
    /// Variables should be returned in canonical form, having all letters converted
    /// to uppercase.
    /// </remarks>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        HashSet<string> uniqueVariables = new HashSet<string>();
        List<string> allTokens = GetTokens(this.rawFormulaString);

        foreach (string item in allTokens)
        {
            if (IsVar(item))
            {
                uniqueVariables.Add(item.ToUpper());
            }
        }

        return uniqueVariables;
    }

    /// <summary>
    /// <para>
    /// Returns a string representation of a canonical form of the formula.
    /// </para>
    /// <para>
    /// The string will contain no spaces.
    /// </para>
    /// <para>
    /// This method executes in O(1) time.
    /// </para>
    /// </summary>
    /// <returns>
    /// A canonical version (string) of the formula. All "equal" formulas
    /// should have the same value here.
    /// </returns>
    public override string ToString()
    {
        return this.normalizedFormulaString;
    }

    /// <summary>
    /// Reports whether "token" is a variable. It must be one or more letters
    /// followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    /// <para>
    /// Given an expression, enumerates the tokens that compose it.
    /// </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = [];
        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        string pattern = string.Format(
            "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
            lpPattern, rpPattern, opPattern, VariableRegExPattern, doublePattern, spacePattern);

        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }
}

/// <summary>
/// Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
    }
}