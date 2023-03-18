using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using DoraTheExplorer.Models;
using DoraTheExplorer.Structure;
using DynamicData;

namespace DoraTheExplorer.Util;

public class ReadFromFile {
    public (List<List<Vertex<Coordinate>>>?, Graph<Coordinate>?, State, List<Coordinate>?, bool) ReadFile(string path)
    {
        int nK = 0;
        int nT = 0;
        string[] lines = System.IO.File.ReadAllLines(path);

        int row = lines.Length;
        for (int i = 0; i < row; i++) {
            lines[i] = lines[i].Replace(" ", "");
        }
        int col = lines[0].Length;
        
        List<List<Vertex<Coordinate>>> vertices = new List<List<Vertex<Coordinate>>>();
        Vertex<Coordinate>[,] verticesArr = new Vertex<Coordinate>[row, col];
        
        Graph<Coordinate> graph = new Graph<Coordinate>();
        State startState = new State(new Coordinate(-1,-1));
        List<Coordinate>? goals = new List<Coordinate>();

        int[,] matrix = new int[row, col]; 

        for (int i = 0; i < lines.Length; i++) {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == 'K') {
                    // -3 : titik mulai
                    matrix[i, j] = 0;
                    startState = new State(new Coordinate(i, j));
                    nK++;
                    
                    verticesArr[i, j] = new Vertex<Coordinate>(new Coordinate(i, j));
                    if (j - 1 >= 0 && matrix[i, j - 1] == 0)
                    {   
                        verticesArr[i, j].ConnectLeft(verticesArr[i, j - 1]);
                    }
                    if (i - 1 >= 0 && matrix[i - 1, j] == 0)
                    {
                        verticesArr[i, j].ConnectUp(verticesArr[i - 1, j]);
                    }
                    graph.AddVertex(verticesArr[i, j]);
                    
                    if (nK > 1)
                    {
                        // Send Alert
                        return (null, null, startState, null, false);
                    }
                } else if (lines[i][j] == 'R') {
                    // 0 : Accessible
                    matrix[i, j] = 0;
                    
                    verticesArr[i, j] = new Vertex<Coordinate>(new Coordinate(i, j));
                    if (j - 1 >= 0 && matrix[i, j - 1] == 0)
                    {   
                        verticesArr[i, j].ConnectLeft(verticesArr[i, j - 1]);
                    }
                    if (i - 1 >= 0 && matrix[i - 1, j] == 0)
                    {
                        verticesArr[i, j].ConnectUp(verticesArr[i - 1, j]);
                    }
                    graph.AddVertex(verticesArr[i, j]);
                }
                else if (lines[i][j] == 'T') {
                    // -999 : Treasure
                    // matrix[i, j] = -999;
                    matrix[i, j] = 0;
                    
                    verticesArr[i, j] = new Vertex<Coordinate>(new Coordinate(i, j));
                    if (j - 1 >= 0 && matrix[i, j - 1] == 0)
                    {   
                        verticesArr[i, j].ConnectLeft(verticesArr[i, j - 1]);
                    }
                    if (i - 1 >= 0 && matrix[i - 1, j] == 0)
                    {
                        verticesArr[i, j].ConnectUp(verticesArr[i - 1, j]);
                    }
                    graph.AddVertex(verticesArr[i, j]);
                    
                    goals.Add(new Coordinate(i,j));
                    nT++;
                } else if (lines[i][j] == 'X') {
                    // -1 : Wall
                    matrix[i, j] = -1;
                } else {
                    // Invalid input
                    // Send Alert
                    return (null, null, startState, null, false);
                }
            }
        }
        for (int i = 0; i < row; i++)
        {
            List<Vertex<Coordinate>> temp = new List<Vertex<Coordinate>>();
            for (int j = 0; j < col; j++)
            {
                temp.Add(verticesArr[i, j]);
            }
            vertices.Add(temp);
        }

        return (vertices, graph, startState, goals, true);
    }
}