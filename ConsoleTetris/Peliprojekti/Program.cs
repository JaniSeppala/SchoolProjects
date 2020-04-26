using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Threading;

namespace Tetris
{
    class Program
    {
        //Lista ns."Gloobaleista muuttujista"
        private static string Directory = System.IO.Directory.GetCurrentDirectory() + "\\highscores.score";//Haetaan polku highscores.score tiedostolle. Tätä käytetään tallennettujen ennätyspisteiden hakemiseen
        private static Tile[] tetrisBlock = new Tile[4]; //Lista joka sisältää pelialueella olevan tetris-palikan tiedot
        private static Tile[] nextBlock = new Tile[4]; //Lista joka sisältää seuraavan pelattavaksi tulevan tetris-palikan tiedot
        private static List<Tile> tiles = new List<Tile>(); //Lista pelialueella olevista palikoista
        private static System.Timers.Timer timer = new System.Timers.Timer(); //Ajastin jolla määritetään aika jonka kuluttua palikkaa siirretään yksi rivi alaspäin
        private static bool blockDown = false; //Tieto siitä kun palikka osuu toiseen palikkaan tai pelialueen pohjaan kun sitä yritetään siirtää yhtä riviä alemmas
        private static bool soundEnabled = true;//Kerrotaan ohjelmalle ovatko äänet käytössä
        private static int blockType; //Tetrispalikan tyyppi
        private static int nextBlockType; //Seuraavan tetrispalikan tyyppi
        private static int blockOrientation; //Tetrispalikan asento
        private static ConsoleColor blockColor; //Tetrispalikan väri
        private static readonly ConsoleColor[] blockColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor)); //Lista kaikista väreistä
        private static bool gameOver = false; //Tieto siitä että pelin lopetusehdot ovat täyttyneet
        private static int interval; //Palikan pudotusnopeutta säätelevä muuttuja
        private static int lines = 0; //Rivilaskuri joka pitää kirjaa montako riviä on tuhottu
        private static int level = 0; //Tasolaskuri joka pitää kirjaa siitä millä levelillä ollaan
        private static bool timerEvent = false;//Tieto siitä että Timer_Event() metodi on käynnissä
        private static bool userEvent = false;//Tieto siitä että pelaaja on painanut jotain näppäintä ja sen komennon suoritus on kesken
        private static int score = 0;//Pistelaskuri
        private static List<Highscore> Highscores = new List<Highscore>();//Lista joka sisältää top10 parhaat pisteet
        //Laskurit jotka pitävät kirjaa kuinka monta mitäkin palikkaa on pelattu sessiossa
        private static int iBlockCounter = 0;
        private static int jBlockCounter = 0;
        private static int lBlockCounter = 0;
        private static int oBlockCounter = 0;
        private static int sBlockCounter = 0;
        private static int zBlockCounter = 0;
        private static int tBlockCounter = 0;

        static void Main(string[] args)
        {
            Console.Title = "Command-Line Tetris";//Annetaan konsoli-ikkunalle nimi
            timer.Elapsed += Timer_Elapsed;//Asetetaan timerin eventti
            timer.AutoReset = true;//Määritetään timer aloittamaan laskemisen alusta jokaisen Timer.Elapsed eventin jälkeen
            MainMenu();//Käynnistetään päävalikko
        }

        //Tässä metodissa luodaan uusi tetrsipalikka satunnaisesti ja arvotaan sille satunnainen väri. Palikka tallennetaan nextBlock-listaan
        static void CreateTetrisBlock()
        {   
            //Arvotaan satunnainen väri seuraavalle palikalle mutta skipataan värit musta, tumman sininen ja valkoinen(indeksit 0, 1 ja 15)
            Random random = new Random();
            blockColor = blockColors[random.Next(2, 15)];

            //Valitaan satunnaisesti minkälainen palikka luodaan ja tallennetaan palikan osien kordinaatit ja värit nextBlock listaan.
            nextBlockType = random.Next(0, 7); 
            if (nextBlockType == 0) //Luodaan I-palikka
            {
                Tile tile = new Tile();
                tile.X = 4;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.X = 7;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(13, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("    ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
            else if (nextBlockType == 1) //Luodaan J-palikka
            {
                Tile tile = new Tile();
                tile.X = 4;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.Y = -1;
                tile.X = 6;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(13, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("   ");
                Console.SetCursorPosition(15, 18);
                Console.Write(" ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
            else if (nextBlockType == 2) //Luodaan L-palikka
            {
                Tile tile = new Tile();
                tile.X = 4;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.Y = -1;
                tile.X = 4;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(13, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("   ");
                Console.SetCursorPosition(13, 18);
                Console.Write(" ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
            else if (nextBlockType == 3) //Luodaan O-palikka
            {
                Tile tile = new Tile();
                tile.X = 5;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(13, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("  ");
                Console.SetCursorPosition(13, 18);
                Console.Write("  ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
            else if (nextBlockType == 4) //Luodaan S-palikka
            {
                Tile tile = new Tile();
                tile.X = 5;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.X = 4;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(14, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("  ");
                Console.SetCursorPosition(13, 18);
                Console.Write("  ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
            else if (nextBlockType == 5) //Luodaan Z-palikka
            {
                Tile tile = new Tile();
                tile.X = 4;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(13, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("  ");
                Console.SetCursorPosition(14, 18);
                Console.Write("  ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
            else if (nextBlockType == 6) //Luodaan T-palikka
            {
                Tile tile = new Tile();
                tile.X = 4;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[0] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[1] = tile;
                tile = new Tile();
                tile.X = 6;
                tile.Y = -2;
                tile.Color = blockColor;
                nextBlock[2] = tile;
                tile = new Tile();
                tile.X = 5;
                tile.Y = -1;
                tile.Color = blockColor;
                nextBlock[3] = tile;
                Console.SetCursorPosition(13, 17);
                Console.BackgroundColor = tile.Color;
                Console.Write("   ");
                Console.SetCursorPosition(14, 18);
                Console.Write(" ");
                Console.ResetColor();
                Console.SetCursorPosition(15, 0);
            }
        }

        //Tässä metodissa piirretään pelialueen speksit jonka jälkeen jäädään looppaamaan gamplay-looppia kunnes gameOver ehdot täyttyvät
        static void StartGame()
        {
            Console.Clear(); //Tyhjennetään ikkuna
            Console.CursorVisible = false; //Piilotetaan kursori

            //Alustetaan kaikki pelisession muuttujat ettei edellisestä pelisessiosta tulevia tietoja päädy uuteen pelisessioon
            gameOver = false;
            nextBlock = new Tile[4];
            tetrisBlock = new Tile[4];
            tiles.Clear();
            lines = 0;
            score = 0;
            iBlockCounter = 0;
            jBlockCounter = 0;
            lBlockCounter = 0;
            oBlockCounter = 0;
            sBlockCounter = 0;
            zBlockCounter = 0;
            tBlockCounter = 0;

            //Piirretään pelialue
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine(" ");
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("..........");
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(" \n            ");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(13, 1);
            Console.Write("SCORE");
            Console.SetCursorPosition(13, 2);
            Console.WriteLine(score);
            Console.SetCursorPosition(13, 4);
            Console.Write("LEVEL:"+level);
            Console.SetCursorPosition(13, 5);
            Console.Write("LINES:"+lines);
            Console.SetCursorPosition(13, 7);
            Console.Write("I-Blocks:"+iBlockCounter);
            Console.SetCursorPosition(13, 8);
            Console.Write("J-Blocks:" + jBlockCounter);
            Console.SetCursorPosition(13, 9);
            Console.Write("L-Blocks:" + lBlockCounter);
            Console.SetCursorPosition(13, 10);
            Console.Write("O-Blocks:" + oBlockCounter);
            Console.SetCursorPosition(13, 11);
            Console.Write("S-Blocks:" + sBlockCounter);
            Console.SetCursorPosition(13, 12);
            Console.Write("Z-Blocks:" + zBlockCounter);
            Console.SetCursorPosition(13, 13);
            Console.Write("T-Blocks:" + tBlockCounter);
            Console.SetCursorPosition(13, 15);
            Console.Write("NEXT");
            Console.SetCursorPosition(15, 0);

            CreateTetrisBlock(); //Luodaan ensimmäinen nextBlock tetris-palikka
            tetrisBlock = nextBlock;//Kopioidaan palikan tiedot tetrisBlockin
            blockType = nextBlockType;//Kopioidaan pelattavan palikan tyyppi nextBlockin tyypistä
            blockOrientation = 0; //Alustetaan palikan asento oletusasentoon

            //Tallennetaan ensimmäisen pelattavan palikan tiedot palikan tyypin mukaan palikkalaskuriin ja kirjoitetaan muuttunut palikkamäärä pelialueen palikkalaskuriin
            if (blockType == 0)
            {
                iBlockCounter++;
                Console.SetCursorPosition(22, 7);
                Console.Write(iBlockCounter);
                Console.SetCursorPosition(15, 0);
            }
            else if (blockType == 1)
            {
                jBlockCounter++;
                Console.SetCursorPosition(22, 8);
                Console.Write(jBlockCounter);
                Console.SetCursorPosition(15, 0);
            }
            else if (blockType == 2)
            {
                lBlockCounter++;
                Console.SetCursorPosition(22, 9);
                Console.Write(lBlockCounter);
                Console.SetCursorPosition(15, 0);
            }
            else if (blockType == 3)
            {
                oBlockCounter++;
                Console.SetCursorPosition(22, 10);
                Console.Write(oBlockCounter);
                Console.SetCursorPosition(15, 0);
            }
            else if (blockType == 4)
            {
                sBlockCounter++;
                Console.SetCursorPosition(22, 11);
                Console.Write(sBlockCounter);
                Console.SetCursorPosition(15, 0);
            }
            else if (blockType == 5)
            {
                zBlockCounter++;
                Console.SetCursorPosition(22, 12);
                Console.Write(zBlockCounter);
                Console.SetCursorPosition(15, 0);
            }
            else if (blockType == 6)
            {
                tBlockCounter++;
                Console.SetCursorPosition(22, 13);
                Console.Write(tBlockCounter);
                Console.SetCursorPosition(15, 0);
            }


            nextBlock = new Tile[4];//Tyhjennetään nextBlock
            CreateTetrisBlock();//Luodaan uusi nextBlock tetris-palikka

            //Käynnistetään uusi säie GamePlayLoop()-metodia varten ja jäädään odottamaan sen valmistumista. Kun GamePlayLoop() on suoritettu niin ajetaan GameOver()-metodi
            Thread gameplayLoop = new Thread(new ThreadStart(GamePlayLoop));
            gameplayLoop.Start();
            gameplayLoop.Join();
            GameOver();
        }

        //Tämä metodi ajetaan aina kun on aika pakottaa palikkaa yksi rivi alaspäin.
        //Ensin ilmoitetaan GamePlayLoopille että timerEventti on käynnistynyt muutamalla timerEventin arvo todeksi. Tämän jälkeen odotetaan mikäli pelaajan komentoa käsitellään tällä hetkellä. Lopuksi ajetaan Descent() metodi ja kun se on ajettu loppuun niin muutetaab timerEventin arvo falseksi
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timerEvent = true; //Ilmoitetaan ohjelmalle että ajastettu toiminto on käynnissä

            //Jos käyttäjän komennon suorittaminen on kesken niin ajetaan tyhjää looppia
            while (userEvent)
            {
                //Odotetaan että käyttäjän komento on suoritettu loppuun
            }
            //Tämä ajetaan vain mikäli gameOver ehdot eivät ole täyttyneet
            if (gameOver == false)
            {
                Descent(); //Suoritetaan Descent metodi
            }
            timerEvent = false; //Ilmoitetaan ohjelmalle että ajastettu toiminto ei ole enää käynnissä
        }

        //Tässä metodissa käsitellään kaikki mitä tapahtuu kun palikkaa yritetään siirtää yksi rivi alaspäin.
        static void Descent()
        {
            //Ajetaan jos pelin lopetus ehdot eivät ole täyttyneet
            if (gameOver == false)
            {
                //Ajetaan niin monta kertaa että kaikki terispalikan osaset on käyty läpi
                foreach (Tile tile in tetrisBlock)
                {
                    //Jos tetris-palikka on alimmalla rivillä
                    if (tile.Y == 19)
                    {
                        //Tässä tyhjennetään pelialueelta seuraavan palikan kuva
                        Console.SetCursorPosition(13, 17);
                        Console.Write("    ");
                        Console.SetCursorPosition(13, 18);
                        Console.Write("    ");
                        Console.SetCursorPosition(15, 0);

                        tiles.AddRange(tetrisBlock); //Lisätään tetrsipalikan kordinaatit pelialueella olevien palikoiden listaan
                        tetrisBlock = nextBlock; //Siirretään seuraavan palikan tiedot pelattavan tetrispalikan tietoihin
                        blockType = nextBlockType; //Siirretään tieto seuraavan palikan tyypistä pelattavan palikan tyyppiin
                        nextBlock = new Tile[4]; //Tyhjennetään tieto seuraavasta palikasta
                        CreateTetrisBlock(); //Luodaan uusi seuraavaksi pelattava palikka CreateTetrisBlock metodilla
                        blockOrientation = 0; //Alustetaan pelattavan palikan asento oletusasentoon

                        //Lisätään palikkalaskuriin uusi pelattava palikka
                        if (blockType == 0)//Jos kyseessä on I-palikka
                        {
                            iBlockCounter++;//Lisätään laskuriin yksi lisää
                            Console.SetCursorPosition(22, 7);//Siirretään osoitin oikealle kohdalle
                            Console.Write(iBlockCounter);//Kirjoitetaan laskurin tieto pelialueelle
                            Console.SetCursorPosition(15, 0);//Resetoidaan kursorin sijainti
                        }
                        else if (blockType == 1)//Jos kyseessä J-palikka
                        {
                            jBlockCounter++;
                            Console.SetCursorPosition(22, 8);
                            Console.Write(jBlockCounter);
                            Console.SetCursorPosition(15, 0);
                        }
                        else if (blockType == 2)//Jos kyseessä L-palikka
                        {
                            lBlockCounter++;
                            Console.SetCursorPosition(22, 9);
                            Console.Write(lBlockCounter);
                            Console.SetCursorPosition(15, 0);
                        }
                        else if (blockType == 3)//Jos kyseessä O-palikka
                        {
                            oBlockCounter++;
                            Console.SetCursorPosition(22, 10);
                            Console.Write(oBlockCounter);
                            Console.SetCursorPosition(15, 0);
                        }
                        else if (blockType == 4)//Jos kyseessä S-palikka
                        {
                            sBlockCounter++;
                            Console.SetCursorPosition(22, 11);
                            Console.Write(sBlockCounter);
                            Console.SetCursorPosition(15, 0);
                        }
                        else if (blockType == 5)//Jos kyseessä Z-palikka
                        {
                            zBlockCounter++;
                            Console.SetCursorPosition(22, 12);
                            Console.Write(zBlockCounter);
                            Console.SetCursorPosition(15, 0);
                        }
                        else if (blockType == 6)//Jos kyseessä T-palikka
                        {
                            tBlockCounter++;
                            Console.SetCursorPosition(22, 13);
                            Console.Write(tBlockCounter);
                            Console.SetCursorPosition(15, 0);
                        }
                        blockDown = true;//Ilmoitetaan ohjelmalle että palikka on laskeutunut
                        break;//Poistutaan foreach loopista
                    }

                    //Ajetaan niin monta kertaa että kaikki pelialueella olevat palikat on käyty läpi
                    foreach (Tile block in tiles)
                    {
                        //Ajetaan jos tetrispalikan jonkun osan kordinaatit ovat samat kuin jonkin pelialueella olevan palikan kordinaatit
                        if (tile.X == block.X && tile.Y == block.Y - 1)
                        {
                            //Tyhjennetään seuraavan palikan kuva pelialueelta
                            Console.SetCursorPosition(13, 17);
                            Console.Write("    ");
                            Console.SetCursorPosition(13, 18);
                            Console.Write("    ");
                            Console.SetCursorPosition(15, 0);

                            tiles.AddRange(tetrisBlock);//Lisätään pelattavan tetris palikan tiedot pelialueella olevien palikoiden tietoihin
                            tetrisBlock = nextBlock;//Lisätään seuraavan palikan tiedot pelattavan palikan tietoihin
                            blockType = nextBlockType; //Muutetaan pelattavan palikan tyyppi seuraavan palikan tyypiksi
                            nextBlock = new Tile[4]; //Tyhjennetään seuraavan palikan tiedot
                            CreateTetrisBlock(); //Luodaan uusi seuraava palikka CreateTetrisBlock metodilla
                            blockOrientation = 0; //Alustetaan palikan asento oletusasentoon

                            //Lisätään palikkalaskuriin uusi pelattava palikka
                            if (blockType == 0)//Jos kyseessä on I-palikka
                            {
                                iBlockCounter++;//Lisätään laskuriin yksi lisää
                                Console.SetCursorPosition(22, 7);//Siirretään kursori oikeaan kohtaan pelialueella
                                Console.Write(iBlockCounter);//Kirjoitetaan pelialueelle laskurin summa
                                Console.SetCursorPosition(15, 0);//Palautetaan kursori oletussijaintiinsa
                            }
                            else if (blockType == 1)//Jos kyseessä on J-palikka
                            {
                                jBlockCounter++;
                                Console.SetCursorPosition(22, 8);
                                Console.Write(jBlockCounter);
                                Console.SetCursorPosition(15, 0);
                            }
                            else if (blockType == 2)//Jos kyseessä on L-palikka
                            {
                                lBlockCounter++;
                                Console.SetCursorPosition(22, 9);
                                Console.Write(lBlockCounter);
                                Console.SetCursorPosition(15, 0);
                            }
                            else if (blockType == 3)//Jos kyseessä on O-palikka
                            {
                                oBlockCounter++;
                                Console.SetCursorPosition(22, 10);
                                Console.Write(oBlockCounter);
                                Console.SetCursorPosition(15, 0);
                            }
                            else if (blockType == 4)//Jos kyseessä on S-palikka
                            {
                                sBlockCounter++;
                                Console.SetCursorPosition(22, 11);
                                Console.Write(sBlockCounter);
                                Console.SetCursorPosition(15, 0);
                            }
                            else if (blockType == 5)//Jos kyseessä on Z-palikka
                            {
                                zBlockCounter++;
                                Console.SetCursorPosition(22, 12);
                                Console.Write(zBlockCounter);
                                Console.SetCursorPosition(15, 0);
                            }
                            else if (blockType == 6)//Jos kyseessä on T-palikka
                            {
                                tBlockCounter++;
                                Console.SetCursorPosition(22, 13);
                                Console.Write(tBlockCounter);
                                Console.SetCursorPosition(15, 0);
                            }
                            blockDown = true;//Ilmoitetaan ohjelmalle että tetrispalikka on laskeutunut ja uusi tetrispalikka on luotu
                            break;//Poistutaan loopista
                        }
                    }

                    //Poistutaan loopista jos tetrispalikka on laskeutunut ja uusi tetrispalikka on luotu
                    if (blockDown)
                    {
                        break;
                    }

                }

                //Ajetaan jos palikka on laskeutunut ja uusi tetrispalikka on luotu
                if (blockDown)
                {
                    int rowCounter = 0;//Alustetaan rivilaskuri
                    int highest = tiles.Min(x => x.Y);//Etsitään pelialueella oleva palikka joka on kaikkein korkeimmalla rivillä

                    //Ajetaan jos korkeimmalla oleva palikka on pelialueen ulkopuolella
                    if (highest < 0)
                    {
                        gameOver = true;//Ilmoitetaan ohjelmalle että pelin lopetuksen ehdot ovat täyttyneet
                    }

                    //Ajetaan niin kauan kunnes ollaan päästy alimmalle riville ja gameOver ehdot eivät ole täyttyneet
                    while (highest <= 19 && gameOver == false)
                    {
                        //Ajetaan jos tarkasteltava rivi on täynnä palikoita
                        if (tiles.Count(x => x.Y == highest) == 10)
                        {
                            //Käydään läpi kaikki palikat
                            foreach (Tile tile in tiles)
                            {
                                //Jos palikka on rivillä joka vastaa highest muuttujan arvoa niin ajetaan tämä
                                if (tile.Y == highest)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);//Asetetaan kursori palikan kordinaatteihin
                                    Console.Write(".");//Piirretään kordinaattien kohtaan piste
                                    Console.SetCursorPosition(15, 0);//Palautetaan kursori oletus kordinaatteihin
                                }
                            }
                            tiles.RemoveAll(x => x.Y == highest);//Poistetaan kaikki palikat jotka ovat käsittelyssä olevalla rivillä

                            //Käydään läpi kaikki pelialueella olevat palikat
                            foreach (Tile tile in tiles)
                            {

                                //Jos palikka on äsken poistetun rivin yläpuolella
                                if (tile.Y < highest)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);//Siirrytään palikan kordinaatteihin
                                    Console.Write(".");//Kirjoitetaan kordinaattien kohdalle piste
                                    tile.Y++;//Siirretään palikan rivikordinaattia yksi alaspäin
                                }
                            }

                            //Käydään läpi kaikki pelialueella olevat palikat
                            foreach (Tile tile in tiles)
                            {
                                //Jos palikan rivikordinaatit ovat samat tai ylenpänä kuin aiemmin poistettujen palikoiden kordinaatit
                                if (tile.Y <= highest)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);//Siirretään kursori palikan kordinaatteihin
                                    Console.BackgroundColor = tile.Color;//Valitaan taustaväriksi palikan väri
                                    Console.Write(" ");//Piirretään tyhjä kohta valitulla taustavärillä valittuihin kordinaatteihin
                                    Console.ResetColor();//Palautetaan oletustaustaväri
                                    Console.SetCursorPosition(15, 0);//Nollataan kursorin sijainti oletuskordinaatteihihn
                                }
                            }
                            rowCounter++;//Lisätään rivilaskurin summaa yhdellä
                            lines++;//Lisätään poistettujen rivien laskurin summaa yhdellä
                            Console.SetCursorPosition(19, 5);//Asetetaan kursori pelialueella kohtaan jossa näkyy poistettavien rivien summa
                            Console.Write(lines);//Piirretään uusi summa kyseiseen kohtaan
                            Console.SetCursorPosition(15, 0);//Asetetaan kursori oletuskordinaatteihin
                        }
                        highest++;//Lisätään hihghest laskurin summaa yhdellä
                    }
                    //Kun rivejä poistetaan niin tason arvoa nostetaan yhdellä jos lines muuttujan arvo on 10 tai enemmän
                    if (lines >= (level + 1) * 10)
                    {
                        level++;//Lisätään tasolaskurin summaa yhdellä

                        if (level < 10)
                        {
                            interval -= 80;//Pienennetään ajastimen aikaväliä 80ms jos ollaan alle levelillä 10
                        }
                        else if (interval > 30)
                        {
                            interval -= 25;//Muuten pienennetään aikaväliä 25ms
                        }
                        timer.Interval = interval;
                        Console.SetCursorPosition(19, 4);//Siirretään kursori pelialueella tason numeron kohdalle
                        Console.Write(level);//Kirjoitetaan uusi taso peliaueelle
                        Console.SetCursorPosition(15, 0);//Asetetaan kursori oletuskordinaatteihin
                    }
                    //Ajetaan jos ollaan tasolla 0 ja yhtään kokonaista riviä ei ole muodostunut tällä terispalikan pudotuksella
                    if (level == 0 && rowCounter == 0)
                    {
                        score += 10;//Lisätään pelaajan pistemäärälaskurin arvoa 10:llä
                    }
                    //Ajetaan jos ollaan tasolla 0 mutta kokonaisia rivejä on muodostunut
                    else if (level == 0)
                    {
                        score += 100 * rowCounter * rowCounter;//Lisätään pelaajan pistemäärälaskuria seuraavasti: 100 * pudotettujen rivien määrä * pudotettujen rivien määrä
                    }
                    //Ajetaan jos ollaan vähintään tasolla 1 mutta yhtään kokonaista riviä ei ole muodostunut tällä tetrispalikan pudotuksella
                    else if (rowCounter == 0)
                    {
                        score += 10 * level;//Lisätään pelaajan pistemäärälaskuria seuraavasti: 10 * tasonnumero
                    }
                    else
                    {
                        score += (10 * level) + (100 * rowCounter * rowCounter);//Lisätään pelaajan pistemäärälaskuria seuraavasti: (10 * tasonnumero) + (100 * pudotettujen rivien määrä * pudotettujen rivien määrä)
                    }

                    //Tulostetaan pelaajalle tieto nykyisestä pistemäärästä pelialueelle
                    Console.SetCursorPosition(13, 2);
                    Console.Write(score);
                    Console.SetCursorPosition(15, 0);
                    if (soundEnabled)
                    {
                        Thread beep = new Thread(new ThreadStart(Beeper));//Alustetaan uusi säie äänimerkkiä varten
                        beep.Start();//Käynnistetään säie
                    }
                    blockDown = false;//Ilmoitetaan ohjelmalle että palikka ei vielä ole osunut mihinkään
                }

                //Jos palikka ei ole vielä törmännyt mihinkään niin siirretään palikkaa yhdellä rivillä alaspäin
                else
                {
                    //Piirretään pelattavan palikan kordinaatteihin pelkkä pistettä
                    foreach (Tile tile in tetrisBlock)
                    {
                        if (tile.Y >= 0)
                        {
                            Console.SetCursorPosition(tile.X, tile.Y);
                            Console.Write(".");
                            Console.SetCursorPosition(15, 0);
                        }
                    }
                    //Annetaan pelattavalle palikalle uudet kordinaatit ja piirretään palikka sen omalla taustavärillä uusien kordinaattien mukaan
                    foreach (Tile tile in tetrisBlock)
                    {
                        tile.Y++;
                        if (tile.Y >= 0)
                        {
                            Console.SetCursorPosition(tile.X, tile.Y);
                            Console.BackgroundColor = tile.Color;
                            Console.Write(" ");
                            Console.ResetColor();
                            Console.SetCursorPosition(15, 0);
                        }
                    }
                }
            }
        }

        //Tässä metodissa on kaikki mitä tapahtuu kun pelaaja häviää pelin
        static void GameOver()
        {
            timer.Enabled = false;//Pysäytetään ajastin

            //Piirretään pelaajalle GAME OVER teksti pelialueen päälle ja palautetaan kursori
            Console.SetCursorPosition(0, 7);
            Console.Write("o----------o\n|GAME OVER!|\no----------o");
            Console.SetCursorPosition(0, 21);
            Console.CursorVisible = true; //Palautetaan kursori

            WriteHighscores(true);//Kutsutaan WriteHighscores metodia joka tarkistaa saiko pelaaja uutta Top10 tulosta ja kysyy pelaajalta että haluaako hän tallentaa tuloksensa mikäli uusi Top10 tulos tuli
            Console.ReadKey();

            //Lopuksi tyhjennetään konsoli ja palataan päävalikkoon
            Console.Clear();
            MainMenu();
        }

        //Tällä metodilla tarkistetaan osuuko palikka johonkin kun pelaaja yrittää liikuttaa palikkaa ja palautetaan tieto osumasta kustuvaan ohjelmaan
        static bool Collision(int x0, int y0, int x1, int y1, int x2, int y2, int x3, int y3)
        {
            bool collision = false;//Luodaan muuttuja joka ilmoittaa osuuko joku palikoista seinään tai toiseen palikkaan kun sitä käännetään

            //Tarkistetaan osuuko palikka seinään
            if (x0 == 0 || x1 == 0 || x2 == 0 || x3 == 0 || x0 == 11 || x1 == 11 || x2 == 11 || x3 == 11)
            {
                collision = true;
            }
            //Jos ei osu seinään niin tarkistetaan osuuko toiseen palikkaan
            else
            {
                //Tarkastetaan jokainen parametrinä tullut kordinaatti ja verrataan sitä jokaiseen kentällä olevaan palikkaan
                foreach (Tile tile in tiles)
                {
                    if ((tile.X == x0 && tile.Y == y0) || (tile.X == x1 && tile.Y == y1) || (tile.X == x2 && tile.Y == y2) || (tile.X == x3 && tile.Y == y3))
                    {
                        collision = true;
                        break;
                    }
                }
            }
            return collision;
        }

        //Tämä metodi antaa pelaajalle äänimerkin kutsuttaessa
        static void Beeper()
        {
            Console.Beep();
        }

        //Tämä metodi ajetaan aina ensimmäisenä sillä se sisältää pelin päävalikon
        static void MainMenu()
        {
            bool success = ReadHighscores(false);//Yritetään lukea highscores.score tiedostoa ReadHighscores()-metodilla ja kerrotaan sen onnistumisesta MainMenu metodille success muuttujalla
        Beginning://Tähän pisteeseen palataan goto komennolla jos on tarvetta

            //Piirretään päävalikko pelaajalle
            Console.WriteLine("o-------------------------------o\n|Welcome to Command-Line Tetris!|\no-------------------------------o\n");
            Console.WriteLine("1) New game");
            if (soundEnabled)
            {
                Console.WriteLine("2) Disable Sound");
            }
            else
            {
                Console.WriteLine("2) Enable Sound");
            }
            Console.WriteLine("3) Highscores\n4) How to play\n5) Exit\n");
            if (success)//Jos highscores.score tiedosto onnistuttiin lukemaan niin ilmoitetaan päävalikossa mikä on paras tulos tähän mennessä
            {
                Console.WriteLine("Highscore: {0}\n", Highscores[0].Score);
            }
            Console.Write("Choose your command:");

            //Kysytään käyttäjältä minkä komennon hän valitsee ja sen mukaan tehdään juttuja
            ConsoleKeyInfo input = Console.ReadKey();
            //Ajetaan tätä niin kauan että käyttäjä antaa kelvollisen valinnan
            while (input.Key != ConsoleKey.D1 && input.Key != ConsoleKey.D2 && input.Key != ConsoleKey.D3 && input.Key != ConsoleKey.D4 && input.Key != ConsoleKey.D5 && input.Key != ConsoleKey.NumPad1 && input.Key != ConsoleKey.NumPad2 && input.Key != ConsoleKey.NumPad3 && input.Key != ConsoleKey.NumPad4 && input.Key != ConsoleKey.NumPad5)
            {
                input = Console.ReadKey(true);
            }
            //Jos käyttäjö valitsee 1:sen niin kysytään käyttäjältä millä tasolla hän haluaa aloittaa jonka jälkeen aloitetaan peli
            if (input.Key == ConsoleKey.NumPad1 || input.Key == ConsoleKey.D1)
            {
                Console.Write("\nChoose a starting level.\nThe level has to be between 0 and 9:");
                input = Console.ReadKey();
                //Loopataan kunnes käyttäjä antaa kelvollisen valinnan 
                while (input.Key != ConsoleKey.D0 && input.Key != ConsoleKey.D1 && input.Key != ConsoleKey.D2 && input.Key != ConsoleKey.D3 && input.Key != ConsoleKey.D4 && input.Key != ConsoleKey.D5 && input.Key != ConsoleKey.D6 && input.Key != ConsoleKey.D7 && input.Key != ConsoleKey.D8 && input.Key != ConsoleKey.D9
                    && input.Key != ConsoleKey.NumPad0 && input.Key != ConsoleKey.NumPad1 && input.Key != ConsoleKey.NumPad2 && input.Key != ConsoleKey.NumPad3 && input.Key != ConsoleKey.NumPad4 && input.Key != ConsoleKey.NumPad5 && input.Key != ConsoleKey.NumPad6 && input.Key != ConsoleKey.NumPad7 && input.Key != ConsoleKey.NumPad8 && input.Key != ConsoleKey.NumPad9)
                {
                    input = Console.ReadKey(true);
                }
                //Muutetaan käyttäjän antama näppäintieto numeroksi ja tallennetaan kyseinen numero level-muuttujaan jonka jälkeen asetetaan interval-muuttujan arvo vastaamaan aloituslevelin arvoa jonka jälkeen käynnistetään peli StartGame()-metodilla
                string leveltext = Convert.ToString(input.KeyChar);
                level = Convert.ToInt32(leveltext);
                interval = 900 - level * 80;
                Console.Write("\nYou chose level {0}.\nPress any key to start...", level);
                Console.ReadKey();
                StartGame();
            }
            //Jos käyttäjä valitsee 2:sen niin muutetaan soundEnabled muuttujan arvo ja tyhjennetään konsoli-ikkuna jonka jälkeen aloitetaan metodin ajaminen Beginning-kohdasta
            else if (input.Key == ConsoleKey.NumPad2 || input.Key == ConsoleKey.D2)
            {
                if (soundEnabled)
                {
                    soundEnabled = false;
                }
                else
                {
                    soundEnabled = true;
                }
                Console.Clear();
                goto Beginning;
            }
            //Jos käyttäjä valitsee kolmosen niin tyhjennetään konsoli-ikkuna jonka jälkeen yritetään lukea highscores.score tiedostoa. Jos tässä onnistutaan niin näytetään ennätykset käyttäjälle ja jos ei niin sitten kerrotaan käyttäjälle että ennätyksiä ei löytynyt
            //Lopuksi jäädään odottamaan käyttäjän syötettä jonka jälkeen aloitetaan metodin ajaminen alusta
            else if (input.Key == ConsoleKey.NumPad3 || input.Key == ConsoleKey.D3)
            {
                Console.Clear();
                success = ReadHighscores(false);
                if (success == false)
                {
                    Console.WriteLine("No highscores found!");
                }
                else
                {
                    Console.WriteLine("TOP 10 Highscores:");
                    for (int i = 0; i < Highscores.Count; i++)
                    {
                        Console.WriteLine("{0}. Score:{2}, Name: {1}", i + 1, Highscores[i].Name, Highscores[i].Score);
                    }
                }
                Console.WriteLine("Press any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
                goto Beginning;
            }
            //Jos käyttäjä valitsee nelosen niin tyhjennetään konsoli-ikkuna jonka jälkeen näytetään peliohjeet käyttäjälle
            //Lopuksi jäädään odottamaan käyttäjän syötettä jonka jälkeen aloitetaan metodin ajaminen alusta
            else if (input.Key == ConsoleKey.NumPad4 || input.Key == ConsoleKey.NumPad4)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Command-Line Tetris!\n\nThe goal of the game is to position the falling blocks\n" +
                    "so that they form solid lines by moving and rotating the blocks\n" +
                    "as they fall down.\n\nAs you clear " +
                    "the lines and increase your level the speed of the blocks\n" +
                    "will increase and the game ends when the falling block\n" +
                    "does not fit inside the playingfield.\n\n" +
                    "Keys:\n" +
                    "UP-Arrow: Rotate the falling block clockwise by 90 degrees.\n" +
                    "DOWN-Arrow: Make the falling block descent.\n" +
                    "LEFT-Arrow: Move the falling block left.\n" +
                    "RIGHT-Arrow: Move the falling block right.\n\n");
                Console.Write("Press any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
                goto Beginning;
            }
            else if (input.Key == ConsoleKey.NumPad5 || input.Key == ConsoleKey.D5)
            {
                Console.Write("\nThank you for playing! Press any key to exit...");
                Console.ReadKey();
            }
        }
        //Tällä metodilla yritetään lukea highscores.score tiedostoa ja jos onnistutaan niin tallennetaan löydökset Highscores listaan
        static bool ReadHighscores(bool displayErrorMessage)
        {
            string[] highscoreText = null;
            bool success = true;
            try
            {
                highscoreText = System.IO.File.ReadAllLines(Directory);
            }
            catch (Exception e)
            {
                success = false;
                if (displayErrorMessage)
                {
                    Console.WriteLine("Something went wrong!\nMessage:{0}",e.Message);
                }
            }
            //Jos tekstiä löytyi niin tallennetaan pisteet ja nimet Highscore olioihin ja sijoitetaan oliot Highscores listaan
            if (success)
            {
                Highscores = new List<Highscore>();
                for (int i = 0; i < highscoreText.Length; i += 2)
                {
                    if (i < 20)
                    {
                        try
                        {
                            Highscore score = new Highscore();
                            score.Name = highscoreText[i];
                            score.Score = Convert.ToInt32(highscoreText[i + 1]);
                            Highscores.Add(score);
                        }
                        catch (Exception e)
                        {
                            if (displayErrorMessage)
                            {
                                Console.WriteLine("Error while reading the file highscores.score!\nError message:{0}", e.Message);
                                Console.ReadKey();
                            }
                        }
                    }
                    else
	                {
                        break;
                    }
                }
            }
            return success;
        }
        //Tässä metodissa ensin tarkistetaan löytyykö tallennettuja ennätyksiä highscires.score tiedostosta. Mikäli ei löydy niin kysytään käyttäjältä haluaako hän luoda sellaisen. Jos taas löytyy niin tarkistetaan pääsikö käyttäjä Top10 listalle.
        //Jos käyttäjä pääsi Top10:iin niin kysytään haluaako hän tallentaa saamansa tuloksen ja mikäli haluaa niin tulos tallennetaan highscores.score tiedostoon
        static void WriteHighscores(bool displayErrorMessage)
        {
            bool success = ReadHighscores(false);
            if (success == false)
            {
                Console.Write("No highscores found. Would you like to create a file to save your score?(y/n)");
                ConsoleKeyInfo input = Console.ReadKey(true);
                while (input.Key != ConsoleKey.Y && input.Key != ConsoleKey.N)
                {
                    input = Console.ReadKey(true);
                }
                if (input.Key == ConsoleKey.Y)
                {
                    Console.Write("\nType your name:");
                    string name = Console.ReadLine();
                    success = true;
                    try
                    {
                        string scoreString = Convert.ToString(score);
                        string[] highscore = { name, scoreString };
                        System.IO.File.WriteAllLines(Directory, highscore);
                    }
                    catch (Exception e)
                    {
                        success = false;
                        if (displayErrorMessage)
                        {
                            Console.WriteLine("There was an error while creating the file!\nError message:{0}\nPress any key to return to main menu...", e.Message);
                        }
                    }
                    if (success)
                    {
                        Console.WriteLine("Highscores saved. Press any key to continue...");
                    }
                }
                else
                {
                    Console.WriteLine("\nThank you for playing! Press any key to return to main menu...");
                }
            }
            else
            {
                bool save = false;
                if (Highscores[0].Score < score)
                {
                    Console.WriteLine("A NEW HIGHSCORE! Would you like to save your score?(y/n)");
                    ConsoleKeyInfo input2 = Console.ReadKey(true);
                    while (input2.Key != ConsoleKey.Y && input2.Key != ConsoleKey.N)
                    {
                        input2 = Console.ReadKey(true);
                    }
                    if (input2.Key == ConsoleKey.Y)
                    {
                        save = true;
                    }
                    else
                    {
                        Console.WriteLine("Press any key to return to main menu...");
                    }
                }
                else if (Highscores.Count < 10 || (Highscores.Count == 10 && Highscores[9].Score < score))
                {
                    Console.WriteLine("You made it to Top 10! Would you like to save your score?(y/n)");
                    ConsoleKeyInfo input2 = Console.ReadKey(true);
                    while (input2.Key != ConsoleKey.Y && input2.Key != ConsoleKey.N)
                    {
                        input2 = Console.ReadKey(true);
                    }
                    if (input2.Key == ConsoleKey.Y)
                    {
                        save = true;
                    }
                    else
                    {
                        Console.WriteLine("Press any key to return to main menu...");
                    }
                }
                else
                {
                    save = false;
                    Console.WriteLine("You did not make it to TOP 10.\nPress any key to return to main menu...");
                }
                if (save)
                {
                    Console.Write("Type your name:");
                    string name = Console.ReadLine();
                    Highscore highscore = new Highscore();
                    highscore.Name = name;
                    highscore.Score = score;
                    Highscores.Add(highscore);
                    Highscores.Sort((a, b) => b.Score.CompareTo(a.Score));
                    List<string> highscoreText = new List<string>();
                    foreach (Highscore h in Highscores)
                    {
                        highscoreText.Add(h.Name);
                        highscoreText.Add(Convert.ToString(h.Score));
                    }
                    System.IO.File.WriteAllLines(Directory, highscoreText);
                    Console.WriteLine("Highscore saved!");
                    Console.WriteLine("Press any key to return to main menu...");
                }
            }
        }
        //Tämä metodi sisältää itse Gameplay loopin jossa käsitellään kaikki pelaajan pelinaikana antamat komennot
        static void GamePlayLoop()
        {
            //Käynnistetään ajastin
            timer.Interval = interval;
            timer.Enabled = true;
            //Loopataan kunnes gameOver ehdot täyttyvät
            while (gameOver == false)
            {
                while (timerEvent)
                {
                    //Odotetaan että Timer_Event on suoritettu loppuun mikäli sellainen on käynnissä
                }
                //Tarkistetaan onko pelaaja painanut jotain näppäintä
                if (Console.KeyAvailable)
                {
                    userEvent = true;
                    ConsoleKeyInfo playerInput = Console.ReadKey();
                    //Jos pelaaja painoi ylös-nuolta niin tarkistetaan mahtuuko palikka kääntymään ja mikäli mahtuu nii käännetään palikkaa ja tallennetaan tieto palikan uudesta asennosta blockOrientation muuttujaan
                    if (playerInput.Key == ConsoleKey.UpArrow)
                    {
                        bool collision = false;
                        if (blockType == 0) //Jos kyseessä on I-palikka
                        {
                            if (blockOrientation == 0) 
                            {
                                collision = Collision(tetrisBlock[0].X + 2, tetrisBlock[0].Y - 2, tetrisBlock[0].X + 2, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 2, tetrisBlock[0].Y, tetrisBlock[0].X + 2, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X += 2;
                                    tetrisBlock[0].Y -= 2;
                                    tetrisBlock[1].X++;
                                    tetrisBlock[1].Y--;
                                    tetrisBlock[3].X--;
                                    tetrisBlock[3].Y++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 1)
                            {
                                collision = Collision(tetrisBlock[0].X - 2, tetrisBlock[0].Y + 2, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 2, tetrisBlock[0].X, tetrisBlock[0].Y + 2, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X -= 2;
                                    tetrisBlock[0].Y += 2;
                                    tetrisBlock[1].X--;
                                    tetrisBlock[1].Y++;
                                    tetrisBlock[3].X++;
                                    tetrisBlock[3].Y--;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation--;
                                }
                            }
                        }
                        else if (blockType == 1) //Jos kyseessä on J-palikka
                        {
                            if (blockOrientation == 0)
                            {
                                collision = Collision(tetrisBlock[0].X + 1, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X++;
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[2].X--;
                                    tetrisBlock[2].Y++;
                                    tetrisBlock[3].X -= 2;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 1)
                            {
                                collision = Collision(tetrisBlock[0].X - 1, tetrisBlock[0].Y, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X--;
                                    tetrisBlock[1].X--;
                                    tetrisBlock[2].Y--;
                                    tetrisBlock[3].X += 2;
                                    tetrisBlock[3].Y--;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 2)
                            {
                                collision = Collision(tetrisBlock[0].X + 2, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X += 2;
                                    tetrisBlock[1].X++;
                                    tetrisBlock[1].Y--;
                                    tetrisBlock[3].X--;
                                    tetrisBlock[3].Y++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 3)
                            {
                                collision = Collision(tetrisBlock[0].X - 2, tetrisBlock[0].Y + 1, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X -= 2;
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[1].Y++;
                                    tetrisBlock[2].X++;
                                    tetrisBlock[3].X++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation = 0;
                                }
                            }
                        }
                        else if (blockType == 2) // Jos kyseessä on L-palikka
                        {
                            if (blockOrientation == 0)
                            {
                                collision = Collision(tetrisBlock[0].X, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[1].Y--;
                                    tetrisBlock[2].X--;
                                    tetrisBlock[3].X++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 1)
                            {
                                collision = Collision(tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 2, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 2, tetrisBlock[0].Y);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[1].Y++;
                                    tetrisBlock[2].X++;
                                    tetrisBlock[3].X++;
                                    tetrisBlock[3].Y -= 2;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 2)
                            {
                                collision = Collision(tetrisBlock[0].X + 1, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 2, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X++;
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[2].X--;
                                    tetrisBlock[2].Y++;
                                    tetrisBlock[3].Y += 2;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 3)
                            {
                                collision = Collision(tetrisBlock[0].X - 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X--;
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[2].X++;
                                    tetrisBlock[2].Y--;
                                    tetrisBlock[3].X -= 2;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation = 0;
                                }
                            }
                        }
                        else if (blockType == 4)//Jos kyseessä on S-palikka
                        {
                            if (blockOrientation == 0)
                            {
                                collision = Collision(tetrisBlock[0].X, tetrisBlock[0].Y - 1, tetrisBlock[0].X, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[1].X--;
                                    tetrisBlock[2].X++;
                                    tetrisBlock[2].Y--;
                                    tetrisBlock[3].X += 2;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 1)
                            {
                                collision = Collision(tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 2, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[1].X++;
                                    tetrisBlock[2].X--;
                                    tetrisBlock[2].Y++;
                                    tetrisBlock[3].X -= 2;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation--;
                                }
                            }
                        }
                        else if (blockType == 5) //Jos kyseessä on Z-palikka
                        {
                            if (blockOrientation == 0)
                            {
                                collision = Collision(tetrisBlock[0].X + 2, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 2, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X += 2;
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[1].X++;
                                    tetrisBlock[2].Y--;
                                    tetrisBlock[3].X--;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 1)
                            {
                                collision = Collision(tetrisBlock[0].X - 2, tetrisBlock[0].Y + 1, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X - 1, tetrisBlock[0].Y + 2, tetrisBlock[0].X, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X -= 2;
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[1].X--;
                                    tetrisBlock[2].Y++;
                                    tetrisBlock[3].X++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation--;
                                }
                            }
                        }
                        else if (blockType == 6)//Jos kyseessä on T-palikka
                        {
                            if (blockOrientation == 0)
                            {
                                collision = Collision(tetrisBlock[0].X + 1, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X++;
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[2].X--;
                                    tetrisBlock[2].Y++;
                                    tetrisBlock[3].X--;
                                    tetrisBlock[3].Y--;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 1)
                            {
                                collision = Collision(tetrisBlock[0].X - 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X--;
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[2].X++;
                                    tetrisBlock[2].Y--;
                                    tetrisBlock[3].X++;
                                    tetrisBlock[3].Y--;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 2)
                            {
                                collision = Collision(tetrisBlock[0].X + 1, tetrisBlock[0].Y - 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 2, tetrisBlock[0].Y);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X++;
                                    tetrisBlock[0].Y--;
                                    tetrisBlock[2].X--;
                                    tetrisBlock[2].Y++;
                                    tetrisBlock[3].X++;
                                    tetrisBlock[3].Y++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation++;
                                }
                            }
                            else if (blockOrientation == 3)
                            {
                                collision = Collision(tetrisBlock[0].X - 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 1, tetrisBlock[0].X + 1, tetrisBlock[0].Y + 1, tetrisBlock[0].X, tetrisBlock[0].Y + 2);
                                if (collision == false)
                                {
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.Write(".");
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    tetrisBlock[0].X--;
                                    tetrisBlock[0].Y++;
                                    tetrisBlock[2].X++;
                                    tetrisBlock[2].Y--;
                                    tetrisBlock[3].X--;
                                    tetrisBlock[3].Y++;
                                    foreach (Tile tile in tetrisBlock)
                                    {
                                        if (tile.Y >= 0)
                                        {
                                            Console.SetCursorPosition(tile.X, tile.Y);
                                            Console.BackgroundColor = tile.Color;
                                            Console.Write(" ");
                                            Console.ResetColor();
                                            Console.SetCursorPosition(15, 0);
                                        }
                                    }
                                    blockOrientation = 0;
                                }
                            }
                        }
                    }


                    //Jos pelaaja painaa alas-nuolinäppäintä niin ajetaan Descent-metodi
                    else if (playerInput.Key == ConsoleKey.DownArrow)
                    {
                        Descent();
                    }

                    //Jos pelaaja painaa oikealle-nuolinäppäintä niin siirretään palikkaa oikealle mikäli se on mahdollista
                    else if (playerInput.Key == ConsoleKey.RightArrow)
                    {
                        bool wallCollision = false;
                        foreach (Tile tile in tetrisBlock)
                        {
                            if (tile.X == 10)
                            {
                                wallCollision = true;
                            }
                            else
                            {
                                foreach (Tile tile1 in tiles)
                                {
                                    if (tile.X + 1 == tile1.X && tile.Y == tile1.Y)
                                    {
                                        wallCollision = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (wallCollision == false)
                        {
                            foreach (Tile tile in tetrisBlock)
                            {
                                if (tile.Y >= 0)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);
                                    Console.Write(".");
                                    Console.SetCursorPosition(15, 0);
                                }
                            }
                            foreach (Tile tile in tetrisBlock)
                            {
                                tile.X++;
                                if (tile.Y >= 0)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);
                                    Console.BackgroundColor = tile.Color;
                                    Console.Write(" ");
                                    Console.ResetColor();
                                    Console.SetCursorPosition(15, 0);
                                }
                            }
                        }
                    }

                    //Jos pelaaja painaa vasemmalle-nuolinäppäintä niin siirretään palikkaa vasemmalle mikäli se on mahdollista
                    else if (playerInput.Key == ConsoleKey.LeftArrow)
                    {
                        bool wallCollision = false;
                        foreach (Tile tile in tetrisBlock)
                        {
                            if (tile.X == 1)
                            {
                                wallCollision = true;
                            }
                            else
                            {
                                foreach (Tile tile1 in tiles)
                                {
                                    if (tile.X - 1 == tile1.X && tile.Y == tile1.Y)
                                    {
                                        wallCollision = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (wallCollision == false)
                        {
                            foreach (Tile tile in tetrisBlock)
                            {
                                if (tile.Y >= 0)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);
                                    Console.Write(".");
                                    Console.SetCursorPosition(15, 0);
                                }
                            }
                            foreach (Tile tile in tetrisBlock)
                            {
                                tile.X--;
                                if (tile.Y >= 0)
                                {
                                    Console.SetCursorPosition(tile.X, tile.Y);
                                    Console.BackgroundColor = tile.Color;
                                    Console.Write(" ");
                                    Console.ResetColor();
                                    Console.SetCursorPosition(15, 0);
                                }
                            }
                        }
                    }
                    userEvent = false; //Ilmoitetaan ohjelmalle että käyttäjän komento on saatu suoritettua
                }
            }
        }
    }

    //Luokka jonka olioista muodostetaan tetris-palikka. Jokainen tetris-palikka sisältää neljä Tile-oliota
    class Tile
    {
        public int X { get; set; }//Tilen sarake-kordinaatti
        public int Y { get; set; }//Tilen rivi-kordinaatti
        public ConsoleColor Color { get; set; }//Tilen väri
    }

    //Luokka jonka olioista muodostetaan tieto ennätyspisteitä luettaessa highscores.score tiedostoa
    class Highscore
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
