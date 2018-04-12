using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_IA1.Mapa
{
    class Coordenada
    {
        public int x;
        public int y;
        public double distancia;

        public Coordenada(int x, int y, double distancia)
        {
            this.x = x;
            this.y = y;
            this.distancia = distancia;
        }
    }
}
