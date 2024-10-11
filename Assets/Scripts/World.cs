using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int size; //wielko�� lasu (kwadrat)
    private char[,] forest;
    private char[,] nextForest;
    public int density; //G�sto�� lasu
    public Transform tree;
    public Transform rock;
    public Transform burnTree;
    public Transform holder;
    private Vector3 position;
    private Object namer;
    public int firePropability;
    public float time;
    private int firePosition;
    private int ignitedTree;
    void Start()    //Jest jaki� problem je�eli chodzi o to podpalanie ale nie wiem gdzie. Generalnie jak si� pan przyjrzy to czasem jest tak �e podpala za du�o drzew.
        //Nie wiem czy poprostu za szybko dzia�a zapisywanie tablicy wzgl�dem pojawiania si� objekt�w czy jak. Nie mam pomys�u :C
    {
        ignitedTree = 1;
        firePosition = 1;
        time = 2f;
        if (density > 100)
            density = 100;
        else if (density < 0)
            density = 0;
        forest = new char[size, size];
        nextForest = new char[size, size];
        Forestation();    //Losowanie drzewek i kamyczk�w
        StartFire();  //Zapocz�tkowanie p�omieni
        nextForest = forest;
        Arson();
        // Burnt(); Zapisywanie do plik�w stosunku spalonych drzew do wszystkich drzew

    }

    private void Update()
    {
        if (ignitedTree != 0)   //Tu sprawdza koniec symulacji.
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                Fire(); //Zapalanie drzewek w tablicy
                Arson();    //Graficzne podpalanie drzewek
                time = 2f;
                forest = nextForest;
            }
        }
        
    }
    private void Forestation()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (Random.Range(1, 101) <= density)
                    forest[i, j] = 'T';
                else
                    forest[i, j] = 'X';
            }
        }
        for (int i = 0; i < size; i++)
        {
            position.y = -i;
            for (int j = 0; j < size; j++)
            {
                position.x = j;
                if (forest[i, j] == 'T')
                {
                    namer = Object.Instantiate(tree, position, tree.rotation, holder);
                    namer.name = "tree" + i + "," + j;
                }
                else
                {
                    namer = Object.Instantiate(rock, position, rock.rotation, holder);
                    namer.name = "rock" + i + "," + j;
                }
            }
        }
    }
    private void StartFire()
    {
        for(int i = 0; i<size; i++)
        {
            if (forest[0,i]=='T')
            {
                forest[0, i] = 'B';
            }
        }
    }
    private void Fire()
    {
        ignitedTree = 0;
        int xLeft;
        int xRight;
        int yUp;
        int yDown;
        for (int x = 0; x < size; x++)
        {
            if (x == 0)
                xLeft = 0;
            else
                xLeft = -1;
            if (x == size - 1)
                xRight = 0;
            else
                xRight = 1;
                for (int y = 0; y < size; y++)
                {
                    if (y == 0)
                        yUp = 0;
                    else
                        yUp = -1;
                    if (y == size - 1)
                        yDown = 0;
                    else
                        yDown = 1;
                    if (forest[x, y] == 'B')
                    {
                        if(forest[x+xLeft,y] == 'T' && Propability())
                        {
                            nextForest[x + xLeft, y] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x+xLeft, y + yUp] == 'T' && Propability())
                        {
                            nextForest[x + xLeft, y + yUp] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x, y + yUp] == 'T' && Propability())
                        {
                            nextForest[x, y + yUp] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x + xRight, y + yUp] == 'T' && Propability())
                        {
                            nextForest[x + xRight, y + yUp] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x + xRight, y] == 'T' && Propability())
                        {
                            nextForest[x + xRight, y] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x + xRight, y + yDown] == 'T' && Propability())
                        {
                            nextForest[x + xRight, y + yDown] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x , y+yDown] == 'T' && Propability())
                        {
                            nextForest[x, y + yDown] = 'B';
                            ignitedTree++;
                        }
                        if(forest[x + xLeft, y + yDown] == 'T' && Propability())
                        {
                            nextForest[x + xLeft, y + yDown] = 'B';
                            ignitedTree++;
                        }
                    }

            }
        }
        firePosition++;
    }
    private void Arson()
    {
        for(int i = 0; i< firePosition; i++)
        {
            position.y = -i;
            for(int j = 0; j< size; j++)
            {
                position.x = j;
                if(forest[i,j] == 'B')
                {
                    Destroy(GameObject.Find("tree" + i + "," + j));
                    namer = Object.Instantiate(burnTree, position, burnTree.rotation, holder);
                    namer.name = "burnTree" + i + "," + j;
                }
            }
        }
    }
    private void Burnt()    //Na to nie zwraca� zbytnio uwagi, to by�o tylko do wykresu.
    {
        int b = 0;
        int t = 0;
        for(int i = 0; i<size; i++)
        {
            for(int j = 0; j<size;j++)
            {
                if (forest[i, j] == 'B')
                    b++;
                else if (forest[i,j]=='T')
                    t++;
            }
        }
        System.IO.File.AppendAllText("C:/Users/Wh1tEW0lf13/Desktop/Wyniki.txt",b+" "+(t+b)+"\n");
    }
    
    private bool Propability()
    {
        if (Random.Range(1, 101) <= firePropability)
            return true;
        else
            return false;
    }
}
//Mam nadziej�, �e jest to akcpetowalne rozwi�zanie zadania. Przez brak czasu niestety nie zoptymalizowa�em go.
//Wiem �e jest tak tragicznie ten kod zoptymalizowany, �e dla ka�dej pr�by, przy wielko�ci lasu 100x100,
//Unity w��cza mi gr� oko�o 2 sekund, co jest stosunkowo d�ugim czasem przy moim procesorze.
//Za to serdecznie przepraszam, gdyby nie kolos w poniedzia�ek, postara�bym si� bardziej. Pozdrawiam Mi�osz :)
