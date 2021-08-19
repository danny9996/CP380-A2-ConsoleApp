using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;
using System.Collections.Generic;


namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers
                .Include(i => i.Reviews)
                .AsEnumerable()
                .Select(re => new {
                    Reviews = re.Reviews.Count,
                    Average = Math.Round(re.Reviews.Average(s => s.stars), 2),
                    Adjust = Math.Round(ratingAdjustmentService.Adjust(re.Reviews.Average(s => s.stars), re.Reviews.Count()), 2),
                    re.title
                }).OrderByDescending(a => a.Adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output
                Console.WriteLine("[{0}] {1} {2} {3} {4}", j + 1, i.Reviews, i.Average, i.Adjust, i.title);
            }
        }
    }
}
