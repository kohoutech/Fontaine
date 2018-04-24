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

            FontFace face = new FontFace();
            readOffsetTable(source, face);
            readTableRecords(source, face);
            return face;
        }

        private static void readOffsetTable(SourceFile source, FontFace face)
        {
            int ver = (int)source.getFour();
            int numtbls = (int)source.getTwo();
            int range = (int)source.getTwo();
            int sel = (int)source.getTwo();
            int shift = (int)source.getTwo();
            face.offsetTable = new OffsetTable(ver, numtbls, range, sel, shift);
        }

        private static void readTableRecords(SourceFile source, FontFace face)
        {
            face.tableRecs = new List<TableRecord>(face.offsetTable.numTables);

            for (int i = 0; i < face.offsetTable.numTables; i++)
            {
                string tag = source.getAsciiString(4);
                uint checksum = source.getFour();
                uint offset = source.getFour();
                uint length = source.getFour();
                TableRecord tablerec = new TableRecord(tag, checksum, offset, length);
                face.tableRecs.Add(tablerec);
            }
        }

    }


//- file i/o ------------------------------------------------------------------

    //byte ordering is big-endian
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
            byte a = srcbuf[srcpos++];
            byte b = srcbuf[srcpos++];
            uint result = (uint)(a * 256 + b);
            return result;
        }

        public uint getFour()
        {
            byte a = srcbuf[srcpos++];
            byte b = srcbuf[srcpos++];
            byte c = srcbuf[srcpos++];
            byte d = srcbuf[srcpos++];
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