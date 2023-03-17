using System;

namespace DoraTheExplorer.Util;

public class ReadFromFile {
    public (int[,], bool) ReadFile(string path) {
        string[] lines = System.IO.File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++) {
            lines[i] = lines[i].Replace(" ", "");
        }

        int[,] matrix = new int[lines.Length, lines[0].Length];
        for (int i = 0; i < lines.Length; i++) {
            for (int j = 0; j < lines[i].Length; j++) {
                if (lines[i][j] == 'K') {
                    // -3 : titik mulai
                    matrix[i, j] = -3;
                } else if (lines[i][j] == 'R') {
                    // 0 : Accessible
                    matrix[i, j] = 0;
                }
                else if (lines[i][j] == 'T') {
                    // -999 : Treasure
                    matrix[i, j] = -999;
                } else if (lines[i][j] == 'X') {
                    // -1 : Wall
                    matrix[i, j] = -1;
                } else {
                    // Invalid input
                    // Send Alert
                    return (matrix, false);
                }
            }
        }
        return (matrix, true);
    }
}