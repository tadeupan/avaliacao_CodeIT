using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sistema_de_Entregas
{
    
    class Produtos
    {
        
        private string caminho = System.Environment.CurrentDirectory.ToString() + "\\bdProdutos.txt";

        public string[] InfoProdutos()
        {

            //Lista com os produtos pré determinados//

            string[] produtos = new string[8];

            produtos[0] = "Livro|1|60";
            produtos[1] = "Joystick|2|95";
            produtos[2] = "Camiseta|3|65";
            produtos[3] = "Material Escolar|4|125";
            produtos[4] = "Cafeteira|5|165";
            produtos[5] = "Televisão|6|170";
            produtos[6] = "Jogo de Panelas|7|190";
            produtos[7] = "Impressora|8|180";

            return produtos;
        }

        public string[] LerBase()
        {
            string[] linhas = null;

            if (!System.IO.File.Exists(caminho))
            {
                EscreveArquivoBD(InfoProdutos());
                linhas = InfoProdutos();
            }
            else
            {
                using (StreamReader Reader = new StreamReader(caminho))
                {
                    linhas = System.IO.File.ReadAllLines(caminho);
                }
            }

            return linhas;

        }

        private void EscreveArquivoBD(string[] ListaDeProdutos)
        {
            //Cria uma base de dados para armazenar futuros produtos//
            System.IO.File.WriteAllLines(caminho, ListaDeProdutos);

        }

        public void AdicionarProdutos(string NomeProduto, string TempoEntrega, string ValorFrete)
        {
            //Insere novas linhas na base de dados
            string[] temp = new string[System.IO.File.ReadAllLines(caminho).Length + 1];
            string[] linhas = System.IO.File.ReadAllLines(caminho);
            string NovaLinha = NomeProduto + "|" + TempoEntrega + "|" + ValorFrete;

            for (int i = 0; i < linhas.Length; i++)
            {
                temp[i] = linhas[i];
            }

            temp[temp.Length - 1] = NovaLinha;

            System.IO.File.WriteAllLines(caminho, temp);
        }
    }
}
