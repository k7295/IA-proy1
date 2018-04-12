using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_IA1.Mapa
{
    class Mapa
    {
        public int[][] matriz { get; set; }
        public int inicioX { get; set; }
        public int inicioY { get; set; }
        public int m { get; set; }
        public int n { get; set; }
        public int finalX { get; set; }
        public int finalY { get; set; }
        public int tamañoCuadro { get; set; }
        public int[] solucion { get; set; }

        public Mapa(int m, int n, int a)
        {
            this.matriz = generarMatrizVacia(m, n, 0);
            this.tamañoCuadro = a;
            this.solucion = generarListaInt(m * n, -1);
            this.m = m;
            this.n = n;

        }

        private int indiceMatriz_Lista(int x, int y)
        {
            int n = this.matriz[0].Length;
            return x * n + y;
        }

        private int[] indiceLista_Matriz(int num)
        {
            int n = this.matriz[0].Length;
            int[] res = new int[2];
            res[0] = (num / n);
            res[1] = (num % n);
            return res;
        }

        public void dijkstra()
        {
            int m = this.matriz.Length;
            int n = this.matriz[0].Length;

            double[] distancias = this.generarListaDouble(m * n, -1);
            int[] visitados = this.generarListaInt(m * n, 0);
            ArrayList listaVecinos = new ArrayList();
            listaVecinos.Add(new Nodo(finalX, finalY, finalX, finalY, 0));

            dijkstraAux(distancias, visitados, listaVecinos);
        }

        private Nodo removerMenor(ArrayList lista, double[] distancias)
        {
            int indice = 0;
            Nodo nodoMenor;
            for (int i = 0; i < lista.Count; i++)
            {
                Nodo nodo1 = (Nodo)lista[i];
                Nodo nodo2 = (Nodo)lista[indice];
                double distanciaAcumulada1 = distancias[indiceMatriz_Lista(nodo1.inicioX, nodo1.inicioY)];
                double distanciaAcumulada2 = distancias[indiceMatriz_Lista(nodo2.inicioX, nodo2.inicioY)];

                if (nodo1.distancia + distanciaAcumulada1 < nodo2.distancia + distanciaAcumulada2)
                {
                    indice = i;
                }
            }

            nodoMenor = (Nodo)lista[indice];
            lista.RemoveAt(indice);
            return nodoMenor;
        }


        private bool esPrimero(Nodo nodo)
        {
            if (nodo.inicioX == nodo.finalX && nodo.inicioY == nodo.finalY)
            {
                return true;
            }
            return false;
        }

        private void dijkstraAux(double[] distancias, int[] visitados, ArrayList listaVecinos)
        {

            if (listaVecinos.Count > 0)
            {
                Nodo nodoMenor = removerMenor(listaVecinos, distancias);
                int inicioX = nodoMenor.inicioX;
                int inicioY = nodoMenor.inicioY;
                int finalX = nodoMenor.finalX;
                int finalY = nodoMenor.finalY;
                double distanciaNodo = nodoMenor.distancia;
                double distanciaAcumulada1 = distancias[indiceMatriz_Lista(inicioX, inicioY)];
                double distanciaAcumulada2 = distancias[indiceMatriz_Lista(finalX, finalY)];

                if (distanciaNodo + distanciaAcumulada1 < distanciaAcumulada2 || distanciaAcumulada2 == -1)
                {
                    distancias[indiceMatriz_Lista(finalX, finalY)] = distanciaNodo + distanciaAcumulada1;
                    solucion[indiceMatriz_Lista(finalX, finalY)] = indiceMatriz_Lista(inicioX, inicioY);

                    if (visitados[indiceMatriz_Lista(finalX, finalY)] == 0) //no esta visitado
                    {
                        listaVecinos.AddRange(obtenerVecinos(finalX, finalY));
                        visitados[indiceMatriz_Lista(finalX, finalY)] = 1;
                    }
                }

                dijkstraAux(distancias, visitados, listaVecinos);
            }
        }

        private ArrayList obtenerVecinos(int x, int y)
        {
            ArrayList listaNueva = new ArrayList();
            int m = this.matriz.Length;
            int n = this.matriz[0].Length;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (0 <= x + i && x + i < m)
                    {
                        if (0 <= y + j && y + j < n)
                        {
                            if (this.matriz[x + i][y + j] != 1)
                            {
                                double distancia;
                                if(!(i == 0 && j == 0))
                                {
                                    if (i != 0 && j != 0)
                                    {
                                        distancia = Math.Sqrt(2) * this.tamañoCuadro;
                                    }
                                    else
                                    {
                                        distancia = this.tamañoCuadro;
                                    }
                                    listaNueva.Add(new Nodo(x, y, x + i, y + j, distancia));
                                }
                            }
                        }
                    }
                }
            }
            return listaNueva;
        }

        private int[] generarListaInt(int n, int valor)
        {
            int[] listaNueva = new int[n];
            for (int i = 0; i < n; i++)
            {
                listaNueva[i] = valor;
            }
            return listaNueva;
        }

        private double[] generarListaDouble(int n, int valor)
        {
            double[] listaNueva = new double[n];
            for (int i = 0; i < n; i++)
            {
                listaNueva[i] = valor;
            }
            return listaNueva;
        }

        private int[][] generarMatrizVacia(int m, int n, int valor)
        {
            int[][] matrizNueva = new int[m][];

            for (int i = 0; i < m; i++)
            {
                matrizNueva[i] = generarListaInt(n, valor);
            }
            return matrizNueva;
        }

        public void colocarInicio(int x, int y)
        {
            this.inicioX = x;
            this.inicioY = y;
        }

        public void colocarFinal(int x, int y)
        {
            this.finalX = x;
            this.finalY = y;
        }

        public void colocarObstaculos(int cantObstaculos)
        {
            int m = this.matriz.Length;
            int n = this.matriz[0].Length;

            Random rnd = new Random();

            int cont = 0;

            while (cont < cantObstaculos)
            {
                int randX = rnd.Next(0, m);
                int randY = rnd.Next(0, n);
                if (this.matriz[randX][randY] != 1)
                {
                    if (!(randX == this.inicioX && randY == this.inicioY))
                    {
                        if (!(randX == this.finalX && randY == this.finalY))
                        {
                            this.matriz[randX][randY] = 1;
                            cont++;
                        }
                    }
                }
            }

        }

        public bool moverOeste()
        {
            if (inicioX > 0)
            {
                if (matriz[inicioY][inicioX - 1] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioX -= 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverEste()
        {
            if (inicioX < n)
            {
                if (matriz[inicioY][inicioX + 1] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioX += 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverNorte()
        {
            if (inicioY > 0)
            {
                if (matriz[inicioY-1][inicioX] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioY -= 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverSur()
        {
            if (inicioY < m)
            {
                if (matriz[inicioY+1][inicioX] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioY += 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverSurOeste()
        {
            if (inicioX > 0 && inicioY < m)
            {
                if (matriz[inicioY + 1][inicioX-1] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioY += 1;
                    inicioX -= 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverNorOeste()
        {
            if (inicioX > 0 && inicioY > 0)
            {
                if (matriz[inicioY - 1][inicioX - 1] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioX -= 1;
                    inicioY -= 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverNorEste()
        {
            if (inicioX < n && inicioY > 0)
            {
                if (matriz[inicioY - 1][inicioX + 1] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioY -= 1;
                    inicioX += 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool moverSurEste()
        {
            if (inicioX < n && inicioY < m)
            {
                if (matriz[inicioY + 1][inicioX + 1] != 1)
                {
                    matriz[inicioY][inicioX] = 0;
                    inicioX += 1;
                    inicioY += 1;
                    matriz[inicioY][inicioX] = 2;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void imprimirArreglo()
        {
            int m = this.matriz.Length;
            int n = this.matriz[0].Length;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    System.Console.Write(this.matriz[i][j] + " ");
                }
                System.Console.WriteLine();
            }
        }

        private bool todosVisitados(int[] visitados)
        {
            for (int i = 0; i < visitados.Length; i++)
            {
                if (visitados[i] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool existeSolucion()
        {
            int[] indice = new int[2];
            indice[0] = inicioX;
            indice[1] = inicioY;

            bool listo = false;
            while (!listo)
            {
                if (indice[0] == this.finalX && indice[1] == this.finalY)
                {
                    listo = true;
                }
                if(indice[0] == -1)
                {
                    return false;
                }
                indice = next(indice);
            }
            return true;
        }

        public ArrayList demeSolucion()
        {
            ArrayList res = new ArrayList();
            int[] indice = new int[2];
            indice[0] = inicioX;
            indice[1] = inicioY;

            bool listo = false;
            while (!listo)
            {
                if (indice[0] == this.finalX && indice[1] == this.finalY)
                {
                    listo = true;
                }
                res.Add(indice);
                indice = next(indice);
            }
            return res;
        }

		public int getElementoPos(int m,int n)
		{
			return matriz[m][n];
		}

		public void setElementoPos(int m, int n,int p)
		{
			matriz[m][n]=p;
		}

		private int[] next(int[] actual)
        {
            return indiceLista_Matriz(solucion[indiceMatriz_Lista(actual[0], actual[1])]);
        }

		public void Main2()
        {
            Mapa m = new Mapa(5, 6, 100);
            m.colocarInicio(0, 0);
            m.colocarFinal(4, 4);
            m.colocarObstaculos(10);
			m.imprimirArreglo();
			/* System.Console.WriteLine("-----------------------------------------------------------------");
			 System.Console.WriteLine("El mapa actual");
			 System.Console.WriteLine("-----------------------------------------------------------------");

			 m.dijkstra();


			 if (m.existeSolucion())
			 {
				 ArrayList lista = m.demeSolucion();
				 System.Console.WriteLine("El tamaño de la solucion es: " + lista.Count);

				 System.Console.WriteLine();
				 System.Console.WriteLine("-----------------------------------------------------------------");
				 System.Console.WriteLine("La solucion es:");
				 System.Console.WriteLine("-----------------------------------------------------------------");
				 System.Console.WriteLine("X Y");

				 foreach (int[] n in lista)
				 {
					 System.Console.WriteLine(n[0] + " " + n[1]);

				 }
			 }else
			 {
				 System.Console.WriteLine("No existe solucion.");
			 }*/








			System.Console.Read();
        }



    }
}
