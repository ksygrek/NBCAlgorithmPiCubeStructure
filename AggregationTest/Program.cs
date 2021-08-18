using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AggregationTest
{
    class Program
    {
        public static ObservableCollection<Iris> coll;
        static void Main(string[] args)
        {
            coll = new ObservableCollection<Iris>();
            //CreateStratum();
            //Aggregate();
            ReadFile();
            //SeperateByClass();
            //StandardDeviation();
            SummarizeByClass();
        }

        public static void ReadFile()
        {
            string path = @"C:\Users\ksygrek\Desktop\mgr\iris.csv";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            //var lines = File.ReadAllLines(path);
            string line;
            while ((line = file.ReadLine()) != null) {
                //line = line.Replace(",", ";");
                //line = line.Replace(".", ",");
                var extract = line.Split(';');
                coll.Add(new Iris()
                {
                    SepalLength = float.Parse(extract[0]),
                    SepalWidth = float.Parse(extract[1]),
                    PetalLength = float.Parse(extract[2]),
                    PetalWidth = float.Parse(extract[3]),
                    ClassName = extract[4].ToString()
                });
            }
        }

        public static void Aggregate()
        {
            string path = @"C:\Users\ksygrek\Desktop\mgr\data.csv";

            var lines = File.ReadAllLines(path);
            int columns = lines[0].Split(';').Length;
            int fileLength = File.ReadAllLines(path).Length;

            int bins = fileLength / 4;
            if (fileLength % 4 != 0)
                bins = bins * 4;

            float[,] bin = new float[4, columns];


            //test
            float[,] a = new float[3, 4] {
                {0,1,2,3 }, // wiersz o indeksie 0
                {4,5,6,6 }, // wiersz o indeksie 1
                {8,9,10,11 } // wiersz o indeksie 2
            };
            float[] res = new float[4];
            for (int i = 0; i < 4; i++)
            {
                float xd = 0;
                for (int j = 0; j < 3; j++)
                {
                    xd = xd + a[j, i];
                }
                res[i] = xd / 3;
                xd = 0;
            }
            //test end

            List<string> list = new List<string>();
            List<float> list1 = new List<float>();

            using (var reader = new StreamReader(path))
            {
                int i = 0;
                while (!reader.EndOfStream)
                {
                    float[] result = new float[columns];

                    var line = reader.ReadLine();

                    var values = line.Split(';');

                    List<float> ob = new List<float>();
                    foreach (string s in values)
                        ob.Add(float.Parse(s));
                    for (int j = 0; j < columns; j++)
                    {
                        bin[i, j] = ob[j];
                    }
                    i++;
                    if (i == 4 || fileLength < 4)
                    {
                        i = 0;
                        fileLength = fileLength - 4;
                        for (int k = 0; k < columns; k++)
                        {
                            float xd = 0;
                            for (int j = 0; j < 4; j++)
                            {
                                xd = xd + bin[j, k];
                            }
                            result[k] = xd / 4;
                            xd = 0;
                        }
                        Console.WriteLine(result[0].ToString() + ";" + result[1].ToString());
                        FillStratum(result);
                    }
                }

            }
        }

        public static void CreateStratum()
        {
            string name = "stratum";
            string path = $@"C:\Users\ksygrek\Desktop\mgr\data{name}.csv";
            string delimiter = ";";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                string createText = "Lol" + delimiter + "Yolo" + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

        }

        public static void FillStratum(float[] data)
        {
            string name = "stratum";
            string path = $@"C:\Users\ksygrek\Desktop\mgr\data{name}.csv";
            string delimiter = ";";
            string appendText = "";
            foreach (float s in data)
                appendText += s.ToString() + delimiter;
            appendText = appendText.Substring(0, appendText.Length - 1);
            appendText += Environment.NewLine;
            File.AppendAllText(path, appendText);
        }

        public static void SeperateByClass()
        {
            foreach (var obj in coll)
            {
                switch (obj.ClassName)
                {
                    case "Iris-setosa":
                        Console.WriteLine("Iris-setosa");
                        break;
                    case "Iris-versicolor":
                        Console.WriteLine("Iris-versicolor");
                        break;
                    case "Iris-virginica":
                        Console.WriteLine("Iris-virginica");
                        break;
                }
            }
        } 

        public static Iris Sum()
        {
            Iris sum = new Iris();
            foreach (var obj in coll)
            {
                sum.SepalLength += obj.SepalLength;
                sum.SepalWidth += obj.SepalWidth;
                sum.PetalLength += obj.PetalLength;
                sum.PetalWidth += obj.PetalWidth;                
            }
            //Console.WriteLine($"sum\n {sum.SepalLength}, {sum.SepalWidth}, {sum.PetalLength}, {sum.PetalWidth}");
            return sum;
        }

        public static Iris Mean()
        {
            Iris means = Sum();
            means.SepalLength /= coll.Count();
            means.SepalWidth /= coll.Count();
            means.PetalLength /= coll.Count();
            means.PetalWidth /= coll.Count();
            Console.WriteLine($"means\n{means.SepalLength}, {means.SepalWidth}, {means.PetalLength}, {means.PetalWidth}");
            return means;
        }
        
        public static object StandardDeviation()
        {
            var mean = Mean();
            Iris stdDeviation = new Iris();
            foreach (var obj in coll)
            {
                stdDeviation.SepalLength += Math.Pow((obj.SepalLength - mean.SepalLength), 2);
                stdDeviation.SepalWidth += Math.Pow((obj.SepalWidth - mean.SepalWidth), 2);
                stdDeviation.PetalLength += Math.Pow((obj.PetalLength - mean.PetalLength), 2);
                stdDeviation.PetalWidth += Math.Pow((obj.PetalWidth - mean.PetalWidth), 2);
            }
            stdDeviation.SepalLength /= coll.Count() - 1;
            stdDeviation.SepalWidth /= coll.Count() - 1;
            stdDeviation.PetalLength /= coll.Count() - 1;
            stdDeviation.PetalWidth /= coll.Count() - 1;

            stdDeviation.SepalLength = Math.Sqrt(stdDeviation.SepalLength);
            stdDeviation.SepalWidth = Math.Sqrt(stdDeviation.SepalWidth);
            stdDeviation.PetalLength = Math.Sqrt(stdDeviation.PetalLength);
            stdDeviation.PetalWidth = Math.Sqrt(stdDeviation.PetalWidth);

            Console.WriteLine($"standard deviation\n{stdDeviation.SepalLength}, {stdDeviation.SepalWidth}, {stdDeviation.PetalLength}, {stdDeviation.PetalWidth}");
            return stdDeviation;
        }

        public static Iris Mean(ObservableCollection<Iris> iris)
        {
            Iris means = new Iris();
            foreach (var obj in iris)
            {
                means.SepalLength += obj.SepalLength;
                means.SepalWidth += obj.SepalWidth;
                means.PetalLength += obj.PetalLength;
                means.PetalWidth += obj.PetalWidth;
            }
            means.SepalLength /= iris.Count();
            means.SepalWidth /= iris.Count();
            means.PetalLength /= iris.Count();
            means.PetalWidth /= iris.Count();
            Console.WriteLine($"means\n{means.SepalLength}, {means.SepalWidth}, {means.PetalLength}, {means.PetalWidth}");
            return means;
        }

        public static Iris StandardDeviation(ObservableCollection<Iris> iris, Iris mean)
        {
            Iris stdDeviation = new Iris();
            foreach (var obj in iris)
            {
                stdDeviation.SepalLength += Math.Pow((obj.SepalLength - mean.SepalLength), 2);
                stdDeviation.SepalWidth += Math.Pow((obj.SepalWidth - mean.SepalWidth), 2);
                stdDeviation.PetalLength += Math.Pow((obj.PetalLength - mean.PetalLength), 2);
                stdDeviation.PetalWidth += Math.Pow((obj.PetalWidth - mean.PetalWidth), 2);
            }
            stdDeviation.SepalLength /= iris.Count() - 1;
            stdDeviation.SepalWidth /= iris.Count() - 1;
            stdDeviation.PetalLength /= iris.Count() - 1;
            stdDeviation.PetalWidth /= iris.Count() - 1;

            stdDeviation.SepalLength = Math.Sqrt(stdDeviation.SepalLength);
            stdDeviation.SepalWidth = Math.Sqrt(stdDeviation.SepalWidth);
            stdDeviation.PetalLength = Math.Sqrt(stdDeviation.PetalLength);
            stdDeviation.PetalWidth = Math.Sqrt(stdDeviation.PetalWidth);

            Console.WriteLine($"standard deviation\n{stdDeviation.SepalLength}, {stdDeviation.SepalWidth}, {stdDeviation.PetalLength}, {stdDeviation.PetalWidth}");
            return stdDeviation;
        }

        public static void SummarizeByClass()
        {
            ObservableCollection<Iris> setosa = new ObservableCollection<Iris>();
            ObservableCollection<Iris> versicolor = new ObservableCollection<Iris>();
            ObservableCollection<Iris> virginica = new ObservableCollection<Iris>();
            foreach (var obj in coll)
            {
                switch (obj.ClassName)
                {
                    case "Iris-setosa":
                        setosa.Add(obj);
                        break;
                    case "Iris-versicolor":
                        versicolor.Add(obj);
                        break;
                    case "Iris-virginica":
                        virginica.Add(obj);
                        break;
                }
            }
            Console.WriteLine("Iris-setosa");
            Iris set = StandardDeviation(setosa, Mean(setosa));
            Console.WriteLine("Iris-versicolor");
            Iris ver = StandardDeviation(versicolor, Mean(versicolor));
            Console.WriteLine("Iris-virginica");
            Iris vir = StandardDeviation(virginica, Mean(virginica));
        }

        public static void GaussianPropability()
        {
            double exponent = Math.Exp(-((x - mean) * *2 / (2 * stdev * *2)));
        }
    }
}
