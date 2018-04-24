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

}
