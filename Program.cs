using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Builders
{
    class Builder
    {
        private string name;
        private Random random;
        private int progress;

        public Builder(string name)
        {
            this.name = name;
            this.random = new Random();
            this.progress = 0;
        }

        public void Build()
        {
            while (progress < 100)
            {
                int increment = random.Next(5, 15); 
                progress += increment;
                if (progress > 100)
                    progress = 100;
                Console.WriteLine($"Строитель {name}: построено {progress}%");
                Thread.Sleep(random.Next(500, 1500));
            }

            Console.WriteLine($"Строитель {name} завершил работу.");
        }

        public int GetProgress()
        {
            return progress;
        }
    }
    class Foreman
    {
        private string name;
        private List<Builder> builders;
        private DateTime lastReportTime;

        public Foreman(string name, List<Builder> builders)
        {
            this.name = name;
            this.builders = builders;
            this.lastReportTime = DateTime.Now;
        }

        public void ManageConstruction()
        {
            while (!IsConstructionComplete())
            {
                ReportProgress();
                Thread.Sleep(2000);
            }
            ReportProgress();
            Console.WriteLine($"Бригадир {name} завершил свою работу.");
        }

        private void ReportProgress()
        {
            int totalProgress = CalculateTotalProgress();
            Console.WriteLine($"Отчет бригадира {name}: {totalProgress}% выполнено.");
        }

        private int CalculateTotalProgress()
        {
            if (builders.Count == 0)
                return 0;

            int totalProgress = 0;
            foreach (var builder in builders)
            {
                totalProgress += builder.GetProgress();
            }
            totalProgress = totalProgress / builders.Count;

            return totalProgress;
        }

        private bool IsConstructionComplete()
        {
            foreach (var builder in builders)
            {
                if (builder.GetProgress() < 100)
                    return false;
            }
            return true;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Builder> builders = new List<Builder>();
            builders.Add(new Builder("Петров"));
            builders.Add(new Builder("Сидоров"));
            builders.Add(new Builder("Козлов"));
            builders.Add(new Builder("Николаев"));
            builders.Add(new Builder("Морозов"));

            Foreman foreman = new Foreman("Иванов", builders);
            List<Thread> builderThreads = new List<Thread>();
            foreach (var builder in builders)
            {
                Thread thread = new Thread(builder.Build);
                builderThreads.Add(thread);
                thread.Start();
            }
            Thread foremanThread = new Thread(foreman.ManageConstruction);
            foremanThread.Start();
            foreach (var thread in builderThreads)
            {
                thread.Join();
            }
            foremanThread.Join();

            Console.WriteLine("Строительство завершено. Дом построен!");
        }
    }
}
