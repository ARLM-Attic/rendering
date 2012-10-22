// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g 2010-07-11 00:42:42


using System.Collections.Generic;
using System.Linq;
using GLSLCompiler;
using GLSLCompiler.Types;
using GLSLCompiler.AST;
using GLSLCompiler.AST.Declarations;
using GLSLCompiler.AST.Statements;
using GLSLCompiler.AST.Expressions;
using GLSLCompiler.Utils;


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;

using IDictionary	= System.Collections.IDictionary;
using Hashtable 	= System.Collections.Hashtable;

public class GLSLParser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"EQUAL", 
		"NEQUAL", 
		"LESS", 
		"LEQUAL", 
		"GREATER", 
		"GEQUAL", 
		"NOT", 
		"MUL", 
		"DIV", 
		"ADD", 
		"INCREMENT", 
		"SUB", 
		"DECREMENT", 
		"OR", 
		"AND", 
		"XOR", 
		"DOT", 
		"COMMA", 
		"SEMICOLON", 
		"COLON", 
		"LBRACKET", 
		"RBRACKET", 
		"LPAREN", 
		"RPAREN", 
		"LBRACE", 
		"RBRACE", 
		"ASSIGN", 
		"ADDASSIGN", 
		"SUBASSIGN", 
		"MULASSIGN", 
		"DIVASSIGN", 
		"UNDERSCORE", 
		"QUOTE", 
		"DQUOTE", 
		"QUESTION", 
		"VOID", 
		"IF", 
		"ELSE", 
		"WHILE", 
		"DO", 
		"FOR", 
		"BREAK", 
		"CONTINUE", 
		"RETURN", 
		"DISCARD", 
		"CONST", 
		"ATTRIBUTE", 
		"UNIFORM", 
		"VARYING", 
		"CENTROID", 
		"INVARIANT", 
		"IN", 
		"OUT", 
		"INOUT", 
		"STRUCT", 
		"INTEGER", 
		"FLOAT", 
		"BOOL", 
		"VEC2", 
		"VEC3", 
		"VEC4", 
		"IVEC2", 
		"IVEC3", 
		"IVEC4", 
		"BVEC2", 
		"BVEC3", 
		"BVEC4", 
		"MAT2", 
		"MAT2X2", 
		"MAT2X3", 
		"MAT2X4", 
		"MAT3", 
		"MAT3X2", 
		"MAT3X3", 
		"MAT3X4", 
		"MAT4", 
		"MAT4X2", 
		"MAT4X3", 
		"MAT4X4", 
		"SAMPLER1D", 
		"SAMPLER2D", 
		"SAMPLER3D", 
		"SAMPLERCUBE", 
		"SAMPLER1DSHADOW", 
		"SAMPLER2DSHADOW", 
		"ID", 
		"INT_CONSTANT", 
		"FLOAT_CONSTANT", 
		"BOOL_CONSTANT", 
		"DECIMAL_CONSTANT", 
		"OCTAL_CONSTANT", 
		"HEX_CONSTANT", 
		"ZERO", 
		"NON_ZERO_DIGIT", 
		"DIGIT_SEQUENCE", 
		"OCTAL_DIGIT", 
		"DIGIT", 
		"HEX_DIGIT", 
		"FRACTIONAL_CONSTANT", 
		"EXPONENT", 
		"FLOAT_SUFFIX", 
		"LETTER", 
		"COMMENT", 
		"WS"
    };

    public const int EXPONENT = 103;
    public const int FLOAT_SUFFIX = 104;
    public const int MAT4X4 = 82;
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
    public const int SUBASSIGN = 32;
    public const int EOF = -1;
    public const int MAT4X3 = 81;
    public const int OCTAL_DIGIT = 99;
    public const int BREAK = 45;
    public const int MAT4X2 = 80;
    public const int LBRACKET = 24;
    public const int QUOTE = 36;
    public const int RPAREN = 27;
    public const int LEQUAL = 7;
    public const int GREATER = 8;
    public const int BOOL_CONSTANT = 92;
    public const int LESS = 6;
    public const int RETURN = 47;
    public const int VOID = 39;
    public const int COMMENT = 106;
    public const int INT_CONSTANT = 90;
    public const int NEQUAL = 5;
    public const int MULASSIGN = 33;
    public const int RBRACE = 29;
    public const int ELSE = 41;
    public const int INVARIANT = 54;
    public const int BOOL = 61;
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
    public const int COLON = 23;
    public const int BVEC4 = 70;
    public const int INCREMENT = 14;
    public const int BVEC2 = 68;
    public const int BVEC3 = 69;
    public const int QUESTION = 38;
    public const int VEC2 = 62;
    public const int VEC3 = 63;
    public const int DIGIT_SEQUENCE = 98;
    public const int ASSIGN = 30;
    public const int IVEC3 = 66;
    public const int IVEC4 = 67;
    public const int DIV = 12;
    public const int MAT2 = 71;
    public const int IVEC2 = 65;
    public const int MAT3 = 75;
    public const int MAT4 = 79;

    // delegates
    // delegators



        public GLSLParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public GLSLParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
       }
        

    override public string[] TokenNames {
		get { return GLSLParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g"; }
    }



    // $ANTLR start "program"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:111:1: program returns [BaseAST res] : decl1= declaration (decl2= declaration )* ;
    public BaseAST program() // throws RecognitionException [1]
    {   
        BaseAST res = null;

        DeclarationAST decl1 = null;

        DeclarationAST decl2 = null;



        	List<DeclarationAST> decls = new List<DeclarationAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:118:2: (decl1= declaration (decl2= declaration )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:118:4: decl1= declaration (decl2= declaration )*
            {
            	PushFollow(FOLLOW_declaration_in_program834);
            	decl1 = declaration();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			decls.Add(decl1);
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:120:5: (decl2= declaration )*
            	do 
            	{
            	    int alt1 = 2;
            	    alt1 = dfa1.Predict(input);
            	    switch (alt1) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:120:6: decl2= declaration
            			    {
            			    	PushFollow(FOLLOW_declaration_in_program841);
            			    	decl2 = declaration();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			decls.Add(decl2);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements


            }

            if ( (state.backtracking==0) )
            {

              	res =  new ShaderAST(decls);

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "program"


    // $ANTLR start "expression"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:125:1: expression returns [ExpressionAST res] : assign= assign_expr ;
    public ExpressionAST expression() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST assign = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:126:2: (assign= assign_expr )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:126:4: assign= assign_expr
            {
            	PushFollow(FOLLOW_assign_expr_in_expression861);
            	assign = assign_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			return assign;
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "expression"


    // $ANTLR start "assign_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:131:1: assign_expr returns [ExpressionAST res] : cond= cond_expr ( ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr ) )? ;
    public ExpressionAST assign_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST cond = null;

        ExpressionAST assign = null;

        ExpressionAST add_assign = null;

        ExpressionAST sub_assign = null;

        ExpressionAST mul_assign = null;

        ExpressionAST div_assign = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:132:2: (cond= cond_expr ( ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr ) )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:132:4: cond= cond_expr ( ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr ) )?
            {
            	PushFollow(FOLLOW_cond_expr_in_assign_expr879);
            	cond = cond_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  cond;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:134:5: ( ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr ) )?
            	int alt3 = 2;
            	alt3 = dfa3.Predict(input);
            	switch (alt3) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:134:6: ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr )
            	        {
            	        	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:134:6: ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr )
            	        	int alt2 = 5;
            	        	switch ( input.LA(1) ) 
            	        	{
            	        	case ASSIGN:
            	        		{
            	        	    alt2 = 1;
            	        	    }
            	        	    break;
            	        	case ADDASSIGN:
            	        		{
            	        	    alt2 = 2;
            	        	    }
            	        	    break;
            	        	case SUBASSIGN:
            	        		{
            	        	    alt2 = 3;
            	        	    }
            	        	    break;
            	        	case MULASSIGN:
            	        		{
            	        	    alt2 = 4;
            	        	    }
            	        	    break;
            	        	case DIVASSIGN:
            	        		{
            	        	    alt2 = 5;
            	        	    }
            	        	    break;
            	        		default:
            	        		    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	        		    NoViableAltException nvae_d2s0 =
            	        		        new NoViableAltException("", 2, 0, input);

            	        		    throw nvae_d2s0;
            	        	}

            	        	switch (alt2) 
            	        	{
            	        	    case 1 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:134:7: ASSIGN assign= assign_expr
            	        	        {
            	        	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_assign_expr885); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_assign_expr_in_assign_expr889);
            	        	        	assign = assign_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			res =  new AssignExpressionAST(res, assign, cond.Line, cond.Column);
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:136:7: ADDASSIGN add_assign= assign_expr
            	        	        {
            	        	        	Match(input,ADDASSIGN,FOLLOW_ADDASSIGN_in_assign_expr895); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_assign_expr_in_assign_expr899);
            	        	        	add_assign = assign_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			AddExpressionAST add = new AddExpressionAST(res, add_assign, cond.Line, add_assign.Column);
            	        	        	  			res =  new AssignExpressionAST(res, add, cond.Line, cond.Column);
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;
            	        	    case 3 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:139:7: SUBASSIGN sub_assign= assign_expr
            	        	        {
            	        	        	Match(input,SUBASSIGN,FOLLOW_SUBASSIGN_in_assign_expr905); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_assign_expr_in_assign_expr909);
            	        	        	sub_assign = assign_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			SubExpressionAST sub = new SubExpressionAST(res, sub_assign, cond.Line, sub_assign.Column);
            	        	        	  			res =  new AssignExpressionAST(res, sub, cond.Line, cond.Column);
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;
            	        	    case 4 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:142:7: MULASSIGN mul_assign= assign_expr
            	        	        {
            	        	        	Match(input,MULASSIGN,FOLLOW_MULASSIGN_in_assign_expr915); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_assign_expr_in_assign_expr919);
            	        	        	mul_assign = assign_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			MulExpressionAST mul = new MulExpressionAST(res, mul_assign, cond.Line, mul_assign.Column);
            	        	        	  			res =  new AssignExpressionAST(res, mul, cond.Line, cond.Column);
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;
            	        	    case 5 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:145:7: DIVASSIGN div_assign= assign_expr
            	        	        {
            	        	        	Match(input,DIVASSIGN,FOLLOW_DIVASSIGN_in_assign_expr925); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_assign_expr_in_assign_expr929);
            	        	        	div_assign = assign_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			DivExpressionAST div = new DivExpressionAST(res, div_assign, cond.Line, div_assign.Column);
            	        	        	  			res =  new AssignExpressionAST(res, div, cond.Line, cond.Column);
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "assign_expr"


    // $ANTLR start "cond_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:151:1: cond_expr returns [ExpressionAST res] : or= or_expr ( QUESTION ontrue= expression COLON onfalse= assign_expr )? ;
    public ExpressionAST cond_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST or = null;

        ExpressionAST ontrue = null;

        ExpressionAST onfalse = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:152:2: (or= or_expr ( QUESTION ontrue= expression COLON onfalse= assign_expr )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:152:4: or= or_expr ( QUESTION ontrue= expression COLON onfalse= assign_expr )?
            {
            	PushFollow(FOLLOW_or_expr_in_cond_expr949);
            	or = or_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  or;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:154:4: ( QUESTION ontrue= expression COLON onfalse= assign_expr )?
            	int alt4 = 2;
            	alt4 = dfa4.Predict(input);
            	switch (alt4) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:154:5: QUESTION ontrue= expression COLON onfalse= assign_expr
            	        {
            	        	Match(input,QUESTION,FOLLOW_QUESTION_in_cond_expr953); if (state.failed) return res;
            	        	PushFollow(FOLLOW_expression_in_cond_expr957);
            	        	ontrue = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	Match(input,COLON,FOLLOW_COLON_in_cond_expr959); if (state.failed) return res;
            	        	PushFollow(FOLLOW_assign_expr_in_cond_expr963);
            	        	onfalse = assign_expr();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new ConditionExpressionAST(res, ontrue, onfalse, or.Line, or.Column);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "cond_expr"


    // $ANTLR start "or_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:159:1: or_expr returns [ExpressionAST res] : xor1= xor_expr ( OR xor2= xor_expr )* ;
    public ExpressionAST or_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST xor1 = null;

        ExpressionAST xor2 = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:160:2: (xor1= xor_expr ( OR xor2= xor_expr )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:160:4: xor1= xor_expr ( OR xor2= xor_expr )*
            {
            	PushFollow(FOLLOW_xor_expr_in_or_expr983);
            	xor1 = xor_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  xor1;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:162:5: ( OR xor2= xor_expr )*
            	do 
            	{
            	    int alt5 = 2;
            	    alt5 = dfa5.Predict(input);
            	    switch (alt5) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:162:6: OR xor2= xor_expr
            			    {
            			    	Match(input,OR,FOLLOW_OR_in_or_expr988); if (state.failed) return res;
            			    	PushFollow(FOLLOW_xor_expr_in_or_expr992);
            			    	xor2 = xor_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new OrExpressionAST(res, xor2, xor1.Line, xor2.Column);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "or_expr"


    // $ANTLR start "xor_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:167:1: xor_expr returns [ExpressionAST res] : and1= and_expr ( XOR and2= and_expr )* ;
    public ExpressionAST xor_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST and1 = null;

        ExpressionAST and2 = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:168:2: (and1= and_expr ( XOR and2= and_expr )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:168:4: and1= and_expr ( XOR and2= and_expr )*
            {
            	PushFollow(FOLLOW_and_expr_in_xor_expr1012);
            	and1 = and_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  and1;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:170:4: ( XOR and2= and_expr )*
            	do 
            	{
            	    int alt6 = 2;
            	    alt6 = dfa6.Predict(input);
            	    switch (alt6) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:170:5: XOR and2= and_expr
            			    {
            			    	Match(input,XOR,FOLLOW_XOR_in_xor_expr1016); if (state.failed) return res;
            			    	PushFollow(FOLLOW_and_expr_in_xor_expr1020);
            			    	and2 = and_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new XorExpressionAST(res, and2, and1.Line, and1.Column);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop6;
            	    }
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "xor_expr"


    // $ANTLR start "and_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:175:1: and_expr returns [ExpressionAST res] : eq1= eq_expr ( AND eq2= eq_expr )* ;
    public ExpressionAST and_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST eq1 = null;

        ExpressionAST eq2 = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:176:2: (eq1= eq_expr ( AND eq2= eq_expr )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:176:4: eq1= eq_expr ( AND eq2= eq_expr )*
            {
            	PushFollow(FOLLOW_eq_expr_in_and_expr1040);
            	eq1 = eq_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  eq1;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:178:4: ( AND eq2= eq_expr )*
            	do 
            	{
            	    int alt7 = 2;
            	    alt7 = dfa7.Predict(input);
            	    switch (alt7) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:178:5: AND eq2= eq_expr
            			    {
            			    	Match(input,AND,FOLLOW_AND_in_and_expr1044); if (state.failed) return res;
            			    	PushFollow(FOLLOW_eq_expr_in_and_expr1048);
            			    	eq2 = eq_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new AndExpressionAST(res, eq2, eq1.Line, eq1.Column);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop7;
            	    }
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "and_expr"


    // $ANTLR start "eq_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:183:1: eq_expr returns [ExpressionAST res] : rel1= rel_expr ( ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr ) )? ;
    public ExpressionAST eq_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST rel1 = null;

        ExpressionAST rel21 = null;

        ExpressionAST rel22 = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:184:2: (rel1= rel_expr ( ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr ) )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:184:4: rel1= rel_expr ( ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr ) )?
            {
            	PushFollow(FOLLOW_rel_expr_in_eq_expr1068);
            	rel1 = rel_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  rel1;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:186:5: ( ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr ) )?
            	int alt9 = 2;
            	alt9 = dfa9.Predict(input);
            	switch (alt9) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:186:6: ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr )
            	        {
            	        	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:186:6: ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr )
            	        	int alt8 = 2;
            	        	int LA8_0 = input.LA(1);

            	        	if ( (LA8_0 == EQUAL) )
            	        	{
            	        	    alt8 = 1;
            	        	}
            	        	else if ( (LA8_0 == NEQUAL) )
            	        	{
            	        	    alt8 = 2;
            	        	}
            	        	else 
            	        	{
            	        	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	        	    NoViableAltException nvae_d8s0 =
            	        	        new NoViableAltException("", 8, 0, input);

            	        	    throw nvae_d8s0;
            	        	}
            	        	switch (alt8) 
            	        	{
            	        	    case 1 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:186:7: EQUAL rel21= rel_expr
            	        	        {
            	        	        	Match(input,EQUAL,FOLLOW_EQUAL_in_eq_expr1074); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_rel_expr_in_eq_expr1078);
            	        	        	rel21 = rel_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			res =  new EqualExpressionAST(res, rel21, rel1.Line, rel1.Column);
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:188:7: NEQUAL rel22= rel_expr
            	        	        {
            	        	        	Match(input,NEQUAL,FOLLOW_NEQUAL_in_eq_expr1084); if (state.failed) return res;
            	        	        	PushFollow(FOLLOW_rel_expr_in_eq_expr1088);
            	        	        	rel22 = rel_expr();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  				res =  new NotEqualExpressionAST(res, rel22, rel1.Line, rel1.Column);		
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "eq_expr"


    // $ANTLR start "rel_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:193:1: rel_expr returns [ExpressionAST res] : add= add_expr ( LESS l_add= add_expr | LEQUAL le_add= add_expr | GREATER g_add= add_expr | GEQUAL ge_add= add_expr )? ;
    public ExpressionAST rel_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST add = null;

        ExpressionAST l_add = null;

        ExpressionAST le_add = null;

        ExpressionAST g_add = null;

        ExpressionAST ge_add = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:194:2: (add= add_expr ( LESS l_add= add_expr | LEQUAL le_add= add_expr | GREATER g_add= add_expr | GEQUAL ge_add= add_expr )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:194:4: add= add_expr ( LESS l_add= add_expr | LEQUAL le_add= add_expr | GREATER g_add= add_expr | GEQUAL ge_add= add_expr )?
            {
            	PushFollow(FOLLOW_add_expr_in_rel_expr1110);
            	add = add_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  add;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:196:5: ( LESS l_add= add_expr | LEQUAL le_add= add_expr | GREATER g_add= add_expr | GEQUAL ge_add= add_expr )?
            	int alt10 = 5;
            	alt10 = dfa10.Predict(input);
            	switch (alt10) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:196:6: LESS l_add= add_expr
            	        {
            	        	Match(input,LESS,FOLLOW_LESS_in_rel_expr1115); if (state.failed) return res;
            	        	PushFollow(FOLLOW_add_expr_in_rel_expr1119);
            	        	l_add = add_expr();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new LessExpressionAST(res, l_add, add.Line, add.Column);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:198:7: LEQUAL le_add= add_expr
            	        {
            	        	Match(input,LEQUAL,FOLLOW_LEQUAL_in_rel_expr1125); if (state.failed) return res;
            	        	PushFollow(FOLLOW_add_expr_in_rel_expr1129);
            	        	le_add = add_expr();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new LessEqualExpressionAST(res, le_add, add.Line, add.Column);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 3 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:200:7: GREATER g_add= add_expr
            	        {
            	        	Match(input,GREATER,FOLLOW_GREATER_in_rel_expr1135); if (state.failed) return res;
            	        	PushFollow(FOLLOW_add_expr_in_rel_expr1139);
            	        	g_add = add_expr();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new GreaterExpressionAST(res, g_add, add.Line, add.Column);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 4 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:202:7: GEQUAL ge_add= add_expr
            	        {
            	        	Match(input,GEQUAL,FOLLOW_GEQUAL_in_rel_expr1145); if (state.failed) return res;
            	        	PushFollow(FOLLOW_add_expr_in_rel_expr1149);
            	        	ge_add = add_expr();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new GreaterEqualExpressionAST(res, ge_add, add.Line, add.Column);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "rel_expr"


    // $ANTLR start "add_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:207:1: add_expr returns [ExpressionAST res] : mul= mul_expr ( ADD add_mul= mul_expr | SUB sub_mul= mul_expr )* ;
    public ExpressionAST add_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST mul = null;

        ExpressionAST add_mul = null;

        ExpressionAST sub_mul = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:208:2: (mul= mul_expr ( ADD add_mul= mul_expr | SUB sub_mul= mul_expr )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:208:4: mul= mul_expr ( ADD add_mul= mul_expr | SUB sub_mul= mul_expr )*
            {
            	PushFollow(FOLLOW_mul_expr_in_add_expr1169);
            	mul = mul_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  mul;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:210:5: ( ADD add_mul= mul_expr | SUB sub_mul= mul_expr )*
            	do 
            	{
            	    int alt11 = 3;
            	    alt11 = dfa11.Predict(input);
            	    switch (alt11) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:210:6: ADD add_mul= mul_expr
            			    {
            			    	Match(input,ADD,FOLLOW_ADD_in_add_expr1174); if (state.failed) return res;
            			    	PushFollow(FOLLOW_mul_expr_in_add_expr1178);
            			    	add_mul = mul_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new AddExpressionAST(res, add_mul, mul.Line, mul.Column);
            			    	  		
            			    	}

            			    }
            			    break;
            			case 2 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:212:7: SUB sub_mul= mul_expr
            			    {
            			    	Match(input,SUB,FOLLOW_SUB_in_add_expr1184); if (state.failed) return res;
            			    	PushFollow(FOLLOW_mul_expr_in_add_expr1188);
            			    	sub_mul = mul_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new SubExpressionAST(res, sub_mul, mul.Line, mul.Column);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "add_expr"


    // $ANTLR start "mul_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:217:1: mul_expr returns [ExpressionAST res] : unary= unary_expr ( MUL mul_unary= unary_expr | DIV div_unary= unary_expr )* ;
    public ExpressionAST mul_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST unary = null;

        ExpressionAST mul_unary = null;

        ExpressionAST div_unary = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:218:2: (unary= unary_expr ( MUL mul_unary= unary_expr | DIV div_unary= unary_expr )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:218:4: unary= unary_expr ( MUL mul_unary= unary_expr | DIV div_unary= unary_expr )*
            {
            	PushFollow(FOLLOW_unary_expr_in_mul_expr1208);
            	unary = unary_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  unary;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:220:5: ( MUL mul_unary= unary_expr | DIV div_unary= unary_expr )*
            	do 
            	{
            	    int alt12 = 3;
            	    alt12 = dfa12.Predict(input);
            	    switch (alt12) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:220:6: MUL mul_unary= unary_expr
            			    {
            			    	Match(input,MUL,FOLLOW_MUL_in_mul_expr1213); if (state.failed) return res;
            			    	PushFollow(FOLLOW_unary_expr_in_mul_expr1217);
            			    	mul_unary = unary_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new MulExpressionAST(res, mul_unary, unary.Line, unary.Column);
            			    	  		
            			    	}

            			    }
            			    break;
            			case 2 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:222:7: DIV div_unary= unary_expr
            			    {
            			    	Match(input,DIV,FOLLOW_DIV_in_mul_expr1223); if (state.failed) return res;
            			    	PushFollow(FOLLOW_unary_expr_in_mul_expr1227);
            			    	div_unary = unary_expr();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			res =  new DivExpressionAST(res, div_unary, unary.Line, unary.Column);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "mul_expr"


    // $ANTLR start "unary_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:227:1: unary_expr returns [ExpressionAST res] : (tok= ( ADD | INCREMENT | SUB | DECREMENT | NOT ) )? postfix= postfix_expr ;
    public ExpressionAST unary_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        IToken tok = null;
        ExpressionAST postfix = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:228:2: ( (tok= ( ADD | INCREMENT | SUB | DECREMENT | NOT ) )? postfix= postfix_expr )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:228:4: (tok= ( ADD | INCREMENT | SUB | DECREMENT | NOT ) )? postfix= postfix_expr
            {
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:228:7: (tok= ( ADD | INCREMENT | SUB | DECREMENT | NOT ) )?
            	int alt13 = 2;
            	int LA13_0 = input.LA(1);

            	if ( (LA13_0 == NOT || (LA13_0 >= ADD && LA13_0 <= DECREMENT)) )
            	{
            	    alt13 = 1;
            	}
            	switch (alt13) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:228:7: tok= ( ADD | INCREMENT | SUB | DECREMENT | NOT )
            	        {
            	        	tok = (IToken)input.LT(1);
            	        	if ( input.LA(1) == NOT || (input.LA(1) >= ADD && input.LA(1) <= DECREMENT) ) 
            	        	{
            	        	    input.Consume();
            	        	    state.errorRecovery = false;state.failed = false;
            	        	}
            	        	else 
            	        	{
            	        	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    throw mse;
            	        	}


            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_postfix_expr_in_unary_expr1270);
            	postfix = postfix_expr();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{
            	  		
            	  			if(tok != null) {
            	  				switch(tok.Type) {
            	  					case INCREMENT:
            	  						res =  new PreIncrementExpressionAST(res, tok.Line, tok.CharPositionInLine);
            	  						break;
            	  					case SUB:
            	  						res =  new MinusExpressionAST(res, tok.Line, tok.CharPositionInLine);
            	  						break;
            	  					case DECREMENT:
            	  						res =  new PreDecrementExpressionAST(res, tok.Line, tok.CharPositionInLine);
            	  						break;
            	  					case NOT:
            	  						res =  new NotExpressionAST(res, tok.Line, tok.CharPositionInLine);
            	  						break;
            	  					case ADD:
            	  					default:
            	  						res =  postfix;
            	  						break;
            	  				}
            	  			}
            	  			else
            	  				res =  postfix;
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "unary_expr"


    // $ANTLR start "postfix_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:254:1: postfix_expr returns [ExpressionAST res] : lval= lvalue ( INCREMENT | DECREMENT )? ;
    public ExpressionAST postfix_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        ExpressionAST lval = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:255:2: (lval= lvalue ( INCREMENT | DECREMENT )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:255:4: lval= lvalue ( INCREMENT | DECREMENT )?
            {
            	PushFollow(FOLLOW_lvalue_in_postfix_expr1288);
            	lval = lvalue();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  lval;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:257:5: ( INCREMENT | DECREMENT )?
            	int alt14 = 3;
            	alt14 = dfa14.Predict(input);
            	switch (alt14) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:257:6: INCREMENT
            	        {
            	        	Match(input,INCREMENT,FOLLOW_INCREMENT_in_postfix_expr1293); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new PostIncrementExpressionAST(res, res.Line, res.Column);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:259:7: DECREMENT
            	        {
            	        	Match(input,DECREMENT,FOLLOW_DECREMENT_in_postfix_expr1299); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new PostDecrementExpressionAST(res, res.Line, res.Column);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "postfix_expr"


    // $ANTLR start "lvalue"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:264:1: lvalue returns [ExpressionAST res] : ( ( constructor_expr )=>constructor= constructor_expr | primary= primary_expr ( ( DOT ( ID | funcall= funccall_expr ) ) | ( LBRACKET expr= expression RBRACKET ) )* );
    public ExpressionAST lvalue() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        IToken ID1 = null;
        ExpressionAST constructor = null;

        ExpressionAST primary = null;

        ExpressionAST funcall = null;

        ExpressionAST expr = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:265:2: ( ( constructor_expr )=>constructor= constructor_expr | primary= primary_expr ( ( DOT ( ID | funcall= funccall_expr ) ) | ( LBRACKET expr= expression RBRACKET ) )* )
            int alt17 = 2;
            alt17 = dfa17.Predict(input);
            switch (alt17) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:265:4: ( constructor_expr )=>constructor= constructor_expr
                    {
                    	PushFollow(FOLLOW_constructor_expr_in_lvalue1325);
                    	constructor = constructor_expr();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  constructor;
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:268:4: primary= primary_expr ( ( DOT ( ID | funcall= funccall_expr ) ) | ( LBRACKET expr= expression RBRACKET ) )*
                    {
                    	PushFollow(FOLLOW_primary_expr_in_lvalue1334);
                    	primary = primary_expr();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  primary;
                    	  		
                    	}
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:270:4: ( ( DOT ( ID | funcall= funccall_expr ) ) | ( LBRACKET expr= expression RBRACKET ) )*
                    	do 
                    	{
                    	    int alt16 = 3;
                    	    alt16 = dfa16.Predict(input);
                    	    switch (alt16) 
                    		{
                    			case 1 :
                    			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:270:5: ( DOT ( ID | funcall= funccall_expr ) )
                    			    {
                    			    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:270:5: ( DOT ( ID | funcall= funccall_expr ) )
                    			    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:270:6: DOT ( ID | funcall= funccall_expr )
                    			    	{
                    			    		Match(input,DOT,FOLLOW_DOT_in_lvalue1339); if (state.failed) return res;
                    			    		// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:270:10: ( ID | funcall= funccall_expr )
                    			    		int alt15 = 2;
                    			    		alt15 = dfa15.Predict(input);
                    			    		switch (alt15) 
                    			    		{
                    			    		    case 1 :
                    			    		        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:270:11: ID
                    			    		        {
                    			    		        	ID1=(IToken)Match(input,ID,FOLLOW_ID_in_lvalue1342); if (state.failed) return res;
                    			    		        	if ( (state.backtracking==0) )
                    			    		        	{

                    			    		        	  			res =  new FieldExpressionAST(((ID1 != null) ? ID1.Text : null), res, primary.Line, primary.Column);
                    			    		        	  		
                    			    		        	}

                    			    		        }
                    			    		        break;
                    			    		    case 2 :
                    			    		        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:272:6: funcall= funccall_expr
                    			    		        {
                    			    		        	PushFollow(FOLLOW_funccall_expr_in_lvalue1349);
                    			    		        	funcall = funccall_expr();
                    			    		        	state.followingStackPointer--;
                    			    		        	if (state.failed) return res;
                    			    		        	if ( (state.backtracking==0) )
                    			    		        	{

                    			    		        	  			string funcName = funcall.Cast<FunctionCallExpressionAST>().Name;
                    			    		        	  			IEnumerable<ExpressionAST> funcArgs = funcall.Cast<FunctionCallExpressionAST>().Arguments;
                    			    		        	  			res =  new FunctionCallExpressionAST(funcName, res.Singleton().Concat(funcArgs), primary.Line, primary.Column);
                    			    		        	  		
                    			    		        	}

                    			    		        }
                    			    		        break;

                    			    		}


                    			    	}


                    			    }
                    			    break;
                    			case 2 :
                    			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:276:10: ( LBRACKET expr= expression RBRACKET )
                    			    {
                    			    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:276:10: ( LBRACKET expr= expression RBRACKET )
                    			    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:276:11: LBRACKET expr= expression RBRACKET
                    			    	{
                    			    		Match(input,LBRACKET,FOLLOW_LBRACKET_in_lvalue1359); if (state.failed) return res;
                    			    		PushFollow(FOLLOW_expression_in_lvalue1363);
                    			    		expr = expression();
                    			    		state.followingStackPointer--;
                    			    		if (state.failed) return res;
                    			    		Match(input,RBRACKET,FOLLOW_RBRACKET_in_lvalue1365); if (state.failed) return res;
                    			    		if ( (state.backtracking==0) )
                    			    		{

                    			    		  			res =  new ArrayExpressionAST(res, expr, primary.Line, primary.Column);
                    			    		  		
                    			    		}

                    			    	}


                    			    }
                    			    break;

                    			default:
                    			    goto loop16;
                    	    }
                    	} while (true);

                    	loop16:
                    		;	// Stops C# compiler whining that label 'loop16' has no statements


                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "lvalue"


    // $ANTLR start "primary_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:281:1: primary_expr returns [ExpressionAST res] : ( ID | INT_CONSTANT | FLOAT_CONSTANT | BOOL_CONSTANT | LPAREN expr= expr_list RPAREN | funccall= funccall_expr );
    public ExpressionAST primary_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        IToken ID2 = null;
        IToken INT_CONSTANT3 = null;
        IToken FLOAT_CONSTANT4 = null;
        IToken BOOL_CONSTANT5 = null;
        IToken LPAREN6 = null;
        IEnumerable<ExpressionAST> expr = null;

        ExpressionAST funccall = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:282:2: ( ID | INT_CONSTANT | FLOAT_CONSTANT | BOOL_CONSTANT | LPAREN expr= expr_list RPAREN | funccall= funccall_expr )
            int alt18 = 6;
            alt18 = dfa18.Predict(input);
            switch (alt18) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:282:4: ID
                    {
                    	ID2=(IToken)Match(input,ID,FOLLOW_ID_in_primary_expr1384); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new LocalVariableExpressionAST(((ID2 != null) ? ID2.Text : null), ID2.Line, ID2.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:285:4: INT_CONSTANT
                    {
                    	INT_CONSTANT3=(IToken)Match(input,INT_CONSTANT,FOLLOW_INT_CONSTANT_in_primary_expr1391); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new IntegerConstantExpressionAST(((INT_CONSTANT3 != null) ? INT_CONSTANT3.Text : null).ToInt32(), INT_CONSTANT3.Line, INT_CONSTANT3.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:288:4: FLOAT_CONSTANT
                    {
                    	FLOAT_CONSTANT4=(IToken)Match(input,FLOAT_CONSTANT,FOLLOW_FLOAT_CONSTANT_in_primary_expr1398); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new FloatConstantExpressionAST(((FLOAT_CONSTANT4 != null) ? FLOAT_CONSTANT4.Text : null).ToFloat(), FLOAT_CONSTANT4.Line, FLOAT_CONSTANT4.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 4 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:291:4: BOOL_CONSTANT
                    {
                    	BOOL_CONSTANT5=(IToken)Match(input,BOOL_CONSTANT,FOLLOW_BOOL_CONSTANT_in_primary_expr1405); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new BoolConstantExpressionAST(((BOOL_CONSTANT5 != null) ? BOOL_CONSTANT5.Text : null).ToBool(), BOOL_CONSTANT5.Line, BOOL_CONSTANT5.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 5 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:294:4: LPAREN expr= expr_list RPAREN
                    {
                    	LPAREN6=(IToken)Match(input,LPAREN,FOLLOW_LPAREN_in_primary_expr1412); if (state.failed) return res;
                    	PushFollow(FOLLOW_expr_list_in_primary_expr1416);
                    	expr = expr_list();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,RPAREN,FOLLOW_RPAREN_in_primary_expr1418); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new ExpressionListExpressionAST(expr, LPAREN6.Line, LPAREN6.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 6 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:297:4: funccall= funccall_expr
                    {
                    	PushFollow(FOLLOW_funccall_expr_in_primary_expr1427);
                    	funccall = funccall_expr();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  funccall;
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "primary_expr"


    // $ANTLR start "funccall_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:302:1: funccall_expr returns [ExpressionAST res] : ID ( args_no_parameters | args= args_with_parameters ) ;
    public ExpressionAST funccall_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        IToken ID7 = null;
        IEnumerable<ExpressionAST> args = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:303:2: ( ID ( args_no_parameters | args= args_with_parameters ) )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:303:4: ID ( args_no_parameters | args= args_with_parameters )
            {
            	ID7=(IToken)Match(input,ID,FOLLOW_ID_in_funccall_expr1443); if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:303:7: ( args_no_parameters | args= args_with_parameters )
            	int alt19 = 2;
            	alt19 = dfa19.Predict(input);
            	switch (alt19) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:303:8: args_no_parameters
            	        {
            	        	PushFollow(FOLLOW_args_no_parameters_in_funccall_expr1446);
            	        	args_no_parameters();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new FunctionCallExpressionAST(((ID7 != null) ? ID7.Text : null), Enumerable.Empty<ExpressionAST>(), ID7.Line, ID7.CharPositionInLine);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:305:7: args= args_with_parameters
            	        {
            	        	PushFollow(FOLLOW_args_with_parameters_in_funccall_expr1454);
            	        	args = args_with_parameters();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new FunctionCallExpressionAST(((ID7 != null) ? ID7.Text : null), args, ID7.Line, ID7.CharPositionInLine);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "funccall_expr"


    // $ANTLR start "args_no_parameters"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:310:1: args_no_parameters : LPAREN ( VOID )? RPAREN ;
    public void args_no_parameters() // throws RecognitionException [1]
    {   
        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:311:2: ( LPAREN ( VOID )? RPAREN )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:311:4: LPAREN ( VOID )? RPAREN
            {
            	Match(input,LPAREN,FOLLOW_LPAREN_in_args_no_parameters1468); if (state.failed) return ;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:311:11: ( VOID )?
            	int alt20 = 2;
            	int LA20_0 = input.LA(1);

            	if ( (LA20_0 == VOID) )
            	{
            	    alt20 = 1;
            	}
            	switch (alt20) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:311:12: VOID
            	        {
            	        	Match(input,VOID,FOLLOW_VOID_in_args_no_parameters1471); if (state.failed) return ;

            	        }
            	        break;

            	}

            	Match(input,RPAREN,FOLLOW_RPAREN_in_args_no_parameters1475); if (state.failed) return ;

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end "args_no_parameters"


    // $ANTLR start "args_with_parameters"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:314:1: args_with_parameters returns [IEnumerable<ExpressionAST> res] : LPAREN exprs= expr_list RPAREN ;
    public IEnumerable<ExpressionAST> args_with_parameters() // throws RecognitionException [1]
    {   
        IEnumerable<ExpressionAST> res = null;

        IEnumerable<ExpressionAST> exprs = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:315:2: ( LPAREN exprs= expr_list RPAREN )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:315:4: LPAREN exprs= expr_list RPAREN
            {
            	Match(input,LPAREN,FOLLOW_LPAREN_in_args_with_parameters1491); if (state.failed) return res;
            	PushFollow(FOLLOW_expr_list_in_args_with_parameters1495);
            	exprs = expr_list();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	Match(input,RPAREN,FOLLOW_RPAREN_in_args_with_parameters1497); if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  exprs;
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "args_with_parameters"


    // $ANTLR start "expr_list"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:321:1: expr_list returns [IEnumerable<ExpressionAST> res] : expr1= expression ( COMMA expr2= expression )* ;
    public IEnumerable<ExpressionAST> expr_list() // throws RecognitionException [1]
    {   
        IEnumerable<ExpressionAST> res = null;

        ExpressionAST expr1 = null;

        ExpressionAST expr2 = null;



        	List<ExpressionAST> list = new List<ExpressionAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:328:2: (expr1= expression ( COMMA expr2= expression )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:328:4: expr1= expression ( COMMA expr2= expression )*
            {
            	PushFollow(FOLLOW_expression_in_expr_list1526);
            	expr1 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			list.Add(expr1);
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:330:4: ( COMMA expr2= expression )*
            	do 
            	{
            	    int alt21 = 2;
            	    int LA21_0 = input.LA(1);

            	    if ( (LA21_0 == COMMA) )
            	    {
            	        alt21 = 1;
            	    }


            	    switch (alt21) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:330:5: COMMA expr2= expression
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_expr_list1530); if (state.failed) return res;
            			    	PushFollow(FOLLOW_expression_in_expr_list1534);
            			    	expr2 = expression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			list.Add(expr2);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop21;
            	    }
            	} while (true);

            	loop21:
            		;	// Stops C# compiler whining that label 'loop21' has no statements


            }

            if ( (state.backtracking==0) )
            {

              	res =  list;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "expr_list"


    // $ANTLR start "constructor_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:334:1: constructor_expr returns [ExpressionAST res] : (tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 ) ( LBRACKET (expr= expression )? RBRACKET )? args= args_with_parameters | ID LBRACKET (expr= expression ) RBRACKET args= args_with_parameters );
    public ExpressionAST constructor_expr() // throws RecognitionException [1]
    {   
        ExpressionAST res = null;

        IToken tok = null;
        ExpressionAST expr = null;

        IEnumerable<ExpressionAST> args = null;



        	bool arr = false;

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:338:2: (tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 ) ( LBRACKET (expr= expression )? RBRACKET )? args= args_with_parameters | ID LBRACKET (expr= expression ) RBRACKET args= args_with_parameters )
            int alt24 = 2;
            int LA24_0 = input.LA(1);

            if ( ((LA24_0 >= INTEGER && LA24_0 <= MAT4X4)) )
            {
                alt24 = 1;
            }
            else if ( (LA24_0 == ID) )
            {
                alt24 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return res;}
                NoViableAltException nvae_d24s0 =
                    new NoViableAltException("", 24, 0, input);

                throw nvae_d24s0;
            }
            switch (alt24) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:338:4: tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 ) ( LBRACKET (expr= expression )? RBRACKET )? args= args_with_parameters
                    {
                    	tok = (IToken)input.LT(1);
                    	if ( (input.LA(1) >= INTEGER && input.LA(1) <= MAT4X4) ) 
                    	{
                    	    input.Consume();
                    	    state.errorRecovery = false;state.failed = false;
                    	}
                    	else 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
                    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	    throw mse;
                    	}

                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:361:12: ( LBRACKET (expr= expression )? RBRACKET )?
                    	int alt23 = 2;
                    	int LA23_0 = input.LA(1);

                    	if ( (LA23_0 == LBRACKET) )
                    	{
                    	    alt23 = 1;
                    	}
                    	switch (alt23) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:361:13: LBRACKET (expr= expression )? RBRACKET
                    	        {
                    	        	Match(input,LBRACKET,FOLLOW_LBRACKET_in_constructor_expr1678); if (state.failed) return res;
                    	        	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:361:22: (expr= expression )?
                    	        	int alt22 = 2;
                    	        	int LA22_0 = input.LA(1);

                    	        	if ( (LA22_0 == NOT || (LA22_0 >= ADD && LA22_0 <= DECREMENT) || LA22_0 == LPAREN || (LA22_0 >= INTEGER && LA22_0 <= MAT4X4) || (LA22_0 >= ID && LA22_0 <= BOOL_CONSTANT)) )
                    	        	{
                    	        	    alt22 = 1;
                    	        	}
                    	        	switch (alt22) 
                    	        	{
                    	        	    case 1 :
                    	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:361:23: expr= expression
                    	        	        {
                    	        	        	PushFollow(FOLLOW_expression_in_constructor_expr1683);
                    	        	        	expr = expression();
                    	        	        	state.followingStackPointer--;
                    	        	        	if (state.failed) return res;

                    	        	        }
                    	        	        break;

                    	        	}

                    	        	Match(input,RBRACKET,FOLLOW_RBRACKET_in_constructor_expr1687); if (state.failed) return res;
                    	        	if ( (state.backtracking==0) )
                    	        	{
                    	        	   arr = true; 
                    	        	}

                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_args_with_parameters_in_constructor_expr1695);
                    	args = args_with_parameters();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			if(arr)
                    	  				res =  Helper.GetConstructionExpressionAST(tok, args, expr, tok.Line, tok.CharPositionInLine);
                    	  			else
                    	  				res =  Helper.GetConstructionExpressionAST(tok, args, tok.Line, tok.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:367:4: ID LBRACKET (expr= expression ) RBRACKET args= args_with_parameters
                    {
                    	Match(input,ID,FOLLOW_ID_in_constructor_expr1702); if (state.failed) return res;
                    	Match(input,LBRACKET,FOLLOW_LBRACKET_in_constructor_expr1704); if (state.failed) return res;
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:367:16: (expr= expression )
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:367:17: expr= expression
                    	{
                    		PushFollow(FOLLOW_expression_in_constructor_expr1709);
                    		expr = expression();
                    		state.followingStackPointer--;
                    		if (state.failed) return res;

                    	}

                    	Match(input,RBRACKET,FOLLOW_RBRACKET_in_constructor_expr1712); if (state.failed) return res;
                    	PushFollow(FOLLOW_args_with_parameters_in_constructor_expr1716);
                    	args = args_with_parameters();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  Helper.GetConstructionExpressionAST(tok, args, expr, tok.Line, tok.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "constructor_expr"


    // $ANTLR start "statement"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:373:1: statement returns [StatementAST res] : (simple= simple_stmt | compound= compound_stmt );
    public StatementAST statement() // throws RecognitionException [1]
    {   
        StatementAST res = null;

        StatementAST simple = null;

        CompoundStatementAST compound = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:374:2: (simple= simple_stmt | compound= compound_stmt )
            int alt25 = 2;
            alt25 = dfa25.Predict(input);
            switch (alt25) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:374:4: simple= simple_stmt
                    {
                    	PushFollow(FOLLOW_simple_stmt_in_statement1736);
                    	simple = simple_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  simple;
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:377:5: compound= compound_stmt
                    {
                    	PushFollow(FOLLOW_compound_stmt_in_statement1746);
                    	compound = compound_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  compound;
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "statement"


    // $ANTLR start "simple_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:382:1: simple_stmt returns [StatementAST res] : ( ( expression_stmt )=>expr_stmt= expression_stmt SEMICOLON | decl_stmt= declaration_stmt | sel_stmt= selection_stmt | iter_stmt= iteration_stmt | jump= jump_stmt SEMICOLON );
    public StatementAST simple_stmt() // throws RecognitionException [1]
    {   
        StatementAST res = null;

        ExpressionStatementAST expr_stmt = null;

        DeclarationStatementAST decl_stmt = null;

        StatementAST sel_stmt = null;

        StatementAST iter_stmt = null;

        StatementAST jump = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:383:2: ( ( expression_stmt )=>expr_stmt= expression_stmt SEMICOLON | decl_stmt= declaration_stmt | sel_stmt= selection_stmt | iter_stmt= iteration_stmt | jump= jump_stmt SEMICOLON )
            int alt26 = 5;
            alt26 = dfa26.Predict(input);
            switch (alt26) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:383:4: ( expression_stmt )=>expr_stmt= expression_stmt SEMICOLON
                    {
                    	PushFollow(FOLLOW_expression_stmt_in_simple_stmt1771);
                    	expr_stmt = expression_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_simple_stmt1773); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  expr_stmt;
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:386:4: decl_stmt= declaration_stmt
                    {
                    	PushFollow(FOLLOW_declaration_stmt_in_simple_stmt1782);
                    	decl_stmt = declaration_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  decl_stmt;
                    	  		
                    	}

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:389:4: sel_stmt= selection_stmt
                    {
                    	PushFollow(FOLLOW_selection_stmt_in_simple_stmt1791);
                    	sel_stmt = selection_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  sel_stmt;
                    	  		
                    	}

                    }
                    break;
                case 4 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:392:4: iter_stmt= iteration_stmt
                    {
                    	PushFollow(FOLLOW_iteration_stmt_in_simple_stmt1800);
                    	iter_stmt = iteration_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  iter_stmt;
                    	  		
                    	}

                    }
                    break;
                case 5 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:395:4: jump= jump_stmt SEMICOLON
                    {
                    	PushFollow(FOLLOW_jump_stmt_in_simple_stmt1809);
                    	jump = jump_stmt();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_simple_stmt1811); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  jump;	
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "simple_stmt"


    // $ANTLR start "declaration_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:400:1: declaration_stmt returns [DeclarationStatementAST res] : decl= declaration ;
    public DeclarationStatementAST declaration_stmt() // throws RecognitionException [1]
    {   
        DeclarationStatementAST res = null;

        DeclarationAST decl = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:401:2: (decl= declaration )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:401:4: decl= declaration
            {
            	PushFollow(FOLLOW_declaration_in_declaration_stmt1829);
            	decl = declaration();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  new DeclarationStatementAST(decl, decl.Line, decl.Column);
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "declaration_stmt"


    // $ANTLR start "expression_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:406:1: expression_stmt returns [ExpressionStatementAST res] : expr= expression ;
    public ExpressionStatementAST expression_stmt() // throws RecognitionException [1]
    {   
        ExpressionStatementAST res = null;

        ExpressionAST expr = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:407:2: (expr= expression )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:407:4: expr= expression
            {
            	PushFollow(FOLLOW_expression_in_expression_stmt1847);
            	expr = expression();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  new ExpressionStatementAST(expr, expr.Line, expr.Column);
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "expression_stmt"


    // $ANTLR start "selection_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:412:1: selection_stmt returns [StatementAST res] : IF LPAREN cond= expression RPAREN ontrue= statement ( ELSE onfalse= statement )? ;
    public StatementAST selection_stmt() // throws RecognitionException [1]
    {   
        StatementAST res = null;

        IToken IF8 = null;
        ExpressionAST cond = null;

        StatementAST ontrue = null;

        StatementAST onfalse = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:413:2: ( IF LPAREN cond= expression RPAREN ontrue= statement ( ELSE onfalse= statement )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:413:4: IF LPAREN cond= expression RPAREN ontrue= statement ( ELSE onfalse= statement )?
            {
            	IF8=(IToken)Match(input,IF,FOLLOW_IF_in_selection_stmt1863); if (state.failed) return res;
            	Match(input,LPAREN,FOLLOW_LPAREN_in_selection_stmt1865); if (state.failed) return res;
            	PushFollow(FOLLOW_expression_in_selection_stmt1869);
            	cond = expression();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	Match(input,RPAREN,FOLLOW_RPAREN_in_selection_stmt1871); if (state.failed) return res;
            	PushFollow(FOLLOW_statement_in_selection_stmt1875);
            	ontrue = statement();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:413:54: ( ELSE onfalse= statement )?
            	int alt27 = 2;
            	alt27 = dfa27.Predict(input);
            	switch (alt27) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:413:55: ELSE onfalse= statement
            	        {
            	        	Match(input,ELSE,FOLLOW_ELSE_in_selection_stmt1878); if (state.failed) return res;
            	        	PushFollow(FOLLOW_statement_in_selection_stmt1882);
            	        	onfalse = statement();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;

            	        }
            	        break;

            	}

            	if ( (state.backtracking==0) )
            	{

            	  			res =  new IfControlStatementAST(cond, ontrue, onfalse, IF8.Line, IF8.CharPositionInLine);
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "selection_stmt"


    // $ANTLR start "iteration_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:418:1: iteration_stmt returns [StatementAST res] : ( WHILE LPAREN cond_while= expression RPAREN body= statement | DO body= statement WHILE LPAREN cond_dowhile= expression RPAREN | FOR LPAREN (init= for_init_expr | SEMICOLON ) (cond_for= expr_list )? SEMICOLON (increment= expr_list )? RPAREN body= statement );
    public StatementAST iteration_stmt() // throws RecognitionException [1]
    {   
        StatementAST res = null;

        IToken WHILE9 = null;
        IToken DO10 = null;
        IToken FOR11 = null;
        ExpressionAST cond_while = null;

        StatementAST body = null;

        ExpressionAST cond_dowhile = null;

        BaseAST init = null;

        IEnumerable<ExpressionAST> cond_for = null;

        IEnumerable<ExpressionAST> increment = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:419:2: ( WHILE LPAREN cond_while= expression RPAREN body= statement | DO body= statement WHILE LPAREN cond_dowhile= expression RPAREN | FOR LPAREN (init= for_init_expr | SEMICOLON ) (cond_for= expr_list )? SEMICOLON (increment= expr_list )? RPAREN body= statement )
            int alt31 = 3;
            switch ( input.LA(1) ) 
            {
            case WHILE:
            	{
                alt31 = 1;
                }
                break;
            case DO:
            	{
                alt31 = 2;
                }
                break;
            case FOR:
            	{
                alt31 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	    NoViableAltException nvae_d31s0 =
            	        new NoViableAltException("", 31, 0, input);

            	    throw nvae_d31s0;
            }

            switch (alt31) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:419:4: WHILE LPAREN cond_while= expression RPAREN body= statement
                    {
                    	WHILE9=(IToken)Match(input,WHILE,FOLLOW_WHILE_in_iteration_stmt1900); if (state.failed) return res;
                    	Match(input,LPAREN,FOLLOW_LPAREN_in_iteration_stmt1902); if (state.failed) return res;
                    	PushFollow(FOLLOW_expression_in_iteration_stmt1906);
                    	cond_while = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,RPAREN,FOLLOW_RPAREN_in_iteration_stmt1908); if (state.failed) return res;
                    	PushFollow(FOLLOW_statement_in_iteration_stmt1912);
                    	body = statement();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new WhileControlStatementAST(cond_while, body, WHILE9.Line, WHILE9.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:422:4: DO body= statement WHILE LPAREN cond_dowhile= expression RPAREN
                    {
                    	DO10=(IToken)Match(input,DO,FOLLOW_DO_in_iteration_stmt1919); if (state.failed) return res;
                    	PushFollow(FOLLOW_statement_in_iteration_stmt1923);
                    	body = statement();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,WHILE,FOLLOW_WHILE_in_iteration_stmt1925); if (state.failed) return res;
                    	Match(input,LPAREN,FOLLOW_LPAREN_in_iteration_stmt1927); if (state.failed) return res;
                    	PushFollow(FOLLOW_expression_in_iteration_stmt1931);
                    	cond_dowhile = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,RPAREN,FOLLOW_RPAREN_in_iteration_stmt1933); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new DoWhileControlStatementAST(cond_dowhile, body, DO10.Line, DO10.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:4: FOR LPAREN (init= for_init_expr | SEMICOLON ) (cond_for= expr_list )? SEMICOLON (increment= expr_list )? RPAREN body= statement
                    {
                    	FOR11=(IToken)Match(input,FOR,FOLLOW_FOR_in_iteration_stmt1940); if (state.failed) return res;
                    	Match(input,LPAREN,FOLLOW_LPAREN_in_iteration_stmt1942); if (state.failed) return res;
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:15: (init= for_init_expr | SEMICOLON )
                    	int alt28 = 2;
                    	alt28 = dfa28.Predict(input);
                    	switch (alt28) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:16: init= for_init_expr
                    	        {
                    	        	PushFollow(FOLLOW_for_init_expr_in_iteration_stmt1947);
                    	        	init = for_init_expr();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return res;

                    	        }
                    	        break;
                    	    case 2 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:37: SEMICOLON
                    	        {
                    	        	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_iteration_stmt1951); if (state.failed) return res;

                    	        }
                    	        break;

                    	}

                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:48: (cond_for= expr_list )?
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);

                    	if ( (LA29_0 == NOT || (LA29_0 >= ADD && LA29_0 <= DECREMENT) || LA29_0 == LPAREN || (LA29_0 >= INTEGER && LA29_0 <= MAT4X4) || (LA29_0 >= ID && LA29_0 <= BOOL_CONSTANT)) )
                    	{
                    	    alt29 = 1;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:49: cond_for= expr_list
                    	        {
                    	        	PushFollow(FOLLOW_expr_list_in_iteration_stmt1957);
                    	        	cond_for = expr_list();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return res;

                    	        }
                    	        break;

                    	}

                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_iteration_stmt1961); if (state.failed) return res;
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:80: (increment= expr_list )?
                    	int alt30 = 2;
                    	int LA30_0 = input.LA(1);

                    	if ( (LA30_0 == NOT || (LA30_0 >= ADD && LA30_0 <= DECREMENT) || LA30_0 == LPAREN || (LA30_0 >= INTEGER && LA30_0 <= MAT4X4) || (LA30_0 >= ID && LA30_0 <= BOOL_CONSTANT)) )
                    	{
                    	    alt30 = 1;
                    	}
                    	switch (alt30) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:425:81: increment= expr_list
                    	        {
                    	        	PushFollow(FOLLOW_expr_list_in_iteration_stmt1966);
                    	        	increment = expr_list();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return res;

                    	        }
                    	        break;

                    	}

                    	Match(input,RPAREN,FOLLOW_RPAREN_in_iteration_stmt1970); if (state.failed) return res;
                    	PushFollow(FOLLOW_statement_in_iteration_stmt1974);
                    	body = statement();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			ExpressionListExpressionAST cond_exprList = new ExpressionListExpressionAST(cond_for, FOR11.Line, FOR11.CharPositionInLine);
                    	  			ExpressionListExpressionAST inc_exprList = new ExpressionListExpressionAST(increment, FOR11.Line, FOR11.CharPositionInLine);
                    	  			res =  new ForControlStatementAST(init, cond_exprList, inc_exprList, body, FOR11.Line, FOR11.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "iteration_stmt"


    // $ANTLR start "for_init_expr"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:432:1: for_init_expr returns [BaseAST res] : ( ( declaration )=>decl= declaration | expr= expr_list SEMICOLON );
    public BaseAST for_init_expr() // throws RecognitionException [1]
    {   
        BaseAST res = null;

        DeclarationAST decl = null;

        IEnumerable<ExpressionAST> expr = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:433:2: ( ( declaration )=>decl= declaration | expr= expr_list SEMICOLON )
            int alt32 = 2;
            alt32 = dfa32.Predict(input);
            switch (alt32) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:433:4: ( declaration )=>decl= declaration
                    {
                    	PushFollow(FOLLOW_declaration_in_for_init_expr1998);
                    	decl = declaration();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  decl;
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:436:4: expr= expr_list SEMICOLON
                    {
                    	PushFollow(FOLLOW_expr_list_in_for_init_expr2007);
                    	expr = expr_list();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_for_init_expr2009); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{
                    	  	
                    	  			res =  new ExpressionListExpressionAST(expr, 0, 0);
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "for_init_expr"


    // $ANTLR start "jump_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:441:1: jump_stmt returns [StatementAST res] : ( BREAK | CONTINUE | DISCARD | RETURN (expr= expression )? );
    public StatementAST jump_stmt() // throws RecognitionException [1]
    {   
        StatementAST res = null;

        IToken BREAK12 = null;
        IToken CONTINUE13 = null;
        IToken DISCARD14 = null;
        IToken RETURN15 = null;
        ExpressionAST expr = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:442:2: ( BREAK | CONTINUE | DISCARD | RETURN (expr= expression )? )
            int alt34 = 4;
            switch ( input.LA(1) ) 
            {
            case BREAK:
            	{
                alt34 = 1;
                }
                break;
            case CONTINUE:
            	{
                alt34 = 2;
                }
                break;
            case DISCARD:
            	{
                alt34 = 3;
                }
                break;
            case RETURN:
            	{
                alt34 = 4;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	    NoViableAltException nvae_d34s0 =
            	        new NoViableAltException("", 34, 0, input);

            	    throw nvae_d34s0;
            }

            switch (alt34) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:442:4: BREAK
                    {
                    	BREAK12=(IToken)Match(input,BREAK,FOLLOW_BREAK_in_jump_stmt2025); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new BreakStatementAST(BREAK12.Line, BREAK12.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:445:4: CONTINUE
                    {
                    	CONTINUE13=(IToken)Match(input,CONTINUE,FOLLOW_CONTINUE_in_jump_stmt2032); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new ContinueStatementAST(CONTINUE13.Line, CONTINUE13.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:448:4: DISCARD
                    {
                    	DISCARD14=(IToken)Match(input,DISCARD,FOLLOW_DISCARD_in_jump_stmt2039); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new DiscardStatementAST(DISCARD14.Line, DISCARD14.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;
                case 4 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:451:4: RETURN (expr= expression )?
                    {
                    	RETURN15=(IToken)Match(input,RETURN,FOLLOW_RETURN_in_jump_stmt2046); if (state.failed) return res;
                    	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:451:11: (expr= expression )?
                    	int alt33 = 2;
                    	int LA33_0 = input.LA(1);

                    	if ( (LA33_0 == NOT || (LA33_0 >= ADD && LA33_0 <= DECREMENT) || LA33_0 == LPAREN || (LA33_0 >= INTEGER && LA33_0 <= MAT4X4) || (LA33_0 >= ID && LA33_0 <= BOOL_CONSTANT)) )
                    	{
                    	    alt33 = 1;
                    	}
                    	switch (alt33) 
                    	{
                    	    case 1 :
                    	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:451:12: expr= expression
                    	        {
                    	        	PushFollow(FOLLOW_expression_in_jump_stmt2051);
                    	        	expr = expression();
                    	        	state.followingStackPointer--;
                    	        	if (state.failed) return res;

                    	        }
                    	        break;

                    	}

                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  new ReturnStatementAST(expr, RETURN15.Line, RETURN15.CharPositionInLine);
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "jump_stmt"


    // $ANTLR start "compound_stmt"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:456:1: compound_stmt returns [CompoundStatementAST res] : lbrace= LBRACE (stmt= statement )* RBRACE ;
    public CompoundStatementAST compound_stmt() // throws RecognitionException [1]
    {   
        CompoundStatementAST res = null;

        IToken lbrace = null;
        StatementAST stmt = null;



        	List<StatementAST> list = new List<StatementAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:463:2: (lbrace= LBRACE (stmt= statement )* RBRACE )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:463:4: lbrace= LBRACE (stmt= statement )* RBRACE
            {
            	lbrace=(IToken)Match(input,LBRACE,FOLLOW_LBRACE_in_compound_stmt2082); if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:463:18: (stmt= statement )*
            	do 
            	{
            	    int alt35 = 2;
            	    alt35 = dfa35.Predict(input);
            	    switch (alt35) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:463:19: stmt= statement
            			    {
            			    	PushFollow(FOLLOW_statement_in_compound_stmt2087);
            			    	stmt = statement();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			list.Add(stmt);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop35;
            	    }
            	} while (true);

            	loop35:
            		;	// Stops C# compiler whining that label 'loop35' has no statements

            	Match(input,RBRACE,FOLLOW_RBRACE_in_compound_stmt2093); if (state.failed) return res;

            }

            if ( (state.backtracking==0) )
            {

              	res =  new CompoundStatementAST(list, true, lbrace.Line, lbrace.CharPositionInLine);

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "compound_stmt"


    // $ANTLR start "declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:468:1: declaration returns [DeclarationAST res] : fully_spec_type= fully_specified_type (decl= simple_declaration | SEMICOLON ) ;
    public DeclarationAST declaration() // throws RecognitionException [1]
    {   
        DeclarationAST res = null;

        GLSLParser.fully_specified_type_return fully_spec_type = null;

        DeclarationAST decl = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:469:2: (fully_spec_type= fully_specified_type (decl= simple_declaration | SEMICOLON ) )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:469:4: fully_spec_type= fully_specified_type (decl= simple_declaration | SEMICOLON )
            {
            	PushFollow(FOLLOW_fully_specified_type_in_declaration2109);
            	fully_spec_type = fully_specified_type();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:469:41: (decl= simple_declaration | SEMICOLON )
            	int alt36 = 2;
            	int LA36_0 = input.LA(1);

            	if ( (LA36_0 == ID) )
            	{
            	    alt36 = 1;
            	}
            	else if ( (LA36_0 == SEMICOLON) )
            	{
            	    alt36 = 2;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	    NoViableAltException nvae_d36s0 =
            	        new NoViableAltException("", 36, 0, input);

            	    throw nvae_d36s0;
            	}
            	switch (alt36) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:469:42: decl= simple_declaration
            	        {
            	        	PushFollow(FOLLOW_simple_declaration_in_declaration2114);
            	        	decl = simple_declaration();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			decl.TypeSpecifier = ((fully_spec_type != null) ? fully_spec_type.typeSpecifier : null);
            	        	  			if(decl.Is<MultipleVariableDeclarationAST>())
            	        	  				decl.Cast<MultipleVariableDeclarationAST>().Qualifier = (TypeQualifier)((fully_spec_type != null) ? fully_spec_type.typeQualifier : 0);
            	        	  			res =  decl;
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:474:7: SEMICOLON
            	        {
            	        	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_declaration2120); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  ((fully_spec_type != null) ? fully_spec_type.typeSpecifier : null) as StructDeclarationAST;
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "declaration"

    public class fully_specified_type_return : ParserRuleReturnScope
    {
        public int typeQualifier;
        public ITypeSpecifier typeSpecifier;
    };

    // $ANTLR start "fully_specified_type"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:479:1: fully_specified_type returns [int typeQualifier, ITypeSpecifier typeSpecifier] : (qualifier= type_qualifier )? type= type_specifier ;
    public GLSLParser.fully_specified_type_return fully_specified_type() // throws RecognitionException [1]
    {   
        GLSLParser.fully_specified_type_return retval = new GLSLParser.fully_specified_type_return();
        retval.Start = input.LT(1);

        int qualifier = 0;

        ITypeSpecifier type = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:480:2: ( (qualifier= type_qualifier )? type= type_specifier )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:480:4: (qualifier= type_qualifier )? type= type_specifier
            {
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:480:4: (qualifier= type_qualifier )?
            	int alt37 = 2;
            	int LA37_0 = input.LA(1);

            	if ( ((LA37_0 >= CONST && LA37_0 <= INVARIANT)) )
            	{
            	    alt37 = 1;
            	}
            	switch (alt37) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:480:5: qualifier= type_qualifier
            	        {
            	        	PushFollow(FOLLOW_type_qualifier_in_fully_specified_type2140);
            	        	qualifier = type_qualifier();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			retval.typeQualifier =  qualifier;
            	        	  		
            	        	}

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_type_specifier_in_fully_specified_type2148);
            	type = type_specifier();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) )
            	{

            	  			retval.typeSpecifier =  type;
            	  		
            	}

            }

            retval.Stop = input.LT(-1);

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "fully_specified_type"


    // $ANTLR start "type_qualifier"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:487:1: type_qualifier returns [int res] : ( CONST | ATTRIBUTE | UNIFORM | VARYING | CENTROID VARYING | INVARIANT | INVARIANT VARYING | INVARIANT CENTROID VARYING );
    public int type_qualifier() // throws RecognitionException [1]
    {   
        int res = 0;

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:488:2: ( CONST | ATTRIBUTE | UNIFORM | VARYING | CENTROID VARYING | INVARIANT | INVARIANT VARYING | INVARIANT CENTROID VARYING )
            int alt38 = 8;
            alt38 = dfa38.Predict(input);
            switch (alt38) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:488:4: CONST
                    {
                    	Match(input,CONST,FOLLOW_CONST_in_type_qualifier2164); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Const;
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:491:4: ATTRIBUTE
                    {
                    	Match(input,ATTRIBUTE,FOLLOW_ATTRIBUTE_in_type_qualifier2171); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Attribute;
                    	  		
                    	}

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:494:4: UNIFORM
                    {
                    	Match(input,UNIFORM,FOLLOW_UNIFORM_in_type_qualifier2178); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Uniform;
                    	  		
                    	}

                    }
                    break;
                case 4 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:497:4: VARYING
                    {
                    	Match(input,VARYING,FOLLOW_VARYING_in_type_qualifier2185); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Varying;
                    	  		
                    	}

                    }
                    break;
                case 5 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:500:5: CENTROID VARYING
                    {
                    	Match(input,CENTROID,FOLLOW_CENTROID_in_type_qualifier2193); if (state.failed) return res;
                    	Match(input,VARYING,FOLLOW_VARYING_in_type_qualifier2195); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Centroid_Varying;
                    	  		
                    	}

                    }
                    break;
                case 6 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:503:4: INVARIANT
                    {
                    	Match(input,INVARIANT,FOLLOW_INVARIANT_in_type_qualifier2202); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Invariant;
                    	  		
                    	}

                    }
                    break;
                case 7 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:506:4: INVARIANT VARYING
                    {
                    	Match(input,INVARIANT,FOLLOW_INVARIANT_in_type_qualifier2209); if (state.failed) return res;
                    	Match(input,VARYING,FOLLOW_VARYING_in_type_qualifier2211); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Invariant_Varying;
                    	  		
                    	}

                    }
                    break;
                case 8 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:509:4: INVARIANT CENTROID VARYING
                    {
                    	Match(input,INVARIANT,FOLLOW_INVARIANT_in_type_qualifier2218); if (state.failed) return res;
                    	Match(input,CENTROID,FOLLOW_CENTROID_in_type_qualifier2220); if (state.failed) return res;
                    	Match(input,VARYING,FOLLOW_VARYING_in_type_qualifier2222); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)TypeQualifier.Invariant_Centroid_Varying;
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "type_qualifier"


    // $ANTLR start "type_specifier"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:514:1: type_specifier returns [ITypeSpecifier res] : (tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 | VOID | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | ID ) | sdecl= struct_declaration ) ( LBRACKET (expr= expression )? RBRACKET )? ;
    public ITypeSpecifier type_specifier() // throws RecognitionException [1]
    {   
        ITypeSpecifier res = null;

        IToken tok = null;
        StructDeclarationAST sdecl = null;

        ExpressionAST expr = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:515:2: ( (tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 | VOID | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | ID ) | sdecl= struct_declaration ) ( LBRACKET (expr= expression )? RBRACKET )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:515:4: (tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 | VOID | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | ID ) | sdecl= struct_declaration ) ( LBRACKET (expr= expression )? RBRACKET )?
            {
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:515:4: (tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 | VOID | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | ID ) | sdecl= struct_declaration )
            	int alt39 = 2;
            	int LA39_0 = input.LA(1);

            	if ( (LA39_0 == VOID || (LA39_0 >= INTEGER && LA39_0 <= ID)) )
            	{
            	    alt39 = 1;
            	}
            	else if ( (LA39_0 == STRUCT) )
            	{
            	    alt39 = 2;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	    NoViableAltException nvae_d39s0 =
            	        new NoViableAltException("", 39, 0, input);

            	    throw nvae_d39s0;
            	}
            	switch (alt39) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:515:5: tok= ( INTEGER | FLOAT | BOOL | VEC2 | VEC3 | VEC4 | IVEC2 | IVEC3 | IVEC4 | BVEC2 | BVEC3 | BVEC4 | MAT2 | MAT3 | MAT4 | MAT2X2 | MAT2X3 | MAT2X4 | MAT3X2 | MAT3X3 | MAT3X4 | MAT4X2 | MAT4X3 | MAT4X4 | VOID | SAMPLER1D | SAMPLER2D | SAMPLER3D | SAMPLERCUBE | SAMPLER1DSHADOW | SAMPLER2DSHADOW | ID )
            	        {
            	        	tok = (IToken)input.LT(1);
            	        	if ( input.LA(1) == VOID || (input.LA(1) >= INTEGER && input.LA(1) <= ID) ) 
            	        	{
            	        	    input.Consume();
            	        	    state.errorRecovery = false;state.failed = false;
            	        	}
            	        	else 
            	        	{
            	        	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    throw mse;
            	        	}

            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new NamedTypeSpecifier(((tok != null) ? tok.Text : null), tok.Line, tok.CharPositionInLine);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:549:4: sdecl= struct_declaration
            	        {
            	        	PushFollow(FOLLOW_struct_declaration_in_type_specifier2407);
            	        	sdecl = struct_declaration();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  sdecl;
            	        	  		
            	        	}

            	        }
            	        break;

            	}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:552:3: ( LBRACKET (expr= expression )? RBRACKET )?
            	int alt41 = 2;
            	int LA41_0 = input.LA(1);

            	if ( (LA41_0 == LBRACKET) )
            	{
            	    alt41 = 1;
            	}
            	switch (alt41) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:552:4: LBRACKET (expr= expression )? RBRACKET
            	        {
            	        	Match(input,LBRACKET,FOLLOW_LBRACKET_in_type_specifier2415); if (state.failed) return res;
            	        	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:552:13: (expr= expression )?
            	        	int alt40 = 2;
            	        	int LA40_0 = input.LA(1);

            	        	if ( (LA40_0 == NOT || (LA40_0 >= ADD && LA40_0 <= DECREMENT) || LA40_0 == LPAREN || (LA40_0 >= INTEGER && LA40_0 <= MAT4X4) || (LA40_0 >= ID && LA40_0 <= BOOL_CONSTANT)) )
            	        	{
            	        	    alt40 = 1;
            	        	}
            	        	switch (alt40) 
            	        	{
            	        	    case 1 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:552:14: expr= expression
            	        	        {
            	        	        	PushFollow(FOLLOW_expression_in_type_specifier2420);
            	        	        	expr = expression();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;

            	        	        }
            	        	        break;

            	        	}

            	        	Match(input,RBRACKET,FOLLOW_RBRACKET_in_type_specifier2424); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new ArrayTypeSpecifier(res, expr, res.Line, res.Column);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "type_specifier"


    // $ANTLR start "simple_declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:557:1: simple_declaration returns [DeclarationAST res] : (var_decls= variable_declarations SEMICOLON | func_decl= function_declaration );
    public DeclarationAST simple_declaration() // throws RecognitionException [1]
    {   
        DeclarationAST res = null;

        IEnumerable<VariableDeclarationAST> var_decls = null;

        FunctionDeclarationAST func_decl = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:558:2: (var_decls= variable_declarations SEMICOLON | func_decl= function_declaration )
            int alt42 = 2;
            int LA42_0 = input.LA(1);

            if ( (LA42_0 == ID) )
            {
                int LA42_1 = input.LA(2);

                if ( (LA42_1 == LPAREN) )
                {
                    alt42 = 2;
                }
                else if ( ((LA42_1 >= COMMA && LA42_1 <= SEMICOLON) || LA42_1 == LBRACKET || LA42_1 == ASSIGN) )
                {
                    alt42 = 1;
                }
                else 
                {
                    if ( state.backtracking > 0 ) {state.failed = true; return res;}
                    NoViableAltException nvae_d42s1 =
                        new NoViableAltException("", 42, 1, input);

                    throw nvae_d42s1;
                }
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return res;}
                NoViableAltException nvae_d42s0 =
                    new NoViableAltException("", 42, 0, input);

                throw nvae_d42s0;
            }
            switch (alt42) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:558:4: var_decls= variable_declarations SEMICOLON
                    {
                    	PushFollow(FOLLOW_variable_declarations_in_simple_declaration2444);
                    	var_decls = variable_declarations();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_simple_declaration2446); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			int line = var_decls.Any() ? var_decls.First().Line : 0;
                    	  			int column = var_decls.Any() ? var_decls.First().Column : 0;
                    	  			res =  new MultipleVariableDeclarationAST(var_decls, line, column);
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:563:4: func_decl= function_declaration
                    {
                    	PushFollow(FOLLOW_function_declaration_in_simple_declaration2455);
                    	func_decl = function_declaration();
                    	state.followingStackPointer--;
                    	if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  func_decl;	
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "simple_declaration"


    // $ANTLR start "variable_declarations"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:568:1: variable_declarations returns [IEnumerable<VariableDeclarationAST> res] : var1= variable_declaration ( COMMA var2= variable_declaration )* ;
    public IEnumerable<VariableDeclarationAST> variable_declarations() // throws RecognitionException [1]
    {   
        IEnumerable<VariableDeclarationAST> res = null;

        VariableDeclarationAST var1 = null;

        VariableDeclarationAST var2 = null;



        	List<VariableDeclarationAST> list = new List<VariableDeclarationAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:575:2: (var1= variable_declaration ( COMMA var2= variable_declaration )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:575:4: var1= variable_declaration ( COMMA var2= variable_declaration )*
            {
            	PushFollow(FOLLOW_variable_declaration_in_variable_declarations2483);
            	var1 = variable_declaration();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			list.Add(var1);
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:577:5: ( COMMA var2= variable_declaration )*
            	do 
            	{
            	    int alt43 = 2;
            	    int LA43_0 = input.LA(1);

            	    if ( (LA43_0 == COMMA) )
            	    {
            	        alt43 = 1;
            	    }


            	    switch (alt43) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:577:6: COMMA var2= variable_declaration
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_variable_declarations2488); if (state.failed) return res;
            			    	PushFollow(FOLLOW_variable_declaration_in_variable_declarations2492);
            			    	var2 = variable_declaration();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			list.Add(var2);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop43;
            	    }
            	} while (true);

            	loop43:
            		;	// Stops C# compiler whining that label 'loop43' has no statements


            }

            if ( (state.backtracking==0) )
            {

              	res =  list;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "variable_declarations"


    // $ANTLR start "variable_declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:582:1: variable_declaration returns [VariableDeclarationAST res] : ID ( LBRACKET (size_expr= expression )? RBRACKET )? ( ASSIGN init_expr= expression )? ;
    public VariableDeclarationAST variable_declaration() // throws RecognitionException [1]
    {   
        VariableDeclarationAST res = null;

        IToken ID16 = null;
        ExpressionAST size_expr = null;

        ExpressionAST init_expr = null;



        	LocalVariableDeclarationAST varDec = new LocalVariableDeclarationAST();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:589:2: ( ID ( LBRACKET (size_expr= expression )? RBRACKET )? ( ASSIGN init_expr= expression )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:589:4: ID ( LBRACKET (size_expr= expression )? RBRACKET )? ( ASSIGN init_expr= expression )?
            {
            	ID16=(IToken)Match(input,ID,FOLLOW_ID_in_variable_declaration2520); if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			varDec.Name = ((ID16 != null) ? ID16.Text : null);
            	  			varDec.Line = ID16.Line;
            	  			varDec.Column = ID16.CharPositionInLine;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:593:4: ( LBRACKET (size_expr= expression )? RBRACKET )?
            	int alt45 = 2;
            	int LA45_0 = input.LA(1);

            	if ( (LA45_0 == LBRACKET) )
            	{
            	    alt45 = 1;
            	}
            	switch (alt45) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:593:5: LBRACKET (size_expr= expression )? RBRACKET
            	        {
            	        	Match(input,LBRACKET,FOLLOW_LBRACKET_in_variable_declaration2524); if (state.failed) return res;
            	        	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:593:14: (size_expr= expression )?
            	        	int alt44 = 2;
            	        	int LA44_0 = input.LA(1);

            	        	if ( (LA44_0 == NOT || (LA44_0 >= ADD && LA44_0 <= DECREMENT) || LA44_0 == LPAREN || (LA44_0 >= INTEGER && LA44_0 <= MAT4X4) || (LA44_0 >= ID && LA44_0 <= BOOL_CONSTANT)) )
            	        	{
            	        	    alt44 = 1;
            	        	}
            	        	switch (alt44) 
            	        	{
            	        	    case 1 :
            	        	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:593:15: size_expr= expression
            	        	        {
            	        	        	PushFollow(FOLLOW_expression_in_variable_declaration2529);
            	        	        	size_expr = expression();
            	        	        	state.followingStackPointer--;
            	        	        	if (state.failed) return res;
            	        	        	if ( (state.backtracking==0) )
            	        	        	{

            	        	        	  			varDec.SizeExpression = size_expr;
            	        	        	  		
            	        	        	}

            	        	        }
            	        	        break;

            	        	}

            	        	Match(input,RBRACKET,FOLLOW_RBRACKET_in_variable_declaration2535); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			varDec.IsArray = true;
            	        	  		
            	        	}

            	        }
            	        break;

            	}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:597:7: ( ASSIGN init_expr= expression )?
            	int alt46 = 2;
            	int LA46_0 = input.LA(1);

            	if ( (LA46_0 == ASSIGN) )
            	{
            	    alt46 = 1;
            	}
            	switch (alt46) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:597:8: ASSIGN init_expr= expression
            	        {
            	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_variable_declaration2542); if (state.failed) return res;
            	        	PushFollow(FOLLOW_expression_in_variable_declaration2546);
            	        	init_expr = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			varDec.InitExpression = init_expr; 
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

            if ( (state.backtracking==0) )
            {

              	res =  varDec;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "variable_declaration"


    // $ANTLR start "function_declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:602:1: function_declaration returns [FunctionDeclarationAST res] : ID LPAREN (pdecls= param_declarations | VOID )? RPAREN (body= compound_stmt | SEMICOLON ) ;
    public FunctionDeclarationAST function_declaration() // throws RecognitionException [1]
    {   
        FunctionDeclarationAST res = null;

        IToken ID17 = null;
        IEnumerable<ParameterDeclarationAST> pdecls = null;

        CompoundStatementAST body = null;



        	List<ParameterDeclarationAST> paramsdecls = new List<ParameterDeclarationAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:606:2: ( ID LPAREN (pdecls= param_declarations | VOID )? RPAREN (body= compound_stmt | SEMICOLON ) )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:606:4: ID LPAREN (pdecls= param_declarations | VOID )? RPAREN (body= compound_stmt | SEMICOLON )
            {
            	ID17=(IToken)Match(input,ID,FOLLOW_ID_in_function_declaration2570); if (state.failed) return res;
            	Match(input,LPAREN,FOLLOW_LPAREN_in_function_declaration2572); if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:606:14: (pdecls= param_declarations | VOID )?
            	int alt47 = 3;
            	alt47 = dfa47.Predict(input);
            	switch (alt47) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:606:15: pdecls= param_declarations
            	        {
            	        	PushFollow(FOLLOW_param_declarations_in_function_declaration2577);
            	        	pdecls = param_declarations();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			if(pdecls != null)
            	        	  				paramsdecls.AddRange(pdecls);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:609:7: VOID
            	        {
            	        	Match(input,VOID,FOLLOW_VOID_in_function_declaration2583); if (state.failed) return res;

            	        }
            	        break;

            	}

            	Match(input,RPAREN,FOLLOW_RPAREN_in_function_declaration2587); if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:609:21: (body= compound_stmt | SEMICOLON )
            	int alt48 = 2;
            	int LA48_0 = input.LA(1);

            	if ( (LA48_0 == LBRACE) )
            	{
            	    alt48 = 1;
            	}
            	else if ( (LA48_0 == SEMICOLON) )
            	{
            	    alt48 = 2;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	    NoViableAltException nvae_d48s0 =
            	        new NoViableAltException("", 48, 0, input);

            	    throw nvae_d48s0;
            	}
            	switch (alt48) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:609:22: body= compound_stmt
            	        {
            	        	PushFollow(FOLLOW_compound_stmt_in_function_declaration2592);
            	        	body = compound_stmt();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			body.NewScope = false;
            	        	  			res =  new FunctionDefinitionAST(((ID17 != null) ? ID17.Text : null), paramsdecls, body, ID17.Line, ID17.CharPositionInLine);
            	        	  		
            	        	}

            	        }
            	        break;
            	    case 2 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:612:7: SEMICOLON
            	        {
            	        	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_function_declaration2598); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			res =  new FunctionDeclarationAST(((ID17 != null) ? ID17.Text : null), paramsdecls, ID17.Line, ID17.CharPositionInLine);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "function_declaration"


    // $ANTLR start "param_declarations"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:617:1: param_declarations returns [IEnumerable<ParameterDeclarationAST> res] : param1= param_declaration ( COMMA param2= param_declaration )* ;
    public IEnumerable<ParameterDeclarationAST> param_declarations() // throws RecognitionException [1]
    {   
        IEnumerable<ParameterDeclarationAST> res = null;

        ParameterDeclarationAST param1 = null;

        ParameterDeclarationAST param2 = null;



        	List<ParameterDeclarationAST> list = new List<ParameterDeclarationAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:624:2: (param1= param_declaration ( COMMA param2= param_declaration )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:624:4: param1= param_declaration ( COMMA param2= param_declaration )*
            {
            	PushFollow(FOLLOW_param_declaration_in_param_declarations2627);
            	param1 = param_declaration();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			list.Add(param1);
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:626:4: ( COMMA param2= param_declaration )*
            	do 
            	{
            	    int alt49 = 2;
            	    int LA49_0 = input.LA(1);

            	    if ( (LA49_0 == COMMA) )
            	    {
            	        alt49 = 1;
            	    }


            	    switch (alt49) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:626:5: COMMA param2= param_declaration
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_param_declarations2631); if (state.failed) return res;
            			    	PushFollow(FOLLOW_param_declaration_in_param_declarations2635);
            			    	param2 = param_declaration();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			list.Add(param2);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop49;
            	    }
            	} while (true);

            	loop49:
            		;	// Stops C# compiler whining that label 'loop49' has no statements


            }

            if ( (state.backtracking==0) )
            {

              	res =  list;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "param_declarations"


    // $ANTLR start "param_declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:631:1: param_declaration returns [ParameterDeclarationAST res] : (qualifier= param_qualifier )? type= type_specifier ID ( LBRACKET size_expr= expression RBRACKET )? ( ASSIGN default_expr= expression )? ;
    public ParameterDeclarationAST param_declaration() // throws RecognitionException [1]
    {   
        ParameterDeclarationAST res = null;

        IToken ID18 = null;
        int qualifier = 0;

        ITypeSpecifier type = null;

        ExpressionAST size_expr = null;

        ExpressionAST default_expr = null;



        	ParameterDeclarationAST paramDec = new ParameterDeclarationAST();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:638:2: ( (qualifier= param_qualifier )? type= type_specifier ID ( LBRACKET size_expr= expression RBRACKET )? ( ASSIGN default_expr= expression )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:638:4: (qualifier= param_qualifier )? type= type_specifier ID ( LBRACKET size_expr= expression RBRACKET )? ( ASSIGN default_expr= expression )?
            {
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:638:4: (qualifier= param_qualifier )?
            	int alt50 = 2;
            	int LA50_0 = input.LA(1);

            	if ( (LA50_0 == CONST || (LA50_0 >= IN && LA50_0 <= INOUT)) )
            	{
            	    alt50 = 1;
            	}
            	switch (alt50) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:638:5: qualifier= param_qualifier
            	        {
            	        	PushFollow(FOLLOW_param_qualifier_in_param_declaration2666);
            	        	qualifier = param_qualifier();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			paramDec.Qualifier = (ParamQualifier)qualifier;
            	        	  		
            	        	}

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_type_specifier_in_param_declaration2674);
            	type = type_specifier();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			paramDec.TypeSpecifier = type;
            	  		
            	}
            	ID18=(IToken)Match(input,ID,FOLLOW_ID_in_param_declaration2677); if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			paramDec.Name = ((ID18 != null) ? ID18.Text : null);
            	  			paramDec.Line = ID18.Line;
            	  			paramDec.Column = ID18.CharPositionInLine;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:646:5: ( LBRACKET size_expr= expression RBRACKET )?
            	int alt51 = 2;
            	int LA51_0 = input.LA(1);

            	if ( (LA51_0 == LBRACKET) )
            	{
            	    alt51 = 1;
            	}
            	switch (alt51) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:646:6: LBRACKET size_expr= expression RBRACKET
            	        {
            	        	Match(input,LBRACKET,FOLLOW_LBRACKET_in_param_declaration2682); if (state.failed) return res;
            	        	PushFollow(FOLLOW_expression_in_param_declaration2686);
            	        	size_expr = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	Match(input,RBRACKET,FOLLOW_RBRACKET_in_param_declaration2688); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			paramDec.SizeExpression = size_expr;
            	        	  			paramDec.IsArray = true;
            	        	  		
            	        	}

            	        }
            	        break;

            	}

            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:649:7: ( ASSIGN default_expr= expression )?
            	int alt52 = 2;
            	int LA52_0 = input.LA(1);

            	if ( (LA52_0 == ASSIGN) )
            	{
            	    alt52 = 1;
            	}
            	switch (alt52) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:649:8: ASSIGN default_expr= expression
            	        {
            	        	Match(input,ASSIGN,FOLLOW_ASSIGN_in_param_declaration2695); if (state.failed) return res;
            	        	PushFollow(FOLLOW_expression_in_param_declaration2699);
            	        	default_expr = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			paramDec.DefaultExpression = default_expr;
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

            if ( (state.backtracking==0) )
            {

              	res =  paramDec;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "param_declaration"


    // $ANTLR start "param_qualifier"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:654:1: param_qualifier returns [int res] : ( IN | OUT | INOUT | CONST | CONST IN );
    public int param_qualifier() // throws RecognitionException [1]
    {   
        int res = 0;

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:655:2: ( IN | OUT | INOUT | CONST | CONST IN )
            int alt53 = 5;
            switch ( input.LA(1) ) 
            {
            case IN:
            	{
                alt53 = 1;
                }
                break;
            case OUT:
            	{
                alt53 = 2;
                }
                break;
            case INOUT:
            	{
                alt53 = 3;
                }
                break;
            case CONST:
            	{
                int LA53_4 = input.LA(2);

                if ( (LA53_4 == IN) )
                {
                    alt53 = 5;
                }
                else if ( (LA53_4 == VOID || (LA53_4 >= STRUCT && LA53_4 <= ID)) )
                {
                    alt53 = 4;
                }
                else 
                {
                    if ( state.backtracking > 0 ) {state.failed = true; return res;}
                    NoViableAltException nvae_d53s4 =
                        new NoViableAltException("", 53, 4, input);

                    throw nvae_d53s4;
                }
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return res;}
            	    NoViableAltException nvae_d53s0 =
            	        new NoViableAltException("", 53, 0, input);

            	    throw nvae_d53s0;
            }

            switch (alt53) 
            {
                case 1 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:655:4: IN
                    {
                    	Match(input,IN,FOLLOW_IN_in_param_qualifier2717); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)ParamQualifier.In;
                    	  		
                    	}

                    }
                    break;
                case 2 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:658:4: OUT
                    {
                    	Match(input,OUT,FOLLOW_OUT_in_param_qualifier2724); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)ParamQualifier.Out;
                    	  		
                    	}

                    }
                    break;
                case 3 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:661:4: INOUT
                    {
                    	Match(input,INOUT,FOLLOW_INOUT_in_param_qualifier2731); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)ParamQualifier.InOut;
                    	  		
                    	}

                    }
                    break;
                case 4 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:664:4: CONST
                    {
                    	Match(input,CONST,FOLLOW_CONST_in_param_qualifier2738); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)ParamQualifier.Const;
                    	  		
                    	}

                    }
                    break;
                case 5 :
                    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:667:4: CONST IN
                    {
                    	Match(input,CONST,FOLLOW_CONST_in_param_qualifier2745); if (state.failed) return res;
                    	Match(input,IN,FOLLOW_IN_in_param_qualifier2747); if (state.failed) return res;
                    	if ( (state.backtracking==0) )
                    	{

                    	  			res =  (int)ParamQualifier.Const_In;
                    	  		
                    	}

                    }
                    break;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "param_qualifier"


    // $ANTLR start "struct_declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:672:1: struct_declaration returns [StructDeclarationAST res] : STRUCT ( ID )? LBRACE fields= field_declarators RBRACE ;
    public StructDeclarationAST struct_declaration() // throws RecognitionException [1]
    {   
        StructDeclarationAST res = null;

        IToken ID19 = null;
        IToken STRUCT20 = null;
        IEnumerable<MultipleVariableDeclarationAST> fields = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:673:2: ( STRUCT ( ID )? LBRACE fields= field_declarators RBRACE )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:673:4: STRUCT ( ID )? LBRACE fields= field_declarators RBRACE
            {
            	STRUCT20=(IToken)Match(input,STRUCT,FOLLOW_STRUCT_in_struct_declaration2763); if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:673:11: ( ID )?
            	int alt54 = 2;
            	int LA54_0 = input.LA(1);

            	if ( (LA54_0 == ID) )
            	{
            	    alt54 = 1;
            	}
            	switch (alt54) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:673:12: ID
            	        {
            	        	ID19=(IToken)Match(input,ID,FOLLOW_ID_in_struct_declaration2766); if (state.failed) return res;

            	        }
            	        break;

            	}

            	Match(input,LBRACE,FOLLOW_LBRACE_in_struct_declaration2770); if (state.failed) return res;
            	PushFollow(FOLLOW_field_declarators_in_struct_declaration2774);
            	fields = field_declarators();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	Match(input,RBRACE,FOLLOW_RBRACE_in_struct_declaration2776); if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  new StructDeclarationAST(((ID19 != null) ? ID19.Text : null), fields, STRUCT20.Line, STRUCT20.CharPositionInLine);
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "struct_declaration"


    // $ANTLR start "field_declarators"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:678:1: field_declarators returns [IEnumerable<MultipleVariableDeclarationAST> res] : field_decl1= field_declarator SEMICOLON (field_decl2= field_declarators )? ;
    public IEnumerable<MultipleVariableDeclarationAST> field_declarators() // throws RecognitionException [1]
    {   
        IEnumerable<MultipleVariableDeclarationAST> res = null;

        MultipleVariableDeclarationAST field_decl1 = null;

        IEnumerable<MultipleVariableDeclarationAST> field_decl2 = null;



        	List<MultipleVariableDeclarationAST> list = new List<MultipleVariableDeclarationAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:685:2: (field_decl1= field_declarator SEMICOLON (field_decl2= field_declarators )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:685:4: field_decl1= field_declarator SEMICOLON (field_decl2= field_declarators )?
            {
            	PushFollow(FOLLOW_field_declarator_in_field_declarators2804);
            	field_decl1 = field_declarator();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			list.Add(field_decl1);
            	  		
            	}
            	Match(input,SEMICOLON,FOLLOW_SEMICOLON_in_field_declarators2808); if (state.failed) return res;
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:687:15: (field_decl2= field_declarators )?
            	int alt55 = 2;
            	int LA55_0 = input.LA(1);

            	if ( (LA55_0 == VOID || (LA55_0 >= STRUCT && LA55_0 <= ID)) )
            	{
            	    alt55 = 1;
            	}
            	switch (alt55) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:687:16: field_decl2= field_declarators
            	        {
            	        	PushFollow(FOLLOW_field_declarators_in_field_declarators2813);
            	        	field_decl2 = field_declarators();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			list.AddRange(field_decl2);
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

            if ( (state.backtracking==0) )
            {

              	res =  list;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "field_declarators"


    // $ANTLR start "field_declarator"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:692:1: field_declarator returns [MultipleVariableDeclarationAST res] : type= type_specifier fdecls= field_declarations ;
    public MultipleVariableDeclarationAST field_declarator() // throws RecognitionException [1]
    {   
        MultipleVariableDeclarationAST res = null;

        ITypeSpecifier type = null;

        IEnumerable<FieldDeclarationAST> fdecls = null;


        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:693:2: (type= type_specifier fdecls= field_declarations )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:693:4: type= type_specifier fdecls= field_declarations
            {
            	PushFollow(FOLLOW_type_specifier_in_field_declarator2833);
            	type = type_specifier();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	PushFollow(FOLLOW_field_declarations_in_field_declarator2837);
            	fdecls = field_declarations();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			res =  new MultipleVariableDeclarationAST(type, fdecls.Cast<VariableDeclarationAST>(), type.Line, type.Column);
            	  		
            	}

            }

        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "field_declarator"


    // $ANTLR start "field_declarations"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:698:1: field_declarations returns [IEnumerable<FieldDeclarationAST> res] : field1= field_declaration ( COMMA field2= field_declaration )* ;
    public IEnumerable<FieldDeclarationAST> field_declarations() // throws RecognitionException [1]
    {   
        IEnumerable<FieldDeclarationAST> res = null;

        FieldDeclarationAST field1 = null;

        FieldDeclarationAST field2 = null;



        	List<FieldDeclarationAST> list = new List<FieldDeclarationAST>();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:705:2: (field1= field_declaration ( COMMA field2= field_declaration )* )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:705:4: field1= field_declaration ( COMMA field2= field_declaration )*
            {
            	PushFollow(FOLLOW_field_declaration_in_field_declarations2865);
            	field1 = field_declaration();
            	state.followingStackPointer--;
            	if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			list.Add(field1);
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:707:5: ( COMMA field2= field_declaration )*
            	do 
            	{
            	    int alt56 = 2;
            	    int LA56_0 = input.LA(1);

            	    if ( (LA56_0 == COMMA) )
            	    {
            	        alt56 = 1;
            	    }


            	    switch (alt56) 
            		{
            			case 1 :
            			    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:707:6: COMMA field2= field_declaration
            			    {
            			    	Match(input,COMMA,FOLLOW_COMMA_in_field_declarations2870); if (state.failed) return res;
            			    	PushFollow(FOLLOW_field_declaration_in_field_declarations2874);
            			    	field2 = field_declaration();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return res;
            			    	if ( (state.backtracking==0) )
            			    	{

            			    	  			list.Add(field2);
            			    	  		
            			    	}

            			    }
            			    break;

            			default:
            			    goto loop56;
            	    }
            	} while (true);

            	loop56:
            		;	// Stops C# compiler whining that label 'loop56' has no statements


            }

            if ( (state.backtracking==0) )
            {

              	res =  list;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "field_declarations"


    // $ANTLR start "field_declaration"
    // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:712:1: field_declaration returns [FieldDeclarationAST res] : ID ( LBRACKET size_expr= expression RBRACKET )? ;
    public FieldDeclarationAST field_declaration() // throws RecognitionException [1]
    {   
        FieldDeclarationAST res = null;

        IToken ID21 = null;
        ExpressionAST size_expr = null;



        	FieldDeclarationAST field = new FieldDeclarationAST();

        try 
    	{
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:719:2: ( ID ( LBRACKET size_expr= expression RBRACKET )? )
            // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:719:4: ID ( LBRACKET size_expr= expression RBRACKET )?
            {
            	ID21=(IToken)Match(input,ID,FOLLOW_ID_in_field_declaration2902); if (state.failed) return res;
            	if ( (state.backtracking==0) )
            	{

            	  			field.Name = ((ID21 != null) ? ID21.Text : null);
            	  			field.Line = ID21.Line;
            	  			field.Column = ID21.CharPositionInLine;
            	  		
            	}
            	// D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:723:4: ( LBRACKET size_expr= expression RBRACKET )?
            	int alt57 = 2;
            	int LA57_0 = input.LA(1);

            	if ( (LA57_0 == LBRACKET) )
            	{
            	    alt57 = 1;
            	}
            	switch (alt57) 
            	{
            	    case 1 :
            	        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:723:5: LBRACKET size_expr= expression RBRACKET
            	        {
            	        	Match(input,LBRACKET,FOLLOW_LBRACKET_in_field_declaration2906); if (state.failed) return res;
            	        	PushFollow(FOLLOW_expression_in_field_declaration2910);
            	        	size_expr = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return res;
            	        	Match(input,RBRACKET,FOLLOW_RBRACKET_in_field_declaration2912); if (state.failed) return res;
            	        	if ( (state.backtracking==0) )
            	        	{

            	        	  			field.SizeExpression = size_expr;
            	        	  			field.IsArray = true;
            	        	  		
            	        	}

            	        }
            	        break;

            	}


            }

            if ( (state.backtracking==0) )
            {

              	res =  field;

            }
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return res;
    }
    // $ANTLR end "field_declaration"

    // $ANTLR start "synpred1_GLSL"
    public void synpred1_GLSL_fragment() {
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:265:4: ( constructor_expr )
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:265:5: constructor_expr
        {
        	PushFollow(FOLLOW_constructor_expr_in_synpred1_GLSL1318);
        	constructor_expr();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred1_GLSL"

    // $ANTLR start "synpred2_GLSL"
    public void synpred2_GLSL_fragment() {
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:383:4: ( expression_stmt )
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:383:5: expression_stmt
        {
        	PushFollow(FOLLOW_expression_stmt_in_synpred2_GLSL1764);
        	expression_stmt();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred2_GLSL"

    // $ANTLR start "synpred3_GLSL"
    public void synpred3_GLSL_fragment() {
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:433:4: ( declaration )
        // D:\\ale\\Work\\GLSLCompiler\\GLSLCompiler\\GLSL.g:433:5: declaration
        {
        	PushFollow(FOLLOW_declaration_in_synpred3_GLSL1991);
        	declaration();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred3_GLSL"

    // Delegated rules

   	public bool synpred1_GLSL() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred1_GLSL_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred3_GLSL() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred3_GLSL_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred2_GLSL() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred2_GLSL_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}


   	protected DFA1 dfa1;
   	protected DFA3 dfa3;
   	protected DFA4 dfa4;
   	protected DFA5 dfa5;
   	protected DFA6 dfa6;
   	protected DFA7 dfa7;
   	protected DFA9 dfa9;
   	protected DFA10 dfa10;
   	protected DFA11 dfa11;
   	protected DFA12 dfa12;
   	protected DFA14 dfa14;
   	protected DFA17 dfa17;
   	protected DFA16 dfa16;
   	protected DFA15 dfa15;
   	protected DFA18 dfa18;
   	protected DFA19 dfa19;
   	protected DFA25 dfa25;
   	protected DFA26 dfa26;
   	protected DFA27 dfa27;
   	protected DFA28 dfa28;
   	protected DFA32 dfa32;
   	protected DFA35 dfa35;
   	protected DFA38 dfa38;
   	protected DFA47 dfa47;
	private void InitializeCyclicDFAs()
	{
    	this.dfa1 = new DFA1(this);
    	this.dfa3 = new DFA3(this);
    	this.dfa4 = new DFA4(this);
    	this.dfa5 = new DFA5(this);
    	this.dfa6 = new DFA6(this);
    	this.dfa7 = new DFA7(this);
    	this.dfa9 = new DFA9(this);
    	this.dfa10 = new DFA10(this);
    	this.dfa11 = new DFA11(this);
    	this.dfa12 = new DFA12(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa17 = new DFA17(this);
    	this.dfa16 = new DFA16(this);
    	this.dfa15 = new DFA15(this);
    	this.dfa18 = new DFA18(this);
    	this.dfa19 = new DFA19(this);
    	this.dfa25 = new DFA25(this);
    	this.dfa26 = new DFA26(this);
    	this.dfa27 = new DFA27(this);
    	this.dfa28 = new DFA28(this);
    	this.dfa32 = new DFA32(this);
    	this.dfa35 = new DFA35(this);
    	this.dfa38 = new DFA38(this);
    	this.dfa47 = new DFA47(this);
	    this.dfa17.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA17_SpecialStateTransition);
	    this.dfa26.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA26_SpecialStateTransition);
	    this.dfa32.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA32_SpecialStateTransition);
	}

    const string DFA1_eotS =
        "\x0a\uffff";
    const string DFA1_eofS =
        "\x01\x01\x09\uffff";
    const string DFA1_minS =
        "\x01\x27\x09\uffff";
    const string DFA1_maxS =
        "\x01\x59\x09\uffff";
    const string DFA1_acceptS =
        "\x01\uffff\x01\x02\x01\x01\x07\uffff";
    const string DFA1_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA1_transitionS = {
            "\x01\x02\x09\uffff\x06\x02\x03\uffff\x20\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA1_eot = DFA.UnpackEncodedString(DFA1_eotS);
    static readonly short[] DFA1_eof = DFA.UnpackEncodedString(DFA1_eofS);
    static readonly char[] DFA1_min = DFA.UnpackEncodedStringToUnsignedChars(DFA1_minS);
    static readonly char[] DFA1_max = DFA.UnpackEncodedStringToUnsignedChars(DFA1_maxS);
    static readonly short[] DFA1_accept = DFA.UnpackEncodedString(DFA1_acceptS);
    static readonly short[] DFA1_special = DFA.UnpackEncodedString(DFA1_specialS);
    static readonly short[][] DFA1_transition = DFA.UnpackEncodedStringArray(DFA1_transitionS);

    protected class DFA1 : DFA
    {
        public DFA1(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 1;
            this.eot = DFA1_eot;
            this.eof = DFA1_eof;
            this.min = DFA1_min;
            this.max = DFA1_max;
            this.accept = DFA1_accept;
            this.special = DFA1_special;
            this.transition = DFA1_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 120:5: (decl2= declaration )*"; }
        }

    }

    const string DFA3_eotS =
        "\x0c\uffff";
    const string DFA3_eofS =
        "\x01\x06\x0b\uffff";
    const string DFA3_minS =
        "\x01\x15\x0b\uffff";
    const string DFA3_maxS =
        "\x01\x22\x0b\uffff";
    const string DFA3_acceptS =
        "\x01\uffff\x05\x01\x01\x02\x05\uffff";
    const string DFA3_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA3_transitionS = {
            "\x03\x06\x01\uffff\x01\x06\x01\uffff\x01\x06\x02\uffff\x01"+
            "\x01\x01\x02\x01\x03\x01\x04\x01\x05",
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
            ""
    };

    static readonly short[] DFA3_eot = DFA.UnpackEncodedString(DFA3_eotS);
    static readonly short[] DFA3_eof = DFA.UnpackEncodedString(DFA3_eofS);
    static readonly char[] DFA3_min = DFA.UnpackEncodedStringToUnsignedChars(DFA3_minS);
    static readonly char[] DFA3_max = DFA.UnpackEncodedStringToUnsignedChars(DFA3_maxS);
    static readonly short[] DFA3_accept = DFA.UnpackEncodedString(DFA3_acceptS);
    static readonly short[] DFA3_special = DFA.UnpackEncodedString(DFA3_specialS);
    static readonly short[][] DFA3_transition = DFA.UnpackEncodedStringArray(DFA3_transitionS);

    protected class DFA3 : DFA
    {
        public DFA3(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 3;
            this.eot = DFA3_eot;
            this.eof = DFA3_eof;
            this.min = DFA3_min;
            this.max = DFA3_max;
            this.accept = DFA3_accept;
            this.special = DFA3_special;
            this.transition = DFA3_transition;

        }

        override public string Description
        {
            get { return "134:5: ( ( ASSIGN assign= assign_expr | ADDASSIGN add_assign= assign_expr | SUBASSIGN sub_assign= assign_expr | MULASSIGN mul_assign= assign_expr | DIVASSIGN div_assign= assign_expr ) )?"; }
        }

    }

    const string DFA4_eotS =
        "\x0d\uffff";
    const string DFA4_eofS =
        "\x01\x02\x0c\uffff";
    const string DFA4_minS =
        "\x01\x15\x0c\uffff";
    const string DFA4_maxS =
        "\x01\x26\x0c\uffff";
    const string DFA4_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x0a\uffff";
    const string DFA4_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA4_transitionS = {
            "\x03\x02\x01\uffff\x01\x02\x01\uffff\x01\x02\x02\uffff\x05"+
            "\x02\x03\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA4_eot = DFA.UnpackEncodedString(DFA4_eotS);
    static readonly short[] DFA4_eof = DFA.UnpackEncodedString(DFA4_eofS);
    static readonly char[] DFA4_min = DFA.UnpackEncodedStringToUnsignedChars(DFA4_minS);
    static readonly char[] DFA4_max = DFA.UnpackEncodedStringToUnsignedChars(DFA4_maxS);
    static readonly short[] DFA4_accept = DFA.UnpackEncodedString(DFA4_acceptS);
    static readonly short[] DFA4_special = DFA.UnpackEncodedString(DFA4_specialS);
    static readonly short[][] DFA4_transition = DFA.UnpackEncodedStringArray(DFA4_transitionS);

    protected class DFA4 : DFA
    {
        public DFA4(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 4;
            this.eot = DFA4_eot;
            this.eof = DFA4_eof;
            this.min = DFA4_min;
            this.max = DFA4_max;
            this.accept = DFA4_accept;
            this.special = DFA4_special;
            this.transition = DFA4_transition;

        }

        override public string Description
        {
            get { return "154:4: ( QUESTION ontrue= expression COLON onfalse= assign_expr )?"; }
        }

    }

    const string DFA5_eotS =
        "\x0e\uffff";
    const string DFA5_eofS =
        "\x01\x01\x0d\uffff";
    const string DFA5_minS =
        "\x01\x11\x0d\uffff";
    const string DFA5_maxS =
        "\x01\x26\x0d\uffff";
    const string DFA5_acceptS =
        "\x01\uffff\x01\x02\x0b\uffff\x01\x01";
    const string DFA5_specialS =
        "\x0e\uffff}>";
    static readonly string[] DFA5_transitionS = {
            "\x01\x0d\x03\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x02\uffff\x05\x01\x03\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA5_eot = DFA.UnpackEncodedString(DFA5_eotS);
    static readonly short[] DFA5_eof = DFA.UnpackEncodedString(DFA5_eofS);
    static readonly char[] DFA5_min = DFA.UnpackEncodedStringToUnsignedChars(DFA5_minS);
    static readonly char[] DFA5_max = DFA.UnpackEncodedStringToUnsignedChars(DFA5_maxS);
    static readonly short[] DFA5_accept = DFA.UnpackEncodedString(DFA5_acceptS);
    static readonly short[] DFA5_special = DFA.UnpackEncodedString(DFA5_specialS);
    static readonly short[][] DFA5_transition = DFA.UnpackEncodedStringArray(DFA5_transitionS);

    protected class DFA5 : DFA
    {
        public DFA5(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 5;
            this.eot = DFA5_eot;
            this.eof = DFA5_eof;
            this.min = DFA5_min;
            this.max = DFA5_max;
            this.accept = DFA5_accept;
            this.special = DFA5_special;
            this.transition = DFA5_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 162:5: ( OR xor2= xor_expr )*"; }
        }

    }

    const string DFA6_eotS =
        "\x0f\uffff";
    const string DFA6_eofS =
        "\x01\x01\x0e\uffff";
    const string DFA6_minS =
        "\x01\x11\x0e\uffff";
    const string DFA6_maxS =
        "\x01\x26\x0e\uffff";
    const string DFA6_acceptS =
        "\x01\uffff\x01\x02\x0c\uffff\x01\x01";
    const string DFA6_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x01\x01\x01\uffff\x01\x0e\x01\uffff\x03\x01\x01\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x02\uffff\x05\x01\x03\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA6_eot = DFA.UnpackEncodedString(DFA6_eotS);
    static readonly short[] DFA6_eof = DFA.UnpackEncodedString(DFA6_eofS);
    static readonly char[] DFA6_min = DFA.UnpackEncodedStringToUnsignedChars(DFA6_minS);
    static readonly char[] DFA6_max = DFA.UnpackEncodedStringToUnsignedChars(DFA6_maxS);
    static readonly short[] DFA6_accept = DFA.UnpackEncodedString(DFA6_acceptS);
    static readonly short[] DFA6_special = DFA.UnpackEncodedString(DFA6_specialS);
    static readonly short[][] DFA6_transition = DFA.UnpackEncodedStringArray(DFA6_transitionS);

    protected class DFA6 : DFA
    {
        public DFA6(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 6;
            this.eot = DFA6_eot;
            this.eof = DFA6_eof;
            this.min = DFA6_min;
            this.max = DFA6_max;
            this.accept = DFA6_accept;
            this.special = DFA6_special;
            this.transition = DFA6_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 170:4: ( XOR and2= and_expr )*"; }
        }

    }

    const string DFA7_eotS =
        "\x10\uffff";
    const string DFA7_eofS =
        "\x01\x01\x0f\uffff";
    const string DFA7_minS =
        "\x01\x11\x0f\uffff";
    const string DFA7_maxS =
        "\x01\x26\x0f\uffff";
    const string DFA7_acceptS =
        "\x01\uffff\x01\x02\x0d\uffff\x01\x01";
    const string DFA7_specialS =
        "\x10\uffff}>";
    static readonly string[] DFA7_transitionS = {
            "\x01\x01\x01\x0f\x01\x01\x01\uffff\x03\x01\x01\uffff\x01\x01"+
            "\x01\uffff\x01\x01\x02\uffff\x05\x01\x03\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA7_eot = DFA.UnpackEncodedString(DFA7_eotS);
    static readonly short[] DFA7_eof = DFA.UnpackEncodedString(DFA7_eofS);
    static readonly char[] DFA7_min = DFA.UnpackEncodedStringToUnsignedChars(DFA7_minS);
    static readonly char[] DFA7_max = DFA.UnpackEncodedStringToUnsignedChars(DFA7_maxS);
    static readonly short[] DFA7_accept = DFA.UnpackEncodedString(DFA7_acceptS);
    static readonly short[] DFA7_special = DFA.UnpackEncodedString(DFA7_specialS);
    static readonly short[][] DFA7_transition = DFA.UnpackEncodedStringArray(DFA7_transitionS);

    protected class DFA7 : DFA
    {
        public DFA7(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 7;
            this.eot = DFA7_eot;
            this.eof = DFA7_eof;
            this.min = DFA7_min;
            this.max = DFA7_max;
            this.accept = DFA7_accept;
            this.special = DFA7_special;
            this.transition = DFA7_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 178:4: ( AND eq2= eq_expr )*"; }
        }

    }

    const string DFA9_eotS =
        "\x12\uffff";
    const string DFA9_eofS =
        "\x01\x03\x11\uffff";
    const string DFA9_minS =
        "\x01\x04\x11\uffff";
    const string DFA9_maxS =
        "\x01\x26\x11\uffff";
    const string DFA9_acceptS =
        "\x01\uffff\x01\x01\x01\uffff\x01\x02\x0e\uffff";
    const string DFA9_specialS =
        "\x12\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x02\x01\x0b\uffff\x03\x03\x01\uffff\x03\x03\x01\uffff\x01"+
            "\x03\x01\uffff\x01\x03\x02\uffff\x05\x03\x03\uffff\x01\x03",
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
            ""
    };

    static readonly short[] DFA9_eot = DFA.UnpackEncodedString(DFA9_eotS);
    static readonly short[] DFA9_eof = DFA.UnpackEncodedString(DFA9_eofS);
    static readonly char[] DFA9_min = DFA.UnpackEncodedStringToUnsignedChars(DFA9_minS);
    static readonly char[] DFA9_max = DFA.UnpackEncodedStringToUnsignedChars(DFA9_maxS);
    static readonly short[] DFA9_accept = DFA.UnpackEncodedString(DFA9_acceptS);
    static readonly short[] DFA9_special = DFA.UnpackEncodedString(DFA9_specialS);
    static readonly short[][] DFA9_transition = DFA.UnpackEncodedStringArray(DFA9_transitionS);

    protected class DFA9 : DFA
    {
        public DFA9(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 9;
            this.eot = DFA9_eot;
            this.eof = DFA9_eof;
            this.min = DFA9_min;
            this.max = DFA9_max;
            this.accept = DFA9_accept;
            this.special = DFA9_special;
            this.transition = DFA9_transition;

        }

        override public string Description
        {
            get { return "186:5: ( ( EQUAL rel21= rel_expr | NEQUAL rel22= rel_expr ) )?"; }
        }

    }

    const string DFA10_eotS =
        "\x16\uffff";
    const string DFA10_eofS =
        "\x01\x05\x15\uffff";
    const string DFA10_minS =
        "\x01\x04\x15\uffff";
    const string DFA10_maxS =
        "\x01\x26\x15\uffff";
    const string DFA10_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x10\uffff";
    const string DFA10_specialS =
        "\x16\uffff}>";
    static readonly string[] DFA10_transitionS = {
            "\x02\x05\x01\x01\x01\x02\x01\x03\x01\x04\x07\uffff\x03\x05"+
            "\x01\uffff\x03\x05\x01\uffff\x01\x05\x01\uffff\x01\x05\x02\uffff"+
            "\x05\x05\x03\uffff\x01\x05",
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
            ""
    };

    static readonly short[] DFA10_eot = DFA.UnpackEncodedString(DFA10_eotS);
    static readonly short[] DFA10_eof = DFA.UnpackEncodedString(DFA10_eofS);
    static readonly char[] DFA10_min = DFA.UnpackEncodedStringToUnsignedChars(DFA10_minS);
    static readonly char[] DFA10_max = DFA.UnpackEncodedStringToUnsignedChars(DFA10_maxS);
    static readonly short[] DFA10_accept = DFA.UnpackEncodedString(DFA10_acceptS);
    static readonly short[] DFA10_special = DFA.UnpackEncodedString(DFA10_specialS);
    static readonly short[][] DFA10_transition = DFA.UnpackEncodedStringArray(DFA10_transitionS);

    protected class DFA10 : DFA
    {
        public DFA10(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 10;
            this.eot = DFA10_eot;
            this.eof = DFA10_eof;
            this.min = DFA10_min;
            this.max = DFA10_max;
            this.accept = DFA10_accept;
            this.special = DFA10_special;
            this.transition = DFA10_transition;

        }

        override public string Description
        {
            get { return "196:5: ( LESS l_add= add_expr | LEQUAL le_add= add_expr | GREATER g_add= add_expr | GEQUAL ge_add= add_expr )?"; }
        }

    }

    const string DFA11_eotS =
        "\x18\uffff";
    const string DFA11_eofS =
        "\x01\x01\x17\uffff";
    const string DFA11_minS =
        "\x01\x04\x17\uffff";
    const string DFA11_maxS =
        "\x01\x26\x17\uffff";
    const string DFA11_acceptS =
        "\x01\uffff\x01\x03\x14\uffff\x01\x01\x01\x02";
    const string DFA11_specialS =
        "\x18\uffff}>";
    static readonly string[] DFA11_transitionS = {
            "\x06\x01\x03\uffff\x01\x16\x01\uffff\x01\x17\x01\uffff\x03"+
            "\x01\x01\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff\x01\x01\x02"+
            "\uffff\x05\x01\x03\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA11_eot = DFA.UnpackEncodedString(DFA11_eotS);
    static readonly short[] DFA11_eof = DFA.UnpackEncodedString(DFA11_eofS);
    static readonly char[] DFA11_min = DFA.UnpackEncodedStringToUnsignedChars(DFA11_minS);
    static readonly char[] DFA11_max = DFA.UnpackEncodedStringToUnsignedChars(DFA11_maxS);
    static readonly short[] DFA11_accept = DFA.UnpackEncodedString(DFA11_acceptS);
    static readonly short[] DFA11_special = DFA.UnpackEncodedString(DFA11_specialS);
    static readonly short[][] DFA11_transition = DFA.UnpackEncodedStringArray(DFA11_transitionS);

    protected class DFA11 : DFA
    {
        public DFA11(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 11;
            this.eot = DFA11_eot;
            this.eof = DFA11_eof;
            this.min = DFA11_min;
            this.max = DFA11_max;
            this.accept = DFA11_accept;
            this.special = DFA11_special;
            this.transition = DFA11_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 210:5: ( ADD add_mul= mul_expr | SUB sub_mul= mul_expr )*"; }
        }

    }

    const string DFA12_eotS =
        "\x1a\uffff";
    const string DFA12_eofS =
        "\x01\x01\x19\uffff";
    const string DFA12_minS =
        "\x01\x04\x19\uffff";
    const string DFA12_maxS =
        "\x01\x26\x19\uffff";
    const string DFA12_acceptS =
        "\x01\uffff\x01\x03\x16\uffff\x01\x01\x01\x02";
    const string DFA12_specialS =
        "\x1a\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x06\x01\x01\uffff\x01\x18\x01\x19\x01\x01\x01\uffff\x01\x01"+
            "\x01\uffff\x03\x01\x01\uffff\x03\x01\x01\uffff\x01\x01\x01\uffff"+
            "\x01\x01\x02\uffff\x05\x01\x03\uffff\x01\x01",
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
            ""
    };

    static readonly short[] DFA12_eot = DFA.UnpackEncodedString(DFA12_eotS);
    static readonly short[] DFA12_eof = DFA.UnpackEncodedString(DFA12_eofS);
    static readonly char[] DFA12_min = DFA.UnpackEncodedStringToUnsignedChars(DFA12_minS);
    static readonly char[] DFA12_max = DFA.UnpackEncodedStringToUnsignedChars(DFA12_maxS);
    static readonly short[] DFA12_accept = DFA.UnpackEncodedString(DFA12_acceptS);
    static readonly short[] DFA12_special = DFA.UnpackEncodedString(DFA12_specialS);
    static readonly short[][] DFA12_transition = DFA.UnpackEncodedStringArray(DFA12_transitionS);

    protected class DFA12 : DFA
    {
        public DFA12(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 12;
            this.eot = DFA12_eot;
            this.eof = DFA12_eof;
            this.min = DFA12_min;
            this.max = DFA12_max;
            this.accept = DFA12_accept;
            this.special = DFA12_special;
            this.transition = DFA12_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 220:5: ( MUL mul_unary= unary_expr | DIV div_unary= unary_expr )*"; }
        }

    }

    const string DFA14_eotS =
        "\x1c\uffff";
    const string DFA14_eofS =
        "\x01\x03\x1b\uffff";
    const string DFA14_minS =
        "\x01\x04\x1b\uffff";
    const string DFA14_maxS =
        "\x01\x26\x1b\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x18\uffff";
    const string DFA14_specialS =
        "\x1c\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x06\x03\x01\uffff\x03\x03\x01\x01\x01\x03\x01\x02\x03\x03"+
            "\x01\uffff\x03\x03\x01\uffff\x01\x03\x01\uffff\x01\x03\x02\uffff"+
            "\x05\x03\x03\uffff\x01\x03",
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
            "",
            "",
            ""
    };

    static readonly short[] DFA14_eot = DFA.UnpackEncodedString(DFA14_eotS);
    static readonly short[] DFA14_eof = DFA.UnpackEncodedString(DFA14_eofS);
    static readonly char[] DFA14_min = DFA.UnpackEncodedStringToUnsignedChars(DFA14_minS);
    static readonly char[] DFA14_max = DFA.UnpackEncodedStringToUnsignedChars(DFA14_maxS);
    static readonly short[] DFA14_accept = DFA.UnpackEncodedString(DFA14_acceptS);
    static readonly short[] DFA14_special = DFA.UnpackEncodedString(DFA14_specialS);
    static readonly short[][] DFA14_transition = DFA.UnpackEncodedStringArray(DFA14_transitionS);

    protected class DFA14 : DFA
    {
        public DFA14(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 14;
            this.eot = DFA14_eot;
            this.eof = DFA14_eof;
            this.min = DFA14_min;
            this.max = DFA14_max;
            this.accept = DFA14_accept;
            this.special = DFA14_special;
            this.transition = DFA14_transition;

        }

        override public string Description
        {
            get { return "257:5: ( INCREMENT | DECREMENT )?"; }
        }

    }

    const string DFA17_eotS =
        "\x25\uffff";
    const string DFA17_eofS =
        "\x02\uffff\x01\x03\x22\uffff";
    const string DFA17_minS =
        "\x01\x1a\x01\uffff\x01\x04\x04\uffff\x01\x00\x1d\uffff";
    const string DFA17_maxS =
        "\x01\x5c\x01\uffff\x01\x26\x04\uffff\x01\x00\x1d\uffff";
    const string DFA17_acceptS =
        "\x01\uffff\x01\x01\x01\uffff\x01\x02\x21\uffff";
    const string DFA17_specialS =
        "\x01\x00\x06\uffff\x01\x01\x1d\uffff}>";
    static readonly string[] DFA17_transitionS = {
            "\x01\x03\x20\uffff\x18\x01\x06\uffff\x01\x02\x03\x03",
            "",
            "\x06\x03\x01\uffff\x0d\x03\x01\x07\x03\x03\x02\uffff\x05\x03"+
            "\x03\uffff\x01\x03",
            "",
            "",
            "",
            "",
            "\x01\uffff",
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
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA17_eot = DFA.UnpackEncodedString(DFA17_eotS);
    static readonly short[] DFA17_eof = DFA.UnpackEncodedString(DFA17_eofS);
    static readonly char[] DFA17_min = DFA.UnpackEncodedStringToUnsignedChars(DFA17_minS);
    static readonly char[] DFA17_max = DFA.UnpackEncodedStringToUnsignedChars(DFA17_maxS);
    static readonly short[] DFA17_accept = DFA.UnpackEncodedString(DFA17_acceptS);
    static readonly short[] DFA17_special = DFA.UnpackEncodedString(DFA17_specialS);
    static readonly short[][] DFA17_transition = DFA.UnpackEncodedStringArray(DFA17_transitionS);

    protected class DFA17 : DFA
    {
        public DFA17(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 17;
            this.eot = DFA17_eot;
            this.eof = DFA17_eof;
            this.min = DFA17_min;
            this.max = DFA17_max;
            this.accept = DFA17_accept;
            this.special = DFA17_special;
            this.transition = DFA17_transition;

        }

        override public string Description
        {
            get { return "264:1: lvalue returns [ExpressionAST res] : ( ( constructor_expr )=>constructor= constructor_expr | primary= primary_expr ( ( DOT ( ID | funcall= funccall_expr ) ) | ( LBRACKET expr= expression RBRACKET ) )* );"; }
        }

    }


    protected internal int DFA17_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA17_0 = input.LA(1);

                   	 
                   	int index17_0 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( ((LA17_0 >= INTEGER && LA17_0 <= MAT4X4)) && (synpred1_GLSL()) ) { s = 1; }

                   	else if ( (LA17_0 == ID) ) { s = 2; }

                   	else if ( (LA17_0 == LPAREN || (LA17_0 >= INT_CONSTANT && LA17_0 <= BOOL_CONSTANT)) ) { s = 3; }

                   	 
                   	input.Seek(index17_0);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA17_7 = input.LA(1);

                   	 
                   	int index17_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred1_GLSL()) ) { s = 1; }

                   	else if ( (true) ) { s = 3; }

                   	 
                   	input.Seek(index17_7);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae17 =
            new NoViableAltException(dfa.Description, 17, _s, input);
        dfa.Error(nvae17);
        throw nvae17;
    }
    const string DFA16_eotS =
        "\x1e\uffff";
    const string DFA16_eofS =
        "\x01\x01\x1d\uffff";
    const string DFA16_minS =
        "\x01\x04\x1d\uffff";
    const string DFA16_maxS =
        "\x01\x26\x1d\uffff";
    const string DFA16_acceptS =
        "\x01\uffff\x01\x03\x1a\uffff\x01\x01\x01\x02";
    const string DFA16_specialS =
        "\x1e\uffff}>";
    static readonly string[] DFA16_transitionS = {
            "\x06\x01\x01\uffff\x09\x01\x01\x1c\x03\x01\x01\x1d\x01\x01"+
            "\x01\uffff\x01\x01\x02\uffff\x05\x01\x03\uffff\x01\x01",
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
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA16_eot = DFA.UnpackEncodedString(DFA16_eotS);
    static readonly short[] DFA16_eof = DFA.UnpackEncodedString(DFA16_eofS);
    static readonly char[] DFA16_min = DFA.UnpackEncodedStringToUnsignedChars(DFA16_minS);
    static readonly char[] DFA16_max = DFA.UnpackEncodedStringToUnsignedChars(DFA16_maxS);
    static readonly short[] DFA16_accept = DFA.UnpackEncodedString(DFA16_acceptS);
    static readonly short[] DFA16_special = DFA.UnpackEncodedString(DFA16_specialS);
    static readonly short[][] DFA16_transition = DFA.UnpackEncodedStringArray(DFA16_transitionS);

    protected class DFA16 : DFA
    {
        public DFA16(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 16;
            this.eot = DFA16_eot;
            this.eof = DFA16_eof;
            this.min = DFA16_min;
            this.max = DFA16_max;
            this.accept = DFA16_accept;
            this.special = DFA16_special;
            this.transition = DFA16_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 270:4: ( ( DOT ( ID | funcall= funccall_expr ) ) | ( LBRACKET expr= expression RBRACKET ) )*"; }
        }

    }

    const string DFA15_eotS =
        "\x20\uffff";
    const string DFA15_eofS =
        "\x01\uffff\x01\x03\x1e\uffff";
    const string DFA15_minS =
        "\x01\x59\x01\x04\x1e\uffff";
    const string DFA15_maxS =
        "\x01\x59\x01\x26\x1e\uffff";
    const string DFA15_acceptS =
        "\x02\uffff\x01\x02\x01\x01\x1c\uffff";
    const string DFA15_specialS =
        "\x20\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x01\x01",
            "\x06\x03\x01\uffff\x0f\x03\x01\x02\x01\x03\x02\uffff\x05\x03"+
            "\x03\uffff\x01\x03",
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
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA15_eot = DFA.UnpackEncodedString(DFA15_eotS);
    static readonly short[] DFA15_eof = DFA.UnpackEncodedString(DFA15_eofS);
    static readonly char[] DFA15_min = DFA.UnpackEncodedStringToUnsignedChars(DFA15_minS);
    static readonly char[] DFA15_max = DFA.UnpackEncodedStringToUnsignedChars(DFA15_maxS);
    static readonly short[] DFA15_accept = DFA.UnpackEncodedString(DFA15_acceptS);
    static readonly short[] DFA15_special = DFA.UnpackEncodedString(DFA15_specialS);
    static readonly short[][] DFA15_transition = DFA.UnpackEncodedStringArray(DFA15_transitionS);

    protected class DFA15 : DFA
    {
        public DFA15(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 15;
            this.eot = DFA15_eot;
            this.eof = DFA15_eof;
            this.min = DFA15_min;
            this.max = DFA15_max;
            this.accept = DFA15_accept;
            this.special = DFA15_special;
            this.transition = DFA15_transition;

        }

        override public string Description
        {
            get { return "270:10: ( ID | funcall= funccall_expr )"; }
        }

    }

    const string DFA18_eotS =
        "\x24\uffff";
    const string DFA18_eofS =
        "\x01\uffff\x01\x06\x22\uffff";
    const string DFA18_minS =
        "\x01\x1a\x01\x04\x22\uffff";
    const string DFA18_maxS =
        "\x01\x5c\x01\x26\x22\uffff";
    const string DFA18_acceptS =
        "\x02\uffff\x01\x02\x01\x03\x01\x04\x01\x05\x01\x01\x1c\uffff\x01"+
        "\x06";
    const string DFA18_specialS =
        "\x24\uffff}>";
    static readonly string[] DFA18_transitionS = {
            "\x01\x05\x3e\uffff\x01\x01\x01\x02\x01\x03\x01\x04",
            "\x06\x06\x01\uffff\x0f\x06\x01\x23\x01\x06\x02\uffff\x05\x06"+
            "\x03\uffff\x01\x06",
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
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA18_eot = DFA.UnpackEncodedString(DFA18_eotS);
    static readonly short[] DFA18_eof = DFA.UnpackEncodedString(DFA18_eofS);
    static readonly char[] DFA18_min = DFA.UnpackEncodedStringToUnsignedChars(DFA18_minS);
    static readonly char[] DFA18_max = DFA.UnpackEncodedStringToUnsignedChars(DFA18_maxS);
    static readonly short[] DFA18_accept = DFA.UnpackEncodedString(DFA18_acceptS);
    static readonly short[] DFA18_special = DFA.UnpackEncodedString(DFA18_specialS);
    static readonly short[][] DFA18_transition = DFA.UnpackEncodedStringArray(DFA18_transitionS);

    protected class DFA18 : DFA
    {
        public DFA18(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 18;
            this.eot = DFA18_eot;
            this.eof = DFA18_eof;
            this.min = DFA18_min;
            this.max = DFA18_max;
            this.accept = DFA18_accept;
            this.special = DFA18_special;
            this.transition = DFA18_transition;

        }

        override public string Description
        {
            get { return "281:1: primary_expr returns [ExpressionAST res] : ( ID | INT_CONSTANT | FLOAT_CONSTANT | BOOL_CONSTANT | LPAREN expr= expr_list RPAREN | funccall= funccall_expr );"; }
        }

    }

    const string DFA19_eotS =
        "\x0b\uffff";
    const string DFA19_eofS =
        "\x0b\uffff";
    const string DFA19_minS =
        "\x01\x1a\x01\x0a\x09\uffff";
    const string DFA19_maxS =
        "\x01\x1a\x01\x5c\x09\uffff";
    const string DFA19_acceptS =
        "\x02\uffff\x01\x01\x01\uffff\x01\x02\x06\uffff";
    const string DFA19_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x01\x01",
            "\x01\x04\x02\uffff\x04\x04\x09\uffff\x01\x04\x01\x02\x0b\uffff"+
            "\x01\x02\x13\uffff\x18\x04\x06\uffff\x04\x04",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA19_eot = DFA.UnpackEncodedString(DFA19_eotS);
    static readonly short[] DFA19_eof = DFA.UnpackEncodedString(DFA19_eofS);
    static readonly char[] DFA19_min = DFA.UnpackEncodedStringToUnsignedChars(DFA19_minS);
    static readonly char[] DFA19_max = DFA.UnpackEncodedStringToUnsignedChars(DFA19_maxS);
    static readonly short[] DFA19_accept = DFA.UnpackEncodedString(DFA19_acceptS);
    static readonly short[] DFA19_special = DFA.UnpackEncodedString(DFA19_specialS);
    static readonly short[][] DFA19_transition = DFA.UnpackEncodedStringArray(DFA19_transitionS);

    protected class DFA19 : DFA
    {
        public DFA19(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 19;
            this.eot = DFA19_eot;
            this.eof = DFA19_eof;
            this.min = DFA19_min;
            this.max = DFA19_max;
            this.accept = DFA19_accept;
            this.special = DFA19_special;
            this.transition = DFA19_transition;

        }

        override public string Description
        {
            get { return "303:7: ( args_no_parameters | args= args_with_parameters )"; }
        }

    }

    const string DFA25_eotS =
        "\x19\uffff";
    const string DFA25_eofS =
        "\x19\uffff";
    const string DFA25_minS =
        "\x01\x0a\x18\uffff";
    const string DFA25_maxS =
        "\x01\x5c\x18\uffff";
    const string DFA25_acceptS =
        "\x01\uffff\x01\x01\x16\uffff\x01\x02";
    const string DFA25_specialS =
        "\x19\uffff}>";
    static readonly string[] DFA25_transitionS = {
            "\x01\x01\x02\uffff\x04\x01\x09\uffff\x01\x01\x01\uffff\x01"+
            "\x18\x0a\uffff\x02\x01\x01\uffff\x0d\x01\x03\uffff\x23\x01",
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
            ""
    };

    static readonly short[] DFA25_eot = DFA.UnpackEncodedString(DFA25_eotS);
    static readonly short[] DFA25_eof = DFA.UnpackEncodedString(DFA25_eofS);
    static readonly char[] DFA25_min = DFA.UnpackEncodedStringToUnsignedChars(DFA25_minS);
    static readonly char[] DFA25_max = DFA.UnpackEncodedStringToUnsignedChars(DFA25_maxS);
    static readonly short[] DFA25_accept = DFA.UnpackEncodedString(DFA25_acceptS);
    static readonly short[] DFA25_special = DFA.UnpackEncodedString(DFA25_specialS);
    static readonly short[][] DFA25_transition = DFA.UnpackEncodedStringArray(DFA25_transitionS);

    protected class DFA25 : DFA
    {
        public DFA25(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 25;
            this.eot = DFA25_eot;
            this.eof = DFA25_eof;
            this.min = DFA25_min;
            this.max = DFA25_max;
            this.accept = DFA25_accept;
            this.special = DFA25_special;
            this.transition = DFA25_transition;

        }

        override public string Description
        {
            get { return "373:1: statement returns [StatementAST res] : (simple= simple_stmt | compound= compound_stmt );"; }
        }

    }

    const string DFA26_eotS =
        "\x36\uffff";
    const string DFA26_eofS =
        "\x36\uffff";
    const string DFA26_minS =
        "\x01\x0a\x01\uffff\x01\x16\x01\x04\x14\uffff\x01\x00\x03\uffff"+
        "\x01\x00\x01\uffff\x01\x00\x17\uffff";
    const string DFA26_maxS =
        "\x01\x5c\x01\uffff\x02\x59\x14\uffff\x01\x00\x03\uffff\x01\x00"+
        "\x01\uffff\x01\x00\x17\uffff";
    const string DFA26_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x04\x01\x01\x02\x07\uffff\x01\x03"+
        "\x01\x04\x02\uffff\x01\x05\x06\uffff\x01\x01\x03\uffff\x17\x01";
    const string DFA26_specialS =
        "\x01\x00\x01\uffff\x01\x01\x01\x02\x14\uffff\x01\x03\x03\uffff"+
        "\x01\x04\x01\uffff\x01\x05\x17\uffff}>";
    static readonly string[] DFA26_transitionS = {
            "\x01\x01\x02\uffff\x04\x01\x09\uffff\x01\x07\x0c\uffff\x01"+
            "\x08\x01\x10\x01\uffff\x03\x11\x04\x14\x06\x08\x03\uffff\x01"+
            "\x08\x18\x02\x06\x08\x01\x03\x01\x04\x01\x05\x01\x06",
            "",
            "\x01\x08\x01\uffff\x01\x18\x01\uffff\x01\x1b\x3e\uffff\x01"+
            "\x08",
            "\x01\x2b\x01\x2c\x01\x27\x01\x28\x01\x29\x01\x2a\x01\uffff"+
            "\x01\x23\x01\x24\x01\x25\x01\x21\x01\x26\x01\x22\x01\x2f\x01"+
            "\x2d\x01\x2e\x01\x20\x01\uffff\x01\x1e\x01\uffff\x01\x1c\x01"+
            "\uffff\x01\x1f\x03\uffff\x01\x31\x01\x32\x01\x33\x01\x34\x01"+
            "\x35\x03\uffff\x01\x30\x32\uffff\x01\x08",
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
            "\x01\uffff",
            "",
            "",
            "",
            "\x01\uffff",
            "",
            "\x01\uffff",
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
            ""
    };

    static readonly short[] DFA26_eot = DFA.UnpackEncodedString(DFA26_eotS);
    static readonly short[] DFA26_eof = DFA.UnpackEncodedString(DFA26_eofS);
    static readonly char[] DFA26_min = DFA.UnpackEncodedStringToUnsignedChars(DFA26_minS);
    static readonly char[] DFA26_max = DFA.UnpackEncodedStringToUnsignedChars(DFA26_maxS);
    static readonly short[] DFA26_accept = DFA.UnpackEncodedString(DFA26_acceptS);
    static readonly short[] DFA26_special = DFA.UnpackEncodedString(DFA26_specialS);
    static readonly short[][] DFA26_transition = DFA.UnpackEncodedStringArray(DFA26_transitionS);

    protected class DFA26 : DFA
    {
        public DFA26(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 26;
            this.eot = DFA26_eot;
            this.eof = DFA26_eof;
            this.min = DFA26_min;
            this.max = DFA26_max;
            this.accept = DFA26_accept;
            this.special = DFA26_special;
            this.transition = DFA26_transition;

        }

        override public string Description
        {
            get { return "382:1: simple_stmt returns [StatementAST res] : ( ( expression_stmt )=>expr_stmt= expression_stmt SEMICOLON | decl_stmt= declaration_stmt | sel_stmt= selection_stmt | iter_stmt= iteration_stmt | jump= jump_stmt SEMICOLON );"; }
        }

    }


    protected internal int DFA26_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA26_0 = input.LA(1);

                   	 
                   	int index26_0 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA26_0 == NOT || (LA26_0 >= ADD && LA26_0 <= DECREMENT)) && (synpred2_GLSL()) ) { s = 1; }

                   	else if ( ((LA26_0 >= INTEGER && LA26_0 <= MAT4X4)) ) { s = 2; }

                   	else if ( (LA26_0 == ID) ) { s = 3; }

                   	else if ( (LA26_0 == INT_CONSTANT) && (synpred2_GLSL()) ) { s = 4; }

                   	else if ( (LA26_0 == FLOAT_CONSTANT) && (synpred2_GLSL()) ) { s = 5; }

                   	else if ( (LA26_0 == BOOL_CONSTANT) && (synpred2_GLSL()) ) { s = 6; }

                   	else if ( (LA26_0 == LPAREN) && (synpred2_GLSL()) ) { s = 7; }

                   	else if ( (LA26_0 == VOID || (LA26_0 >= CONST && LA26_0 <= INVARIANT) || LA26_0 == STRUCT || (LA26_0 >= SAMPLER1D && LA26_0 <= SAMPLER2DSHADOW)) ) { s = 8; }

                   	else if ( (LA26_0 == IF) ) { s = 16; }

                   	else if ( ((LA26_0 >= WHILE && LA26_0 <= FOR)) ) { s = 17; }

                   	else if ( ((LA26_0 >= BREAK && LA26_0 <= DISCARD)) ) { s = 20; }

                   	 
                   	input.Seek(index26_0);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA26_2 = input.LA(1);

                   	 
                   	int index26_2 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA26_2 == LBRACKET) ) { s = 24; }

                   	else if ( (LA26_2 == SEMICOLON || LA26_2 == ID) ) { s = 8; }

                   	else if ( (LA26_2 == LPAREN) && (synpred2_GLSL()) ) { s = 27; }

                   	 
                   	input.Seek(index26_2);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA26_3 = input.LA(1);

                   	 
                   	int index26_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA26_3 == LBRACKET) ) { s = 28; }

                   	else if ( (LA26_3 == ID) ) { s = 8; }

                   	else if ( (LA26_3 == SEMICOLON) ) { s = 30; }

                   	else if ( (LA26_3 == LPAREN) && (synpred2_GLSL()) ) { s = 31; }

                   	else if ( (LA26_3 == DOT) && (synpred2_GLSL()) ) { s = 32; }

                   	else if ( (LA26_3 == INCREMENT) && (synpred2_GLSL()) ) { s = 33; }

                   	else if ( (LA26_3 == DECREMENT) && (synpred2_GLSL()) ) { s = 34; }

                   	else if ( (LA26_3 == MUL) && (synpred2_GLSL()) ) { s = 35; }

                   	else if ( (LA26_3 == DIV) && (synpred2_GLSL()) ) { s = 36; }

                   	else if ( (LA26_3 == ADD) && (synpred2_GLSL()) ) { s = 37; }

                   	else if ( (LA26_3 == SUB) && (synpred2_GLSL()) ) { s = 38; }

                   	else if ( (LA26_3 == LESS) && (synpred2_GLSL()) ) { s = 39; }

                   	else if ( (LA26_3 == LEQUAL) && (synpred2_GLSL()) ) { s = 40; }

                   	else if ( (LA26_3 == GREATER) && (synpred2_GLSL()) ) { s = 41; }

                   	else if ( (LA26_3 == GEQUAL) && (synpred2_GLSL()) ) { s = 42; }

                   	else if ( (LA26_3 == EQUAL) && (synpred2_GLSL()) ) { s = 43; }

                   	else if ( (LA26_3 == NEQUAL) && (synpred2_GLSL()) ) { s = 44; }

                   	else if ( (LA26_3 == AND) && (synpred2_GLSL()) ) { s = 45; }

                   	else if ( (LA26_3 == XOR) && (synpred2_GLSL()) ) { s = 46; }

                   	else if ( (LA26_3 == OR) && (synpred2_GLSL()) ) { s = 47; }

                   	else if ( (LA26_3 == QUESTION) && (synpred2_GLSL()) ) { s = 48; }

                   	else if ( (LA26_3 == ASSIGN) && (synpred2_GLSL()) ) { s = 49; }

                   	else if ( (LA26_3 == ADDASSIGN) && (synpred2_GLSL()) ) { s = 50; }

                   	else if ( (LA26_3 == SUBASSIGN) && (synpred2_GLSL()) ) { s = 51; }

                   	else if ( (LA26_3 == MULASSIGN) && (synpred2_GLSL()) ) { s = 52; }

                   	else if ( (LA26_3 == DIVASSIGN) && (synpred2_GLSL()) ) { s = 53; }

                   	 
                   	input.Seek(index26_3);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA26_24 = input.LA(1);

                   	 
                   	int index26_24 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred2_GLSL()) ) { s = 53; }

                   	else if ( (true) ) { s = 8; }

                   	 
                   	input.Seek(index26_24);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA26_28 = input.LA(1);

                   	 
                   	int index26_28 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred2_GLSL()) ) { s = 53; }

                   	else if ( (true) ) { s = 8; }

                   	 
                   	input.Seek(index26_28);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA26_30 = input.LA(1);

                   	 
                   	int index26_30 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred2_GLSL()) ) { s = 53; }

                   	else if ( (true) ) { s = 8; }

                   	 
                   	input.Seek(index26_30);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae26 =
            new NoViableAltException(dfa.Description, 26, _s, input);
        dfa.Error(nvae26);
        throw nvae26;
    }
    const string DFA27_eotS =
        "\x1b\uffff";
    const string DFA27_eofS =
        "\x1b\uffff";
    const string DFA27_minS =
        "\x01\x0a\x1a\uffff";
    const string DFA27_maxS =
        "\x01\x5c\x1a\uffff";
    const string DFA27_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x18\uffff";
    const string DFA27_specialS =
        "\x1b\uffff}>";
    static readonly string[] DFA27_transitionS = {
            "\x01\x02\x02\uffff\x04\x02\x09\uffff\x01\x02\x01\uffff\x02"+
            "\x02\x09\uffff\x02\x02\x01\x01\x0d\x02\x03\uffff\x23\x02",
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
            "",
            ""
    };

    static readonly short[] DFA27_eot = DFA.UnpackEncodedString(DFA27_eotS);
    static readonly short[] DFA27_eof = DFA.UnpackEncodedString(DFA27_eofS);
    static readonly char[] DFA27_min = DFA.UnpackEncodedStringToUnsignedChars(DFA27_minS);
    static readonly char[] DFA27_max = DFA.UnpackEncodedStringToUnsignedChars(DFA27_maxS);
    static readonly short[] DFA27_accept = DFA.UnpackEncodedString(DFA27_acceptS);
    static readonly short[] DFA27_special = DFA.UnpackEncodedString(DFA27_specialS);
    static readonly short[][] DFA27_transition = DFA.UnpackEncodedStringArray(DFA27_transitionS);

    protected class DFA27 : DFA
    {
        public DFA27(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 27;
            this.eot = DFA27_eot;
            this.eof = DFA27_eof;
            this.min = DFA27_min;
            this.max = DFA27_max;
            this.accept = DFA27_accept;
            this.special = DFA27_special;
            this.transition = DFA27_transition;

        }

        override public string Description
        {
            get { return "413:54: ( ELSE onfalse= statement )?"; }
        }

    }

    const string DFA28_eotS =
        "\x11\uffff";
    const string DFA28_eofS =
        "\x11\uffff";
    const string DFA28_minS =
        "\x01\x0a\x10\uffff";
    const string DFA28_maxS =
        "\x01\x5c\x10\uffff";
    const string DFA28_acceptS =
        "\x01\uffff\x01\x01\x0e\uffff\x01\x02";
    const string DFA28_specialS =
        "\x11\uffff}>";
    static readonly string[] DFA28_transitionS = {
            "\x01\x01\x02\uffff\x04\x01\x05\uffff\x01\x10\x03\uffff\x01"+
            "\x01\x0c\uffff\x01\x01\x09\uffff\x06\x01\x03\uffff\x23\x01",
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
            ""
    };

    static readonly short[] DFA28_eot = DFA.UnpackEncodedString(DFA28_eotS);
    static readonly short[] DFA28_eof = DFA.UnpackEncodedString(DFA28_eofS);
    static readonly char[] DFA28_min = DFA.UnpackEncodedStringToUnsignedChars(DFA28_minS);
    static readonly char[] DFA28_max = DFA.UnpackEncodedStringToUnsignedChars(DFA28_maxS);
    static readonly short[] DFA28_accept = DFA.UnpackEncodedString(DFA28_acceptS);
    static readonly short[] DFA28_special = DFA.UnpackEncodedString(DFA28_specialS);
    static readonly short[][] DFA28_transition = DFA.UnpackEncodedStringArray(DFA28_transitionS);

    protected class DFA28 : DFA
    {
        public DFA28(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 28;
            this.eot = DFA28_eot;
            this.eof = DFA28_eof;
            this.min = DFA28_min;
            this.max = DFA28_max;
            this.accept = DFA28_accept;
            this.special = DFA28_special;
            this.transition = DFA28_transition;

        }

        override public string Description
        {
            get { return "425:15: (init= for_init_expr | SEMICOLON )"; }
        }

    }

    const string DFA32_eotS =
        "\x2f\uffff";
    const string DFA32_eofS =
        "\x2f\uffff";
    const string DFA32_minS =
        "\x01\x0a\x06\uffff\x01\x16\x02\uffff\x01\x04\x05\uffff\x01\x00"+
        "\x03\uffff\x01\x00\x17\uffff\x01\x00\x02\uffff";
    const string DFA32_maxS =
        "\x01\x5c\x06\uffff\x01\x59\x02\uffff\x01\x59\x05\uffff\x01\x00"+
        "\x03\uffff\x01\x00\x17\uffff\x01\x00\x02\uffff";
    const string DFA32_acceptS =
        "\x01\uffff\x06\x01\x01\uffff\x01\x01\x01\x02\x01\uffff\x01\x01"+
        "\x06\uffff\x02\x01\x1a\uffff\x01\x01";
    const string DFA32_specialS =
        "\x01\x00\x06\uffff\x01\x01\x02\uffff\x01\x02\x05\uffff\x01\x03"+
        "\x03\uffff\x01\x04\x17\uffff\x01\x05\x02\uffff}>";
    static readonly string[] DFA32_transitionS = {
            "\x01\x09\x02\uffff\x04\x09\x09\uffff\x01\x09\x0c\uffff\x01"+
            "\x0b\x09\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\x06"+
            "\x03\uffff\x01\x08\x18\x07\x06\x0b\x01\x0a\x03\x09",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x13\x01\uffff\x01\x10\x01\uffff\x01\x09\x3e\uffff\x01"+
            "\x12",
            "",
            "",
            "\x06\x09\x01\uffff\x0b\x09\x01\x2c\x01\uffff\x01\x14\x01\uffff"+
            "\x01\x09\x03\uffff\x05\x09\x03\uffff\x01\x09\x32\uffff\x01\x2e",
            "",
            "",
            "",
            "",
            "",
            "\x01\uffff",
            "",
            "",
            "",
            "\x01\uffff",
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
            "\x01\uffff",
            "",
            ""
    };

    static readonly short[] DFA32_eot = DFA.UnpackEncodedString(DFA32_eotS);
    static readonly short[] DFA32_eof = DFA.UnpackEncodedString(DFA32_eofS);
    static readonly char[] DFA32_min = DFA.UnpackEncodedStringToUnsignedChars(DFA32_minS);
    static readonly char[] DFA32_max = DFA.UnpackEncodedStringToUnsignedChars(DFA32_maxS);
    static readonly short[] DFA32_accept = DFA.UnpackEncodedString(DFA32_acceptS);
    static readonly short[] DFA32_special = DFA.UnpackEncodedString(DFA32_specialS);
    static readonly short[][] DFA32_transition = DFA.UnpackEncodedStringArray(DFA32_transitionS);

    protected class DFA32 : DFA
    {
        public DFA32(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 32;
            this.eot = DFA32_eot;
            this.eof = DFA32_eof;
            this.min = DFA32_min;
            this.max = DFA32_max;
            this.accept = DFA32_accept;
            this.special = DFA32_special;
            this.transition = DFA32_transition;

        }

        override public string Description
        {
            get { return "432:1: for_init_expr returns [BaseAST res] : ( ( declaration )=>decl= declaration | expr= expr_list SEMICOLON );"; }
        }

    }


    protected internal int DFA32_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA32_0 = input.LA(1);

                   	 
                   	int index32_0 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA32_0 == CONST) && (synpred3_GLSL()) ) { s = 1; }

                   	else if ( (LA32_0 == ATTRIBUTE) && (synpred3_GLSL()) ) { s = 2; }

                   	else if ( (LA32_0 == UNIFORM) && (synpred3_GLSL()) ) { s = 3; }

                   	else if ( (LA32_0 == VARYING) && (synpred3_GLSL()) ) { s = 4; }

                   	else if ( (LA32_0 == CENTROID) && (synpred3_GLSL()) ) { s = 5; }

                   	else if ( (LA32_0 == INVARIANT) && (synpred3_GLSL()) ) { s = 6; }

                   	else if ( ((LA32_0 >= INTEGER && LA32_0 <= MAT4X4)) ) { s = 7; }

                   	else if ( (LA32_0 == STRUCT) && (synpred3_GLSL()) ) { s = 8; }

                   	else if ( (LA32_0 == NOT || (LA32_0 >= ADD && LA32_0 <= DECREMENT) || LA32_0 == LPAREN || (LA32_0 >= INT_CONSTANT && LA32_0 <= BOOL_CONSTANT)) ) { s = 9; }

                   	else if ( (LA32_0 == ID) ) { s = 10; }

                   	else if ( (LA32_0 == VOID || (LA32_0 >= SAMPLER1D && LA32_0 <= SAMPLER2DSHADOW)) && (synpred3_GLSL()) ) { s = 11; }

                   	 
                   	input.Seek(index32_0);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA32_7 = input.LA(1);

                   	 
                   	int index32_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA32_7 == LBRACKET) ) { s = 16; }

                   	else if ( (LA32_7 == LPAREN) ) { s = 9; }

                   	else if ( (LA32_7 == ID) && (synpred3_GLSL()) ) { s = 18; }

                   	else if ( (LA32_7 == SEMICOLON) && (synpred3_GLSL()) ) { s = 19; }

                   	 
                   	input.Seek(index32_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA32_10 = input.LA(1);

                   	 
                   	int index32_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA32_10 == LBRACKET) ) { s = 20; }

                   	else if ( ((LA32_10 >= EQUAL && LA32_10 <= GEQUAL) || (LA32_10 >= MUL && LA32_10 <= COMMA) || LA32_10 == LPAREN || (LA32_10 >= ASSIGN && LA32_10 <= DIVASSIGN) || LA32_10 == QUESTION) ) { s = 9; }

                   	else if ( (LA32_10 == SEMICOLON) ) { s = 44; }

                   	else if ( (LA32_10 == ID) && (synpred3_GLSL()) ) { s = 46; }

                   	 
                   	input.Seek(index32_10);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA32_16 = input.LA(1);

                   	 
                   	int index32_16 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred3_GLSL()) ) { s = 46; }

                   	else if ( (true) ) { s = 9; }

                   	 
                   	input.Seek(index32_16);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA32_20 = input.LA(1);

                   	 
                   	int index32_20 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred3_GLSL()) ) { s = 46; }

                   	else if ( (true) ) { s = 9; }

                   	 
                   	input.Seek(index32_20);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA32_44 = input.LA(1);

                   	 
                   	int index32_44 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred3_GLSL()) ) { s = 46; }

                   	else if ( (true) ) { s = 9; }

                   	 
                   	input.Seek(index32_44);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae32 =
            new NoViableAltException(dfa.Description, 32, _s, input);
        dfa.Error(nvae32);
        throw nvae32;
    }
    const string DFA35_eotS =
        "\x1a\uffff";
    const string DFA35_eofS =
        "\x1a\uffff";
    const string DFA35_minS =
        "\x01\x0a\x19\uffff";
    const string DFA35_maxS =
        "\x01\x5c\x19\uffff";
    const string DFA35_acceptS =
        "\x01\uffff\x01\x02\x01\x01\x17\uffff";
    const string DFA35_specialS =
        "\x1a\uffff}>";
    static readonly string[] DFA35_transitionS = {
            "\x01\x02\x02\uffff\x04\x02\x09\uffff\x01\x02\x01\uffff\x01"+
            "\x02\x01\x01\x09\uffff\x02\x02\x01\uffff\x0d\x02\x03\uffff\x23"+
            "\x02",
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
            ""
    };

    static readonly short[] DFA35_eot = DFA.UnpackEncodedString(DFA35_eotS);
    static readonly short[] DFA35_eof = DFA.UnpackEncodedString(DFA35_eofS);
    static readonly char[] DFA35_min = DFA.UnpackEncodedStringToUnsignedChars(DFA35_minS);
    static readonly char[] DFA35_max = DFA.UnpackEncodedStringToUnsignedChars(DFA35_maxS);
    static readonly short[] DFA35_accept = DFA.UnpackEncodedString(DFA35_acceptS);
    static readonly short[] DFA35_special = DFA.UnpackEncodedString(DFA35_specialS);
    static readonly short[][] DFA35_transition = DFA.UnpackEncodedStringArray(DFA35_transitionS);

    protected class DFA35 : DFA
    {
        public DFA35(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 35;
            this.eot = DFA35_eot;
            this.eof = DFA35_eof;
            this.min = DFA35_min;
            this.max = DFA35_max;
            this.accept = DFA35_accept;
            this.special = DFA35_special;
            this.transition = DFA35_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 463:18: (stmt= statement )*"; }
        }

    }

    const string DFA38_eotS =
        "\x0b\uffff";
    const string DFA38_eofS =
        "\x0b\uffff";
    const string DFA38_minS =
        "\x01\x31\x05\uffff\x01\x27\x04\uffff";
    const string DFA38_maxS =
        "\x01\x36\x05\uffff\x01\x59\x04\uffff";
    const string DFA38_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\uffff\x01"+
        "\x07\x01\x08\x01\x06\x01\uffff";
    const string DFA38_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA38_transitionS = {
            "\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\x06",
            "",
            "",
            "",
            "",
            "",
            "\x01\x09\x0c\uffff\x01\x07\x01\x08\x04\uffff\x20\x09",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA38_eot = DFA.UnpackEncodedString(DFA38_eotS);
    static readonly short[] DFA38_eof = DFA.UnpackEncodedString(DFA38_eofS);
    static readonly char[] DFA38_min = DFA.UnpackEncodedStringToUnsignedChars(DFA38_minS);
    static readonly char[] DFA38_max = DFA.UnpackEncodedStringToUnsignedChars(DFA38_maxS);
    static readonly short[] DFA38_accept = DFA.UnpackEncodedString(DFA38_acceptS);
    static readonly short[] DFA38_special = DFA.UnpackEncodedString(DFA38_specialS);
    static readonly short[][] DFA38_transition = DFA.UnpackEncodedStringArray(DFA38_transitionS);

    protected class DFA38 : DFA
    {
        public DFA38(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 38;
            this.eot = DFA38_eot;
            this.eof = DFA38_eof;
            this.min = DFA38_min;
            this.max = DFA38_max;
            this.accept = DFA38_accept;
            this.special = DFA38_special;
            this.transition = DFA38_transition;

        }

        override public string Description
        {
            get { return "487:1: type_qualifier returns [int res] : ( CONST | ATTRIBUTE | UNIFORM | VARYING | CENTROID VARYING | INVARIANT | INVARIANT VARYING | INVARIANT CENTROID VARYING );"; }
        }

    }

    const string DFA47_eotS =
        "\x0c\uffff";
    const string DFA47_eofS =
        "\x0c\uffff";
    const string DFA47_minS =
        "\x01\x1b\x04\uffff\x01\x18\x06\uffff";
    const string DFA47_maxS =
        "\x01\x59\x04\uffff\x01\x59\x06\uffff";
    const string DFA47_acceptS =
        "\x01\uffff\x01\x01\x06\uffff\x01\x03\x02\uffff\x01\x02";
    const string DFA47_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA47_transitionS = {
            "\x01\x08\x0b\uffff\x01\x05\x09\uffff\x01\x01\x05\uffff\x23"+
            "\x01",
            "",
            "",
            "",
            "",
            "\x01\x01\x02\uffff\x01\x0b\x3d\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA47_eot = DFA.UnpackEncodedString(DFA47_eotS);
    static readonly short[] DFA47_eof = DFA.UnpackEncodedString(DFA47_eofS);
    static readonly char[] DFA47_min = DFA.UnpackEncodedStringToUnsignedChars(DFA47_minS);
    static readonly char[] DFA47_max = DFA.UnpackEncodedStringToUnsignedChars(DFA47_maxS);
    static readonly short[] DFA47_accept = DFA.UnpackEncodedString(DFA47_acceptS);
    static readonly short[] DFA47_special = DFA.UnpackEncodedString(DFA47_specialS);
    static readonly short[][] DFA47_transition = DFA.UnpackEncodedStringArray(DFA47_transitionS);

    protected class DFA47 : DFA
    {
        public DFA47(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 47;
            this.eot = DFA47_eot;
            this.eof = DFA47_eof;
            this.min = DFA47_min;
            this.max = DFA47_max;
            this.accept = DFA47_accept;
            this.special = DFA47_special;
            this.transition = DFA47_transition;

        }

        override public string Description
        {
            get { return "606:14: (pdecls= param_declarations | VOID )?"; }
        }

    }

 

    public static readonly BitSet FOLLOW_declaration_in_program834 = new BitSet(new ulong[]{0xFC7E008000000002UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_declaration_in_program841 = new BitSet(new ulong[]{0xFC7E008000000002UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_expression861 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_cond_expr_in_assign_expr879 = new BitSet(new ulong[]{0x00000007C0000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_assign_expr885 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_assign_expr889 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ADDASSIGN_in_assign_expr895 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_assign_expr899 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SUBASSIGN_in_assign_expr905 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_assign_expr909 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MULASSIGN_in_assign_expr915 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_assign_expr919 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DIVASSIGN_in_assign_expr925 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_assign_expr929 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_or_expr_in_cond_expr949 = new BitSet(new ulong[]{0x0000004000000002UL});
    public static readonly BitSet FOLLOW_QUESTION_in_cond_expr953 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_cond_expr957 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_COLON_in_cond_expr959 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_assign_expr_in_cond_expr963 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_xor_expr_in_or_expr983 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_OR_in_or_expr988 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_xor_expr_in_or_expr992 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_and_expr_in_xor_expr1012 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_XOR_in_xor_expr1016 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_and_expr_in_xor_expr1020 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_eq_expr_in_and_expr1040 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_AND_in_and_expr1044 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_eq_expr_in_and_expr1048 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_rel_expr_in_eq_expr1068 = new BitSet(new ulong[]{0x0000000000000032UL});
    public static readonly BitSet FOLLOW_EQUAL_in_eq_expr1074 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_rel_expr_in_eq_expr1078 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NEQUAL_in_eq_expr1084 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_rel_expr_in_eq_expr1088 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_expr1110 = new BitSet(new ulong[]{0x00000000000003C2UL});
    public static readonly BitSet FOLLOW_LESS_in_rel_expr1115 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_expr1119 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LEQUAL_in_rel_expr1125 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_expr1129 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_GREATER_in_rel_expr1135 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_expr1139 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_GEQUAL_in_rel_expr1145 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_add_expr_in_rel_expr1149 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_mul_expr_in_add_expr1169 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FOLLOW_ADD_in_add_expr1174 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_mul_expr_in_add_expr1178 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FOLLOW_SUB_in_add_expr1184 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_mul_expr_in_add_expr1188 = new BitSet(new ulong[]{0x000000000000A002UL});
    public static readonly BitSet FOLLOW_unary_expr_in_mul_expr1208 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_MUL_in_mul_expr1213 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_unary_expr_in_mul_expr1217 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_DIV_in_mul_expr1223 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_unary_expr_in_mul_expr1227 = new BitSet(new ulong[]{0x0000000000001802UL});
    public static readonly BitSet FOLLOW_set_in_unary_expr1247 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_postfix_expr_in_unary_expr1270 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_lvalue_in_postfix_expr1288 = new BitSet(new ulong[]{0x0000000000014002UL});
    public static readonly BitSet FOLLOW_INCREMENT_in_postfix_expr1293 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DECREMENT_in_postfix_expr1299 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constructor_expr_in_lvalue1325 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primary_expr_in_lvalue1334 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_DOT_in_lvalue1339 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_ID_in_lvalue1342 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_funccall_expr_in_lvalue1349 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_lvalue1359 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_lvalue1363 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_lvalue1365 = new BitSet(new ulong[]{0x0000000001100002UL});
    public static readonly BitSet FOLLOW_ID_in_primary_expr1384 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INT_CONSTANT_in_primary_expr1391 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FLOAT_CONSTANT_in_primary_expr1398 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BOOL_CONSTANT_in_primary_expr1405 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_primary_expr1412 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expr_list_in_primary_expr1416 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_primary_expr1418 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_funccall_expr_in_primary_expr1427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_funccall_expr1443 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_args_no_parameters_in_funccall_expr1446 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_args_with_parameters_in_funccall_expr1454 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_args_no_parameters1468 = new BitSet(new ulong[]{0x0000008008000000UL});
    public static readonly BitSet FOLLOW_VOID_in_args_no_parameters1471 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_args_no_parameters1475 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAREN_in_args_with_parameters1491 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expr_list_in_args_with_parameters1495 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_args_with_parameters1497 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expr_list1526 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_expr_list1530 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_expr_list1534 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_set_in_constructor_expr1558 = new BitSet(new ulong[]{0x0000000005000000UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_constructor_expr1678 = new BitSet(new ulong[]{0xF80000000601E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_constructor_expr1683 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_constructor_expr1687 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_args_with_parameters_in_constructor_expr1695 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_constructor_expr1702 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_constructor_expr1704 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_constructor_expr1709 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_constructor_expr1712 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_args_with_parameters_in_constructor_expr1716 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_simple_stmt_in_statement1736 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_compound_stmt_in_statement1746 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_stmt_in_simple_stmt1771 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_simple_stmt1773 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_stmt_in_simple_stmt1782 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selection_stmt_in_simple_stmt1791 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_iteration_stmt_in_simple_stmt1800 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_jump_stmt_in_simple_stmt1809 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_simple_stmt1811 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_declaration_stmt1829 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_expression_stmt1847 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IF_in_selection_stmt1863 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_selection_stmt1865 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_selection_stmt1869 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_selection_stmt1871 = new BitSet(new ulong[]{0xFC7FFD801401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_statement_in_selection_stmt1875 = new BitSet(new ulong[]{0x0000020000000002UL});
    public static readonly BitSet FOLLOW_ELSE_in_selection_stmt1878 = new BitSet(new ulong[]{0xFC7FFD801401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_statement_in_selection_stmt1882 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_WHILE_in_iteration_stmt1900 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_iteration_stmt1902 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_iteration_stmt1906 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_iteration_stmt1908 = new BitSet(new ulong[]{0xFC7FFD801401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_statement_in_iteration_stmt1912 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DO_in_iteration_stmt1919 = new BitSet(new ulong[]{0xFC7FFD801401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_statement_in_iteration_stmt1923 = new BitSet(new ulong[]{0x0000040000000000UL});
    public static readonly BitSet FOLLOW_WHILE_in_iteration_stmt1925 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_iteration_stmt1927 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_iteration_stmt1931 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_iteration_stmt1933 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FOR_in_iteration_stmt1940 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_iteration_stmt1942 = new BitSet(new ulong[]{0xFC7E00800441E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_for_init_expr_in_iteration_stmt1947 = new BitSet(new ulong[]{0xF80000000441E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_iteration_stmt1951 = new BitSet(new ulong[]{0xF80000000441E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expr_list_in_iteration_stmt1957 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_iteration_stmt1961 = new BitSet(new ulong[]{0xF80000000C01E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expr_list_in_iteration_stmt1966 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_iteration_stmt1970 = new BitSet(new ulong[]{0xFC7FFD801401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_statement_in_iteration_stmt1974 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_for_init_expr1998 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_list_in_for_init_expr2007 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_for_init_expr2009 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_BREAK_in_jump_stmt2025 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONTINUE_in_jump_stmt2032 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DISCARD_in_jump_stmt2039 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RETURN_in_jump_stmt2046 = new BitSet(new ulong[]{0xF80000000401E402UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_jump_stmt2051 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRACE_in_compound_stmt2082 = new BitSet(new ulong[]{0xFC7FFD803401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_statement_in_compound_stmt2087 = new BitSet(new ulong[]{0xFC7FFD803401E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_RBRACE_in_compound_stmt2093 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_fully_specified_type_in_declaration2109 = new BitSet(new ulong[]{0x0000000000400000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_simple_declaration_in_declaration2114 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_declaration2120 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_type_qualifier_in_fully_specified_type2140 = new BitSet(new ulong[]{0xFC7E008000000000UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_type_specifier_in_fully_specified_type2148 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONST_in_type_qualifier2164 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ATTRIBUTE_in_type_qualifier2171 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_UNIFORM_in_type_qualifier2178 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_VARYING_in_type_qualifier2185 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CENTROID_in_type_qualifier2193 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_VARYING_in_type_qualifier2195 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INVARIANT_in_type_qualifier2202 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INVARIANT_in_type_qualifier2209 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_VARYING_in_type_qualifier2211 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INVARIANT_in_type_qualifier2218 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_CENTROID_in_type_qualifier2220 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_VARYING_in_type_qualifier2222 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_type_specifier2241 = new BitSet(new ulong[]{0x0000000001000002UL});
    public static readonly BitSet FOLLOW_struct_declaration_in_type_specifier2407 = new BitSet(new ulong[]{0x0000000001000002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_type_specifier2415 = new BitSet(new ulong[]{0xF80000000601E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_type_specifier2420 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_type_specifier2424 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_declarations_in_simple_declaration2444 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_simple_declaration2446 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_declaration_in_simple_declaration2455 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_declaration_in_variable_declarations2483 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_variable_declarations2488 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_variable_declaration_in_variable_declarations2492 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ID_in_variable_declaration2520 = new BitSet(new ulong[]{0x0000000041000002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_variable_declaration2524 = new BitSet(new ulong[]{0xF80000000601E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_variable_declaration2529 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_variable_declaration2535 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_variable_declaration2542 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_variable_declaration2546 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_function_declaration2570 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_LPAREN_in_function_declaration2572 = new BitSet(new ulong[]{0xFFFE008008000000UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_param_declarations_in_function_declaration2577 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_VOID_in_function_declaration2583 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_RPAREN_in_function_declaration2587 = new BitSet(new ulong[]{0xFC7FFD801441E400UL,0x000000001FFFFFFFUL});
    public static readonly BitSet FOLLOW_compound_stmt_in_function_declaration2592 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_function_declaration2598 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_param_declaration_in_param_declarations2627 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_param_declarations2631 = new BitSet(new ulong[]{0xFFFE008000000000UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_param_declaration_in_param_declarations2635 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_param_qualifier_in_param_declaration2666 = new BitSet(new ulong[]{0xFC7E008000000000UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_type_specifier_in_param_declaration2674 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_ID_in_param_declaration2677 = new BitSet(new ulong[]{0x0000000041000002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_param_declaration2682 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_param_declaration2686 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_param_declaration2688 = new BitSet(new ulong[]{0x0000000040000002UL});
    public static readonly BitSet FOLLOW_ASSIGN_in_param_declaration2695 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_param_declaration2699 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IN_in_param_qualifier2717 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OUT_in_param_qualifier2724 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INOUT_in_param_qualifier2731 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONST_in_param_qualifier2738 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_CONST_in_param_qualifier2745 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FOLLOW_IN_in_param_qualifier2747 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRUCT_in_struct_declaration2763 = new BitSet(new ulong[]{0x0000000010000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_ID_in_struct_declaration2766 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_LBRACE_in_struct_declaration2770 = new BitSet(new ulong[]{0xFC7E008000000000UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_field_declarators_in_struct_declaration2774 = new BitSet(new ulong[]{0x0000000020000000UL});
    public static readonly BitSet FOLLOW_RBRACE_in_struct_declaration2776 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_field_declarator_in_field_declarators2804 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_SEMICOLON_in_field_declarators2808 = new BitSet(new ulong[]{0xFC7E008000000002UL,0x0000000003FFFFFFUL});
    public static readonly BitSet FOLLOW_field_declarators_in_field_declarators2813 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_type_specifier_in_field_declarator2833 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_field_declarations_in_field_declarator2837 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_field_declaration_in_field_declarations2865 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_COMMA_in_field_declarations2870 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000002000000UL});
    public static readonly BitSet FOLLOW_field_declaration_in_field_declarations2874 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_ID_in_field_declaration2902 = new BitSet(new ulong[]{0x0000000001000002UL});
    public static readonly BitSet FOLLOW_LBRACKET_in_field_declaration2906 = new BitSet(new ulong[]{0xF80000000401E400UL,0x000000001E07FFFFUL});
    public static readonly BitSet FOLLOW_expression_in_field_declaration2910 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_RBRACKET_in_field_declaration2912 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_constructor_expr_in_synpred1_GLSL1318 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_stmt_in_synpred2_GLSL1764 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_synpred3_GLSL1991 = new BitSet(new ulong[]{0x0000000000000002UL});

}
