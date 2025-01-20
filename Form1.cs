using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms; 

namespace TaTeTi_1._0
{
    public partial class Form1 : Form
    {


        private SoundPlayer soundPlayer;
        private SoundPlayer put;

        private Panel[,] panels;
        
        Game game = new Game();
        Bot bot = null;

        private Panel menuPanel;
        private Panel difficultyPanel;

        private Panel configurationPanle;


        private Label scoreLabel;
        public Form1()
        {
            
            // Suscribirse al evento OnGameEnd
            game.OnGameEnd += OnGameEnd;

            InitializeComponent();

            soundPlayer = new SoundPlayer("Resources/sound.wav"); 
            soundPlayer.PlayLooping(); // Reproduce en loop

            put = new SoundPlayer("Resources/put.wav"); 

            // Establecer el tamaño de la ventana a 480x480
            this.ClientSize = new Size(480, 480);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();
            
            // Colocar imagen de fondo
            this.BackgroundImage = Image.FromFile("Resources/ta te ti vacio.png");


            // Inicializamos los paneles
            InitializePanel(ref menuPanel);
            InitializePanel(ref difficultyPanel, true);
            InitializeConfigurationPanel(ref configurationPanle);
        }
        
        private void AdjustButtonPanelPosition(Panel panel, byte newCenterWidth = 0, byte newCenterHeight = 0)
        {   

            if (panel != null)
            {
                panel.Controls[0].Location = new Point(
                    (this.ClientSize.Width - 200) / 2 + newCenterWidth,
                    (this.ClientSize.Height - panel.Controls[0].Height) / 2 + newCenterHeight
                );
            }
        }

        // Inicialización del panel del menú principal
        private void InitializePanel(ref Panel panel, bool isDifficultyPanel = false)
        {
            panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = !isDifficultyPanel
            };
            this.Controls.Add(panel);

            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Location = new Point(
                    (this.ClientSize.Width - 200) / 2,
                    (this.ClientSize.Height - 200) / 2
                ),
                Size = new Size(200, 200)
            };

            // Añadir los botones
            if (!isDifficultyPanel)
            {
                AddMenuButton("1 Jugador", MenuButtonClick, buttonPanel);
                AddMenuButton("2 Jugadores", MenuButtonClick, buttonPanel);
                AddMenuButton("Configuración", MenuButtonClick, buttonPanel);
                AddMenuButton("Salir", MenuButtonClick, buttonPanel);
            }
            else
            {
                
                AddMenuButton("Muy Fácil", DifficultyButtonClick, buttonPanel);
                AddMenuButton("Fácil", DifficultyButtonClick, buttonPanel);
                AddMenuButton("Normal", DifficultyButtonClick, buttonPanel);
                AddMenuButton("Difícil", DifficultyButtonClick, buttonPanel);
                AddMenuButton("Experto", DifficultyButtonClick, buttonPanel);
                AddMenuButton("Volver", DifficultyButtonClick, buttonPanel);    
            }

            panel.Controls.Add(buttonPanel);
        }
        private void InitializeConfigurationPanel(ref Panel panel)
        {
            panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Visible = false
            };
            this.Controls.Add(panel);

            // Crear un FlowLayoutPanel para los elementos
            var buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Anchor = AnchorStyles.None,
                Location = new Point(
                    (this.ClientSize.Width - 200) / 2,
                    (this.ClientSize.Height - 200) / 2
                ),
                Size = new Size(200, 200)
            };

            // Crear un panel para el label y el checkbox
            var musicPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Anchor = AnchorStyles.None
            };

            // Añadir el label de configuración
            var configLabel = new Label
            {
                Text = "Música",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true
            };
            musicPanel.Controls.Add(configLabel); 

            // Crear y agregar el CheckBox al musicPanel
            var musicCheckBox = new CheckBox
            {
                Text = "Activar Música",
                AutoSize = true 
            };
            musicCheckBox.CheckedChanged += (sender, e) => MusicCheckBoxChanged(musicCheckBox.Checked);
            musicPanel.Controls.Add(musicCheckBox); 

            buttonPanel.Controls.Add(musicPanel);

            // Agregar el botón "Volver" usando AddMenuButton
            AddMenuButton("Volver", DifficultyButtonClick, buttonPanel);

            // Agregar el buttonPanel al panel de configuración
            panel.Controls.Add(buttonPanel);
        }



        private void MusicCheckBoxChanged(bool isChecked)
        {
            if (isChecked)
            {
                soundPlayer.PlayLooping(); 
            }
            else
            {
                soundPlayer.Stop(); 
            }
        }
        // Función para agregar botones
        private void AddMenuButton(string text, EventHandler onClick, FlowLayoutPanel buttonPanels)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(200, 50),
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10) // Espaciado entre botones
            };
            button.Click += onClick;
            buttonPanels.Controls.Add(button);
        }

        // Manejador de eventos para los botones del menú principal
        private void MenuButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            switch (clickedButton.Text)
            {
                case "1 Jugador":
                    menuPanel.Visible = false;
                    difficultyPanel.Visible = true;
                    AdjustButtonPanelPosition(difficultyPanel);
                    break;
                case "2 Jugadores":
                    bot = null;
                    menuPanel.Visible = false;
                    matrizConstructor();
                    break;
                case "Configuración":
                    menuPanel.Visible = false;
                    configurationPanle.Visible = true;
                    AdjustButtonPanelPosition(configurationPanle, 0, 50);
                    break;
                case "Salir":
                    this.Close();
                    break;
            }
        }

        // Manejador de eventos para los botones de dificultad
        private void DifficultyButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            switch (clickedButton.Text)
            {
                case "Muy Fácil":
                    bot = new BotVeryEasy();
                    difficultyPanel.Visible = false; 
                    matrizConstructor();

                    break;
                case "Fácil":
                    bot = new BotEasy();
                    difficultyPanel.Visible = false; 
                    matrizConstructor();
                    break;
                case "Normal":
                    bot = new BotMedium();
                    difficultyPanel.Visible = false; 
                    matrizConstructor();
                    break;
                case "Difícil":
                    bot = new BotHard();
                    difficultyPanel.Visible = false; 
                    matrizConstructor();

                    break;
                case "Experto":
                    bot = new BotExpert();
                    difficultyPanel.Visible = false; 
                    matrizConstructor();
                    break;
                case "Volver":
                    configurationPanle.Visible = false;
                    difficultyPanel.Visible = false; 
                    menuPanel.Visible = true;
                    break;
            }
        }

        private void matrizConstructor()
        {
            // Eliminar paneles anteriores si existen
            if (panels != null)
            {
                foreach (var panel in panels)
                {
                    if (panel != null)
                    {
                        this.Controls.Remove(panel);
                        panel.Dispose(); // Asegúrate de liberar recursos
                    }
                }
            }
            panels = new Panel[3, 3];
            
            // Crear y configurar el botón "Volver"
            Button backButton = new Button
            {
                Text = "Volver",
                Size = new Size(100, 50),
                Location = new Point(10, 10), 
                BackColor = Color.White,
                ForeColor = Color.Black
            };

            backButton.Click += BackButton_Click; 
            this.Controls.Add(backButton); 

            byte player1Score = game.Player1Score;
            byte player2Score = game.Player2Score;
            
            if (scoreLabel != null)
            {
                this.Controls.Remove(scoreLabel);
                scoreLabel.Dispose(); // Liberar recursos del label anterior
            }


            scoreLabel = new Label
            {
                Text = "P1: " + player1Score + "   P2: " + player2Score,
                Location = new Point(340, 10),
                Size = new Size(200, 30),
                BackColor = Color.Transparent,
                ForeColor = Color.Black,
                Font = new Font("Arial", 14, FontStyle.Bold)
            };

            this.Controls.Add(scoreLabel);


            // Crear una matriz de 3x3
            byte panelSize = 135;
            byte margin = 25;

            for (byte row = 0; row < 3; row++)
            {
                for (byte col = 0; col < 3; col++)
                {
                    panels[row, col] = new Panel();
                    panels[row, col].Size = new Size(panelSize, panelSize);
                    panels[row, col].BackColor = Color.Transparent;

                    // Colocar el panel en su posición en la cuadrícula
                    panels[row, col].Location = new Point((col * (panelSize + margin)) + 10, (row * (panelSize + margin)) + 10);


                    // Captura las variables row y col
                    byte capturedRow = row;
                    byte capturedCol = col;

                    // Asignar el evento de clic para cada panel
                    panels[row, col].Click += (sender, e) => Panel_Click(sender, e, capturedRow, capturedCol); // prestar atencion 


                    // Añadir el panel al formulario
                    this.Controls.Add(panels[row, col]);
                }

            }
            //colocar proximamente en otro lado, primer turno del bot
            bool firstTurn = game.PlayerInit;
            if (!firstTurn && bot != null)
            {
                botPlaying(firstTurn);
            }

        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // Ocultar la matriz de paneles
            foreach (var panel in panels)
            {
                panel.Visible = false;
            }
            game.gameFullReset();
            // Mostrar el menú principal
            menuPanel.Visible = true;
        }
        private string currentTurn(bool player)
        {
            string turn = "";
            if (player)
            {
                turn = "Resources/circle.png";
            }
            else
            {
                turn = "Resources/cross.png";
            }
            return turn;
        }


        // Manejar el clic en cualquier panel
        private void Panel_Click(object sender, EventArgs e, byte row, byte col)
        {
            string turn;
            bool player;

            Panel clickedPanel = sender as Panel;

            player = game.Player;

            turn = currentTurn(player);
            if (game.newMove(row, col))
            {
            //colocamos x o cruz segun corresponda

                clickedPanel.BackgroundImage = Image.FromFile(turn);
                clickedPanel.BackgroundImageLayout = ImageLayout.Stretch;
                //put.Play();
                if (bot != null)
                {
                    botPlaying(!player);
                }
            }
            // Mostrar un mensaje con el número del cuadrado
            // MessageBox.Show(col.ToString() + " " +row.ToString());
            
        }
    
        private void OnGameEnd()
        {
            //MessageBox.Show($"el jugador {player1Score}{player2Score} gano en form");
            matrizConstructor();
        }

        private void botPlaying(bool player)
        {
            byte[] botMove;
            byte botRow;
            byte botCol;
            string turn;

            bot.GameState = game.GameState;
            turn = currentTurn(player);
            botMove = bot.playing(player);
            if (botMove != null)
            {
                botRow = botMove[0];
                botCol = botMove[1];
                if (game.newMove(botRow, botCol))
                {
                panels[botRow, botCol].BackgroundImage = Image.FromFile(turn);
                panels[botRow, botCol].BackgroundImageLayout = ImageLayout.Stretch;
                //put.Play();
                }
            }
        }
    
    }
}


            // byte action = 0;

            // switch (action)
            // {
            //     case 1:
            //         bot = new BotVeryEasy();
            //         break;

            //     case 2:
            //         bot = new BotEasy();
            //         break;

            //     case 3:
            //         bot = new BotMedium();
            //         break;

            //     case 4:
            //         bot = new BotHard();
            //         break;

            //     case 5:
            //         bot = new BotExpert();
            //         break;

            //     default:
                   
            //         bot = null; 
            //         break;
            // }