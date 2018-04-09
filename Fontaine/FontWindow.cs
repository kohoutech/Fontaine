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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fontaine
{
    public partial class FontWindow : Form
    {
        Fontaine fontaine;
        FontFace curfont;

        public FontWindow()
        {
            fontaine = new Fontaine();
            curfont = null;

            InitializeComponent();
        }

//- file menu -----------------------------------------------------------------

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            String filename = "test.ttf";
            curfont = FontFile.readFile(filename);
        }

        private void exitFileMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
