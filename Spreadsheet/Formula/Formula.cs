// <summary>
// <para>
// This code is provided to start your assignment. It was written
// by Profs Joe, Danny, Jim, and Travis. You should keep this attribution
// at the top of your code where you have your header comment, along
// with any other required information.
// </para>
// <para>
// You should remove/add/adjust comments in your file as appropriate
// to represent your work and any changes you make.
// </para>
// </summary>

namespace Formula;

using System.Text.RegularExpressions;

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
                                throw new FormulaFormatException("Operator or '(' must be followed by a number, variable, or '('.");
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
                        throw new FormulaFormatException("Number, variable, or ')' must be followed by an operator or ')'.");
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