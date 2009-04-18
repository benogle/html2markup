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

using HTML2Markup;

namespace HTML2Markup.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string html = @"<h2>This is <em>my</em> title</h2> 
                            <p>Then I have <b>a paragraph</b></p>
                            <p>Then I have <b>another para!</b></p>
                            Some random <i>text</i>!!
                            <p>Then I have <b>another para!</b></p>
                            <ol>
                                <li><b>list</b> item1</li>
                                <li>list item2</li>
                                <li>list item3</li>
                                <li>list item4</li>
                                <li>     <ul>
                                    <li>sub item1</li>
                                    <li>sub item2</li>
                                    <li>sub item3</li>
                                    <li>sub item4</li>
                                </ul></li>
                                <li>list item5</li>
                            </ol><ul>
                                <li>list item1</li>
                                <li>list item2</li>
                                <li>list item3</li>
                                <li>list item4</li>
                            </ul>
                            <table>"
                            + "   <tr><th colspan\"2\">title</th></tr>"
                            + "   <tr><td colspan\"omg\">cell</td><td colspan\"omg\">cell2</td></tr>"
                            + "   <tr><td>cell3</td><td>cell4</td></tr>"
                            + "   <tr><td>cell5</td><td>cell6</td></tr>"
                            + "</table>"

                            + "<img src='/contentimg.png' /> "
                            + "<p>This is some <strong>sample text</strong>. You are using <a href=\"http://www.fckeditor.net/\">FCKeditor</a>. this is some text</p>"
                            + "<p>yeah omg whoa <span style=\"background-color: rgb(255, 0, 0);\">and </span>some <span style=\"color: rgb(153, 204, 0);\">color</span>!!</p>";

            Console.Write(MarkupConverter.HTML2Textile(html));
        }
    }
}
