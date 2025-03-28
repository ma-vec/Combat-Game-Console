using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using v02_combat;

Arena campo = new Arena();
List<Giocatore> giocatori = new List<Giocatore>();

for (int i = 0; i < 10; i++)
{
    Giocatore G = new Giocatore("Player" + (i + 1));
    giocatori.Add(G);
    Thread g = new Thread(G.Lotta);
    g.Start(campo);
}