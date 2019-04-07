using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_de_Entregas
{
    public partial class Form1 : Form
    {
        Produtos produto = new Produtos();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            string[] temp = null;
            MontaGrid(1,temp);
        }

        private void MontaGrid(int id, string[] lista)
        {
            if (id == 1)
            {

                Produtos produtos = new Produtos();

                string[] lstprodutos = produtos.LerBase();

                if (GridItems.Rows.Count > 0)
                {
                    GridItems.Rows.Clear();
                    GridItems.Columns.Clear();
                }

                GridItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                GridItems.RowHeadersVisible = false;
                GridItems.ReadOnly = true;
                GridItems.AllowUserToAddRows = false;
                GridItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                GridItems.Columns.Add("Nome Produto", "Nome Produto");
                GridItems.Columns.Add("Tempo de Entrega", "Tempo de Entrega");
                GridItems.Columns.Add("Valor Frete", "Valor Frete");
                
                foreach (var item in lstprodutos)
                {
                    string[] temp = item.Split('|');

                    GridItems.Rows.Add(temp[0], temp[1] , temp[2]);
                }

                if (GridItems.SelectedRows.Count > 0)
                {
                    int index = GridItems.SelectedRows[0].Index;

                    if (index >= 0)
                        GridItems.Rows[index].Selected = false;
                }

            }
            else
            {
                if (GridEntrega.Rows.Count > 0)
                {
                    GridEntrega.Rows.Clear();
                    GridEntrega.Columns.Clear();
                }

                GridEntrega.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                GridEntrega.RowHeadersVisible = false;
                GridEntrega.ReadOnly = true;
                GridEntrega.AllowUserToAddRows = false;
                GridEntrega.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                GridEntrega.Columns.Add("Nome Produto", "Nome Produto");
                GridEntrega.Columns.Add("Tempo de Entrega", "Tempo de Entrega");
                GridEntrega.Columns.Add("Valor do Frete", "Valor do Frete");

                for (int i = 0; i < lista.Length; i++)
                {
                    if(lista[i] != null)
                    {
                        string[] temp = lista[i].Split('|');
                        GridEntrega.Rows.Add(temp[0], temp[1], temp[2]);
                    }
               }
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtEntrega.Text == "" || txtFrete.Text == "" || txtItem.Text == "")
            {
                return;
            }

            Produtos Produtos = new Produtos();

            string NomeProduto = txtItem.Text;
            string TempoEntrega = txtEntrega.Text;
            string ValorFrete = txtFrete.Text;
            string[] temp = null;

            txtItem.Text = "";
            txtEntrega.Text = "";
            txtFrete.Text = "";

            Produtos.AdicionarProdutos(NomeProduto, TempoEntrega, ValorFrete);

            MontaGrid(1,temp);
        }

        private void BtnCalcular_Click(object sender, EventArgs e)
        {
            string[] items = null;
            float[] valores = null;
            float[] prazos = null;
            string[] linhas = null;
            float valorFrete = 0;
            float tempo = 0;
            float resultado = 0;
            float valorItem = 0;
            string item = "";
            float tempoEntrega = float.Parse(txtTotalEntrega.Text);

            //verifica se tem items selecionados no grid
            if (GridItems.SelectedRows.Count > 0)
            {
                int j = 0;
                items = new string[GridItems.SelectedRows.Count];
                valores = new float[GridItems.SelectedRows.Count];
                prazos = new float[GridItems.SelectedRows.Count];
                linhas = new string[GridItems.SelectedRows.Count];

                for (int i = 0; i < GridItems.Rows.Count; i++)
                {
                    if (GridItems.Rows[i].Selected == true)
                    {
                        item = GridItems.Rows[i].Cells[0].Value.ToString();
                        valorFrete = float.Parse(GridItems.Rows[i].Cells[2].Value.ToString());
                        tempo = float.Parse(GridItems.Rows[i].Cells[1].Value.ToString());
                        resultado = valorFrete / tempo;

                        //preenche vetores com linhas selecionadas no grid para efetuar a comparação
                        linhas[j] = item + "|" + resultado + "|" + tempo + "|" + valorFrete;
                        j++;
                    }
                }
            }
            else
            {
               items = new string[GridItems.Rows.Count];
               valores = new float[GridItems.Rows.Count];
               prazos = new float[GridItems.Rows.Count];
               linhas = new string[GridItems.Rows.Count];

                for (int i = 0; i < GridItems.Rows.Count; i++)
                {
                    item = GridItems.Rows[i].Cells[0].Value.ToString();
                    valorFrete = float.Parse(GridItems.Rows[i].Cells[2].Value.ToString());
                    tempo = float.Parse(GridItems.Rows[i].Cells[1].Value.ToString());
                    resultado = valorFrete / tempo;

                    //preenche vetor com todas as linhas do grid para efetuar a comparação
                    linhas[i] = item + "|" + resultado + "|" + tempo +"|"+ valorFrete;
                }           
            }

            items = new string[linhas.Length];
            string[] ItensOrdenados = linhas;
            List<float> lstValores = new List<float>();
            for (int i = 0; i < linhas.Length; i++)
            {
                string[] temp = linhas[i].Split('|');
                lstValores.Add(float.Parse(temp[1]));
            }

            lstValores.Sort();
            
            //Ordena Valores pelo Valor do frete dividido pela quantidade de horas de entrega de cada produto
            linhas = new string[lstValores.Count];
            int posicao = 0;
            for (int i = lstValores.Count; i > 0; i--)
            {
                string verificaValor = lstValores[i - 1].ToString();

                for (int cont = 0; cont < ItensOrdenados.Length; cont++)
                {
                    string[] temp = ItensOrdenados[cont].Split('|');

                    if (verificaValor == temp[1])
                    {
                        linhas[posicao] = temp[0] +"|"+ temp[1] + "|" + temp[2] + "|"+ temp[3];
                        posicao++;
                    }
                   
                }
            }

            //Ordena os itens de acordo com o tempo de entrega de cada um para com o tempo total de entrega (21 horas) e monta o grid de entrega
           items = new string[linhas.Length];
           float tempoTotal = 0;
           float valorTotal = 0;
           
           for (int i = 0; i < linhas.Length; i++)
           {
                string[] temp = linhas[i].Split('|');
                tempoTotal = tempoTotal + float.Parse(temp[2]);
                valorTotal = valorTotal + float.Parse(temp[3]);

                if (tempoTotal <= float.Parse(txtTotalEntrega.Text))
                {
                    items[i] = temp[0] + "|" + temp[2] + "|" + temp[3];
                }else
                {
                    tempoTotal = tempoTotal - float.Parse(temp[2]);
                    valorTotal = valorTotal - float.Parse(temp[3]);
                    break;
                }

            }

            LblTempo.Text = tempoTotal + " hs";
            LblValor.Text = valorTotal.ToString("C");

            MontaGrid(2,items);
        }
    }
}
