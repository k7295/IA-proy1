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
        public int m { get; set; }
        public int n { get; set; }
        public int finalX { get; set; }
        public int finalY { get; set; } 
        public int tamañoCuadro { get; set; }
        public int[][][] solucion { get; set; }
        public bool[][] visitados { get; set; }
        public bool existeSolucion_atr { get; set; }
        public bool modoDiagonal;

        public Mapa(int m, int n, int a)
        {
            this.matriz = generarMatrizVacia(m, n, 0);
            this.tamañoCuadro = a;
            this.m = m;
            this.n = n;

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
                if (this.matriz[randX][randY] == 0)
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
                            if (this.matriz[x][y] != 1)
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
                                            lista.Add(new Coordenada(x, y, tamañoCuadro * Math.Sqrt(2) ));
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
            double costeTotal =this.calcularHeuristica(primera);
            int cont = 0;

            foreach (Coordenada c in vecinos)
            {
                double costeActual = this.calcularHeuristica(c);
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

        public void a_estrella(bool diagonales)
        {
            this.modoDiagonal = diagonales;
            this.existeSolucion_atr = true;
            ArrayList vecinos = new ArrayList();
            Coordenada c = new Coordenada(finalX, finalY, 0);
            vecinos.Add(c);
            this.solucion[finalX][finalY][0] = finalX;
            this.solucion[finalX][finalY][1] = finalY;
            a_estrella_aux(vecinos, false);
        }

        public bool existeSolucion()
        {
            return this.existeSolucion_atr;
        }

        private void a_estrella_aux(ArrayList vecinos, bool termino)
        {
            bool termino_actual = false;
            if(vecinos.Count == 0)
            {
                this.existeSolucion_atr = false;
                termino = true;
            }
            if (!termino)
            {
                

                Coordenada c = obtenerMenor(vecinos);
                if(c.x == this.inicioX && c.y == this.inicioY)
                {
                    termino_actual = true;
                }
                
                visitados[c.x][c.y] = true;

                ArrayList vecinos_nuevos = obtenerVecinos(c);
                foreach (Coordenada c_nuevo in vecinos_nuevos)
                {
                    if (this.solucion[c_nuevo.x][c_nuevo.y][0] == -1)
                    {
                        this.solucion[c_nuevo.x][c_nuevo.y][0] = c.x;
                        this.solucion[c_nuevo.x][c_nuevo.y][1] = c.y;
                        this.coste_Total[c_nuevo.x][c_nuevo.y] = c_nuevo.distancia + this.coste_Total[c.x][c.y];
                        vecinos.Add(c_nuevo);

                    }
                    else
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
            return this.distancias_M[c_actual.x][c_actual.y] + this.coste_Total[c_anterior.x][c_anterior.y] + c_actual.distancia;
        }

        private double calcularHeuristica(Coordenada c)
        {
            return this.distancias_M[c.x][c.y] + this.coste_Total[c.x][c.y];
        }

        public int[] next(int[] actual)
        {
            return solucion[actual[0]][actual[1]];
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

        public void limpiarRuta()
        {
            for(int i = 0;i< m; i++)
            {
                for(int u = 0;u< n; u++)
                {
                    if (matriz[i][u] == 3)
                    {
                        matriz[i][u] = 0;
                    }
                }
            }
        }

        public void enableDiagonal()
        {
            modoDiagonal = true;
        }

        public void disableDiagonal()
        {
            modoDiagonal = false;
        }

        public bool crearRuta()
        {
            this.distancias_M = generarMatrizVaciaInt(m, n, 0);
            this.coste_Total = generarMatrizVaciaDouble(m, n, 0);
            this.solucion = generarMatrizSolucion(m, n, -1, -1);
            this.visitados = generarMatrizVisitados(m, n);
            a_estrella(modoDiagonal);
            if (existeSolucion())
            {
                bool termino = false;
                int[] actual = new int[2];
                actual[0] = inicioX;
                actual[1] = inicioY;
                while (!termino)
                {
                    System.Console.WriteLine(actual[0] + " " + actual[1]);

                    matriz[actual[0]][actual[1]] = 3;
                    if (actual[0] == finalX && actual[1] == finalY)
                    {
                        matriz[actual[0]][actual[1]] = 4;
                        termino = true;
                    }
                    actual = next(actual);

                }
                matriz[inicioX][inicioY] = 2;
                return true;
            }
            else
            {
                return false;
            }
            
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

        
        public bool moverOeste()
        {
            if (inicioY > 0)
            {
                if (matriz[inicioX][inicioY - 1] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioY -= 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioY+1 < n)
            {
                if (matriz[inicioX][inicioY + 1] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioY += 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioX > 0)
            {
                if (matriz[inicioX-1][inicioY] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioX -= 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioX + 1 < m)
            {
                if (matriz[inicioX+1][inicioY] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioX += 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioY > 0 && inicioX + 1 < m)
            {
                if (matriz[inicioX + 1][inicioY-1] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioX += 1;
                    inicioY -= 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioY > 0 && inicioX > 0)
            {
                if (matriz[inicioX - 1][inicioY - 1] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioX -= 1;
                    inicioY -= 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioY + 1 < n && inicioX > 0)
            {
                if (matriz[inicioX - 1][inicioY + 1] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioX -= 1;
                    inicioY += 1;
                    matriz[inicioX][inicioY] = 2;
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
            if (inicioY+1 < n && inicioX+1 < m)
            {
                if (matriz[inicioX + 1][inicioY + 1] != 1)
                {
                    matriz[inicioX][inicioY] = 0;
                    inicioX += 1;
                    inicioY += 1;
                    matriz[inicioX][inicioY] = 2;
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

        

		public int getElementoPos(int m,int n)
		{
			return matriz[m][n];
		}

		public void setElementoPos(int m, int n,int p)
		{
			matriz[m][n]=p;
		}
        

		public void Main2()
        {
            Mapa m = new Mapa(5, 6, 100);
            m.colocarInicio(0, 0);
            m.colocarFinal(4, 4);
            m.colocarObstaculos(10);
			m.imprimirArreglo();
		








			System.Console.Read();
        }



    }
}
