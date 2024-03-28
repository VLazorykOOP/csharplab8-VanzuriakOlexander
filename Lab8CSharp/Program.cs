using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TextProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the task");
            string? str = Console.ReadLine();
            int n = 0;
            if (str != null) n = int.Parse(str);
            if (n == 1)
            {
                string inputFilePath = "task1.txt";
                string outputFilePath = "outputTask1.txt";

                List<string> lines = new List<string>();
                try
                {
                    lines = File.ReadAllLines(inputFilePath).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    return;
                }

                List<string> ipAddresses = new List<string>();

                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    foreach (string line in lines)
                    {
                        string modifiedLine = line;
                        MatchCollection matches = Regex.Matches(line, @"\b(?:\d{1,3}\.){3}\d{1,3}\b");
                        foreach (Match match in matches)
                        {
                            string ipAddress = match.Value;

                            bool isValidIpAddress = true;
                            string[] parts = ipAddress.Split('.');
                            foreach (string part in parts)
                            {
                                int number;
                                if (!int.TryParse(part, out number) || number > 255)
                                {
                                    isValidIpAddress = false;
                                    break;
                                }
                            }

                            if (isValidIpAddress)
                            {
                                ipAddresses.Add(ipAddress);
                                modifiedLine = modifiedLine.Replace(ipAddress, "ip");
                            }
                        }
                        writer.WriteLine(modifiedLine);
                    }
                }

                Console.WriteLine("IP-adresses:");
                foreach (string ipAddress in ipAddresses)
                {
                    Console.WriteLine(ipAddress);
                }
            }
            else if (n == 2)
            {
                string inputFilePath = "task2.txt";
                string outputFilePath = "outputTask2.txt";
                int targetWordLength = 5;

                List<string> wordsOfTargetLength = new List<string>();

                try
                {
                    using (StreamReader reader = new StreamReader(inputFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] words = line.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string word in words)
                            {
                                if (word.Length == targetWordLength)
                                {
                                    wordsOfTargetLength.Add(word);
                                }
                            }
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        foreach (string word in wordsOfTargetLength)
                        {
                            writer.WriteLine(word);
                        }
                    }

                    Console.WriteLine($"Found words length ({targetWordLength}):");
                    foreach (string word in wordsOfTargetLength)
                    {
                        Console.WriteLine(word);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            else if (n == 3)
            {
                string inputFilePath = "task3.txt";
                string outputFilePath = "outputTask3.txt";

                List<string> words = new List<string>();

                try
                {
                    using (StreamReader reader = new StreamReader(inputFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] lineWords = line.Split(new char[] { ' ', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                            words.AddRange(lineWords);
                        }
                    }

                    int maxLength = words.Max(w => w.Length);

                    List<string> longestWords = words.Where(w => w.Length == maxLength).ToList();
                    foreach (string word in longestWords)
                    {
                        words.Remove(word);
                    }

                    using (StreamWriter writer = new StreamWriter(outputFilePath))
                    {
                        foreach (string word in words)
                        {
                            writer.Write(word + " ");
                        }
                    }

                    Console.WriteLine("Text after removing words:");
                    foreach (string word in words)
                    {
                        Console.Write(word + " ");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            else if (n == 4)
            {
                string filePath = "numbers.bin";
                int functionN = 10;

                try
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                    {
                        for (int i = functionN; i >= 1; i--)
                        {
                            double number = 1.0 / i;
                            writer.Write(number);
                        }
                    }
                    Console.WriteLine($"Written {functionN} numbers in file {filePath}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    return;
                }

                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                    {
                        long position = 0;
                        int counter = 1;

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            double number = reader.ReadDouble();

                            if (counter % 3 == 0)
                            {
                                Console.WriteLine($"Numeber {number} with index {counter}");
                            }

                            position += sizeof(double);
                            reader.BaseStream.Seek(position, SeekOrigin.Begin);
                            counter++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Eror: {e.Message}");
                }
            }
            else if (n == 5)
            {
                string studentLastName = "Vanzuryak";

                string folder_1 = @"c:\temp\" + studentLastName + "1";
                string folder_2 = @"c:\temp\" + studentLastName + "2";
                string file_t1 = Path.Combine(folder_1, "t1.txt");
                string file_t2 = Path.Combine(folder_1, "t2.txt");
                string file_t3 = Path.Combine(folder_2, "t3.txt");

                Directory.CreateDirectory(folder_1);
                Directory.CreateDirectory(folder_2);

                File.WriteAllText(file_t1, "<Shevchenko Stepan Ivanovych, 2001> year of birth, place of residence <Sumy city>");
                File.WriteAllText(file_t2, "<Komar Serhiy Fedorovych, 2000 > year of birth, place of residence <Kyiv city>");

                File.Copy(file_t1, file_t3);
                File.AppendAllText(file_t3, Environment.NewLine);
                File.AppendAllText(file_t3, File.ReadAllText(file_t2));

                Console.WriteLine($"Information about files in folder {folder_1}:");
                DisplayFileInfo(folder_1);

                Console.WriteLine($"Information about files in folder {folder_2} (before moving and copying operations):");
                DisplayFileInfo(folder_2);

                File.Move(file_t2, Path.Combine(folder_2, Path.GetFileName(file_t2)));

                File.Copy(file_t1, Path.Combine(folder_2, Path.GetFileName(file_t1)));

                Directory.Move(folder_2, @"d:\temp\ALL");
                Directory.Delete(folder_1);

                Console.WriteLine($"Information about files in folder ALL:");
                DisplayFileInfo(@"d:\temp\ALL");
            }

            static void DisplayFileInfo(string folder)
            {
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine($"File name: {fileInfo.Name}");
                    Console.WriteLine($"Size: {fileInfo.Length} bytes");
                    Console.WriteLine($"Creation date: {fileInfo.CreationTime}");
                    Console.WriteLine($"Last modified date: {fileInfo.LastWriteTime}");
                    Console.WriteLine();
                }
            }
        }
    }
}
