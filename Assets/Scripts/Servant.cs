using System.Collections;
using System.IO;

/*Класс, описывающий служебные методы и функции*/
public static class Servant  {
    
    public static void PrintArrayToFile(int[,] fArray, string fPath)
    {
        using (StreamWriter file = new StreamWriter(fPath))
        {
            string str = string.Empty;
            for (int i = 0; i < fArray.GetLength(0); i++)
            {
                for (int j = 0; j < fArray.GetLength(1); j++)
                {
                    str += fArray[i, j].ToString();
                }
                str += "\n";
            }
            file.Write(str);
        }
    }
}
