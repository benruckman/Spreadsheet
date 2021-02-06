using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        [TestMethod]
        public void BasicConstructorTest()
        {
            Formula f = new Formula("1+1");
        }

        [TestMethod]
        public void ConstructorWithVariableTest()
        {
            Formula f = new Formula("A1 + b6 / 5");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidCharacterInFormulaTest()
        {
            Formula f = new Formula("4+3*2-3s");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StartingWithPlusTest()
        {
            Formula f = new Formula("+7*9");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StartingWithMinusTest()
        {
            Formula f = new Formula("-7*9");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StartingWithMultiplyTest()
        {
            Formula f = new Formula("*7*9");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StartingWithDivideTest()
        {
            Formula f = new Formula("/7*9");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void StartingWithCloseParenthesesTest()
        {
            Formula f = new Formula(")7*9");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EndingWithPlusTest()
        {
            Formula f = new Formula("7*9+");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EndingWithMinusTest()
        {
            Formula f = new Formula("7*9-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EndingWithMultiplyTest()
        {
            Formula f = new Formula("7*9*");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EndingWithDivideTest()
        {
            Formula f = new Formula("7*9/");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EndingWithOpenParenthesesTest()
        {
            Formula f = new Formula("7*9(");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void EmptyStringTest()
        {
            Formula f = new Formula("");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreCloseThanOpenParenthesesTest()
        {
            Formula f = new Formula("(9+1)/2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MoreOpenThanCloseParenthesesTest()
        {
            Formula f = new Formula("((9+1)/2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PlusFollowsOpenParenthesesTest()
        {
            Formula f = new Formula("8+7*(+2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MinusFollowsOpenParenthesesTest()
        {
            Formula f = new Formula("8+7*(-2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivideFollowsOpenParenthesesTest()
        {
            Formula f = new Formula("8+7*(/2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultiplyFollowsOpenParenthesesTest()
        {
            Formula f = new Formula("8+7*(*2)");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CloseParenthesesFollowsOpenParenthesesTest()
        {
            Formula f = new Formula("8+7*()+2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CloseParenthesesFollowsPlusTest()
        {
            Formula f = new Formula("(8+7+)2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PlusFollowsPlusTest()
        {
            Formula f = new Formula("8+7++2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MinusFollowsPlusTest()
        {
            Formula f = new Formula("8+7+-2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultiplyFollowsPlusTest()
        {
            Formula f = new Formula("8+7+*2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DividesFollowsPlusTest()
        {
            Formula f = new Formula("8+7+/2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ClosedParenthesesFollowsMinusTest()
        {
            Formula f = new Formula("(8+7-)2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PlusFollowsMinusTest()
        {
            Formula f = new Formula("8+7-+2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MinusFollowsMinusTest()
        {
            Formula f = new Formula("8+7--2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultiplyFollowsMinusTest()
        {
            Formula f = new Formula("8+7-*2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivideFollowsMinusTest()
        {
            Formula f = new Formula("8+7-/2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ClosedParenthesesFollowsMultiplyTest()
        {
            Formula f = new Formula("(8+7*)2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PlusFollowsMultiplyTest()
        {
            Formula f = new Formula("8+7*+2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MinusFollowsMultiplyTest()
        {
            Formula f = new Formula("8+7*-2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultiplyFollowsMultiplyTest()
        {
            Formula f = new Formula("8+7**2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivideFollowsMultiplyTest()
        {
            Formula f = new Formula("8+7*/2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ClosedParenthesesFollowsDivideTest()
        {
            Formula f = new Formula("(8+7/)2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void PlusFollowsDivideTest()
        {
            Formula f = new Formula("8+7/+2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MinusFollowsDivideTest()
        {
            Formula f = new Formula("8+7/-2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void MultiplyFollowsDivideTest()
        {
            Formula f = new Formula("8+7/*2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void DivideFollowsDivideTest()
        {
            Formula f = new Formula("8+7//2");
        }

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void CloseParentheseBeginsFormulaTest()
        {
            Formula f = new Formula(")7+8");
        }

        [TestMethod]
        public void BasicEvaluateTest()
        {
            Formula f = new Formula(" 7 + 8/4 - 2 * 3");
            Assert.AreEqual(Double.Parse("3"), f.Evaluate(s => 0));
        }

        [TestMethod]
        public void AreNotEqualWithVariables()
        {
            Assert.IsFalse(new Formula("x1+y2").Equals(new Formula("y2+x1")));
        }

        [TestMethod]
        public void AreEqualWithDoublesTest()
        {
            Assert.IsTrue(new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")));
        }

        [TestMethod]
        public void AreEqualOperatorTest()
        {
            Assert.IsTrue(new Formula("2.0 + x7") == new Formula("2.000 + x7"));
        }

        [TestMethod]
        public void AreNotEqualOperatorTest()
        {
            Assert.IsTrue(new Formula("2.0 + x7") != new Formula("2.001 + x7"));
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            Formula f = new Formula("a2 - a5 + d4");

            Assert.IsTrue(f.GetVariables().Contains("a2"));
            Assert.IsTrue(f.GetVariables().Contains("a5"));
            Assert.IsTrue(f.GetVariables().Contains("d4"));
        }

        [TestMethod]
        public void DivideByZeroTest()
        {
            Formula f = new Formula("420 / 0");
            f.Evaluate(s => 0);
        }

        [TestMethod]
        public void EqualHashCodeTest()
        {
            Assert.IsTrue(new Formula("2.0 + x7").GetHashCode() == new Formula("2.000 + x7").GetHashCode());
        }

        [TestMethod]
        public void ClosedParenthesesMultiplyTest()
        {
            Formula f = new Formula("(2 + 6) * 3");
            Assert.AreEqual(Double.Parse("24"), f.Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("9")]
        public void TestLeftToRight()
        {
            Assert.AreEqual(Double.Parse("15"), new Formula ("2*6+3").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("10")]
        public void TestOrderOperations()
        {
            Assert.AreEqual(Double.Parse("20"), new Formula("2+6*3").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("11")]
        public void TestParenthesesTimes()
        {
            Assert.AreEqual(Double.Parse("24"), new Formula("(2+6)*3").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("12")]
        public void TestTimesParentheses()
        {
            Assert.AreEqual(Double.Parse("16"), new Formula("2*(3+5)").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("13")]
        public void TestPlusParentheses()
        {
            Assert.AreEqual(Double.Parse("10"), new Formula("2+(3+5)").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("14")]
        public void TestPlusComplex()
        {
            Assert.AreEqual(Double.Parse("50"), new Formula("2+(3+5*9)").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("15")]
        public void TestOperatorAfterParens()
        {
            Assert.AreEqual(Double.Parse("0"), new Formula("(1*1)-2/2").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("16")]
        public void TestComplexTimesParentheses()
        {
            Assert.AreEqual(Double.Parse("26"), new Formula("2+3*(3+5)").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("17")]
        public void TestComplexAndParentheses()
        {
            Assert.AreEqual(Double.Parse("194"), new Formula("2+3*5+(3+4*8)*5+2").Evaluate(s => 0));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("27")]
        public void TestComplexNestedParensRight()
        {
            Assert.AreEqual(Double.Parse("6"), new Formula("x1+(x2+(x3+(x4+(x5+x6))))").Evaluate(s => 1));
        }

        [TestMethod(), Timeout(5000)]
        [TestCategory("28")]
        public void TestComplexNestedParensLeft()
        {
            Assert.AreEqual(Double.Parse("12"), new Formula("((((x1+x2)+x3)+x4)+x5)+x6").Evaluate(s => 2));
        }

        [TestMethod]
        public void TestRepeatedVar()
        {
            Assert.AreEqual(Double.Parse("0"), new Formula("a4-a4*a4/a4").Evaluate(s => 3));
        }

        //GRADING TESTS BELOW

        // Normalizer tests
        [TestMethod(), Timeout(2000)]
        [TestCategory("1")]
        public void TestNormalizerGetVars()
        {
            Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
            HashSet<string> vars = new HashSet<string>(f.GetVariables());

            Assert.IsTrue(vars.SetEquals(new HashSet<string> { "X1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("2")]
        public void TestNormalizerEquals()
        {
            Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);

            Assert.IsTrue(f.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("3")]
        public void TestNormalizerToString()
        {
            Formula f = new Formula("2+x1", s => s.ToUpper(), s => true);
            Formula f2 = new Formula(f.ToString());

            Assert.IsTrue(f.Equals(f2));
        }

        // Validator tests
        [TestMethod(), Timeout(2000)]
        [TestCategory("4")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorFalse()
        {
            Formula f = new Formula("2+x1", s => s, s => false);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("5")]
        public void TestValidatorX1()
        {
            Formula f = new Formula("2+x", s => s, s => (s == "x"));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("6")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorX2()
        {
            Formula f = new Formula("2+y1", s => s, s => (s == "x"));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("7")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestValidatorX3()
        {
            Formula f = new Formula("2+x1", s => s, s => (s == "x"));
        }


        // Simple tests that return FormulaErrors
        [TestMethod(), Timeout(2000)]
        [TestCategory("8")]
        public void TestUnknownVariable()
        {
            Formula f = new Formula("2+X1");
            Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("9")]
        public void TestDivideByZero()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("10")]
        public void TestDivideByZeroVars()
        {
            Formula f = new Formula("(5 + X1) / (X1 - 3)");
            Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
        }


        // Tests of syntax errors detected by the constructor
        [TestMethod(), Timeout(2000)]
        [TestCategory("11")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSingleOperator()
        {
            Formula f = new Formula("+");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("12")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOperator()
        {
            Formula f = new Formula("2+5+");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("13")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraCloseParen()
        {
            Formula f = new Formula("2+5*7)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("14")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExtraOpenParen()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("15")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator()
        {
            Formula f = new Formula("5x");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("16")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator2()
        {
            Formula f = new Formula("5+5x");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("17")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator3()
        {
            Formula f = new Formula("5+7+(5)8");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("18")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNoOperator4()
        {
            Formula f = new Formula("5 5");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("19")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestDoubleOperator()
        {
            Formula f = new Formula("5 + + 3");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("20")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmpty()
        {
            Formula f = new Formula("");
        }

        // Some more complicated formula evaluations
        [TestMethod(), Timeout(2000)]
        [TestCategory("21")]
        public void TestComplex1()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("22")]
        public void TestRightParens()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("23")]
        public void TestLeftParens()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("53")]
        public void TestRepeatedVarTeacherTest()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Test of the Equals method
        [TestMethod(), Timeout(2000)]
        [TestCategory("24")]
        public void TestEqualsBasic()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula("X1+X2");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("25")]
        public void TestEqualsWhitespace()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula(" X1  +  X2   ");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("26")]
        public void TestEqualsDouble()
        {
            Formula f1 = new Formula("2+X1*3.00");
            Formula f2 = new Formula("2.00+X1*3.0");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("27")]
        public void TestEqualsComplex()
        {
            Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
            Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
            Assert.IsTrue(f1 == f2);
        }


        [TestMethod(), Timeout(2000)]
        [TestCategory("28")]
        public void TestEqualsNullAndString()
        {
            Formula f = new Formula("2");
            Assert.IsFalse(f.Equals(null));
            Assert.IsFalse(f.Equals(""));
        }


        // Tests of == operator
        [TestMethod(), Timeout(2000)]
        [TestCategory("29")]
        public void TestEq()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("30")]
        public void TestEqFalse()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsFalse(f1 == f2);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("31")]
        public void TestEqNull()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(null == f1);
            Assert.IsFalse(f1 == null);
            Assert.IsTrue(f1 == f2);
        }


        // Tests of != operator
        [TestMethod(), Timeout(2000)]
        [TestCategory("32")]
        public void TestNotEq()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("33")]
        public void TestNotEqTrue()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsTrue(f1 != f2);
        }


        // Test of ToString method
        [TestMethod(), Timeout(2000)]
        [TestCategory("34")]
        public void TestString()
        {
            Formula f = new Formula("2*5");
            Assert.IsTrue(f.Equals(new Formula(f.ToString())));
        }


        // Tests of GetHashCode method
        [TestMethod(), Timeout(2000)]
        [TestCategory("35")]
        public void TestHashCode()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("2*5");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }

        // Technically the hashcodes could not be equal and still be valid,
        // extremely unlikely though. Check their implementation if this fails.
        [TestMethod(), Timeout(2000)]
        [TestCategory("36")]
        public void TestHashCodeFalse()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("3/8*2+(7)");
            Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("37")]
        public void TestHashCodeComplex()
        {
            Formula f1 = new Formula("2 * 5 + 4.00 - _x");
            Formula f2 = new Formula("2*5+4-_x");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }


        // Tests of GetVariables method
        [TestMethod(), Timeout(2000)]
        [TestCategory("38")]
        public void TestVarsNone()
        {
            Formula f = new Formula("2*5");
            Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("39")]
        public void TestVarsSimple()
        {
            Formula f = new Formula("2*X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("40")]
        public void TestVarsTwo()
        {
            Formula f = new Formula("2*X2+Y3");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
            Assert.AreEqual(actual.Count, 2);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("41")]
        public void TestVarsDuplicate()
        {
            Formula f = new Formula("2*X2+X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("42")]
        public void TestVarsComplex()
        {
            Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
            Assert.AreEqual(actual.Count, 5);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod(), Timeout(2000)]
        [TestCategory("43")]
        public void TestMultipleFormulae()
        {
            Formula f1 = new Formula("2 + a1");
            Formula f2 = new Formula("3");
            Assert.AreEqual(2.0, f1.Evaluate(x => 0));
            Assert.AreEqual(3.0, f2.Evaluate(x => 0));
            Assert.IsFalse(new Formula(f1.ToString()) == new Formula(f2.ToString()));
            IEnumerator<string> f1Vars = f1.GetVariables().GetEnumerator();
            IEnumerator<string> f2Vars = f2.GetVariables().GetEnumerator();
            Assert.IsFalse(f2Vars.MoveNext());
            Assert.IsTrue(f1Vars.MoveNext());
        }

        // Repeat this test to increase its weight
        [TestMethod(), Timeout(2000)]
        [TestCategory("44")]
        public void TestMultipleFormulaeB()
        {
            TestMultipleFormulae();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("45")]
        public void TestMultipleFormulaeC()
        {
            TestMultipleFormulae();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("46")]
        public void TestMultipleFormulaeD()
        {
            TestMultipleFormulae();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("47")]
        public void TestMultipleFormulaeE()
        {
            TestMultipleFormulae();
        }

        // Stress test for constructor
        [TestMethod(), Timeout(2000)]
        [TestCategory("48")]
        public void TestConstructor()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // This test is repeated to increase its weight
        [TestMethod(), Timeout(2000)]
        [TestCategory("49")]
        public void TestConstructorB()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("50")]
        public void TestConstructorC()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("51")]
        public void TestConstructorD()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod(), Timeout(2000)]
        [TestCategory("52")]
        public void TestConstructorE()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }
    }
}
