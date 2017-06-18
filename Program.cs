using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace translation_of_mars_project
{
    class MarsSurface
    {
        int WIDTH = 0;
        int LENGTH = 0;
        private static int[,] marsSize;

        static int currentPosX = 0;
        static int currentPosY = 0;

        /**
         * sets the size of the Martian surface.
         * @param x
         * @param y
         */
        public MarsSurface(int x, int y)
        {
            WIDTH = x;
            LENGTH = y;
            marsSize = new int[LENGTH + 1, WIDTH + 1];

            for (int i = 0; i < LENGTH; i++)
            {
                for (int k = 0; k < WIDTH; k++)
                {
                    marsSize[k, i] = 0;
                }
            }
        }
        /**
         * shows the map
         * 
         */
        public void returnMap()
        {
            for (int i = 0; i < LENGTH; i++)
            {
                for (int k = 0; k < WIDTH; k++)
                {
                    System.Console.WriteLine(marsSize[k,i]);
                }
                System.Console.WriteLine();
            }
        }

        /**
         * sets initial starting point
         * @param xOrig
         * @param yOrig
         * @throws ArrayIndexOutOfBoundsException
         */
        public void setOriginalPos(int xOrig, int yOrig) //throws ArrayIndexOutOfBoundsException
        {
            currentPosX=xOrig;
            currentPosY=yOrig;
            marsSize [xOrig, yOrig] = 1;

        }

        /**
         * move the rover on the map. called whenever the fed input says "M".
         * @param heading
         */
        public static void traverse(char heading)
        {
            marsSize[currentPosX, currentPosY] = 0;
            switch (heading)
            {
                case 'N':
                    currentPosY += 1;
                    marsSize[currentPosX, currentPosY] = 1;
                    break;
                case 'S':
                    currentPosY -= 1;
                    marsSize[currentPosX, currentPosY] = 1;
                    break;
                case 'W':
                    currentPosX -= 1;
                    marsSize[currentPosX, currentPosY] = 1;
                    break;
                case 'E':
                    currentPosX += 1;
                    marsSize[currentPosX, currentPosY] = 1;
                    break;
            }

        }
        /**
         * returns position of rover
         * @return
         */
        public String returnCurrentPos()
        {
            return currentPosX + " " + currentPosY;
        }
    }
    class Rover
    {
        //LENGTH = NORTH (+1) and SOUTH (-1).
        //WIDTH = EAST (+1) AND WEST (-1).
        private char currentDirection;
        private char newDirection;

        /**
         * 
         * @param direction
         */
        public Rover(String direction)
        {
            currentDirection = direction.First();
            //Any non-directional letter is handled in the direction method.
        }

        /**
         * changes direction of rover
         * @param leftOrRight
         */
        public void direction(char leftOrRight)
        {
            if (leftOrRight == 'L')
            {
                //Switch statements cascade down. If the direction is not one of those, an exception is thrown.
                switch (currentDirection)
                {
                    case 'N':
                        newDirection = 'W';
                        break;
                    case 'S':
                        newDirection = 'E';
                        break;
                    case 'W':
                        newDirection = 'S';
                        break;
                    case 'E':
                        newDirection = 'N';
                        break;
                    default:
                        throw new System.ArgumentException("Invalid direction was given: " + currentDirection);
                }
            }
            else if (leftOrRight == 'R')
            {
                switch (currentDirection)
                {
                    case 'N':
                        newDirection = 'E';
                        break;
                    case 'S':
                        newDirection = 'W';
                        break;
                    case 'W':
                        newDirection = 'N';
                        break;
                    case 'E':
                        newDirection = 'S';
                        break;
                    default:
                        throw new System.ArgumentException("Invalid direction was given: " + currentDirection);
                }
            }
            currentDirection = newDirection;
        }

        /**
         * returns the current direction of the rover.
         * @return char
         */
        //
        public char returnCurrDir()
        {
            return currentDirection;
        }

        /**
         * parses chararray. Accepts 'L','M','R' as input, nothing else.
         */
        public void parse(String input)
        {
            char[] arrayVer = input.ToUpper().ToCharArray();

            foreach (char k in arrayVer)
            {
                if (k == 'L' || k == 'R')
                {
                    direction(k);
                }
                else if (k == 'M')
                {
                    MarsSurface.traverse(currentDirection);
                }
                else
                {
                    throw new System.ArrayTypeMismatchException("Invalid char was found: " + k);
                }
            }
        }
    }
    class Program
    {
        static MarsSurface surface;
        static Rover rover;
        static ArrayList roverLocations = new ArrayList();

        /**
         * repeatedly get new rover info.
         * @param direction
         * @param parsedData
         */
        public static void repeat(String[] direction, String parsedData)
        {
            try
            {

                surface.setOriginalPos(Convert.ToInt32(direction[0]), Convert.ToInt32(direction[1]));
                rover = new Rover(direction[2].ToUpper());
                rover.parse(parsedData);
                roverLocations.Add(surface.returnCurrentPos() + " " + rover.returnCurrDir());

            }
            catch (System.ArgumentOutOfRangeException e)
            {
                System.Console.WriteLine(e.Message);
            }
            catch (System.ArrayTypeMismatchException e)
            {
                System.Console.WriteLine(e.Message); ;
            }
            catch (System.ArgumentException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        /**
         * print out locations of the rovers
         * @param l
         */
        public static void listAllLocations(ArrayList list)
        {
            foreach (String s in roverLocations)
            {
                System.Console.WriteLine("Rover is in position: ");
                System.Console.WriteLine(s);
            }
        }
        static void Main(string[] args)
        {
            System.Console.WriteLine("Please enter a map size.");
            String[] mapSize = System.Console.ReadLine().Split(' ');
            if (mapSize.Length != 2)
            {
                System.Console.WriteLine("This must be at least a 2 by 2 map.");
            }
            else
            {
                try
                {
                    System.Console.WriteLine("Please enter the starting point of the rover.");
                    surface = new MarsSurface(Convert.ToInt32(mapSize[0]), Convert.ToInt32(mapSize[1]));
                    String input = System.Console.ReadLine();
                    String initLoc = "";
                    while (input != null)
                    {
                        if (input.Contains(" "))
                        {
                            initLoc = input;
                        }
                        else
                        {
                            repeat(initLoc.Split(' '), input);
                            listAllLocations(roverLocations);
                        }

                        input = System.Console.ReadLine();
                    }
                    System.Console.WriteLine("Next");
                    listAllLocations(roverLocations);

                }
                catch (System.FormatException e)
                {
                    System.Console.WriteLine("The integers were not correctly read.");
                    System.Console.WriteLine(e.Message);
                }
            }
        }
    }
}