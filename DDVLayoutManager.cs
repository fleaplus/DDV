﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace DDV
{
/** Consistent rules for laying out nucleotides in a recursively tiled, left to 
*   right manner using the following numbers:
     
 name       modulo size    padding  thickness (derived)
---------   -----  -----   -------   ----------------
XInColumn    100   1        0           1
LineInColumn 1000  100nt    0           1
ColumnInRow  100   100KB    4           104    100 * 1 + 4
RowInTile    10    10MB     40          1040   1000 * 1 + 40
XInTile      3     100MB    400         10800  (100 * 104) + 400
YInTile      4     300MB    1600        12000  (10*1040) + 1600
TileColumn   9     1.2GB    6400        38800  (3 * 10800) + 6400
TileRow      inf   10.8GB   25600       73600  (4 * 12000) + 25600


Example Problem:  Index: 751,270,123
The order in which the levels are calculated is not important.
XInColumn     751,270,123 % 100 = 23
LineInColumn  7512701 % 1000  = 701
ColumnInRow   7512 % 100 = 12
RowInTile     75 % 10 = 5
Tile X        2
Tile Y        0

To go from mouse coordinates to a genome index, you must use the inverse function: index_from_screen()

coordinate_in_chunk(index, size, modulo){
	return (int)(index / size) % modulo
}

position_on_screen(index, padding){
	xy = [0, 0]
	for i, level in enumerate(levels)
		part = i % 2
		xy[part] += level.thickness * coordinate_in_chunk(index, level.count, level.modulo)
}

/ ** The order of level that x and y are decomposed is important so that 
* the padding is subtracted rather than being counted towards the index.
* /
index_from_screen(x, y){
	index_from_yx = 0
	yx_remaining = [y, x]  //order reversed
	for level i, level in enumerate(reverse(levels)):
		part = i % 2
		number_of_full_increments = (int)(yx_remaining[part] / level.thickness)
		index_from_yx += level.modulo * number_of_full_increments // add total nulceotide size for every full increment of this level e.g. Tile Y height
		yx_remaining[part] -= number_of_full_increments * level.thickness  //subtract the credited coordinates to shift to relative coordinates in that level
	return index_from_yx
}
*/


    /** Simple POD object for Levels table **/
    class LayoutLevel
    {
        public int modulo, padding, thickness;
        public long chunk_size;
        public LayoutLevel(string name, int modulo, long chunk_size, int padding, int thickness)
        {
            this.modulo = modulo;
            this.chunk_size = chunk_size;
            this.padding = padding;
            this.thickness = thickness;
        }

        public LayoutLevel(string name, int modulo, List<LayoutLevel> levels)
        {
            this.modulo = modulo;
            LayoutLevel child = levels[levels.Count - 1];
            this.chunk_size =  child.modulo * child.chunk_size;
            this.padding = 6 * (int)Math.Pow(3, levels.Count - 2); // third level (count=2) should be 6, then 18
            LayoutLevel lastParallel = levels[levels.Count - 2];
            this.thickness = lastParallel.modulo * lastParallel.thickness + padding;
        }
    }


    class DDVLayoutManager
    {
        public List<LayoutLevel> levels;
        public DDVLayoutManager()
        {
            levels = new List<LayoutLevel>();
            /**  name       modulo size    padding and thickness are derived
                ---------    ----  -----
                XInColumn    100  1
                LineInColumn 1000 100nt
                ColumnInRow  100  100KB
                RowInTile    10   10MB
                XInTile      3    100MB
                YInTile      4    300MB
                TileColumn   9    1.2GB
                TileRow      inf  10.8GB
             */
            levels.Add(new LayoutLevel("XInColumn",    100, 1, 0, 1));
            levels.Add(new LayoutLevel("LineInColumn", 1000, 100, 0, 1));
            levels.Add(new LayoutLevel("ColumnInRow",  100, levels));
            levels.Add(new LayoutLevel("RowInTile",    10,  levels));
            levels.Add(new LayoutLevel("XInTile",      3,   levels));
            levels.Add(new LayoutLevel("YInTile",      4,   levels));
            levels.Add(new LayoutLevel("TileColumn",   9,   levels));
            levels.Add(new LayoutLevel("TileRow",      999, levels));
        }

        public int[] position_on_screen(long index)
        {
	        int[] xy = new int[]{0, 0};
	        for (int i = 0; i < this.levels.Count; ++i)
            {
                LayoutLevel level = this.levels[i];
		        int part = i % 2;
                int coordinate_in_chunk = ((int)(index / level.chunk_size)) % level.modulo;
		        xy[part] += level.thickness * coordinate_in_chunk;
            }
            return xy;
        }

        public long index_from_screen(int x, int y)
        {
	        long index_from_xy = 0;
	        int[] xy_remaining = {x, y}; 
	        for (int i = this.levels.Count-1; i >= 0; --i) //reverse
            {
                LayoutLevel level = this.levels[i];
		        int part = i % 2;
		        int number_of_full_increments = (int)(xy_remaining[part] / level.thickness);
                index_from_xy += level.chunk_size * number_of_full_increments; // add total nulceotide size for every full increment of this level e.g. Tile Y height
		        xy_remaining[part] -= number_of_full_increments * level.thickness;  //subtract the credited coordinates to shift to relative coordinates in that level
            }
            return index_from_xy;
        }

        /** Similar to position_on_screen(index) but it instead returns the largest x and y values that the layout will need from
         * any index in between 0 and last_index.
         */ 
        public int[] max_dimensions(long last_index)
        {
            int[] xy = new int[] { 0, 0 };
            for (int i = 0; i < this.levels.Count; ++i)
            {
                LayoutLevel level = this.levels[i];
                int part = i % 2;
                int coordinate_in_chunk = Math.Min((int)(Math.Ceiling(last_index / (double)level.chunk_size)), level.modulo);  //how many of these will you need up to a full modulo worth
                if (coordinate_in_chunk > 1) { 
                    xy[part] = Math.Max(xy[part], level.thickness * coordinate_in_chunk); // not cumulative, just take the max size for either x or y
                }
            }
            return xy;
        }

        public string toString()
        {
            string json = "[";
            foreach( LayoutLevel level in this.levels)
            {
                json += "{" + "modulo: " + level.modulo + ", chunk_size:" + level.chunk_size + ", padding: " + level.padding + ", thickness: " + level.thickness + "},";
            }
            json = json.Substring(0, json.Length - 1) + "]";  // no last comma
                            
            return json;
        }


        public bool process_file(System.IO.StreamReader streamFASTAFile, BackgroundWorker worker, Bitmap b, BitmapData bmd)
        {
            int nucleotidesProcessed = 0;
            string read = "";
            while (((read = streamFASTAFile.ReadLine()) != null))
            {
                if (read != "")
                {
                    worker.ReportProgress(nucleotidesProcessed);

                    if (read.Substring(0, 1) == ">")
                    {

                    }

                    else
                    {
                        read = Utils.CleanInputFile(read);
                        read = Utils.ConvertToDigits(read);


                        //----------------------------New Tiled Layout style----------------------------------
                        int[] xy = { 0, 0 };
                        for (int c = 0; c < read.Length; c++)
                        {
                            xy = this.position_on_screen(nucleotidesProcessed++);
                            Utils.Write1BaseToBMPUncompressed4X(read[c], ref b, xy[0], xy[1], ref bmd);
                        }
                    }
                }
            }

            return true;
        }
    }


}
