using System;
using System.Threading;

class Program
{
    static void Main()
    {
        //  gets and validates file size
        double fileSize;
        while (true)
        {
            Console.Write("Enter file size in MB: ");
            string input = Console.ReadLine();

            if (double.TryParse(input, out fileSize) && fileSize > 0)
                break;
            
            Console.WriteLine("Please enter a number greater than 0");
        }

        
        double remaining = fileSize;
        double downloaded = 0;
        int secondsElapsed = 0;

        double currentSpeed = GetRandomSpeed();
        int secondsUntilSpeedChange = Random.Shared.Next(2, 6); 

        Console.WriteLine();

        
        while (remaining > 0)
        {
            // download for one second at the current speed
            double chunk = Math.Min(currentSpeed, remaining);
            remaining -= chunk;
            downloaded += chunk;
            secondsElapsed++;

            // ETA based on thw average speed so far
            double averageSpeed = downloaded / secondsElapsed;
            double etaAverage = remaining / averageSpeed;

            // ETA based on the current speed
            double etaCurrent = remaining / currentSpeed;

            double percent = downloaded / fileSize * 100;

            Console.Write(
                $"\rSpeed: {currentSpeed:F1} MB/s | " +
                $"Done: {percent:F1}% | " +
                $"ETA (avg): {FormatTime(etaAverage)} | " +
                $"ETA (current): {FormatTime(etaCurrent)}    ");

            // changes speed when the countdown reaches 0
            secondsUntilSpeedChange--;
            if (secondsUntilSpeedChange <= 0)
            {
                currentSpeed = GetRandomSpeed();
                secondsUntilSpeedChange = Random.Shared.Next(2, 6);
            }

            Thread.Sleep(1000);
        }

        Console.WriteLine( );
        Console.WriteLine($"Download complete. Total time: {FormatTime(secondsElapsed)}");
    }

    // random speed between 1 and 5
    static double GetRandomSpeed()
    {
        return 1 + Random.Shared.NextDouble() * 4;
    }

    //seconds to miliseconds
    static string FormatTime(double seconds)
    {
        int totalSeconds = (int)seconds;   

        int minutes = totalSeconds / 60;   
        int secs = totalSeconds % 60;     

        return minutes+ ":" + secs;
    }
}