// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g 2010-07-11 00:42:44


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public class GLSLLexer : Lexer {
    public const int EXPONENT = 103;
    public const int MAT4X4 = 82;
    public const int FLOAT_SUFFIX = 104;
    public const int WHILE = 42;
    public const int LETTER = 105;
    public const int CONST = 49;
    public const int VARYING = 52;
    public const int MAT2X4 = 74;
    public const int DO = 43;
    public const int DQUOTE = 37;
    public const int MAT2X2 = 72;
    public const int MAT2X3 = 73;
    public const int SAMPLER1DSHADOW = 87;
    public const int NOT = 10;
    public const int UNIFORM = 51;
    public const int EOF = -1;
    public const int SUBASSIGN = 32;
    public const int MAT4X3 = 81;
    public const int MAT4X2 = 80;
    public const int BREAK = 45;
    public const int OCTAL_DIGIT = 99;
    public const int LBRACKET = 24;
    public const int LEQUAL = 7;
    public const int RPAREN = 27;
    public const int QUOTE = 36;
    public const int GREATER = 8;
    public const int BOOL_CONSTANT = 92;
    public const int RETURN = 47;
    public const int LESS = 6;
    public const int VOID = 39;
    public const int COMMENT = 106;
    public const int INT_CONSTANT = 90;
    public const int NEQUAL = 5;
    public const int MULASSIGN = 33;
    public const int RBRACE = 29;
    public const int ELSE = 41;
    public const int BOOL = 61;
    public const int INVARIANT = 54;
    public const int UNDERSCORE = 35;
    public const int SEMICOLON = 22;
    public const int SAMPLERCUBE = 86;
    public const int MUL = 11;
    public const int DECREMENT = 16;
    public const int WS = 107;
    public const int DISCARD = 48;
    public const int ADDASSIGN = 31;
    public const int CENTROID = 53;
    public const int OUT = 56;
    public const int OR = 17;
    public const int OCTAL_CONSTANT = 94;
    public const int MAT3X3 = 77;
    public const int SAMPLER1D = 83;
    public const int MAT3X4 = 78;
    public const int LBRACE = 28;
    public const int ATTRIBUTE = 50;
    public const int MAT3X2 = 76;
    public const int FOR = 44;
    public const int SUB = 15;
    public const int FLOAT = 60;
    public const int AND = 18;
    public const int ID = 89;
    public const int LPAREN = 26;
    public const int ZERO = 96;
    public const int IF = 40;
    public const int INOUT = 57;
    public const int VEC4 = 64;
    public const int IN = 55;
    public const int CONTINUE = 46;
    public const int COMMA = 21;
    public const int GEQUAL = 9;
    public const int EQUAL = 4;
    public const int SAMPLER2D = 84;
    public const int SAMPLER2DSHADOW = 88;
    public const int DIGIT = 100;
    public const int RBRACKET = 25;
    public const int DOT = 20;
    public const int FRACTIONAL_CONSTANT = 102;
    public const int ADD = 13;
    public const int INTEGER = 59;
    public const int DIVASSIGN = 34;
    public const int XOR = 19;
    public const int FLOAT_CONSTANT = 91;
    public const int NON_ZERO_DIGIT = 97;
    public const int SAMPLER3D = 85;
    public const int HEX_DIGIT = 101;
    public const int DECIMAL_CONSTANT = 93;
    public const int STRUCT = 58;
    public const int HEX_CONSTANT = 95;
    public const int BVEC4 = 70;
    public const int COLON = 23;
    public const int BVEC2 = 68;
    public const int INCREMENT = 14;
    public const int BVEC3 = 69;
    public const int VEC2 = 62;
    public const int QUESTION = 38;
    public const int VEC3 = 63;
    public const int DIGIT_SEQUENCE = 98;
    public const int ASSIGN = 30;
    public const int IVEC3 = 66;
    public const int IVEC4 = 67;
    public const int DIV = 12;
    public const int MAT2 = 71;
    public const int MAT3 = 75;
    public const int IVEC2 = 65;
    public const int MAT4 = 79;

    // delegates
    // delegators

    public GLSLLexer() 
    {
		InitializeCyclicDFAs();
    }
    public GLSLLexer(ICharStream input)
		: this(input, null) {
    }
    public GLSLLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g";} 
    }

    // $ANTLR start "EQUAL"
    public void mEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:7:7: ( '==' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:7:9: '=='
            {
            	Match("=="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQUAL"

    // $ANTLR start "NEQUAL"
    public void mNEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:8:8: ( '!=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:8:10: '!='
            {
            	Match("!="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEQUAL"

    // $ANTLR start "LESS"
    public void mLESS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LESS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:9:6: ( '<' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:9:8: '<'
            {
            	Match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LESS"

    // $ANTLR start "LEQUAL"
    public void mLEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LEQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:10:8: ( '<=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:10:10: '<='
            {
            	Match("<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LEQUAL"

    // $ANTLR start "GREATER"
    public void mGREATER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GREATER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:11:9: ( '>' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:11:11: '>'
            {
            	Match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GREATER"

    // $ANTLR start "GEQUAL"
    public void mGEQUAL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GEQUAL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:12:8: ( '>=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:12:10: '>='
            {
            	Match(">="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GEQUAL"

    // $ANTLR start "NOT"
    public void mNOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:13:5: ( '!' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:13:7: '!'
            {
            	Match('!'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NOT"

    // $ANTLR start "MUL"
    public void mMUL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MUL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:14:5: ( '*' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:14:7: '*'
            {
            	Match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MUL"

    // $ANTLR start "DIV"
    public void mDIV() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIV;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:15:5: ( '/' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:15:7: '/'
            {
            	Match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIV"

    // $ANTLR start "ADD"
    public void mADD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ADD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:16:5: ( '+' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:16:7: '+'
            {
            	Match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ADD"

    // $ANTLR start "INCREMENT"
    public void mINCREMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INCREMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:17:11: ( '++' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:17:13: '++'
            {
            	Match("++"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INCREMENT"

    // $ANTLR start "SUB"
    public void mSUB() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SUB;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:18:5: ( '-' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:18:7: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SUB"

    // $ANTLR start "DECREMENT"
    public void mDECREMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DECREMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:19:11: ( '--' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:19:13: '--'
            {
            	Match("--"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DECREMENT"

    // $ANTLR start "OR"
    public void mOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:20:4: ( '||' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:20:6: '||'
            {
            	Match("||"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OR"

    // $ANTLR start "AND"
    public void mAND() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AND;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:21:5: ( '&&' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:21:7: '&&'
            {
            	Match("&&"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AND"

    // $ANTLR start "XOR"
    public void mXOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = XOR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:22:5: ( '^^' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:22:7: '^^'
            {
            	Match("^^"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "XOR"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:23:5: ( '.' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:23:7: '.'
            {
            	Match('.'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOT"

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:24:7: ( ',' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:24:9: ','
            {
            	Match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMA"

    // $ANTLR start "SEMICOLON"
    public void mSEMICOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SEMICOLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:25:11: ( ';' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:25:13: ';'
            {
            	Match(';'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SEMICOLON"

    // $ANTLR start "COLON"
    public void mCOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:26:7: ( ':' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:26:9: ':'
            {
            	Match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COLON"

    // $ANTLR start "LBRACKET"
    public void mLBRACKET() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LBRACKET;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:27:10: ( '[' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:27:12: '['
            {
            	Match('['); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LBRACKET"

    // $ANTLR start "RBRACKET"
    public void mRBRACKET() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RBRACKET;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:28:10: ( ']' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:28:12: ']'
            {
            	Match(']'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RBRACKET"

    // $ANTLR start "LPAREN"
    public void mLPAREN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LPAREN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:29:8: ( '(' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:29:10: '('
            {
            	Match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LPAREN"

    // $ANTLR start "RPAREN"
    public void mRPAREN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RPAREN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:30:8: ( ')' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:30:10: ')'
            {
            	Match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RPAREN"

    // $ANTLR start "LBRACE"
    public void mLBRACE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LBRACE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:31:8: ( '{' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:31:10: '{'
            {
            	Match('{'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LBRACE"

    // $ANTLR start "RBRACE"
    public void mRBRACE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RBRACE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:32:8: ( '}' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:32:10: '}'
            {
            	Match('}'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RBRACE"

    // $ANTLR start "ASSIGN"
    public void mASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:33:8: ( '=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:33:10: '='
            {
            	Match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ASSIGN"

    // $ANTLR start "ADDASSIGN"
    public void mADDASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ADDASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:34:11: ( '+=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:34:13: '+='
            {
            	Match("+="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ADDASSIGN"

    // $ANTLR start "SUBASSIGN"
    public void mSUBASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SUBASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:35:11: ( '-=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:35:13: '-='
            {
            	Match("-="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SUBASSIGN"

    // $ANTLR start "MULASSIGN"
    public void mMULASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MULASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:36:11: ( '*=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:36:13: '*='
            {
            	Match("*="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MULASSIGN"

    // $ANTLR start "DIVASSIGN"
    public void mDIVASSIGN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIVASSIGN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:37:11: ( '/=' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:37:13: '/='
            {
            	Match("/="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIVASSIGN"

    // $ANTLR start "UNDERSCORE"
    public void mUNDERSCORE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UNDERSCORE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:38:12: ( '_' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:38:14: '_'
            {
            	Match('_'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UNDERSCORE"

    // $ANTLR start "QUOTE"
    public void mQUOTE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = QUOTE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:39:7: ( '\\'' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:39:9: '\\''
            {
            	Match('\''); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "QUOTE"

    // $ANTLR start "DQUOTE"
    public void mDQUOTE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DQUOTE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:40:8: ( '\"' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:40:10: '\"'
            {
            	Match('\"'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DQUOTE"

    // $ANTLR start "QUESTION"
    public void mQUESTION() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = QUESTION;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:41:10: ( '?' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:41:12: '?'
            {
            	Match('?'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "QUESTION"

    // $ANTLR start "VOID"
    public void mVOID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VOID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:42:6: ( 'void' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:42:8: 'void'
            {
            	Match("void"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VOID"

    // $ANTLR start "IF"
    public void mIF() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IF;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:43:4: ( 'if' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:43:6: 'if'
            {
            	Match("if"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IF"

    // $ANTLR start "ELSE"
    public void mELSE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ELSE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:44:6: ( 'else' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:44:8: 'else'
            {
            	Match("else"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ELSE"

    // $ANTLR start "WHILE"
    public void mWHILE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WHILE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:45:7: ( 'while' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:45:9: 'while'
            {
            	Match("while"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WHILE"

    // $ANTLR start "DO"
    public void mDO() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DO;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:46:4: ( 'do' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:46:6: 'do'
            {
            	Match("do"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DO"

    // $ANTLR start "FOR"
    public void mFOR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FOR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:47:5: ( 'for' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:47:7: 'for'
            {
            	Match("for"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FOR"

    // $ANTLR start "BREAK"
    public void mBREAK() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BREAK;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:48:7: ( 'break' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:48:9: 'break'
            {
            	Match("break"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BREAK"

    // $ANTLR start "CONTINUE"
    public void mCONTINUE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CONTINUE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:49:10: ( 'continue' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:49:12: 'continue'
            {
            	Match("continue"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CONTINUE"

    // $ANTLR start "RETURN"
    public void mRETURN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RETURN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:50:8: ( 'return' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:50:10: 'return'
            {
            	Match("return"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RETURN"

    // $ANTLR start "DISCARD"
    public void mDISCARD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DISCARD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:51:9: ( 'discard' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:51:11: 'discard'
            {
            	Match("discard"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DISCARD"

    // $ANTLR start "CONST"
    public void mCONST() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CONST;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:52:7: ( 'const' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:52:9: 'const'
            {
            	Match("const"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CONST"

    // $ANTLR start "ATTRIBUTE"
    public void mATTRIBUTE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ATTRIBUTE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:53:11: ( 'attribute' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:53:13: 'attribute'
            {
            	Match("attribute"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ATTRIBUTE"

    // $ANTLR start "UNIFORM"
    public void mUNIFORM() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = UNIFORM;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:54:9: ( 'uniform' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:54:11: 'uniform'
            {
            	Match("uniform"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "UNIFORM"

    // $ANTLR start "VARYING"
    public void mVARYING() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VARYING;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:55:9: ( 'varying' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:55:11: 'varying'
            {
            	Match("varying"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VARYING"

    // $ANTLR start "CENTROID"
    public void mCENTROID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CENTROID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:56:10: ( 'centroid' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:56:12: 'centroid'
            {
            	Match("centroid"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CENTROID"

    // $ANTLR start "INVARIANT"
    public void mINVARIANT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INVARIANT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:57:11: ( 'invariant' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:57:13: 'invariant'
            {
            	Match("invariant"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INVARIANT"

    // $ANTLR start "IN"
    public void mIN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:58:4: ( 'in' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:58:6: 'in'
            {
            	Match("in"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IN"

    // $ANTLR start "OUT"
    public void mOUT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OUT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:59:5: ( 'out' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:59:7: 'out'
            {
            	Match("out"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OUT"

    // $ANTLR start "INOUT"
    public void mINOUT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INOUT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:60:7: ( 'inout' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:60:9: 'inout'
            {
            	Match("inout"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INOUT"

    // $ANTLR start "STRUCT"
    public void mSTRUCT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRUCT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:61:8: ( 'struct' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:61:10: 'struct'
            {
            	Match("struct"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "STRUCT"

    // $ANTLR start "INTEGER"
    public void mINTEGER() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INTEGER;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:62:9: ( 'int' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:62:11: 'int'
            {
            	Match("int"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INTEGER"

    // $ANTLR start "FLOAT"
    public void mFLOAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FLOAT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:63:7: ( 'float' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:63:9: 'float'
            {
            	Match("float"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FLOAT"

    // $ANTLR start "BOOL"
    public void mBOOL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BOOL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:64:6: ( 'bool' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:64:8: 'bool'
            {
            	Match("bool"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BOOL"

    // $ANTLR start "VEC2"
    public void mVEC2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VEC2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:65:6: ( 'vec2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:65:8: 'vec2'
            {
            	Match("vec2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VEC2"

    // $ANTLR start "VEC3"
    public void mVEC3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VEC3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:66:6: ( 'vec3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:66:8: 'vec3'
            {
            	Match("vec3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VEC3"

    // $ANTLR start "VEC4"
    public void mVEC4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = VEC4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:67:6: ( 'vec4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:67:8: 'vec4'
            {
            	Match("vec4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "VEC4"

    // $ANTLR start "IVEC2"
    public void mIVEC2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IVEC2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:68:7: ( 'ivec2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:68:9: 'ivec2'
            {
            	Match("ivec2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IVEC2"

    // $ANTLR start "IVEC3"
    public void mIVEC3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IVEC3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:69:7: ( 'ivec3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:69:9: 'ivec3'
            {
            	Match("ivec3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IVEC3"

    // $ANTLR start "IVEC4"
    public void mIVEC4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = IVEC4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:70:7: ( 'ivec4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:70:9: 'ivec4'
            {
            	Match("ivec4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "IVEC4"

    // $ANTLR start "BVEC2"
    public void mBVEC2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BVEC2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:71:7: ( 'bvec2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:71:9: 'bvec2'
            {
            	Match("bvec2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BVEC2"

    // $ANTLR start "BVEC3"
    public void mBVEC3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BVEC3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:72:7: ( 'bvec3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:72:9: 'bvec3'
            {
            	Match("bvec3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BVEC3"

    // $ANTLR start "BVEC4"
    public void mBVEC4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BVEC4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:73:7: ( 'bvec4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:73:9: 'bvec4'
            {
            	Match("bvec4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BVEC4"

    // $ANTLR start "MAT2"
    public void mMAT2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:74:6: ( 'mat2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:74:8: 'mat2'
            {
            	Match("mat2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT2"

    // $ANTLR start "MAT2X2"
    public void mMAT2X2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT2X2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:75:8: ( 'mat2x2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:75:10: 'mat2x2'
            {
            	Match("mat2x2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT2X2"

    // $ANTLR start "MAT2X3"
    public void mMAT2X3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT2X3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:76:8: ( 'mat2x3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:76:10: 'mat2x3'
            {
            	Match("mat2x3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT2X3"

    // $ANTLR start "MAT2X4"
    public void mMAT2X4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT2X4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:77:8: ( 'mat2x4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:77:10: 'mat2x4'
            {
            	Match("mat2x4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT2X4"

    // $ANTLR start "MAT3"
    public void mMAT3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:78:6: ( 'mat3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:78:8: 'mat3'
            {
            	Match("mat3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT3"

    // $ANTLR start "MAT3X2"
    public void mMAT3X2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT3X2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:79:8: ( 'mat3x2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:79:10: 'mat3x2'
            {
            	Match("mat3x2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT3X2"

    // $ANTLR start "MAT3X3"
    public void mMAT3X3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT3X3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:80:8: ( 'mat3x3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:80:10: 'mat3x3'
            {
            	Match("mat3x3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT3X3"

    // $ANTLR start "MAT3X4"
    public void mMAT3X4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT3X4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:81:8: ( 'mat3x4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:81:10: 'mat3x4'
            {
            	Match("mat3x4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT3X4"

    // $ANTLR start "MAT4"
    public void mMAT4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:82:6: ( 'mat4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:82:8: 'mat4'
            {
            	Match("mat4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT4"

    // $ANTLR start "MAT4X2"
    public void mMAT4X2() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT4X2;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:83:8: ( 'mat4x2' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:83:10: 'mat4x2'
            {
            	Match("mat4x2"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT4X2"

    // $ANTLR start "MAT4X3"
    public void mMAT4X3() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT4X3;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:84:8: ( 'mat4x3' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:84:10: 'mat4x3'
            {
            	Match("mat4x3"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT4X3"

    // $ANTLR start "MAT4X4"
    public void mMAT4X4() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MAT4X4;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:85:8: ( 'mat4x4' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:85:10: 'mat4x4'
            {
            	Match("mat4x4"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MAT4X4"

    // $ANTLR start "SAMPLER1D"
    public void mSAMPLER1D() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMPLER1D;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:86:11: ( 'sampler1D' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:86:13: 'sampler1D'
            {
            	Match("sampler1D"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMPLER1D"

    // $ANTLR start "SAMPLER2D"
    public void mSAMPLER2D() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMPLER2D;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:87:11: ( 'sampler2D' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:87:13: 'sampler2D'
            {
            	Match("sampler2D"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMPLER2D"

    // $ANTLR start "SAMPLER3D"
    public void mSAMPLER3D() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMPLER3D;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:88:11: ( 'sampler3D' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:88:13: 'sampler3D'
            {
            	Match("sampler3D"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMPLER3D"

    // $ANTLR start "SAMPLERCUBE"
    public void mSAMPLERCUBE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMPLERCUBE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:89:13: ( 'samplerCube' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:89:15: 'samplerCube'
            {
            	Match("samplerCube"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMPLERCUBE"

    // $ANTLR start "SAMPLER1DSHADOW"
    public void mSAMPLER1DSHADOW() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMPLER1DSHADOW;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:90:17: ( 'sampler1DShadow' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:90:19: 'sampler1DShadow'
            {
            	Match("sampler1DShadow"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMPLER1DSHADOW"

    // $ANTLR start "SAMPLER2DSHADOW"
    public void mSAMPLER2DSHADOW() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SAMPLER2DSHADOW;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:91:17: ( 'sampler2DShadow' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:91:19: 'sampler2DShadow'
            {
            	Match("sampler2DShadow"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SAMPLER2DSHADOW"

    // $ANTLR start "INT_CONSTANT"
    public void mINT_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INT_CONSTANT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:733:2: ( DECIMAL_CONSTANT | OCTAL_CONSTANT | HEX_CONSTANT )
            int alt1 = 3;
            int LA1_0 = input.LA(1);

            if ( (LA1_0 == '0') )
            {
                switch ( input.LA(2) ) 
                {
                case 'X':
                case 'x':
                	{
                    alt1 = 3;
                    }
                    break;
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                	{
                    alt1 = 2;
                    }
                    break;
                	default:
                    	alt1 = 1;
                    	break;}

            }
            else if ( ((LA1_0 >= '1' && LA1_0 <= '9')) )
            {
                alt1 = 1;
            }
            else 
            {
                NoViableAltException nvae_d1s0 =
                    new NoViableAltException("", 1, 0, input);

                throw nvae_d1s0;
            }
            switch (alt1) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:733:5: DECIMAL_CONSTANT
                    {
                    	mDECIMAL_CONSTANT(); 

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:734:4: OCTAL_CONSTANT
                    {
                    	mOCTAL_CONSTANT(); 

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:735:4: HEX_CONSTANT
                    {
                    	mHEX_CONSTANT(); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INT_CONSTANT"

    // $ANTLR start "DECIMAL_CONSTANT"
    public void mDECIMAL_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:739:2: ( ZERO | NON_ZERO_DIGIT ( DIGIT_SEQUENCE )? )
            int alt3 = 2;
            int LA3_0 = input.LA(1);

            if ( (LA3_0 == '0') )
            {
                alt3 = 1;
            }
            else if ( ((LA3_0 >= '1' && LA3_0 <= '9')) )
            {
                alt3 = 2;
            }
            else 
            {
                NoViableAltException nvae_d3s0 =
                    new NoViableAltException("", 3, 0, input);

                throw nvae_d3s0;
            }
            switch (alt3) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:739:4: ZERO
                    {
                    	mZERO(); 

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:740:4: NON_ZERO_DIGIT ( DIGIT_SEQUENCE )?
                    {
                    	mNON_ZERO_DIGIT(); 
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:740:19: ( DIGIT_SEQUENCE )?
                    	int alt2 = 2;
                    	int LA2_0 = input.LA(1);

                    	if ( ((LA2_0 >= '0' && LA2_0 <= '9')) )
                    	{
                    	    alt2 = 1;
                    	}
                    	switch (alt2) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:740:20: DIGIT_SEQUENCE
                    	        {
                    	        	mDIGIT_SEQUENCE(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DECIMAL_CONSTANT"

    // $ANTLR start "ZERO"
    public void mZERO() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:744:2: ( '0' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:744:4: '0'
            {
            	Match('0'); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "ZERO"

    // $ANTLR start "NON_ZERO_DIGIT"
    public void mNON_ZERO_DIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:748:2: ( OCTAL_DIGIT | '8' | '9' )
            int alt4 = 3;
            switch ( input.LA(1) ) 
            {
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            	{
                alt4 = 1;
                }
                break;
            case '8':
            	{
                alt4 = 2;
                }
                break;
            case '9':
            	{
                alt4 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d4s0 =
            	        new NoViableAltException("", 4, 0, input);

            	    throw nvae_d4s0;
            }

            switch (alt4) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:748:4: OCTAL_DIGIT
                    {
                    	mOCTAL_DIGIT(); 

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:748:17: '8'
                    {
                    	Match('8'); 

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:748:21: '9'
                    {
                    	Match('9'); 

                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NON_ZERO_DIGIT"

    // $ANTLR start "DIGIT"
    public void mDIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:752:2: ( ZERO | NON_ZERO_DIGIT )
            int alt5 = 2;
            int LA5_0 = input.LA(1);

            if ( (LA5_0 == '0') )
            {
                alt5 = 1;
            }
            else if ( ((LA5_0 >= '1' && LA5_0 <= '9')) )
            {
                alt5 = 2;
            }
            else 
            {
                NoViableAltException nvae_d5s0 =
                    new NoViableAltException("", 5, 0, input);

                throw nvae_d5s0;
            }
            switch (alt5) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:752:4: ZERO
                    {
                    	mZERO(); 

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:753:4: NON_ZERO_DIGIT
                    {
                    	mNON_ZERO_DIGIT(); 

                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIGIT"

    // $ANTLR start "OCTAL_CONSTANT"
    public void mOCTAL_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:757:2: ( ZERO ( OCTAL_DIGIT )+ )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:757:4: ZERO ( OCTAL_DIGIT )+
            {
            	mZERO(); 
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:757:9: ( OCTAL_DIGIT )+
            	int cnt6 = 0;
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);

            	    if ( ((LA6_0 >= '1' && LA6_0 <= '7')) )
            	    {
            	        alt6 = 1;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:757:10: OCTAL_DIGIT
            			    {
            			    	mOCTAL_DIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt6 >= 1 ) goto loop6;
            		            EarlyExitException eee6 =
            		                new EarlyExitException(6, input);
            		            throw eee6;
            	    }
            	    cnt6++;
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "OCTAL_CONSTANT"

    // $ANTLR start "OCTAL_DIGIT"
    public void mOCTAL_DIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:761:2: ( ( '1' .. '7' ) )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:761:4: ( '1' .. '7' )
            {
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:761:4: ( '1' .. '7' )
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:761:5: '1' .. '7'
            	{
            		MatchRange('1','7'); 

            	}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "OCTAL_DIGIT"

    // $ANTLR start "HEX_CONSTANT"
    public void mHEX_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:765:2: ( ZERO ( 'x' | 'X' ) ( HEX_DIGIT )+ )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:765:4: ZERO ( 'x' | 'X' ) ( HEX_DIGIT )+
            {
            	mZERO(); 
            	if ( input.LA(1) == 'X' || input.LA(1) == 'x' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:765:19: ( HEX_DIGIT )+
            	int cnt7 = 0;
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);

            	    if ( ((LA7_0 >= '0' && LA7_0 <= '9') || (LA7_0 >= 'A' && LA7_0 <= 'F') || (LA7_0 >= 'a' && LA7_0 <= 'f')) )
            	    {
            	        alt7 = 1;
            	    }


            	    switch (alt7) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:765:20: HEX_DIGIT
            			    {
            			    	mHEX_DIGIT(); 

            			    }
            			    break;

            			default:
            			    if ( cnt7 >= 1 ) goto loop7;
            		            EarlyExitException eee7 =
            		                new EarlyExitException(7, input);
            		            throw eee7;
            	    }
            	    cnt7++;
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "HEX_CONSTANT"

    // $ANTLR start "HEX_DIGIT"
    public void mHEX_DIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:769:2: ( DIGIT | 'a' .. 'f' | 'A' .. 'F' )
            int alt8 = 3;
            switch ( input.LA(1) ) 
            {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            	{
                alt8 = 1;
                }
                break;
            case 'a':
            case 'b':
            case 'c':
            case 'd':
            case 'e':
            case 'f':
            	{
                alt8 = 2;
                }
                break;
            case 'A':
            case 'B':
            case 'C':
            case 'D':
            case 'E':
            case 'F':
            	{
                alt8 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d8s0 =
            	        new NoViableAltException("", 8, 0, input);

            	    throw nvae_d8s0;
            }

            switch (alt8) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:769:4: DIGIT
                    {
                    	mDIGIT(); 

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:769:10: 'a' .. 'f'
                    {
                    	MatchRange('a','f'); 

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:769:19: 'A' .. 'F'
                    {
                    	MatchRange('A','F'); 

                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "HEX_DIGIT"

    // $ANTLR start "FLOAT_CONSTANT"
    public void mFLOAT_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FLOAT_CONSTANT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:773:5: ( FRACTIONAL_CONSTANT ( EXPONENT )? ( FLOAT_SUFFIX )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:773:9: FRACTIONAL_CONSTANT ( EXPONENT )? ( FLOAT_SUFFIX )?
            {
            	mFRACTIONAL_CONSTANT(); 
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:773:29: ( EXPONENT )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == 'E' || LA9_0 == 'e') )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:773:30: EXPONENT
            	        {
            	        	mEXPONENT(); 

            	        }
            	        break;

            	}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:773:41: ( FLOAT_SUFFIX )?
            	int alt10 = 2;
            	int LA10_0 = input.LA(1);

            	if ( (LA10_0 == 'F' || LA10_0 == 'f') )
            	{
            	    alt10 = 1;
            	}
            	switch (alt10) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:773:42: FLOAT_SUFFIX
            	        {
            	        	mFLOAT_SUFFIX(); 

            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FLOAT_CONSTANT"

    // $ANTLR start "FRACTIONAL_CONSTANT"
    public void mFRACTIONAL_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:777:2: ( DIGIT_SEQUENCE DOT ( DIGIT_SEQUENCE )? | DOT DIGIT_SEQUENCE )
            int alt12 = 2;
            int LA12_0 = input.LA(1);

            if ( ((LA12_0 >= '0' && LA12_0 <= '9')) )
            {
                alt12 = 1;
            }
            else if ( (LA12_0 == '.') )
            {
                alt12 = 2;
            }
            else 
            {
                NoViableAltException nvae_d12s0 =
                    new NoViableAltException("", 12, 0, input);

                throw nvae_d12s0;
            }
            switch (alt12) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:777:5: DIGIT_SEQUENCE DOT ( DIGIT_SEQUENCE )?
                    {
                    	mDIGIT_SEQUENCE(); 
                    	mDOT(); 
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:777:24: ( DIGIT_SEQUENCE )?
                    	int alt11 = 2;
                    	int LA11_0 = input.LA(1);

                    	if ( ((LA11_0 >= '0' && LA11_0 <= '9')) )
                    	{
                    	    alt11 = 1;
                    	}
                    	switch (alt11) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:777:25: DIGIT_SEQUENCE
                    	        {
                    	        	mDIGIT_SEQUENCE(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:778:4: DOT DIGIT_SEQUENCE
                    {
                    	mDOT(); 
                    	mDIGIT_SEQUENCE(); 

                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FRACTIONAL_CONSTANT"

    // $ANTLR start "DIGIT_SEQUENCE"
    public void mDIGIT_SEQUENCE() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:782:2: ( DIGIT ( DIGIT )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:782:4: DIGIT ( DIGIT )*
            {
            	mDIGIT(); 
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:782:10: ( DIGIT )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( ((LA13_0 >= '0' && LA13_0 <= '9')) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:782:11: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;

            			default:
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIGIT_SEQUENCE"

    // $ANTLR start "EXPONENT"
    public void mEXPONENT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:786:2: ( ( 'e' | 'E' ) ( ADD | SUB )? DIGIT_SEQUENCE )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:786:4: ( 'e' | 'E' ) ( ADD | SUB )? DIGIT_SEQUENCE
            {
            	if ( input.LA(1) == 'E' || input.LA(1) == 'e' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:786:14: ( ADD | SUB )?
            	int alt14 = 2;
            	int LA14_0 = input.LA(1);

            	if ( (LA14_0 == '+' || LA14_0 == '-') )
            	{
            	    alt14 = 1;
            	}
            	switch (alt14) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:
            	        {
            	        	if ( input.LA(1) == '+' || input.LA(1) == '-' ) 
            	        	{
            	        	    input.Consume();

            	        	}
            	        	else 
            	        	{
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    Recover(mse);
            	        	    throw mse;}


            	        }
            	        break;

            	}

            	mDIGIT_SEQUENCE(); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXPONENT"

    // $ANTLR start "FLOAT_SUFFIX"
    public void mFLOAT_SUFFIX() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:790:2: ( 'f' | 'F' )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:
            {
            	if ( input.LA(1) == 'F' || input.LA(1) == 'f' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "FLOAT_SUFFIX"

    // $ANTLR start "BOOL_CONSTANT"
    public void mBOOL_CONSTANT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = BOOL_CONSTANT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:794:2: ( 'true' | 'false' )
            int alt15 = 2;
            int LA15_0 = input.LA(1);

            if ( (LA15_0 == 't') )
            {
                alt15 = 1;
            }
            else if ( (LA15_0 == 'f') )
            {
                alt15 = 2;
            }
            else 
            {
                NoViableAltException nvae_d15s0 =
                    new NoViableAltException("", 15, 0, input);

                throw nvae_d15s0;
            }
            switch (alt15) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:794:4: 'true'
                    {
                    	Match("true"); 


                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:795:4: 'false'
                    {
                    	Match("false"); 


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "BOOL_CONSTANT"

    // $ANTLR start "ID"
    public void mID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:798:5: ( ( LETTER | UNDERSCORE ) ( LETTER | DIGIT | UNDERSCORE )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:798:7: ( LETTER | UNDERSCORE ) ( LETTER | DIGIT | UNDERSCORE )*
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:798:29: ( LETTER | DIGIT | UNDERSCORE )*
            	do 
            	{
            	    int alt16 = 4;
            	    switch ( input.LA(1) ) 
            	    {
            	    case 'A':
            	    case 'B':
            	    case 'C':
            	    case 'D':
            	    case 'E':
            	    case 'F':
            	    case 'G':
            	    case 'H':
            	    case 'I':
            	    case 'J':
            	    case 'K':
            	    case 'L':
            	    case 'M':
            	    case 'N':
            	    case 'O':
            	    case 'P':
            	    case 'Q':
            	    case 'R':
            	    case 'S':
            	    case 'T':
            	    case 'U':
            	    case 'V':
            	    case 'W':
            	    case 'X':
            	    case 'Y':
            	    case 'Z':
            	    case 'a':
            	    case 'b':
            	    case 'c':
            	    case 'd':
            	    case 'e':
            	    case 'f':
            	    case 'g':
            	    case 'h':
            	    case 'i':
            	    case 'j':
            	    case 'k':
            	    case 'l':
            	    case 'm':
            	    case 'n':
            	    case 'o':
            	    case 'p':
            	    case 'q':
            	    case 'r':
            	    case 's':
            	    case 't':
            	    case 'u':
            	    case 'v':
            	    case 'w':
            	    case 'x':
            	    case 'y':
            	    case 'z':
            	    	{
            	        alt16 = 1;
            	        }
            	        break;
            	    case '0':
            	    case '1':
            	    case '2':
            	    case '3':
            	    case '4':
            	    case '5':
            	    case '6':
            	    case '7':
            	    case '8':
            	    case '9':
            	    	{
            	        alt16 = 2;
            	        }
            	        break;
            	    case '_':
            	    	{
            	        alt16 = 3;
            	        }
            	        break;

            	    }

            	    switch (alt16) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:798:30: LETTER
            			    {
            			    	mLETTER(); 

            			    }
            			    break;
            			case 2 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:798:39: DIGIT
            			    {
            			    	mDIGIT(); 

            			    }
            			    break;
            			case 3 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:798:47: UNDERSCORE
            			    {
            			    	mUNDERSCORE(); 

            			    }
            			    break;

            			default:
            			    goto loop16;
            	    }
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ID"

    // $ANTLR start "LETTER"
    public void mLETTER() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:802:2: ( ( 'a' .. 'z' | 'A' .. 'Z' ) )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:802:6: ( 'a' .. 'z' | 'A' .. 'Z' )
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "LETTER"

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:807:5: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' | '/*' ( options {greedy=false; } : . )* '*/' )
            int alt20 = 2;
            int LA20_0 = input.LA(1);

            if ( (LA20_0 == '/') )
            {
                int LA20_1 = input.LA(2);

                if ( (LA20_1 == '/') )
                {
                    alt20 = 1;
                }
                else if ( (LA20_1 == '*') )
                {
                    alt20 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d20s1 =
                        new NoViableAltException("", 20, 1, input);

                    throw nvae_d20s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d20s0 =
                    new NoViableAltException("", 20, 0, input);

                throw nvae_d20s0;
            }
            switch (alt20) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:807:9: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
                    {
                    	Match("//"); 

                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:807:14: (~ ( '\\n' | '\\r' ) )*
                    	do 
                    	{
                    	    int alt17 = 2;
                    	    int LA17_0 = input.LA(1);

                    	    if ( ((LA17_0 >= '\u0000' && LA17_0 <= '\t') || (LA17_0 >= '\u000B' && LA17_0 <= '\f') || (LA17_0 >= '\u000E' && LA17_0 <= '\uFFFF')) )
                    	    {
                    	        alt17 = 1;
                    	    }


                    	    switch (alt17) 
                    		{
                    			case 1 :
                    			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:807:14: ~ ( '\\n' | '\\r' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFF') ) 
                    			    	{
                    			    	    input.Consume();

                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    	    Recover(mse);
                    			    	    throw mse;}


                    			    }
                    			    break;

                    			default:
                    			    goto loop17;
                    	    }
                    	} while (true);

                    	loop17:
                    		;	// Stops C# compiler whining that label 'loop17' has no statements

                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:807:28: ( '\\r' )?
                    	int alt18 = 2;
                    	int LA18_0 = input.LA(1);

                    	if ( (LA18_0 == '\r') )
                    	{
                    	    alt18 = 1;
                    	}
                    	switch (alt18) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:807:28: '\\r'
                    	        {
                    	        	Match('\r'); 

                    	        }
                    	        break;

                    	}

                    	Match('\n'); 
                    	_channel=HIDDEN;

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:808:9: '/*' ( options {greedy=false; } : . )* '*/'
                    {
                    	Match("/*"); 

                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:808:14: ( options {greedy=false; } : . )*
                    	do 
                    	{
                    	    int alt19 = 2;
                    	    int LA19_0 = input.LA(1);

                    	    if ( (LA19_0 == '*') )
                    	    {
                    	        int LA19_1 = input.LA(2);

                    	        if ( (LA19_1 == '/') )
                    	        {
                    	            alt19 = 2;
                    	        }
                    	        else if ( ((LA19_1 >= '\u0000' && LA19_1 <= '.') || (LA19_1 >= '0' && LA19_1 <= '\uFFFF')) )
                    	        {
                    	            alt19 = 1;
                    	        }


                    	    }
                    	    else if ( ((LA19_0 >= '\u0000' && LA19_0 <= ')') || (LA19_0 >= '+' && LA19_0 <= '\uFFFF')) )
                    	    {
                    	        alt19 = 1;
                    	    }


                    	    switch (alt19) 
                    		{
                    			case 1 :
                    			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:808:42: .
                    			    {
                    			    	MatchAny(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop19;
                    	    }
                    	} while (true);

                    	loop19:
                    		;	// Stops C# compiler whining that label 'loop19' has no statements

                    	Match("*/"); 

                    	_channel=HIDDEN;

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:811:5: ( ( ' ' | '\\t' | '\\r' | '\\n' | '\\u000C' ) )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:811:9: ( ' ' | '\\t' | '\\r' | '\\n' | '\\u000C' )
            {
            	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || (input.LA(1) >= '\f' && input.LA(1) <= '\r') || input.LA(1) == ' ' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WS"

    override public void mTokens() // throws RecognitionException 
    {
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:8: ( EQUAL | NEQUAL | LESS | LEQUAL | GREATER | GEQUAL | NOT | MUL | DIV | ADD | INCREMENT | SUB | DECREMENT | OR | AND | XOR | DOT | COMMA | SEMICOLON | COLON | LBRACKET | RBRACKET | LPAREN | RPAREN | LBRACE | RBRACE | ASSIGN | ADDASSIGN | SUBASSIGN | MULASSIGN | DIVASSIGN | UNDERSCORE | QUOTE | DQUOTE | QUESTION | VOID | IF | ELSE | WHILE | DO | FOR | BREAK | CONTINUE | RETURN | DISCARD | CONST | ATTRIBUTE | UNIFORM | VARYING | CENTROID | INVARIANT | IN | OUT | INOUT | STRUCT | INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4 | MAT4X2 | MAT4X3 | MAT4X4 | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | INT_CONSTANT | FLOAT_CONSTANT | BOOL_CONSTANT | ID | COMMENT | WS )
        int alt21 = 91;
        alt21 = dfa21.Predict(input);
        switch (alt21) 
        {
            case 1 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:10: EQUAL
                {
                	mEQUAL(); 

                }
                break;
            case 2 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:16: NEQUAL
                {
                	mNEQUAL(); 

                }
                break;
            case 3 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:23: LESS
                {
                	mLESS(); 

                }
                break;
            case 4 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:28: LEQUAL
                {
                	mLEQUAL(); 

                }
                break;
            case 5 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:35: GREATER
                {
                	mGREATER(); 

                }
                break;
            case 6 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:43: GEQUAL
                {
                	mGEQUAL(); 

                }
                break;
            case 7 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:50: NOT
                {
                	mNOT(); 

                }
                break;
            case 8 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:54: MUL
                {
                	mMUL(); 

                }
                break;
            case 9 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:58: DIV
                {
                	mDIV(); 

                }
                break;
            case 10 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:62: ADD
                {
                	mADD(); 

                }
                break;
            case 11 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:66: INCREMENT
                {
                	mINCREMENT(); 

                }
                break;
            case 12 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:76: SUB
                {
                	mSUB(); 

                }
                break;
            case 13 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:80: DECREMENT
                {
                	mDECREMENT(); 

                }
                break;
            case 14 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:90: OR
                {
                	mOR(); 

                }
                break;
            case 15 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:93: AND
                {
                	mAND(); 

                }
                break;
            case 16 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:97: XOR
                {
                	mXOR(); 

                }
                break;
            case 17 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:101: DOT
                {
                	mDOT(); 

                }
                break;
            case 18 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:105: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 19 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:111: SEMICOLON
                {
                	mSEMICOLON(); 

                }
                break;
            case 20 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:121: COLON
                {
                	mCOLON(); 

                }
                break;
            case 21 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:127: LBRACKET
                {
                	mLBRACKET(); 

                }
                break;
            case 22 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:136: RBRACKET
                {
                	mRBRACKET(); 

                }
                break;
            case 23 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:145: LPAREN
                {
                	mLPAREN(); 

                }
                break;
            case 24 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:152: RPAREN
                {
                	mRPAREN(); 

                }
                break;
            case 25 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:159: LBRACE
                {
                	mLBRACE(); 

                }
                break;
            case 26 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:166: RBRACE
                {
                	mRBRACE(); 

                }
                break;
            case 27 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:173: ASSIGN
                {
                	mASSIGN(); 

                }
                break;
            case 28 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:180: ADDASSIGN
                {
                	mADDASSIGN(); 

                }
                break;
            case 29 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:190: SUBASSIGN
                {
                	mSUBASSIGN(); 

                }
                break;
            case 30 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:200: MULASSIGN
                {
                	mMULASSIGN(); 

                }
                break;
            case 31 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:210: DIVASSIGN
                {
                	mDIVASSIGN(); 

                }
                break;
            case 32 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:220: UNDERSCORE
                {
                	mUNDERSCORE(); 

                }
                break;
            case 33 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:231: QUOTE
                {
                	mQUOTE(); 

                }
                break;
            case 34 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:237: DQUOTE
                {
                	mDQUOTE(); 

                }
                break;
            case 35 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:244: QUESTION
                {
                	mQUESTION(); 

                }
                break;
            case 36 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:253: VOID
                {
                	mVOID(); 

                }
                break;
            case 37 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:258: IF
                {
                	mIF(); 

                }
                break;
            case 38 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:261: ELSE
                {
                	mELSE(); 

                }
                break;
            case 39 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:266: WHILE
                {
                	mWHILE(); 

                }
                break;
            case 40 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:272: DO
                {
                	mDO(); 

                }
                break;
            case 41 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:275: FOR
                {
                	mFOR(); 

                }
                break;
            case 42 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:279: BREAK
                {
                	mBREAK(); 

                }
                break;
            case 43 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:285: CONTINUE
                {
                	mCONTINUE(); 

                }
                break;
            case 44 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:294: RETURN
                {
                	mRETURN(); 

                }
                break;
            case 45 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:301: DISCARD
                {
                	mDISCARD(); 

                }
                break;
            case 46 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:309: CONST
                {
                	mCONST(); 

                }
                break;
            case 47 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:315: ATTRIBUTE
                {
                	mATTRIBUTE(); 

                }
                break;
            case 48 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:325: UNIFORM
                {
                	mUNIFORM(); 

                }
                break;
            case 49 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:333: VARYING
                {
                	mVARYING(); 

                }
                break;
            case 50 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:341: CENTROID
                {
                	mCENTROID(); 

                }
                break;
            case 51 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:350: INVARIANT
                {
                	mINVARIANT(); 

                }
                break;
            case 52 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:360: IN
                {
                	mIN(); 

                }
                break;
            case 53 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:363: OUT
                {
                	mOUT(); 

                }
                break;
            case 54 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:367: INOUT
                {
                	mINOUT(); 

                }
                break;
            case 55 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:373: STRUCT
                {
                	mSTRUCT(); 

                }
                break;
            case 56 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:380: INTEGER
                {
                	mINTEGER(); 

                }
                break;
            case 57 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:388: FLOAT
                {
                	mFLOAT(); 

                }
                break;
            case 58 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:394: BOOL
                {
                	mBOOL(); 

                }
                break;
            case 59 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:399: VEC2
                {
                	mVEC2(); 

                }
                break;
            case 60 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:404: VEC3
                {
                	mVEC3(); 

                }
                break;
            case 61 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:409: VEC4
                {
                	mVEC4(); 

                }
                break;
            case 62 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:414: IVEC2
                {
                	mIVEC2(); 

                }
                break;
            case 63 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:420: IVEC3
                {
                	mIVEC3(); 

                }
                break;
            case 64 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:426: IVEC4
                {
                	mIVEC4(); 

                }
                break;
            case 65 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:432: BVEC2
                {
                	mBVEC2(); 

                }
                break;
            case 66 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:438: BVEC3
                {
                	mBVEC3(); 

                }
                break;
            case 67 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:444: BVEC4
                {
                	mBVEC4(); 

                }
                break;
            case 68 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:450: MAT2
                {
                	mMAT2(); 

                }
                break;
            case 69 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:455: MAT2X2
                {
                	mMAT2X2(); 

                }
                break;
            case 70 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:462: MAT2X3
                {
                	mMAT2X3(); 

                }
                break;
            case 71 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:469: MAT2X4
                {
                	mMAT2X4(); 

                }
                break;
            case 72 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:476: MAT3
                {
                	mMAT3(); 

                }
                break;
            case 73 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:481: MAT3X2
                {
                	mMAT3X2(); 

                }
                break;
            case 74 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:488: MAT3X3
                {
                	mMAT3X3(); 

                }
                break;
            case 75 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:495: MAT3X4
                {
                	mMAT3X4(); 

                }
                break;
            case 76 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:502: MAT4
                {
                	mMAT4(); 

                }
                break;
            case 77 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:507: MAT4X2
                {
                	mMAT4X2(); 

                }
                break;
            case 78 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:514: MAT4X3
                {
                	mMAT4X3(); 

                }
                break;
            case 79 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:521: MAT4X4
                {
                	mMAT4X4(); 

                }
                break;
            case 80 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:528: SAMPLER1D
                {
                	mSAMPLER1D(); 

                }
                break;
            case 81 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:538: SAMPLER2D
                {
                	mSAMPLER2D(); 

                }
                break;
            case 82 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:548: SAMPLER3D
                {
                	mSAMPLER3D(); 

                }
                break;
            case 83 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:558: SAMPLERCUBE
                {
                	mSAMPLERCUBE(); 

                }
                break;
            case 84 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:570: SAMPLER1DSHADOW
                {
                	mSAMPLER1DSHADOW(); 

                }
                break;
            case 85 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:586: SAMPLER2DSHADOW
                {
                	mSAMPLER2DSHADOW(); 

                }
                break;
            case 86 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:602: INT_CONSTANT
                {
                	mINT_CONSTANT(); 

                }
                break;
            case 87 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:615: FLOAT_CONSTANT
                {
                	mFLOAT_CONSTANT(); 

                }
                break;
            case 88 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:630: BOOL_CONSTANT
                {
                	mBOOL_CONSTANT(); 

                }
                break;
            case 89 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:644: ID
                {
                	mID(); 

                }
                break;
            case 90 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:647: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 91 :
                // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:1:655: WS
                {
                	mWS(); 

                }
                break;

        }

    }


    protected DFA21 dfa21;
	private void InitializeCyclicDFAs()
	{
	    this.dfa21 = new DFA21(this);
	}

    const string DFA21_eotS =
        "\x01\uffff\x01\x30\x01\x32\x01\x34\x01\x36\x01\x38\x01\x3b\x01"+
        "\x3e\x01\x41\x03\uffff\x01\x42\x09\uffff\x01\x44\x03\uffff\x0e\x2d"+
        "\x04\x5e\x01\x2d\x18\uffff\x03\x2d\x01\x68\x01\x6c\x03\x2d\x01\x70"+
        "\x10\x2d\x01\uffff\x05\x5e\x04\x2d\x01\uffff\x02\x2d\x01\u008d\x01"+
        "\uffff\x03\x2d\x01\uffff\x01\x2d\x01\u0092\x0a\x2d\x01\u009e\x03"+
        "\x2d\x04\x5e\x01\x2d\x01\u00a5\x01\x2d\x01\u00a7\x01\u00a8\x01\u00a9"+
        "\x02\x2d\x01\uffff\x01\x2d\x01\u00af\x02\x2d\x01\uffff\x03\x2d\x01"+
        "\u00b5\x07\x2d\x01\uffff\x02\x2d\x01\u00c2\x01\u00c4\x01\u00c6\x01"+
        "\u00c7\x01\uffff\x01\x2d\x03\uffff\x01\x2d\x01\u00ca\x01\u00cb\x01"+
        "\u00cc\x01\u00cd\x01\uffff\x01\u00ce\x01\x2d\x01\u00d0\x01\u00c7"+
        "\x01\u00d1\x01\uffff\x01\u00d2\x01\u00d3\x01\u00d4\x01\x2d\x01\u00d6"+
        "\x07\x2d\x01\uffff\x01\x2d\x01\uffff\x01\x2d\x02\uffff\x02\x2d\x05"+
        "\uffff\x01\x2d\x05\uffff\x01\x2d\x01\uffff\x01\x2d\x01\u00eb\x02"+
        "\x2d\x01\u00ee\x01\x2d\x01\u00f0\x01\u00f1\x01\u00f2\x01\u00f3\x01"+
        "\u00f4\x01\u00f5\x01\u00f6\x01\u00f7\x01\u00f8\x01\u00f9\x01\x2d"+
        "\x01\u00fb\x02\x2d\x01\uffff\x01\x2d\x01\u00ff\x01\uffff\x01\x2d"+
        "\x0a\uffff\x01\x2d\x01\uffff\x01\u0105\x01\u0106\x01\x2d\x01\uffff"+
        "\x04\x2d\x01\u010c\x02\uffff\x01\u010d\x01\u010f\x01\u0111\x01\u0112"+
        "\x01\x2d\x02\uffff\x01\x2d\x01\uffff\x01\x2d\x02\uffff\x03\x2d\x01"+
        "\u0119\x02\x2d\x01\uffff\x04\x2d\x01\u0120\x01\u0121\x02\uffff";
    const string DFA21_eofS =
        "\u0122\uffff";
    const string DFA21_minS =
        "\x01\x09\x05\x3d\x01\x2a\x01\x2b\x01\x2d\x03\uffff\x01\x30\x09"+
        "\uffff\x01\x30\x03\uffff\x01\x61\x01\x66\x01\x6c\x01\x68\x01\x69"+
        "\x01\x61\x01\x6f\x02\x65\x01\x74\x01\x6e\x01\x75\x02\x61\x04\x2e"+
        "\x01\x72\x18\uffff\x01\x69\x01\x72\x01\x63\x02\x30\x01\x65\x01\x73"+
        "\x01\x69\x01\x30\x01\x73\x01\x72\x01\x6f\x01\x6c\x01\x65\x01\x6f"+
        "\x01\x65\x02\x6e\x02\x74\x01\x69\x01\x74\x01\x72\x01\x6d\x01\x74"+
        "\x01\uffff\x05\x2e\x01\x75\x01\x64\x01\x79\x01\x32\x01\uffff\x01"+
        "\x61\x01\x75\x01\x30\x01\uffff\x01\x63\x01\x65\x01\x6c\x01\uffff"+
        "\x01\x63\x01\x30\x01\x61\x01\x73\x01\x61\x01\x6c\x01\x63\x01\x73"+
        "\x01\x74\x01\x75\x01\x72\x01\x66\x01\x30\x01\x75\x01\x70\x01\x32"+
        "\x04\x2e\x01\x65\x01\x30\x01\x69\x03\x30\x01\x72\x01\x74\x01\uffff"+
        "\x01\x32\x01\x30\x01\x65\x01\x61\x01\uffff\x01\x74\x01\x65\x01\x6b"+
        "\x01\x30\x01\x32\x01\x69\x01\x74\x02\x72\x01\x69\x01\x6f\x01\uffff"+
        "\x01\x63\x01\x6c\x04\x30\x01\uffff\x01\x6e\x03\uffff\x01\x69\x04"+
        "\x30\x01\uffff\x01\x30\x01\x72\x03\x30\x01\uffff\x03\x30\x01\x6e"+
        "\x01\x30\x01\x6f\x01\x6e\x01\x62\x01\x72\x01\x74\x01\x65\x01\x32"+
        "\x01\uffff\x01\x32\x01\uffff\x01\x32\x02\uffff\x01\x67\x01\x61\x05"+
        "\uffff\x01\x64\x05\uffff\x01\x75\x01\uffff\x01\x69\x01\x30\x01\x75"+
        "\x01\x6d\x01\x30\x01\x72\x0a\x30\x01\x6e\x01\x30\x01\x65\x01\x64"+
        "\x01\uffff\x01\x74\x01\x30\x01\uffff\x01\x31\x0a\uffff\x01\x74\x01"+
        "\uffff\x02\x30\x01\x65\x01\uffff\x03\x44\x01\x75\x01\x30\x02\uffff"+
        "\x04\x30\x01\x62\x02\uffff\x01\x68\x01\uffff\x01\x68\x02\uffff\x01"+
        "\x65\x02\x61\x01\x30\x02\x64\x01\uffff\x02\x6f\x02\x77\x02\x30\x02"+
        "\uffff";
    const string DFA21_maxS =
        "\x01\x7d\x08\x3d\x03\uffff\x01\x39\x09\uffff\x01\x7a\x03\uffff"+
        "\x01\x6f\x01\x76\x01\x6c\x01\x68\x02\x6f\x01\x76\x01\x6f\x01\x65"+
        "\x01\x74\x01\x6e\x01\x75\x01\x74\x01\x61\x04\x39\x01\x72\x18\uffff"+
        "\x01\x69\x01\x72\x01\x63\x02\x7a\x01\x65\x01\x73\x01\x69\x01\x7a"+
        "\x01\x73\x01\x72\x01\x6f\x01\x6c\x01\x65\x01\x6f\x01\x65\x02\x6e"+
        "\x02\x74\x01\x69\x01\x74\x01\x72\x01\x6d\x01\x74\x01\uffff\x05\x39"+
        "\x01\x75\x01\x64\x01\x79\x01\x34\x01\uffff\x01\x61\x01\x75\x01\x7a"+
        "\x01\uffff\x01\x63\x01\x65\x01\x6c\x01\uffff\x01\x63\x01\x7a\x01"+
        "\x61\x01\x73\x01\x61\x01\x6c\x01\x63\x02\x74\x01\x75\x01\x72\x01"+
        "\x66\x01\x7a\x01\x75\x01\x70\x01\x34\x04\x39\x01\x65\x01\x7a\x01"+
        "\x69\x03\x7a\x01\x72\x01\x74\x01\uffff\x01\x34\x01\x7a\x01\x65\x01"+
        "\x61\x01\uffff\x01\x74\x01\x65\x01\x6b\x01\x7a\x01\x34\x01\x69\x01"+
        "\x74\x02\x72\x01\x69\x01\x6f\x01\uffff\x01\x63\x01\x6c\x04\x7a\x01"+
        "\uffff\x01\x6e\x03\uffff\x01\x69\x04\x7a\x01\uffff\x01\x7a\x01\x72"+
        "\x03\x7a\x01\uffff\x03\x7a\x01\x6e\x01\x7a\x01\x6f\x01\x6e\x01\x62"+
        "\x01\x72\x01\x74\x01\x65\x01\x34\x01\uffff\x01\x34\x01\uffff\x01"+
        "\x34\x02\uffff\x01\x67\x01\x61\x05\uffff\x01\x64\x05\uffff\x01\x75"+
        "\x01\uffff\x01\x69\x01\x7a\x01\x75\x01\x6d\x01\x7a\x01\x72\x0a\x7a"+
        "\x01\x6e\x01\x7a\x01\x65\x01\x64\x01\uffff\x01\x74\x01\x7a\x01\uffff"+
        "\x01\x43\x0a\uffff\x01\x74\x01\uffff\x02\x7a\x01\x65\x01\uffff\x03"+
        "\x44\x01\x75\x01\x7a\x02\uffff\x04\x7a\x01\x62\x02\uffff\x01\x68"+
        "\x01\uffff\x01\x68\x02\uffff\x01\x65\x02\x61\x01\x7a\x02\x64\x01"+
        "\uffff\x02\x6f\x02\x77\x02\x7a\x02\uffff";
    const string DFA21_acceptS =
        "\x09\uffff\x01\x0e\x01\x0f\x01\x10\x01\uffff\x01\x12\x01\x13\x01"+
        "\x14\x01\x15\x01\x16\x01\x17\x01\x18\x01\x19\x01\x1a\x01\uffff\x01"+
        "\x21\x01\x22\x01\x23\x13\uffff\x01\x59\x01\x5b\x01\x01\x01\x1b\x01"+
        "\x02\x01\x07\x01\x04\x01\x03\x01\x06\x01\x05\x01\x1e\x01\x08\x01"+
        "\x1f\x01\x5a\x01\x09\x01\x0b\x01\x1c\x01\x0a\x01\x0d\x01\x1d\x01"+
        "\x0c\x01\x11\x01\x57\x01\x20\x19\uffff\x01\x56\x09\uffff\x01\x25"+
        "\x03\uffff\x01\x34\x03\uffff\x01\x28\x1c\uffff\x01\x38\x04\uffff"+
        "\x01\x29\x0b\uffff\x01\x35\x06\uffff\x01\x24\x01\uffff\x01\x3b\x01"+
        "\x3c\x01\x3d\x05\uffff\x01\x26\x05\uffff\x01\x3a\x0c\uffff\x01\x44"+
        "\x01\uffff\x01\x48\x01\uffff\x01\x4c\x01\x58\x02\uffff\x01\x36\x01"+
        "\x3e\x01\x3f\x01\x40\x01\x27\x01\uffff\x01\x39\x01\x2a\x01\x41\x01"+
        "\x42\x01\x43\x01\uffff\x01\x2e\x14\uffff\x01\x2c\x02\uffff\x01\x37"+
        "\x01\uffff\x01\x45\x01\x46\x01\x47\x01\x49\x01\x4a\x01\x4b\x01\x4d"+
        "\x01\x4e\x01\x4f\x01\x31\x01\uffff\x01\x2d\x03\uffff\x01\x30\x05"+
        "\uffff\x01\x2b\x01\x32\x05\uffff\x01\x33\x01\x2f\x01\uffff\x01\x50"+
        "\x01\uffff\x01\x51\x01\x52\x06\uffff\x01\x53\x06\uffff\x01\x54\x01"+
        "\x55";
    const string DFA21_specialS =
        "\u0122\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x02\x2e\x01\uffff\x02\x2e\x12\uffff\x01\x2e\x01\x02\x01\x18"+
            "\x03\uffff\x01\x0a\x01\x17\x01\x12\x01\x13\x01\x05\x01\x07\x01"+
            "\x0d\x01\x08\x01\x0c\x01\x06\x01\x28\x07\x29\x01\x2a\x01\x2b"+
            "\x01\x0f\x01\x0e\x01\x03\x01\x01\x01\x04\x01\x19\x01\uffff\x1a"+
            "\x2d\x01\x10\x01\uffff\x01\x11\x01\x0b\x01\x16\x01\uffff\x01"+
            "\x23\x01\x20\x01\x21\x01\x1e\x01\x1c\x01\x1f\x02\x2d\x01\x1b"+
            "\x03\x2d\x01\x27\x01\x2d\x01\x25\x02\x2d\x01\x22\x01\x26\x01"+
            "\x2c\x01\x24\x01\x1a\x01\x1d\x03\x2d\x01\x14\x01\x09\x01\x15",
            "\x01\x2f",
            "\x01\x31",
            "\x01\x33",
            "\x01\x35",
            "\x01\x37",
            "\x01\x3a\x04\uffff\x01\x3a\x0d\uffff\x01\x39",
            "\x01\x3c\x11\uffff\x01\x3d",
            "\x01\x3f\x0f\uffff\x01\x40",
            "",
            "",
            "",
            "\x0a\x43",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "",
            "",
            "\x01\x46\x03\uffff\x01\x47\x09\uffff\x01\x45",
            "\x01\x48\x07\uffff\x01\x49\x07\uffff\x01\x4a",
            "\x01\x4b",
            "\x01\x4c",
            "\x01\x4e\x05\uffff\x01\x4d",
            "\x01\x51\x0a\uffff\x01\x50\x02\uffff\x01\x4f",
            "\x01\x53\x02\uffff\x01\x52\x03\uffff\x01\x54",
            "\x01\x56\x09\uffff\x01\x55",
            "\x01\x57",
            "\x01\x58",
            "\x01\x59",
            "\x01\x5a",
            "\x01\x5c\x12\uffff\x01\x5b",
            "\x01\x5d",
            "\x01\x43\x01\uffff\x01\x43\x07\x5f\x02\x43",
            "\x01\x43\x01\uffff\x01\x60\x07\x61\x01\x62\x01\x63",
            "\x01\x43\x01\uffff\x01\x60\x07\x61\x01\x62\x01\x63",
            "\x01\x43\x01\uffff\x01\x60\x07\x61\x01\x62\x01\x63",
            "\x01\x64",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x65",
            "\x01\x66",
            "\x01\x67",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x0e"+
            "\x2d\x01\x6a\x04\x2d\x01\x6b\x01\x2d\x01\x69\x04\x2d",
            "\x01\x6d",
            "\x01\x6e",
            "\x01\x6f",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\x71",
            "\x01\x72",
            "\x01\x73",
            "\x01\x74",
            "\x01\x75",
            "\x01\x76",
            "\x01\x77",
            "\x01\x78",
            "\x01\x79",
            "\x01\x7a",
            "\x01\x7b",
            "\x01\x7c",
            "\x01\x7d",
            "\x01\x7e",
            "\x01\x7f",
            "\x01\u0080",
            "",
            "\x01\x43\x01\uffff\x01\x43\x07\x5f\x02\x43",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\u0085",
            "\x01\u0086",
            "\x01\u0087",
            "\x01\u0088\x01\u0089\x01\u008a",
            "",
            "\x01\u008b",
            "\x01\u008c",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "\x01\u008e",
            "\x01\u008f",
            "\x01\u0090",
            "",
            "\x01\u0091",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u0093",
            "\x01\u0094",
            "\x01\u0095",
            "\x01\u0096",
            "\x01\u0097",
            "\x01\u0099\x01\u0098",
            "\x01\u009a",
            "\x01\u009b",
            "\x01\u009c",
            "\x01\u009d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u009f",
            "\x01\u00a0",
            "\x01\u00a1\x01\u00a2\x01\u00a3",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\x43\x01\uffff\x01\u0081\x07\u0082\x01\u0083\x01\u0084",
            "\x01\u00a4",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00a6",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00aa",
            "\x01\u00ab",
            "",
            "\x01\u00ac\x01\u00ad\x01\u00ae",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00b0",
            "\x01\u00b1",
            "",
            "\x01\u00b2",
            "\x01\u00b3",
            "\x01\u00b4",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00b6\x01\u00b7\x01\u00b8",
            "\x01\u00b9",
            "\x01\u00ba",
            "\x01\u00bb",
            "\x01\u00bc",
            "\x01\u00bd",
            "\x01\u00be",
            "",
            "\x01\u00bf",
            "\x01\u00c0",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x17"+
            "\x2d\x01\u00c1\x02\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x17"+
            "\x2d\x01\u00c3\x02\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x17"+
            "\x2d\x01\u00c5\x02\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "\x01\u00c8",
            "",
            "",
            "",
            "\x01\u00c9",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00cf",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00d5",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00d7",
            "\x01\u00d8",
            "\x01\u00d9",
            "\x01\u00da",
            "\x01\u00db",
            "\x01\u00dc",
            "\x01\u00dd\x01\u00de\x01\u00df",
            "",
            "\x01\u00e0\x01\u00e1\x01\u00e2",
            "",
            "\x01\u00e3\x01\u00e4\x01\u00e5",
            "",
            "",
            "\x01\u00e6",
            "\x01\u00e7",
            "",
            "",
            "",
            "",
            "",
            "\x01\u00e8",
            "",
            "",
            "",
            "",
            "",
            "\x01\u00e9",
            "",
            "\x01\u00ea",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00ec",
            "\x01\u00ed",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00ef",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00fa",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u00fc",
            "\x01\u00fd",
            "",
            "\x01\u00fe",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "\x01\u0100\x01\u0101\x01\u0102\x0f\uffff\x01\u0103",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\u0104",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u0107",
            "",
            "\x01\u0108",
            "\x01\u0109",
            "\x01\u010a",
            "\x01\u010b",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            "",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x12\x2d\x01\u010e\x07\x2d\x04\uffff\x01"+
            "\x2d\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x12\x2d\x01\u0110\x07\x2d\x04\uffff\x01"+
            "\x2d\x01\uffff\x1a\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u0113",
            "",
            "",
            "\x01\u0114",
            "",
            "\x01\u0115",
            "",
            "",
            "\x01\u0116",
            "\x01\u0117",
            "\x01\u0118",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x01\u011a",
            "\x01\u011b",
            "",
            "\x01\u011c",
            "\x01\u011d",
            "\x01\u011e",
            "\x01\u011f",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "\x0a\x2d\x07\uffff\x1a\x2d\x04\uffff\x01\x2d\x01\uffff\x1a"+
            "\x2d",
            "",
            ""
    };

    static readonly short[] DFA21_eot = DFA.UnpackEncodedString(DFA21_eotS);
    static readonly short[] DFA21_eof = DFA.UnpackEncodedString(DFA21_eofS);
    static readonly char[] DFA21_min = DFA.UnpackEncodedStringToUnsignedChars(DFA21_minS);
    static readonly char[] DFA21_max = DFA.UnpackEncodedStringToUnsignedChars(DFA21_maxS);
    static readonly short[] DFA21_accept = DFA.UnpackEncodedString(DFA21_acceptS);
    static readonly short[] DFA21_special = DFA.UnpackEncodedString(DFA21_specialS);
    static readonly short[][] DFA21_transition = DFA.UnpackEncodedStringArray(DFA21_transitionS);

    protected class DFA21 : DFA
    {
        public DFA21(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 21;
            this.eot = DFA21_eot;
            this.eof = DFA21_eof;
            this.min = DFA21_min;
            this.max = DFA21_max;
            this.accept = DFA21_accept;
            this.special = DFA21_special;
            this.transition = DFA21_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( EQUAL | NEQUAL | LESS | LEQUAL | GREATER | GEQUAL | NOT | MUL | DIV | ADD | INCREMENT | SUB | DECREMENT | OR | AND | XOR | DOT | COMMA | SEMICOLON | COLON | LBRACKET | RBRACKET | LPAREN | RPAREN | LBRACE | RBRACE | ASSIGN | ADDASSIGN | SUBASSIGN | MULASSIGN | DIVASSIGN | UNDERSCORE | QUOTE | DQUOTE | QUESTION | VOID | IF | ELSE | WHILE | DO | FOR | BREAK | CONTINUE | RETURN | DISCARD | CONST | ATTRIBUTE | UNIFORM | VARYING | CENTROID | INVARIANT | IN | OUT | INOUT | STRUCT | INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4 | MAT4X2 | MAT4X3 | MAT4X4 | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | INT_CONSTANT | FLOAT_CONSTANT | BOOL_CONSTANT | ID | COMMENT | WS );"; }
        }

    }

 
    
}
