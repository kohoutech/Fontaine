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
            readTables(source, face);
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

        private static void readTables(SourceFile source, FontFace face)
        {
            face.tables = new List<Table>();
            Table tbl = null;
                        
            foreach (TableRecord rec in face.tableRecs)
            {
                switch (rec.tag)
                {
                        //required tables
                    case "cmap":
                        tbl = new CMapTable();
                        break;
                    case "head":
                        tbl = new FontHeaderTable();
                            break;
                    case "hhea":
                        tbl = new HorzHeaderTable();
                        break;
                    case "hmtx":
                        tbl = new HorzMetricsTable();
                        break;
                    case "maxp":
                        tbl = new  MaximumProfileTable();
                        break;
                    case "name":
                        tbl = new NamingTable();
                        break;
                    case "OS/2":
                        tbl = new OS2MetricsTable();
                        break;
                    case "post":
                        tbl = new PostscriptTable();
                        break;

                        //true type outline tables
                    case "cvt ":
                        tbl = new ControlValueTable();
                        break;
                    case "fpgm":
                        tbl = new FontProgramTable();
                        break;
                    case "glyf":
                        tbl = new GlyphDataTable();
                        break;
                    case "loca":
                        tbl = new LocationIndexTable();
                        break;
                    case "prep":
                        tbl = new ControlValueProgramTable();
                        break;
                    case "gasp":
                        tbl = new GridScanProcedureTable();
                        break;

                        //advanced typographic tables
                    case "GPOS":
                        tbl = new GlyphPositioningTable();
                        break;
                    case "GDEF":
                        tbl = new GlyphDefTable();
                        break;
                    case "GSUB": 
                        tbl = new GlyphSubstituteTable(); 
                        break;
                    case "JSTF":
                        tbl = new JustificationTable();
                        break;

                        //other tables
                    case "DSIG":
                        tbl = new DigitalSignatureTable();
                        break;
                    case "hdmx":
                        tbl = new HorzDevMetricsTable();
                        break;
                    case "kern":
                        tbl = new KerningTable();
                        break;
                    case "LTSH":
                        tbl = new LinearThresholdTable();
                        break;
                    case "meta":
                        tbl = new MetadataTable();
                        break;
                    case "PCLT":
                        tbl = new PLCFiveTable();
                        break;
                    case "VDMX":
                        tbl = new VertDevMetricsTable();
                        break;

                    default:
                        tbl = new Table();
                        break;
                }
                tbl.data = source.getRange(rec.offset, rec.length);
                tbl.parseData();
                face.tables.Add(tbl);

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