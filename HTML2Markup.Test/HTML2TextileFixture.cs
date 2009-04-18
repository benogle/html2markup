//Copyright (c) 2008 Ben Ogle

//Permission is hereby granted, free of charge, to any person
//obtaining a copy of this software and associated documentation
//files (the "Software"), to deal in the Software without
//restriction, including without limitation the rights to use,
//copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the
//Software is furnished to do so, subject to the following
//conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;

using HTML2Markup;

namespace HTML2Markup.Test
{
    [TestFixture]
    public class HTML2TextileFixture
    {

        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Dispose()
        {
        }

        public string ParseHTML(string html)
        {
            return MarkupConverter.HTML2Textile(html);
        }

        #region paragraphs

        [Test]
        public void PNoStyle()
        {
            string s = @"<p>my text</p><p>Yeah it is</p><p>woot</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("my text\n\nYeah it is\n\nwoot\n\n", t);
        }

        [Test]
        public void PStyle()
        {
            string s = "<p >my text</p><p style=\"text-align: center;\">Yeah it is</p><p>woot</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("my text\n\np{text-align: center}. Yeah it is\n\nwoot\n\n", t);
        }

        [Test]
        public void PMultiStyle()
        {
            string s = "<p >my text</p><p style=\"text-align: center; color: red;\">Yeah it is</p><p>woot</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("my text\n\np{text-align: center;color: red}. Yeah it is\n\nwoot\n\n", t);
        }

        [Test]
        public void P_noPara()
        {
            string s = "<p >my text</p>Random text<p>woot</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("my text\n\nRandom text\n\nwoot\n\n", t);
        }

        #endregion

        #region lists

        [Test]
        public void UL_nestedOL()
        {
            string s = "<ul><li>li1</li><li>li2</li><li><ol><li>num1</li><li>num2</li></ol></li><li>li3</li></ul>";
            string t = ParseHTML(s);

            Assert.AreEqual("* li1\n* li2\n## num1\n## num2\n* li3\n\n", t);
        }

        [Test]
        public void UL_nestedOL_n_Text()
        {
            string s = "<ul><li>li1</li><li>li2</li><li>extra<ol><li>num1</li><li>num2</li></ol></li><li>li3</li></ul>";
            string t = ParseHTML(s);

            Assert.AreEqual("* li1\n* li2\n* extra\n## num1\n## num2\n* li3\n\n", t);
        }

        #endregion

        #region pre/code

        [Test]
        public void PRE_simple()
        {
            string s = "some text <pre>hey this is my pre</pre> more text";
            string t = ParseHTML(s);

            Assert.AreEqual("some text\n\npre. \nhey this is my pre\n.pre\n\nmore text", t);
        }

        [Test]
        public void PRE_newlines()
        {
            string s = "some text <pre>hey\n\nthis\nis\nmy\n\n\npre</pre> more text";
            string t = ParseHTML(s);

            Assert.AreEqual("some text\n\npre. \nhey\n\nthis\nis\nmy\n\n\npre\n.pre\n\nmore text", t);
        }

        [Test]
        public void PRECODE_simple()
        {
            string s = "some text <pre>  <code>hey this is my code</code> </pre> more text";
            string t = ParseHTML(s);

            Assert.AreEqual("some text\n\nbc. \nhey this is my code\n.bc\n\nmore text", t);
        }

        [Test]
        public void CODE_simple()
        {
            string s = "<p>some text <code>code</code> more text</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("some text @code@ more text\n\n", t);
        }

        #endregion

        #region glyphs

        [Test]
        public void GLYPH_replace()
        {
            string s = "some text &nbsp; &amp; &copy; ";
            string t = ParseHTML(s);

            Assert.AreEqual("some text   & (C)", t);
        }

        [Test]
        public void GLYPH_charCode()
        {
            string s = "some text &#169; &#179; &#8721;";
            string t = ParseHTML(s);

            Assert.AreEqual("some text (C) ³ ∑", t);
        }

        #endregion

        #region style

        [Test]
        public void STYLE_caseGoodStyle()
        {
            string s = "<p style=\"position: absolute; TeXT-aliGN: center; top: 0px;\">text!</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("p{text-align: center}. text!\n\n", t);
        }

        [Test]
        public void STYLE_Expression()
        {
            string s = "<p style=\"color: expressIon(#123); TeXT-aliGN: left; top: 0px;\">text!</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("p{text-align: left}. text!\n\n", t);
        }

        [Test]
        public void STYLE_caseBaaaad()
        {
            string s = "<p style=\"color: expressIon(#123); text-align: left; TOP: 0px;\">text!</p>";
            string t = ParseHTML(s);

            Assert.AreEqual("p{text-align: left}. text!\n\n", t);
        }

        #endregion

        #region links

        [Test]
        public void LINK_wikiInternal()
        {
            string s = "text! and <a href=\"/Project/something/Wiki/Wowza\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link|something:Wowza] ok!!", t);
        }

        [Test]
        public void LINK_bug()
        {
            string s = "text! and <a href=\"/bUg/1\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link|bug 1] ok!!", t);
        }

        [Test]
        public void LINK_task()
        {
            string s = "text! and <a href=\"/tAsk/1\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link|task 1] ok!!", t);
        }

        [Test]
        public void LINK_external()
        {
            string s = "text! and <a href=\"/issue/1\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link|/issue/1] ok!!", t);
        }

        [Test]
        public void LINK_externalWiki()
        {
            string s = "text! and <a href=\"http://something.com/Project/something/Wiki/Wowza\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link|http://something.com/Project/something/Wiki/Wowza] ok!!", t);
        }

        [Test]
        public void LINK_externalWikiTitle()
        {
            string s = "text! and <a title=\"title\" href=\"http://something.com/Project/something/Wiki/Wowza\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link (title)|http://something.com/Project/something/Wiki/Wowza] ok!!", t);
        }

        [Test]
        public void LINK_wikiInternalTitle()
        {
            string s = "text! and <a title=\"title!\" href=\"/Project/something/Wiki/Wowza\">a link</a> ok!!";
            string t = ParseHTML(s);

            Assert.AreEqual("text! and [a link (title!)|something:Wowza] ok!!", t);
        }


        #endregion
    }
}