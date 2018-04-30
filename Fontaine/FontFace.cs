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

namespace Fontaine
{
    public class FontFace
    {
        public OffsetTable offsetTable;
        public List<TableRecord> tableRecs;
        public List<Table> tables;
    }

    public class OffsetTable 
    {
        public int sfntVersion;
        public int numTables;
        public int searchRange;
        public int entrySelector;
        public int rangeShift;

        public OffsetTable(int ver, int numtbls, int range, int sel, int shift)
        {
            sfntVersion = ver;
            numTables = numtbls;
            searchRange = range;
            entrySelector = sel;
            rangeShift = shift;
        }
    }

    public class TableRecord
    {
        public String tag;
        public uint checkSum;
        public uint offset;
        public uint length;        

        public TableRecord(String _tag, uint _check, uint _ofs, uint _len)
        {
            tag = _tag;
            checkSum = _check;
            offset = _ofs;
            length = _len;            
        }
    }

//-----------------------------------------------------------------------------

    public class Table
    {
        public byte[] data;

        public virtual void parseData()
        {
        }
    }

    public class CMapTable : Table
    {
        public int version;
        public int numTables;
    }

    public class ControlValueTable : Table          //cvt
    {
    }

    public class ControlValueProgramTable : Table       //prep
    {
    }

    public class DigitalSignatureTable : Table          //DSIG
    {
    }

    public class FontHeaderTable : Table        //head
    {
        int majorVersion;
        int minorVersion;
        double fontRevision;
        int checkSumAdjustment;
        int magicNumber;
        int flags;
        int unitsPerEm;
        DateTime created;
        DateTime modified;
        int xMin;
        int yMin;
        int xMax;
        int yMax;
        int macStyle;
        int lowestRecPPEM;
        int fontDirectionHint;
        int indexToLocFormat;
        int glyphDataFormat;

        public override void parseData()
        {
            majorVersion = data[0] * 256 + data[1];
            minorVersion = data[2] * 256 + data[3];
        }

    }

    public class FontProgramTable : Table
    {
    }

    public class GlyphDataTable : Table     //glyp
    {
        int numberOfContours;
        int xMin;
        int yMin;
        int xMax;
        int yMax;

        public override void parseData()
        {
            numberOfContours = data[0] * 256 + data[1];
            xMin = data[2] * 256 + data[3];
            yMin = data[4] * 256 + data[5];
            xMax = data[6] * 256 + data[7];
            yMax = data[8] * 256 + data[8];
        }
    }

    public class GlyphDefTable : Table
    {
    }

    public class GlyphPositioningTable : Table
    {
    }

    public class GlyphSubstituteTable : Table
    {
    }

    public class GridScanProcedureTable : Table
    {
    }

    public class HorzHeaderTable : Table        //hhea
    {
        int majorVersion;
        int minorVersion;
        int ascender;
        int descender;
        int lineGap;
        int advanceWidthMax;
        int minLeftSideBearing;
        int minRightSideBearing;
        int xMaxExtent;
        int caretSlopeRise;
        int caretSlopeRun;
        int caretOffset;
        int res1;
        int res2;
        int res3;
        int res4;
        int metricDataFormat;
        int numberOfHMetrics;

        public override void parseData()
        {
            majorVersion = data[0] * 256 + data[1];
            minorVersion = data[2] * 256 + data[3];
            ascender = data[4] * 256 + data[5];
            descender = data[6] * 256 + data[7];
            lineGap = data[8] * 256 + data[9];
            advanceWidthMax = data[10] * 256 + data[11];
            minLeftSideBearing = data[12] * 256 + data[13];
            minRightSideBearing = data[14] * 256 + data[15];
            xMaxExtent = data[16] * 256 + data[17];
            caretSlopeRise = data[18] * 256 + data[19];
            caretSlopeRun = data[20] * 256 + data[21];
            caretOffset = data[22] * 256 + data[23];
            res1 = data[24] * 256 + data[25];
            res2 = data[26] * 256 + data[27];
            res3 = data[28] * 256 + data[29];
            res4 = data[30] * 256 + data[31];
            metricDataFormat = data[32] * 256 + data[33];
            numberOfHMetrics = data[34] * 256 + data[35];
        }
    }

    public class HorzDevMetricsTable : Table
    {
    }

    public class HorzMetricsTable : Table       //hmtx
    {
    }

    public class JustificationTable : Table
    {
    }

    public class KerningTable : Table
    {
    }

    public class LinearThresholdTable : Table
    {
    }

    public class LocationIndexTable : Table         //loca
    {
        List<uint> glyphLocations;

        public override void parseData()
        {
            glyphLocations = new List<uint>(data.Length / 4);
            for (int i = 0; i < data.Length; i = i + 4)
            {
                uint loc = data[i];
                loc = loc * 256 + data[i + 1];
                loc = loc * 256 + data[i + 2];
                loc = loc * 256 + data[i + 3];
                glyphLocations.Add(loc);
            }
        }
    }

    public class MaximumProfileTable : Table        //maxp
    {
        double version;
        int numGlyphs;
        int maxPoints;
        int maxContours;
        int maxCompositePoints;
        int maxCompositeContours;
        int maxZones;
        int maxTwilightPoints;
        int maxStorage;
        int maxFunctionDefs;
        int maxInstructionDefs;
        int maxStackElements;
        int maxSizeOfInstructions;
        int maxComponentElements;
        int maxComponentDepth;

        public override void parseData()
        {
            version = 1.0;
            numGlyphs = data[4] * 256 + data[5];
            maxPoints = data[6] * 256 + data[7];
            maxContours = data[8] * 256 + data[9];
            maxCompositePoints = data[10] * 256 + data[11];
            maxCompositeContours = data[12] * 256 + data[13];
            maxZones = data[14] * 256 + data[15];
            maxTwilightPoints = data[16] * 256 + data[17];
            maxStorage = data[18] * 256 + data[19];
            maxFunctionDefs = data[20] * 256 + data[21];
            maxInstructionDefs = data[22] * 256 + data[23];
            maxStackElements = data[24] * 256 + data[25];
            maxSizeOfInstructions = data[26] * 256 + data[27];
            maxComponentElements = data[28] * 256 + data[29];
            maxComponentDepth = data[30] * 256 + data[31];
        }
    }

    public class MetadataTable : Table
    {
    }

    public class NamingTable : Table        //name
    {
    }

    public class OS2MetricsTable : Table
    {
    }

    public class PLCFiveTable : Table
    {
    }

    public class PostscriptTable : Table
    {
    }

    public class VertDevMetricsTable : Table
    {
    }

}
