using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_IA1.Mapa
{
    class Nodo
    {
        public int inicioX;
        public int inicioY;
        public int finalX;
        public int finalY;
        public double distancia;

        public Nodo(int inicioX, int inicioY, int finalX, int finalY, double distancia)
        {
            this.inicioX = inicioX;
            this.inicioY = inicioY;
            this.finalX = finalX;
            this.finalY = finalY;

            this.distancia = distancia;
        }
    }
}
