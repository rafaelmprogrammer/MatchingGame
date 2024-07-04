using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // Objeto Random para escolher ícones aleatórios para os quadrados
        Random random = new Random();

        // Lista de ícones. Cada ícone aparece duas vezes na lista.
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        // firstClicked aponta para o primeiro Label clicado pelo jogador
        // Será nulo se o jogador ainda não tiver clicado em um label
        Label firstClicked = null;

        // secondClicked aponta para o segundo Label clicado pelo jogador
        Label secondClicked = null;

        // Construtor do formulário
        public Form1()
        {
            InitializeComponent();
            // Chama o método para atribuir ícones aos quadrados
            AssignIconsToSquares();
        }

        /// <summary>
        /// Atribui cada ícone da lista de ícones a um quadrado aleatório
        /// </summary>
        private void AssignIconsToSquares()
        {
            // O TableLayoutPanel tem 16 labels,
            // e a lista de ícones tem 16 ícones,
            // portanto, um ícone é retirado aleatoriamente da lista
            // e adicionado a cada label
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    // Seleciona um ícone aleatório da lista
                    int randomNumber = random.Next(icons.Count);
                    // Atribui o ícone ao texto do label
                    iconLabel.Text = icons[randomNumber];
                    // Oculta o ícone alterando sua cor para a cor de fundo do label
                    iconLabel.ForeColor = iconLabel.BackColor;
                    // Remove o ícone da lista para não ser reutilizado
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        /// <summary>
        /// Manipulador de eventos para o clique em cada label
        /// </summary>
        /// <param name="sender">O label que foi clicado</param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            // Ignora cliques se o timer estiver ativo
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // Ignora cliques em ícones já revelados (cor preta)
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // Se firstClicked é nulo, é o primeiro ícone clicado
                // Define firstClicked para o label clicado e altera sua cor para preta
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                // Se chegou aqui, o timer não está ativo e firstClicked não é nulo
                // Portanto, este é o segundo ícone clicado
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Verifica se o jogador ganhou
                CheckForWinner();

                // Se os ícones clicados são iguais, mantém a cor preta e reseta os cliques
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // Se os ícones são diferentes, inicia o timer para ocultar os ícones após um tempo
                timer1.Start();
            }
        }

        // Manipulador de eventos para o tick do timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Para o timer
            timer1.Stop();

            // Oculta ambos os ícones
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Reseta os cliques para permitir novos cliques
            firstClicked = null;
            secondClicked = null;
        }

        /// <summary>
        /// Verifica se todos os ícones foram correspondidos
        /// </summary>
        private void CheckForWinner()
        {
            // Percorre todos os labels no TableLayoutPanel,
            // verificando se os ícones foram correspondidos
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    // Se encontrar um ícone não correspondido, sai do método
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // Se não encontrou ícones não correspondidos, o jogador ganhou
            // Exibe uma mensagem de vitória e fecha o formulário
            MessageBox.Show("Você correspondeu todos os ícones!", "Parabéns");
            Close();
        }
    }
}

