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

using System.Collections.Generic;
using System.Text;

using Sgml;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace HTML2Markup
{
    public enum ListType
    {
        Unordered,
        Ordered
    }

    public class Parser
    {
        private SgmlReader _sgmlReader;
        private StringBuilder _output;

        private ParserNode _currentNode;

        private int _lastNewLines = 0;

        public Parser()
        {
            _sgmlReader = new SgmlReader();  
            _sgmlReader.DocType = "HTML";  
            _sgmlReader.WhitespaceHandling = WhitespaceHandling.All;
            _sgmlReader.CaseFolding = CaseFolding.ToLower;
        }

        public string HTML2Markup(string html)
        {
            _lastNewLines = 0;
            _currentNode = null;

            //replace &nbsp; type junk with their actual chars or textile representations of them.
            html = ProcessGlyphs(html);

            //add a div as the root node or the SGML lib only will process the first element!
            html = string.Format("<div>{0}</div>", html);

            StringReader rdr = new StringReader(html);

            _output = new StringBuilder();

            _sgmlReader.InputStream = rdr;

            MemorySgmlReader memRdr = new MemorySgmlReader(_sgmlReader);

            foreach(ParserNode n in memRdr.Nodes)
            {
                _currentNode = n;
                switch (n.NodeType)
                {
                    case XmlNodeType.Element:
                        HandleElementStart(n);
                        break;
                    case XmlNodeType.EndElement:
                        HandleElementEnd(n);
                        break;
                    case XmlNodeType.Text:
                        HandleText(n);
                        break;
                }
            }

            return _output.ToString();
        }

        #region private 

        /// <summary>
        /// appends newlines to the end of the output. Dont call this. 
        /// Call AddOutput().
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int AppendNewlines(int num)
        {
            if (num > 0)
            {
                _output.Append(RepeatStr("\n", num));
            }
            else
                num = 0;

            return num;
        }

        #endregion

        #region utilities

        protected ParserNode CurrentNode
        {
            get { return _currentNode; }
        }

        /// <summary>
        /// Will repeat string $s $times times. i.e. 
        /// 
        /// RepeatStr('#', 4); --> '####'
        /// </summary>
        /// <param name="s"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        protected string RepeatStr(string s, int times)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < times; i++)
                sb.Append(s);
            return sb.ToString();
        }

        

        protected void AddOutput(string s, int numNewLines, bool before)
        {
            int numPrintedLines = 0;
            bool didNewLines = false;

            if (before && numNewLines > 0 && numNewLines > _lastNewLines && _output.Length > 0)
            {
                int num = AppendNewlines(numNewLines - _lastNewLines);
                if (s.Length == 0)
                    numPrintedLines = num + _lastNewLines;
                didNewLines = true;
            }

            _output.Append(s);

            if (!before && s.Length > 0)
            {
                numPrintedLines = numNewLines;
                AppendNewlines(numNewLines);
                didNewLines = true;
            }
            else if (!before && numNewLines > 0 && numNewLines > _lastNewLines)
            {
                numPrintedLines = AppendNewlines(numNewLines - _lastNewLines);
                numPrintedLines += _lastNewLines;
                didNewLines = true;
            }

            if(s.Length > 0 || didNewLines)
                _lastNewLines = numPrintedLines;
        }

        protected void AddOutput(string s)
        {
            AddOutput(s, 0, false);
        }

        #endregion

        #region overridable

        protected virtual string ProcessGlyphs(string s)
        {
            return s;
        }

        protected virtual void ConvertStart()
        {
        }

        protected virtual void HandleElementStart(ParserNode node)
        {   
        }

        protected virtual void HandleElementEnd(ParserNode node)
        {   
        }

        protected virtual void HandleText(ParserNode node)
        {
        }

        #endregion
    }
}
