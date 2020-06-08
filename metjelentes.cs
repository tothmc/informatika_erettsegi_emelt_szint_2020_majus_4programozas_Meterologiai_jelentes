using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace metjelentes
{
    class Jelentes
    {
        private string telepules;
        private byte ora;
        private byte perc;
        private string szelirany;
        private byte szelerosseg;
        private byte homerseklet;

        public byte Homerseklet
        {
            get { return homerseklet; }
            set { homerseklet = value; }
        }
        public byte Szelerosseg
        {
            get { return szelerosseg; }
            set { szelerosseg = value; }
        }
        public string Szelirany
        {
            get { return szelirany; }
            set { szelirany = value; }
        }
        public string Telepules
        {
            get { return telepules; }
            set { telepules = value; }
        }
        public byte Ora
        {
            get { return ora; }
            set { ora = value; }
        }
        public byte Perc
        {
            get { return perc; }
            set { perc = value; }
        }

        public Jelentes(string _sor) {
            string[] aktsor = _sor.Split();

            this.telepules = aktsor[0];
            this.ora = byte.Parse(aktsor[1].Substring(0,2));
            this.perc = byte.Parse(aktsor[1].Substring(2,2));
            this.szelirany = aktsor[2].Substring(0,3);
            this.szelerosseg = byte.Parse(aktsor[2].Substring(3,2));
            this.homerseklet = byte.Parse(aktsor[3]);
        }
    }

    class Metjelentes
    {
        private static List<Jelentes> jelentesek = new List<Jelentes>();
        static void Main(string[] args)
        {
            #region
            /* code by: Tóth Marcell */
            #endregion
            Feladat1(ref jelentesek);
            Feladat2(ref jelentesek);
            Feladat3(ref jelentesek);
            Feladat4(ref jelentesek);
            Feladat5(ref jelentesek);
            Feladat6(ref jelentesek);
            Console.ReadKey();
        }

        private static void Feladat6(ref List<Jelentes> jelentesek)
        {
            string utvonal;
            var csoportTelepulekkent = jelentesek.GroupBy(x => x.Telepules);
            foreach (var csoport in csoportTelepulekkent)
            {
                utvonal = csoport.Key + ".txt";
                FileStream fs = new FileStream(utvonal, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(csoport.Key);
                foreach (var log in csoport)
                {
                    sw.Write("{0:D2}:{1:D2} ", log.Ora, log.Perc);
                    for (int i = 0; i < log.Szelerosseg; i++)
                    {
                        sw.Write("#");
                    }
                    sw.WriteLine();
                }
                sw.Close();
                fs.Close();
            }
            Console.WriteLine("6. feladat");
            Console.WriteLine("A fájlok elkészültek.");
        }

        private static void Feladat5(ref List<Jelentes> jelentesek)
        {
            Console.WriteLine("5.feladat");
            var csoportTelepulesenkent = jelentesek.GroupBy(x => x.Telepules);

            foreach (var csoport in csoportTelepulesenkent) 
            {
                double kozepHo = 0;
                int osszeg = 0;
                int db = 0;
                byte minimum = jelentesek.Where(x => x.Telepules == csoport.Key).Min(x=>x.Homerseklet);
                byte maximum = jelentesek.Where(x => x.Telepules == csoport.Key).Max(x => x.Homerseklet);
                bool nincsAdat = false;
                int dbJelentes1ora = jelentesek.Where(x => x.Telepules == csoport.Key).Where(x => x.Ora == 1).Count();
                int dbJelentes7ora = jelentesek.Where(x => x.Telepules == csoport.Key).Where(x => x.Ora == 7).Count();
                int dbJelentes13ora = jelentesek.Where(x => x.Telepules == csoport.Key).Where(x => x.Ora == 13).Count();
                int dbJelentes19ora = jelentesek.Where(x => x.Telepules == csoport.Key).Where(x => x.Ora == 19).Count();

                Console.Write(csoport.Key + " ");
                
                foreach (var log in csoport)
                {
                    if (log.Ora == 1 || log.Ora == 7 || log.Ora == 13 || log.Ora == 19 )
                    {
                        osszeg += log.Homerseklet;
                        db++;
                    }
                }
                if (dbJelentes1ora == 0 || dbJelentes7ora == 0 || dbJelentes13ora == 0 || dbJelentes19ora == 0)
                {
                    nincsAdat = true;
                }

                if (nincsAdat)
                {
                    Console.Write("NA; ");
                }
                else 
                {
                    kozepHo = Math.Round(((double)osszeg / db),0);
                    Console.Write("Középhőmérséklet {0}; ", kozepHo);
                }

                Console.Write("Hőmérséklet-ingadozás: {0} ", maximum - minimum);
                /* 
                Console.Write(" Jelentések száma 1-7-13-19 órakor: {0} {1} {2} {3}", dbJelentes1ora, dbJelentes7ora, dbJelentes13ora, dbJelentes19ora);
                */
                Console.WriteLine();
            }

        }

        private static void Feladat4(ref List<Jelentes> jelentesek)
        {
            Console.WriteLine("4. feladat");
            var szelcsend = jelentesek.Where(x => x.Szelirany == "000").Where(x => x.Szelerosseg == 0);
                        
            if (szelcsend.Count() != 0)
            {
                foreach (var log in szelcsend)
                {
                    Console.WriteLine("{0} {1:D2}:{2:D2}", log.Telepules, log.Ora, log.Perc);
                }
            }
            else { Console.WriteLine("Nem volt szélcsend a mérések idején!"); }
        }

        private static void Feladat3(ref List<Jelentes> jelentesek)
        {
            //Min és max meghatározása
            var minimum = jelentesek.OrderBy(x => x.Homerseklet).First();
            var maximum = jelentesek.OrderByDescending(x => x.Homerseklet).First();

            //Kiiratás
            Console.WriteLine("3. feladat");
            Console.WriteLine("A legalacsonyabb hőmérséklet: {0} {1}:{2} {3} fok", minimum.Telepules, minimum.Ora, minimum.Perc, minimum.Homerseklet);
            Console.WriteLine("A legmagasabb hőmérséklet: {0} {1}:{2} {3} fok", maximum.Telepules, maximum.Ora, maximum.Perc, maximum.Homerseklet);
        }

        private static void Feladat2(ref List<Jelentes> jelentesek)
        {
            //Település bekérése
            string bekertTelepules;

            Console.WriteLine("2. feladat");
            Console.Write("Adja meg egy település kódját! Település: ");
            bekertTelepules = Console.ReadLine();

            //Kiiratás
            var utolsolog = jelentesek.Where(x => x.Telepules == bekertTelepules).Last();
            Console.WriteLine("Az utolsó mérési adat a megadott településről {0:D2}:{1:D2}-kor érkezett.", utolsolog.Ora, utolsolog.Perc);
        }

        private static void Feladat1(ref List<Jelentes> jelentesek)
        {
            FileStream fs = new FileStream("tavirathu13.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);

            while (!sr.EndOfStream)
            {
                jelentesek.Add(new Jelentes(sr.ReadLine()));
            }

            sr.Close();
            fs.Close();
        }
    }
}
