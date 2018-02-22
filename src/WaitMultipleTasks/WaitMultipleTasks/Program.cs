using System;
using System.Linq;
using System.Threading.Tasks;

namespace WaitMultipleTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync();
            Console.ReadKey();
        }

        public static async void RunAsync()
        {
            Console.WriteLine("Starting");

            int delayInterval = 1800;

            int delayIntervalFTI = 2000;

            // Este é uma func com task, nesta eu tenho como dizer que é async
            var funcTask = new Func<Task<int>>(async () => {
                Console.WriteLine($"Working for {delayInterval} at {Environment.TickCount}");

                await Task.Delay(delayInterval);

                Console.WriteLine($"Working for {delayInterval} finished at {Environment.TickCount}");

                return delayInterval;
            });

            // Esta é uma func async que chama um método
            var funcTask1 = new Func<Task<int>>(async () => { return await DoWork(500); });

            // Esta é uma func async que roda com o parêmetro local
            var funcTask2 = new Func<Task<int>>(async () => {
                Console.WriteLine($"Working for {delayIntervalFTI} at {Environment.TickCount}");

                await Task.Delay(delayIntervalFTI);

                Console.WriteLine($"Working for {delayIntervalFTI} finished at {Environment.TickCount}");

                return delayIntervalFTI;
            });

            // Esta é uma func async que chama um método
            var funcTask3 = new Func<Task<int>>(async () => { return await DoWork(300); });

            var funcTasksList = Enumerable.Range(1, 5)
                .Select(x => new Func<Task<int>>(async () => {
                    Console.WriteLine($"Working for {delayIntervalFTI} at {Environment.TickCount}");

                    await Task.Delay(delayIntervalFTI);

                    Console.WriteLine($"Working for {delayIntervalFTI} finished at {Environment.TickCount}");

                    return delayIntervalFTI;
                }))
                .ToList();
            
            // Adiciona as tasks que não estão na lista
            funcTasksList.Add(funcTask1);
            funcTasksList.Add(funcTask2);
            funcTasksList.Add(funcTask);
            funcTasksList.Add(funcTask3);
            
            // Aguarda coloca todas as tasks para rodar (com o comando Task.Run) e aguarda todas serem concluídas
            int[] result = await Task.WhenAll(funcTasksList.Select(s => Task.Run(s)));

            Console.WriteLine("Work for a total of " + result.Sum() + " ms");
        }

        /// <summary>
        /// Método async que pode realizar um processamento e retornar um valor.
        /// </summary>
        /// <param name="delayInterval">Tempo para demorar, como é apenas uma simulação passo em milisegundos para que cada task demore um pouco.</param>
        /// <returns></returns>
        private async static Task<int> DoWork(int delayInterval)
        {
            Console.WriteLine($"Working for {delayInterval} at {Environment.TickCount}");

            await Task.Delay(delayInterval);

            Console.WriteLine($"Working for {delayInterval} finished at {Environment.TickCount}");

            return delayInterval;
        }
    }
}
