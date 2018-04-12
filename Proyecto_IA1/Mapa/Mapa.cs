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
        public int[][] distancias_M { get; set; }
        public double[][] coste_Total { get; set; }
        public int inicioX { get; set; }
        public int inicioY { get; set; }
        public int finalX { get; set; }
        public int finalY { get; set; }
        public int tamañoCuadro { get; set; }
        public int[][][] solucion { get; set; }
        public bool[][] visitados { get; set; }
        public bool modoDiagonal;

        public Mapa(int m, int n, int a)
        {
            this.matriz = generarMatrizVaciaInt(m, n, 0);
            this.distancias_M = generarMatrizVaciaInt(m, n, 0);
            this.coste_Total = generarMatrizVaciaDouble(m, n, 0);
            this.tamañoCuadro = a;
            this.solucion = generarMatrizSolucion(m, n, -1, -1);
            this.visitados = generarMatrizVisitados(m, n);
        }

        private int[][][] generarMatrizSolucion(int m, int n, int valorX, int valorY)
        {
            int[][][] res = new int[m][][];
            for (int i = 0; i < m; i++)
            {
                res[i] = new int[n][];
                for (int j = 0; j < n; j++)
                {
                    int[] coordenada = new int[2];
                    coordenada[0] = valorX;
                    coordenada[1] = valorY;
                    res[i][j] = coordenada;
                }
            }
            return res;
        }

        private bool[][] generarMatrizVisitados(int m, int n)
        {
            bool[][] res = new bool[m][];
            for (int i = 0; i < m; i++)
            {
                res[i] = new bool[n];
                for (int j = 0; j < n; j++)
                {
                    res[i][j] = false;
                }
            }
            return res;
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

        private int[][] generarMatrizVaciaInt(int m, int n, int valor)
        {
            int[][] matrizNueva = new int[m][];

            for (int i = 0; i < m; i++)
            {
                matrizNueva[i] = generarListaInt(n, valor);
            }
            return matrizNueva;
        }

        private double[][] generarMatrizVaciaDouble(int m, int n, int valor)
        {
            double[][] matrizNueva = new double[m][];

            for (int i = 0; i < m; i++)
            {
                matrizNueva[i] = generarListaDouble(n, valor);
            }
            return matrizNueva;
        }

        private void colocarInicio(int x, int y)
        {
            this.inicioX = x;
            this.inicioY = y;
        }

        private void colocarFinal(int x, int y)
        {
            this.finalX = x;
            this.finalY = y;
        }


        private void colocarObstaculos(int cantObstaculos)
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

        private void calcular_distancias_M()
        {
            int m = this.matriz.Length;
            int n = this.matriz[0].Length;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    this.distancias_M[i][j] = Math.Abs(inicioX - i) + Math.Abs(inicioY - j);
                }
            }
        }

        private ArrayList obtenerVecinos(Coordenada c)
        {
            int m = this.matriz.Length;
            int n = this.matriz[0].Length;
            ArrayList lista = new ArrayList();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int x = c.x + i;
                    int y = c.y + j;

                    if (!(x >= m || x < 0))
                    {
                        if (!(y >= n || y < 0))
                        {
                            if(this.matriz[x][y] != 1)
                            {
                                if (!this.visitados[x][y])
                                {
                                    if (i == 0 || j == 0)
                                    {
                                        lista.Add(new Coordenada(x, y, tamañoCuadro));
                                    }
                                    else
                                    {
                                        if (this.modoDiagonal)
                                        {
                                            lista.Add(new Coordenada(x, y, tamañoCuadro * Math.Sqrt(2)));
                                        }
                                    }
                                }
                            } 
                        }
                    }
                }
            }
            return lista;
        }

        private int indiceMenor(ArrayList vecinos)
        {
            int i = 0;
            Coordenada primera = (Coordenada)vecinos[0];
            double costeTotal = this.coste_Total[primera.x][primera.y] + this.distancias_M[primera.x][primera.y];
            int cont = 0;

            foreach (Coordenada c in vecinos)
            {
                double costeActual = this.coste_Total[c.x][c.y] + this.distancias_M[c.x][c.y];
                if (costeActual < costeTotal)
                {
                    i = cont;
                }
                cont++;
            }
            return i;
        }

        private Coordenada obtenerMenor(ArrayList vecinos)
        {
            int indice = indiceMenor(vecinos);
            Coordenada c = (Coordenada)vecinos[indice];
            vecinos.RemoveAt(indice);

            return c;
        }

        private int indiceFinal(ArrayList vecinos)
        {
            int cont = 0;
            foreach (Coordenada c in vecinos)
            {
                if (c.x == inicioX && c.y == inicioY)
                {
                    return cont;
                }
                cont++;
            }
            return -1;
        }

        private void a_estrella(bool diagonales)
        {
            this.modoDiagonal = diagonales;
            ArrayList vecinos = new ArrayList();
            Coordenada c = new Coordenada(finalX, finalY, 0);
            vecinos.Add(c);
            this.solucion[finalX][finalY][0] = finalX;
            this.solucion[finalX][finalY][1] = finalY;
            a_estrella_aux(vecinos, false);
        }



        private void a_estrella_aux(ArrayList vecinos, bool termino)
        {

            bool termino_actual = false;
            if (!termino)
            {

                Coordenada c;

                if (indiceFinal(vecinos) != -1)
                {
                    c = (Coordenada)vecinos[indiceFinal(vecinos)];
                    termino_actual = true;
                } else
                {
                    c = obtenerMenor(vecinos);
                }

                visitados[c.x][c.y] = true;

                ArrayList vecinos_nuevos = obtenerVecinos(c);
                foreach (Coordenada c_nuevo in vecinos_nuevos)
                {
                    if (this.solucion[c_nuevo.x][c_nuevo.y][0] == -1)
                    {
                        this.solucion[c_nuevo.x][c_nuevo.y][0] = c.x;
                        this.solucion[c_nuevo.x][c_nuevo.y][1] = c.y;
                        vecinos.Add(c_nuevo);
                    } else
                    {
                        if (calcularHeuristica(c_nuevo, c) < calcularHeuristica(c_nuevo))
                        {
                            this.solucion[c_nuevo.x][c_nuevo.y][0] = c.x;
                            this.solucion[c_nuevo.x][c_nuevo.y][1] = c.y;

                            this.coste_Total[c_nuevo.x][c_nuevo.y] = c_nuevo.distancia + this.coste_Total[c.x][c.y];
                        }
                    }
                }
                a_estrella_aux(vecinos, termino_actual);
            }
        }

        private double calcularHeuristica(Coordenada c_actual, Coordenada c_anterior)
        {
            return this.distancias_M[c_actual.x][c_actual.y] + this.coste_Total[c_anterior.x][c_anterior.y];
        }

        private double calcularHeuristica(Coordenada c)
        {
            return this.distancias_M[c.x][c.y] + this.coste_Total[c.x][c.y];
        }

        private int[] next(int[] actual)
        {
            return solucion[actual[0]][actual[1]];
        }

        private void imprimirArreglo()
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


        static void Main(string[] args)
        {
            Mapa m = new Mapa(6,6,100);
            m.colocarInicio(0, 0);
            m.colocarFinal(5, 5);
            m.colocarObstaculos(10);
            m.a_estrella(true);
            m.imprimirArreglo();

            bool termino = false;
            int[] actual = new int[2];
            actual[0] = 0;
            actual[1] = 0;
            while (!termino)
            {
                System.Console.WriteLine(actual[0] + " " + actual[1]);
                if(actual[0] == 5 && actual[1] == 5)
                {
                    termino = true;
                }
                actual = m.next(actual);

            }

            System.Console.Read();
        }
    } 
}
