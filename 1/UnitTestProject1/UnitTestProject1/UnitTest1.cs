using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterpreterPlugin;
using Plugin;

namespace Testing
{
    [TestClass]
    public class UnitTest1
    {
        //Lexer lex = new Lexer();

        [TestMethod]
        public void TestLexer()
        {
            Lexer lex = new Lexer();
            Assert.AreEqual(-1, 1);
        }

        [TestMethod]
        public void TestSyntaxTree()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestSemantic()
        {
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestCodeGen()
        {
            Assert.AreEqual(1, 1);
        }
    }
}
