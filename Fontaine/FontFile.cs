/* ----------------------------------------------------------------------------
Fontaine : a font editor
Copyright (C) 2010-2018  George E Greaney

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

//open type file specification at https://docs.microsoft.com/en-us/typography/opentype/spec/

namespace Fontaine
{
    public class FontFile
    {
        public static FontFace readFile(String filename)
        {
            SourceFile source = new SourceFile(filename);
            System.IO.StreamWriter outfile = new System.IO.StreamWriter("dump.txt");

            String line = null;
            String ascii = null;
            byte[] bytes = source.srcbuf;
            for (int i = 0; i < bytes.Length;  )
            {

                if ((i % 16) == 0)
                {
                    line = i.ToString("X8") + " ";
                    ascii = "  ";
                }
                line += " " + bytes[i].ToString("X2");
                if ((bytes[i] >= 0x020) && (bytes[i] < 0x7f))
                {
                    ascii += (char)bytes[i];
                }
                else
                {
                    ascii += '.';
                }
                i++;
                if ((i % 16) == 0)
                {
                 outfile.WriteLine(line + ascii);   
                }
            }
            outfile.Close();
            Console.WriteLine("done!");

            return null;
        }
    }


//- file i/o ------------------------------------------------------------------

    public class SourceFile
    {
        public Byte[] srcbuf;
        public uint srclen;
        public uint srcpos;

        public SourceFile(String se1name)
        {
            srcbuf = File.ReadAllBytes(se1name);
            srclen = (uint)srcbuf.Length;
            srcpos = 0;
        }

        public uint getPos()
        {
            return srcpos;
        }

        public byte[] getRange(uint ofs, uint len)
        {
            byte[] result = new byte[len];
            Array.Copy(srcbuf, ofs, result, 0, len);
            return result;
        }

        public uint getOne()
        {
            byte a = srcbuf[srcpos++];
            uint result = (uint)(a);
            return result;
        }

        public uint getTwo()
        {
            byte b = srcbuf[srcpos++];
            byte a = srcbuf[srcpos++];
            uint result = (uint)(a * 256 + b);
            return result;
        }

        public uint getFour()
        {
            byte d = srcbuf[srcpos++];
            byte c = srcbuf[srcpos++];
            byte b = srcbuf[srcpos++];
            byte a = srcbuf[srcpos++];
            uint result = (uint)(a * 256 + b);
            result = (result * 256 + c);
            result = (result * 256 + d);
            return result;
        }

        //fixed len string
        public String getAsciiString(int width)
        {
            String result = "";
            for (int i = 0; i < width; i++)
            {
                byte a = srcbuf[srcpos++];
                if ((a >= 0x20) && (a <= 0x7E))
                {
                    result += (char)a;
                }
            }
            return result;
        }

        public void skip(uint delta)
        {
            srcpos += delta;
        }

        public void seek(uint pos)
        {
            srcpos = pos;
        }
    }

}


//Console.WriteLine("there's no sun in the shadow of the wizard");