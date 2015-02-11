﻿using Reactor.Math;
using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RFont_Generator
{
    public class FontGenerator
    {
        public static Library FreeTypeLibrary = new Library();
        public Font Build(string filename, int size, int dpi)
        {
            Face face = new Face(FreeTypeLibrary, filename);
            face.SetCharSize(size*dpi, size*dpi, (uint)dpi, (uint)dpi);
            Font font = new Font();
            font.Name = face.FamilyName;
            face.LoadChar((uint)32, (LoadFlags.Render | LoadFlags.Monochrome | LoadFlags.Pedantic), LoadTarget.Normal);
            font.SpaceWidth = face.Glyph.Metrics.HorizontalAdvance >> 6;
            font.LineHeight = face.Height >> 6;
            font.Kerning = face.HasKerning;
            font.Size = size;
            font.Glyphs = new List<FontGlyph>();

            
            for(int i = 33; i<126; i++)
            {


                uint charIndex = face.GetCharIndex((uint)i);
                face.LoadGlyph(charIndex, (LoadFlags.Render | LoadFlags.Color | LoadFlags.Pedantic | LoadFlags.CropBitmap ), LoadTarget.Normal);
                
                FontGlyph glyph = new FontGlyph();
                glyph.bitmap = face.Glyph.Bitmap.ToGdipBitmap(Color.White);
                glyph.Bounds = new Reactor.Math.Rectangle(0, 0, glyph.bitmap.Width+2, glyph.bitmap.Height+2);
                glyph.CharIndex = i;
                glyph.Offset = new Vector2(face.Glyph.Metrics.HorizontalBearingX >> 6, face.Glyph.Metrics.HorizontalBearingY >> 6);
                glyph.Advance = face.Glyph.Advance.X >> 6;
                
                font.Glyphs.Add(glyph);
            }
            font.Glyphs.Sort(new FontGlyphSizeSorter());
            font.Glyphs.Reverse();
            var missed = -1;
            var width = 16;
            Bitmap b = new Bitmap(1, 1);
            while(missed!=0)
            {
                Application.DoEvents();
                missed = 0;
                AtlasNode root = new AtlasNode();
                root.bounds = new Reactor.Math.Rectangle(0, 0, width, width);
                b.Dispose();
                b = new Bitmap(width, width);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.Black);

                for (var i = 0; i < font.Glyphs.Count; i++)
                {
                    FontGlyph glyph = font.Glyphs[i];
                    AtlasNode result = root.Insert(glyph.Bounds);

                    if (result != null)
                    {
                        Reactor.Math.Rectangle bounds = result.bounds;
                        g.DrawImageUnscaledAndClipped(glyph.bitmap, bounds);
                        glyph.Bounds = bounds;
                        font.Glyphs[i] = glyph;
                    }
                    else
                    {
                        missed += 1;
                        break;
                    }
                    
                    
                }
                width += 16;
            }

            if (missed > 0)
                MessageBox.Show("Oops, looks like there wasn't enough room!\r\nMissed: " + missed, "Missed Glyphs", MessageBoxButtons.OK);

            font.Bitmap = b;
            return font;
        }
    }
}