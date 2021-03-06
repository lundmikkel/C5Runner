﻿using System;
using System.Diagnostics;
using System.Linq;
using C5.Intervals;

namespace C5.Ucsc
{
    class Program
    {
        public static void Main(string[] args)
        {
            /*
            Console.WriteLine("Parsing raw maf-file.");

            UcscHumanGenomeParser.ParseMafToAlignments("Y:/Downloads/chr1.maf", "Y:/Downloads/chr1.compressed.txt");
            
            /*/

            var sw = new Stopwatch();
            sw.Start();
            var intervals = UcscHumanGenomeParser.ParseCompressedMaf("Y:/Downloads/chr1.compressed.txt").ToArray();
            Sorting.Timsort(intervals, IntervalExtensions.CreateComparer<UcscHumanGenomeParser.SequenceInterval, int>());
            Console.WriteLine("Reading the intervals: " + sw.ElapsedMilliseconds + " ms");

            sw.Restart();
            var lclist = new LayeredContainmentList<UcscHumanGenomeParser.SequenceInterval, int>(intervals);
            Console.WriteLine("LCList: " + sw.ElapsedMilliseconds + " ms");
            Console.Out.WriteLine("Count: " + lclist.Count);
            Console.Out.WriteLine("Contained intervals: " + lclist.ContainmentCount);
            Console.Out.WriteLine("Layers: " + lclist.ContainmentDegree);
            //Console.Out.WriteLine(lclist.MaximumDepth);
            //Console.Out.WriteLine(lclist.CountOverlaps(270000000));

            sw.Restart();
            var nclist = new NestedContainmentList<UcscHumanGenomeParser.SequenceInterval, int>(intervals);
            Console.WriteLine("NCList: " + sw.ElapsedMilliseconds + " ms");


            //*/

            //Console.Out.WriteLine("Number of intervals: " + File.ReadLines(@"//VBOXSVR/maf/parsed/chr1.compressed.maf").Count());
            //UcscHumanGenomeParser.ParseMaf("//VBOXSVR/maf/raw/chr1.maf", "//VBOXSVR/maf/parsed/maf_compressed.txt");
            //UcscHumanGenomeParser.ParseMaf("../../Data/maf.txt", "../../Data/maf_compressed.txt");
            /*
            var genes = UcscHumanGenomeParser.ParseFile("../../Data/ucsc-human-default.txt");
            Console.Out.WriteLine("Creating LCList");
            var lclist = new LayeredContainmentList<UcscHumanGene, GenomePosition>(genes);
            Console.Out.WriteLine("Done creating LCList");
            Console.Out.WriteLine(lclist.Count);
             */

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
