using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace v02_combat
{
    internal class Giocatore
    {
        string name;
        int PF_iniziali;
        int PF;
        int attacco;
        int difesa;

        public Giocatore(string name)
        {
            this.name = name;
            Random casuale = new Random();
            PF_iniziali = casuale.Next(1000, 5000);
            PF = PF_iniziali;
            attacco = casuale.Next(200, 500);
            difesa = casuale.Next(0, 100);
        }

        public void Lotta(object arenaObj)
        {
            Arena arena = (Arena)arenaObj;
            Console.WriteLine(name + " è pronto a combattere");

            // Accedo all'arena
            arena.accessi.WaitOne();
            Console.WriteLine(name + " è entrato nell'arena");
            arena.combattenti.Add(this);

            // Attendo l'avversario
            while (arena.combattenti.Count < 2)
            {
                Console.WriteLine(name + " attende il suo avversario...");
                Thread.Sleep(500);
            }

            // Seleziona l'avversario
            Giocatore avversario;
            if (arena.combattenti[0] == this)
            {
                avversario = arena.combattenti[1];
            }
            else
            {
                avversario = arena.combattenti[0];
            }

            Console.WriteLine(name + " sfida " + avversario.name);

            Random random = new Random();

            while (this.PF > 0 && avversario.PF > 0) //fino alla morteeeeee
            {
                Attacca(this, avversario, random);

                if (avversario.PF > 0)
                {
                    Attacca(avversario, this, random);
                }
            }

            if (this.PF > 0) //decreta il vincitore
            {
                Console.WriteLine(this.name + " ha vinto!");
            }
            else
            {
                Console.WriteLine(avversario.name + " ha vinto!");
            }

            // Rimuove i combattenti dall'arena
            arena.combattenti.Remove(this);
            arena.combattenti.Remove(avversario);

            // Rilascia il semaforo
            arena.accessi.Release();
        }

        private void Attacca(Giocatore attaccante, Giocatore difensore, Random random)
        {
            int percSingoloAttacco = random.Next(1, 101);

            if (percSingoloAttacco >= difensore.difesa)
            {
                // Il difensore riesce a parare parzialmente il colpo
                int riduzioneDanni = random.Next(50, 101);
                int danniEffettivi = attaccante.attacco * (100 - riduzioneDanni) / 100;
                difensore.PF -= danniEffettivi;

                Console.WriteLine(attaccante.name + " attacca " + difensore.name +
                    " infliggendo " + danniEffettivi + " danni! (Parziale)");
            }
            else
            {
                // Subisce il danno pieno
                difensore.PF -= attaccante.attacco;
                Console.WriteLine(attaccante.name + " attacca " + difensore.name +
                    " infliggendo " + attaccante.attacco + " danni!");
            }

            if (difensore.PF <= 0)
            {
                Console.WriteLine(difensore.name + " è stato sconfitto!");
            }
            Thread.Sleep(1000);
        }
    }

}
