﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace GNGSCT_Gen
{
    class Program
    {
        static void Sct()
        {
            List<string> lines = new List<string>();
            List<string> lline = new List<string>();
            List<string> Geolines = new List<string>();
            List<string> backline = new List<string>();
            string[] sline = new string[4];
            string line;
            string finishline;
            bool Regions = true;
            bool Geo = true;
            bool place = false;
            bool insert = false;
            int Geoline = 0;
            bool firstaxi = false;
            string tline = "Taxiway";
            WriteLine("Enter sector name:");
            string name = ReadLine();
            StreamReader file = new StreamReader(name + ".sct");
            bool xv = true;
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("[GEO]"))
                {
                    Regions = false;
                    Geo = true;
                }
                else if (line.StartsWith("[REGIONS]"))
                {
                    if (Geo)
                    {
                        foreach (var l in backline)
                        {
                            Geolines.Add(l);
                        }
                        place = false;
                        insert = false;
                        backline.Clear();
                    }
                    Regions = true;
                    Geo = false;
                }
                else if (Geo)
                {
                    if (line.StartsWith(";") && place)
                    {
                        foreach (var l in backline)
                        {
                            Geolines.Add(l);
                        }
                        place = false;
                        insert = false;
                        backline.Clear();
                    }
                    if (line.StartsWith(";"))
                    {
                        Geolines.Add("REGIONNAME " + name + "_GROUND");
                        place = true;
                        tline = line;
                        tline = tline.TrimStart("; - ".ToCharArray());
                    }
                    else if (place)
                    {
                        lline = line.Split(' ').ToList();
                        lline.RemoveAt(lline.Count - 1);
                        if (!insert)
                        {
                            if (tline.StartsWith("Taxiway"))
                            {
                                lline.Insert(0, "COLOR_Taxiway");
                            }
                            else if (tline.StartsWith("HS"))
                            {
                                lline.Insert(0, "COLOR_Stopbar");
                            }
                            Geolines.Add(lline[0] + ' ' + lline[1] + ' ' + lline[2]);
                            backline.Insert(0, lline[1] + ' ' + lline[2]);
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            insert = true;
                        }
                        if (lline.Count >= 2)
                        {
                            if (!(lline[0] + ' ' + lline[1]).Equals(Geolines[Geolines.Count - 1]))
                            {
                                Geolines.Add(lline[0] + ' ' + lline[1]);
                                backline.Insert(0, lline[0] + ' ' + lline[1]);
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                            else
                            {
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                        }
                        if (lline.Count >= 2)
                        {
                            if (!(lline[0] + ' ' + lline[1]).Equals(Geolines[Geolines.Count - 1]))
                            {
                                Geolines.Add(lline[0] + ' ' + lline[1]);
                                backline.Insert(0, lline[0] + ' ' + lline[1]);
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                            else
                            {
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                        }
                    }
                }
                if (Regions)
                {
                    if (xv)
                    {
                        if (!(lline.Count == 0)) { WriteLine("ERROR, last item not complete: " + lline.Count); foreach (var y in lline) { WriteLine(y); } }
                        xv = false;
                    }
                    if (line.StartsWith(";"))
                    {
                        lines.Add("REGIONNAME " + name + "_GROUND");
                    }
                    else
                    {
                        if (line.StartsWith("Runway"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("Runway"))
                            {
                                lline[0] = "COLOR_RunwayConcrete";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else if (line.StartsWith("Taxiway"))
                        {
                            if (!firstaxi)
                            {
                                firstaxi = true;
                                Geoline = lines.Count;
                            }
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("Taxiway"))
                            {
                                lline[0] = "COLOR_Taxiway";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else if (line.StartsWith("HS"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("HS"))
                            {
                                lline[0] = "COLOR_Stopbar";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else if (line.StartsWith("Apron"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("Apron"))
                            {
                                lline[0] = "COLOR_HardSurface1";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else if (line.StartsWith("Overlay"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("Overlay"))
                            {
                                lline[0] = "COLOR_GrasSurface2";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else if (line.StartsWith("Building"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("Building"))
                            {
                                lline[0] = "COLOR_Building";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else if (line.StartsWith("Inter"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(1);
                            if (lline[0].StartsWith("Inter"))
                            {
                                lline[0] = "COLOR_HardSurface2";
                                finishline = $"{lline[0]} {lline[1]} {lline[2]}";
                                lines.Add(finishline);
                            }
                        }
                        else
                        {
                            if (!line.Equals("[REGIONS]"))
                            {
                                lines.Add(line);
                            }
                        }
                    }
                }
            }
            file.Close();
            Geolines.Reverse();
            foreach (var x in Geolines)
            {
                lines.Insert((Geoline - 1), x);
            }
            StreamWriter newfile = new StreamWriter(name + "_new.sct");
            foreach (var lin in lines)
            {
                newfile.WriteLine(lin);
            }
            newfile.Close();
            WriteLine("Press any key to continue..."); ReadKey();
        }
        static void Gng()
        {
            List<string> lines = new List<string>();
            List<string> lline = new List<string>();
            List<string> Geolines = new List<string>();
            List<string> backline = new List<string>();
            string[] sline = new string[4];
            string line;
            string finishline;
            bool Regions = true;
            bool Geo = true;
            bool place = false;
            bool insert = false;
            int Geoline = 0;
            bool firstaxi = false;
            string tline = "Taxiway";
            WriteLine("Enter sector name:");
            string name = ReadLine();
            StreamReader file = new StreamReader(name + ".sct");
            bool xv = true;
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("[GEO]"))
                {
                    Regions = false;
                    Geo = true;
                }
                else if (line.StartsWith("[REGIONS]"))
                {
                    if (Geo)
                    {
                        foreach (var l in backline)
                        {
                            Geolines.Add(l);
                        }
                        place = false;
                        insert = false;
                        backline.Clear();
                    }
                    Regions = true;
                    Geo = false;
                }
                else if (Geo)
                {
                    if (line.StartsWith(";") && place)
                    {
                        foreach (var l in backline)
                        {
                            Geolines.Add(l);
                        }
                        place = false;
                        insert = false;
                        backline.Clear();
                    }
                    if (line.StartsWith(";"))
                    {
                        tline = line;
                        tline = tline.TrimStart("; - ".ToCharArray());
                        line = line + ";";
                        Geolines.Add(line);
                        place = true;
                    }
                    else if (place)
                    {
                        lline = line.Split(' ').ToList();
                        lline.RemoveAt(lline.Count - 1);
                        if (!insert)
                        {
                            if (tline.StartsWith("Taxiway"))
                            {
                                Geolines.Add("COLOR_Taxiway");
                            }
                            if (tline.StartsWith("HS"))
                            {
                                Geolines.Add("COLOR_Stopbar");
                            }
                            Geolines.Add(lline[0] + ' ' + lline[1]);
                            backline.Insert(0, lline[0] + ' ' + lline[1]);
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            insert = true;
                        }
                        if (lline.Count >= 2)
                        {
                            if (!(lline[0] + ' ' + lline[1]).Equals(Geolines[Geolines.Count - 1]))
                            {
                                Geolines.Add(lline[0] + ' ' + lline[1]);
                                backline.Insert(0, lline[0] + ' ' + lline[1]);
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                            else
                            {
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                        }
                        if (lline.Count >= 2)
                        {
                            if (!(lline[0] + ' ' + lline[1]).Equals(Geolines[Geolines.Count - 1]))
                            {
                                Geolines.Add(lline[0] + ' ' + lline[1]);
                                backline.Insert(0, lline[0] + ' ' + lline[1]);
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                            else
                            {
                                lline.RemoveAt(0);
                                lline.RemoveAt(0);
                            }
                        }
                    }
                }
                if (Regions)
                {
                    if (xv)
                    {
                        if (!(lline.Count == 0)) { WriteLine("ERROR, last item not complete: " + lline.Count); foreach (var y in lline) { WriteLine(y); } }
                        xv = false;
                    }
                    if (line.StartsWith(";"))
                    {
                        line = line + ";";
                        lines.Add(line);
                    }
                    else
                    {
                        if (line.StartsWith("Runway"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_RunwayConcrete");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);
                        }
                        else if (line.StartsWith("Taxiway"))
                        {
                            if (!firstaxi)
                            {
                                firstaxi = true;
                                Geoline = lines.Count;
                            }
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_Taxiway");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);
                        }
                        else if (line.StartsWith("HS"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_Stopbar");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);

                        }
                        else if (line.StartsWith("Apron"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_HardSurface1");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);
                        }
                        else if (line.StartsWith("Overlay"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_GrasSurface2");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);
                        }
                        else if (line.StartsWith("Building"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_Building");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);
                        }
                        else if (line.StartsWith("Inter"))
                        {
                            lline = line.Split(' ').ToList();
                            lline.RemoveAt(0);
                            lline.RemoveAt(0);
                            lines.Add("COLOR_HardSurface2");
                            finishline = $"{lline[0]} {lline[1]}";
                            lines.Add(finishline);
                        }
                        else
                        {
                            if (!line.Equals("[REGIONS]"))
                            {
                                lines.Add(line);
                            }
                        }
                    }
                }
            }
            file.Close();
            Geolines.Reverse();
            foreach (var x in Geolines)
            {
                lines.Insert((Geoline - 1), x);
            }
            StreamWriter newfile = new StreamWriter(name + "_GNG.sct");
            foreach (var lin in lines)
            {
                newfile.WriteLine(lin);
            }
            newfile.Close();
            WriteLine("Press any key to continue..."); ReadKey();
        }
        static void Main(string[] args)
        {
            int press = 0;
            WriteLine("Press 1 to Generate sct file and press 2 to generate GNG file");
            press = int.Parse(ReadLine());
            if (press == 1)
            {
                Sct();
            }
            else if (press == 2)
            {
                Gng();
            }
            else
            {
                WriteLine("Please enter a valid number");
            }
        }
    }
}