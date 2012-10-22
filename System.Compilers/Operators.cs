using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers
{
    public enum Operators
    {
        None,
        Addition,
        Subtraction,
        Multiply,
        Division,
        Modulus,
        Equality,
        Inequality,
        Cast,
        Explicit = Cast,
        Implicit,
        LessThan,
        LessThanOrEquals,
        GreaterThan,
        GreaterThanOrEquals,
        LogicOr,
        LogicAnd,
        LogicXor,
        ConditionalOr,
        ConditionalAnd,
        Not,
        UnaryPlus,
        UnaryNegation,
        TernaryDecision,
        PreIncrement,
        PreDecrement,
        PostIncrement,
        PostDecrement,
        Indexer
    }


    public static class OperatorsExtensor
    {
        public static string Token(this Operators op)
        {
            switch (op)
            {
                case Operators.Addition: return "+";
                case Operators.Subtraction: return "-";
                case Operators.Multiply: return "*";
                case Operators.Division: return "/";
                case Operators.Modulus: return "%";
                case Operators.LessThan: return "<";
                case Operators.LessThanOrEquals: return "<=";
                case Operators.LogicAnd: return "&";
                case Operators.LogicOr: return "|";
                case Operators.LogicXor: return "^";
                case Operators.GreaterThan: return ">";
                case Operators.GreaterThanOrEquals: return ">=";
                case Operators.Equality: return "==";
                case Operators.Inequality: return "!=";
                case Operators.ConditionalAnd: return "&&";
                case Operators.ConditionalOr: return "||";

                case Operators.PreDecrement:
                case Operators.PostDecrement:
                    return "--";
                case Operators.PreIncrement:
                case Operators.PostIncrement:
                    return "++";

                case Operators.Not: return "!";
                case Operators.UnaryNegation: return "-";
                case Operators.UnaryPlus: return "+";
            }

            return "";
        }

        public static Operators Parse(string op)
        {
            return (Operators)Enum.Parse(typeof(Operators), op);
        }
    }
}
