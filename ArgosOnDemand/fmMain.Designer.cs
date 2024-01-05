namespace ArgosOnDemand
{
    partial class fmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlInferior = new Panel();
            pnlbottom = new Panel();
            pnlCards = new Panel();
            pnlSistema = new Panel();
            IconSistema = new PictureBox();
            lblDescricaoSistema = new Label();
            lblSistema = new Label();
            btnSistema = new Button();
            pnlcardRede = new Panel();
            IconRede = new PictureBox();
            lblDescricaoRede = new Label();
            lblRede = new Label();
            btnRede = new Button();
            pnlCardGerenciamento = new Panel();
            IconGerenciamento = new PictureBox();
            lblDescricaoGerenciamento = new Label();
            btnAbrirGerenciamento = new Button();
            lblGerenciamento = new Label();
            pnlArgos = new Panel();
            btnGitHub = new Button();
            btnIrTelegram = new Button();
            btnDocumentacao = new Button();
            lblDescricaoArgos = new Label();
            lblArgos = new Label();
            pnlSuperior = new Panel();
            pnlDivisor = new Panel();
            logoMultilog = new PictureBox();
            MainMenu = new MenuStrip();
            telasMenu = new ToolStripMenuItem();
            dashMenu = new ToolStripMenuItem();
            sairMenu = new ToolStripMenuItem();
            pictureBox5 = new PictureBox();
            pnlInferior.SuspendLayout();
            pnlCards.SuspendLayout();
            pnlSistema.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IconSistema).BeginInit();
            pnlcardRede.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IconRede).BeginInit();
            pnlCardGerenciamento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)IconGerenciamento).BeginInit();
            pnlArgos.SuspendLayout();
            pnlSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logoMultilog).BeginInit();
            MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // pnlInferior
            // 
            pnlInferior.BackColor = Color.FromArgb(24, 24, 24);
            pnlInferior.Controls.Add(pnlbottom);
            pnlInferior.Controls.Add(pnlCards);
            pnlInferior.Dock = DockStyle.Bottom;
            pnlInferior.Location = new Point(0, 354);
            pnlInferior.Name = "pnlInferior";
            pnlInferior.Size = new Size(1350, 357);
            pnlInferior.TabIndex = 0;
            // 
            // pnlbottom
            // 
            pnlbottom.BackColor = Color.FromArgb(15, 15, 15);
            pnlbottom.Dock = DockStyle.Bottom;
            pnlbottom.Location = new Point(0, 322);
            pnlbottom.Name = "pnlbottom";
            pnlbottom.Size = new Size(1350, 35);
            pnlbottom.TabIndex = 6;
            // 
            // pnlCards
            // 
            pnlCards.Anchor = AnchorStyles.None;
            pnlCards.BackColor = Color.Transparent;
            pnlCards.Controls.Add(pnlSistema);
            pnlCards.Controls.Add(pnlcardRede);
            pnlCards.Controls.Add(pnlCardGerenciamento);
            pnlCards.Location = new Point(214, 40);
            pnlCards.Name = "pnlCards";
            pnlCards.Size = new Size(920, 239);
            pnlCards.TabIndex = 4;
            // 
            // pnlSistema
            // 
            pnlSistema.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pnlSistema.BackColor = Color.Transparent;
            pnlSistema.Controls.Add(IconSistema);
            pnlSistema.Controls.Add(lblDescricaoSistema);
            pnlSistema.Controls.Add(lblSistema);
            pnlSistema.Controls.Add(btnSistema);
            pnlSistema.Location = new Point(669, 9);
            pnlSistema.Name = "pnlSistema";
            pnlSistema.Size = new Size(240, 219);
            pnlSistema.TabIndex = 5;
            // 
            // IconSistema
            // 
            IconSistema.Image = Properties.Resources.IconSystemCard;
            IconSistema.Location = new Point(8, 23);
            IconSistema.Name = "IconSistema";
            IconSistema.Size = new Size(62, 53);
            IconSistema.SizeMode = PictureBoxSizeMode.Zoom;
            IconSistema.TabIndex = 7;
            IconSistema.TabStop = false;
            // 
            // lblDescricaoSistema
            // 
            lblDescricaoSistema.ForeColor = Color.DarkGray;
            lblDescricaoSistema.Location = new Point(2, 119);
            lblDescricaoSistema.Name = "lblDescricaoSistema";
            lblDescricaoSistema.Padding = new Padding(7, 0, 0, 0);
            lblDescricaoSistema.Size = new Size(234, 30);
            lblDescricaoSistema.TabIndex = 9;
            lblDescricaoSistema.Text = "Configurações detalhadas do Argos.";
            lblDescricaoSistema.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSistema
            // 
            lblSistema.AutoSize = true;
            lblSistema.BackColor = Color.Transparent;
            lblSistema.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblSistema.ForeColor = Color.Gainsboro;
            lblSistema.Location = new Point(8, 99);
            lblSistema.Name = "lblSistema";
            lblSistema.Size = new Size(58, 19);
            lblSistema.TabIndex = 8;
            lblSistema.Text = "Sistema";
            lblSistema.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSistema
            // 
            btnSistema.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnSistema.BackColor = Color.FromArgb(18, 143, 134);
            btnSistema.FlatAppearance.BorderColor = Color.FromArgb(18, 143, 134);
            btnSistema.FlatAppearance.BorderSize = 0;
            btnSistema.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSistema.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnSistema.FlatStyle = FlatStyle.Flat;
            btnSistema.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnSistema.ForeColor = Color.White;
            btnSistema.Location = new Point(70, 173);
            btnSistema.Name = "btnSistema";
            btnSistema.Size = new Size(101, 34);
            btnSistema.TabIndex = 5;
            btnSistema.Text = "Abrir";
            btnSistema.UseVisualStyleBackColor = false;
            // 
            // pnlcardRede
            // 
            pnlcardRede.Anchor = AnchorStyles.Top;
            pnlcardRede.BackColor = Color.Transparent;
            pnlcardRede.Controls.Add(IconRede);
            pnlcardRede.Controls.Add(lblDescricaoRede);
            pnlcardRede.Controls.Add(lblRede);
            pnlcardRede.Controls.Add(btnRede);
            pnlcardRede.Location = new Point(340, 9);
            pnlcardRede.Name = "pnlcardRede";
            pnlcardRede.Size = new Size(240, 219);
            pnlcardRede.TabIndex = 3;
            // 
            // IconRede
            // 
            IconRede.Image = Properties.Resources.IconRede;
            IconRede.Location = new Point(8, 23);
            IconRede.Name = "IconRede";
            IconRede.Size = new Size(62, 53);
            IconRede.SizeMode = PictureBoxSizeMode.Zoom;
            IconRede.TabIndex = 4;
            IconRede.TabStop = false;
            // 
            // lblDescricaoRede
            // 
            lblDescricaoRede.ForeColor = Color.DarkGray;
            lblDescricaoRede.Location = new Point(2, 119);
            lblDescricaoRede.Name = "lblDescricaoRede";
            lblDescricaoRede.Padding = new Padding(7, 0, 0, 0);
            lblDescricaoRede.Size = new Size(234, 30);
            lblDescricaoRede.TabIndex = 6;
            lblDescricaoRede.Text = "Monitoramento e status de rede.";
            lblDescricaoRede.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRede
            // 
            lblRede.AutoSize = true;
            lblRede.BackColor = Color.Transparent;
            lblRede.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblRede.ForeColor = Color.Gainsboro;
            lblRede.Location = new Point(8, 99);
            lblRede.Name = "lblRede";
            lblRede.Size = new Size(40, 19);
            lblRede.TabIndex = 5;
            lblRede.Text = "Rede";
            lblRede.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRede
            // 
            btnRede.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnRede.BackColor = Color.FromArgb(18, 143, 134);
            btnRede.FlatAppearance.BorderColor = Color.FromArgb(18, 143, 134);
            btnRede.FlatAppearance.BorderSize = 0;
            btnRede.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnRede.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnRede.FlatStyle = FlatStyle.Flat;
            btnRede.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnRede.ForeColor = Color.White;
            btnRede.Location = new Point(67, 173);
            btnRede.Name = "btnRede";
            btnRede.Size = new Size(101, 34);
            btnRede.TabIndex = 4;
            btnRede.Text = "Abrir";
            btnRede.UseVisualStyleBackColor = false;
            // 
            // pnlCardGerenciamento
            // 
            pnlCardGerenciamento.BackColor = Color.Transparent;
            pnlCardGerenciamento.Controls.Add(IconGerenciamento);
            pnlCardGerenciamento.Controls.Add(lblDescricaoGerenciamento);
            pnlCardGerenciamento.Controls.Add(btnAbrirGerenciamento);
            pnlCardGerenciamento.Controls.Add(lblGerenciamento);
            pnlCardGerenciamento.Location = new Point(11, 9);
            pnlCardGerenciamento.Name = "pnlCardGerenciamento";
            pnlCardGerenciamento.Size = new Size(240, 219);
            pnlCardGerenciamento.TabIndex = 0;
            // 
            // IconGerenciamento
            // 
            IconGerenciamento.Image = Properties.Resources.IconGerenciamento;
            IconGerenciamento.Location = new Point(8, 23);
            IconGerenciamento.Name = "IconGerenciamento";
            IconGerenciamento.Size = new Size(63, 53);
            IconGerenciamento.SizeMode = PictureBoxSizeMode.Zoom;
            IconGerenciamento.TabIndex = 7;
            IconGerenciamento.TabStop = false;
            // 
            // lblDescricaoGerenciamento
            // 
            lblDescricaoGerenciamento.ForeColor = Color.DarkGray;
            lblDescricaoGerenciamento.Location = new Point(3, 119);
            lblDescricaoGerenciamento.Name = "lblDescricaoGerenciamento";
            lblDescricaoGerenciamento.Padding = new Padding(7, 0, 0, 0);
            lblDescricaoGerenciamento.Size = new Size(234, 46);
            lblDescricaoGerenciamento.TabIndex = 3;
            lblDescricaoGerenciamento.Text = "Faça o controle das funções, manutenção no Argos.";
            lblDescricaoGerenciamento.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnAbrirGerenciamento
            // 
            btnAbrirGerenciamento.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnAbrirGerenciamento.BackColor = Color.FromArgb(18, 143, 134);
            btnAbrirGerenciamento.FlatAppearance.BorderColor = Color.FromArgb(18, 143, 134);
            btnAbrirGerenciamento.FlatAppearance.BorderSize = 0;
            btnAbrirGerenciamento.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnAbrirGerenciamento.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnAbrirGerenciamento.FlatStyle = FlatStyle.Flat;
            btnAbrirGerenciamento.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnAbrirGerenciamento.ForeColor = Color.White;
            btnAbrirGerenciamento.Location = new Point(66, 173);
            btnAbrirGerenciamento.Name = "btnAbrirGerenciamento";
            btnAbrirGerenciamento.Size = new Size(101, 34);
            btnAbrirGerenciamento.TabIndex = 2;
            btnAbrirGerenciamento.Text = "Abrir";
            btnAbrirGerenciamento.UseVisualStyleBackColor = false;
            // 
            // lblGerenciamento
            // 
            lblGerenciamento.AutoSize = true;
            lblGerenciamento.BackColor = Color.Transparent;
            lblGerenciamento.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblGerenciamento.ForeColor = Color.Gainsboro;
            lblGerenciamento.Location = new Point(8, 99);
            lblGerenciamento.Name = "lblGerenciamento";
            lblGerenciamento.Size = new Size(104, 19);
            lblGerenciamento.TabIndex = 0;
            lblGerenciamento.Text = "Gerenciamento";
            lblGerenciamento.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlArgos
            // 
            pnlArgos.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pnlArgos.BackColor = Color.Transparent;
            pnlArgos.Controls.Add(btnGitHub);
            pnlArgos.Controls.Add(btnIrTelegram);
            pnlArgos.Controls.Add(btnDocumentacao);
            pnlArgos.Controls.Add(lblDescricaoArgos);
            pnlArgos.Controls.Add(lblArgos);
            pnlArgos.Location = new Point(240, 77);
            pnlArgos.Name = "pnlArgos";
            pnlArgos.Size = new Size(552, 205);
            pnlArgos.TabIndex = 4;
            // 
            // btnGitHub
            // 
            btnGitHub.BackColor = Color.Transparent;
            btnGitHub.BackgroundImage = Properties.Resources.IconGitHub;
            btnGitHub.BackgroundImageLayout = ImageLayout.Zoom;
            btnGitHub.FlatAppearance.BorderSize = 0;
            btnGitHub.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnGitHub.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnGitHub.FlatStyle = FlatStyle.Flat;
            btnGitHub.Location = new Point(274, 151);
            btnGitHub.Margin = new Padding(3, 2, 3, 2);
            btnGitHub.Name = "btnGitHub";
            btnGitHub.Size = new Size(38, 33);
            btnGitHub.TabIndex = 4;
            btnGitHub.UseVisualStyleBackColor = false;
            btnGitHub.Click += btnGitHub_Click;
            // 
            // btnIrTelegram
            // 
            btnIrTelegram.BackColor = Color.FromArgb(18, 143, 134);
            btnIrTelegram.FlatAppearance.BorderColor = Color.FromArgb(18, 143, 134);
            btnIrTelegram.FlatAppearance.BorderSize = 0;
            btnIrTelegram.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnIrTelegram.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnIrTelegram.FlatStyle = FlatStyle.Flat;
            btnIrTelegram.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnIrTelegram.ForeColor = Color.White;
            btnIrTelegram.Location = new Point(141, 151);
            btnIrTelegram.Name = "btnIrTelegram";
            btnIrTelegram.Size = new Size(124, 33);
            btnIrTelegram.TabIndex = 3;
            btnIrTelegram.Text = "Ir ao Telegram";
            btnIrTelegram.UseVisualStyleBackColor = false;
            btnIrTelegram.Click += btnIrTelegram_Click;
            // 
            // btnDocumentacao
            // 
            btnDocumentacao.BackColor = Color.Transparent;
            btnDocumentacao.FlatAppearance.BorderColor = Color.White;
            btnDocumentacao.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDocumentacao.FlatAppearance.MouseOverBackColor = Color.DimGray;
            btnDocumentacao.FlatStyle = FlatStyle.Flat;
            btnDocumentacao.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnDocumentacao.ForeColor = Color.White;
            btnDocumentacao.Location = new Point(12, 151);
            btnDocumentacao.Name = "btnDocumentacao";
            btnDocumentacao.Size = new Size(114, 33);
            btnDocumentacao.TabIndex = 2;
            btnDocumentacao.Text = "Documentação";
            btnDocumentacao.UseVisualStyleBackColor = false;
            // 
            // lblDescricaoArgos
            // 
            lblDescricaoArgos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblDescricaoArgos.AutoSize = true;
            lblDescricaoArgos.BackColor = Color.Transparent;
            lblDescricaoArgos.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblDescricaoArgos.ForeColor = Color.DarkGray;
            lblDescricaoArgos.Location = new Point(8, 89);
            lblDescricaoArgos.Name = "lblDescricaoArgos";
            lblDescricaoArgos.Size = new Size(413, 19);
            lblDescricaoArgos.TabIndex = 1;
            lblDescricaoArgos.Text = "Sistema especialista em Torre de Controle e operações Multilog. ";
            // 
            // lblArgos
            // 
            lblArgos.AutoSize = true;
            lblArgos.BackColor = Color.Transparent;
            lblArgos.Dock = DockStyle.Top;
            lblArgos.Font = new Font("Segoe UI", 40.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblArgos.ForeColor = Color.White;
            lblArgos.Location = new Point(0, 0);
            lblArgos.Margin = new Padding(0);
            lblArgos.Name = "lblArgos";
            lblArgos.Size = new Size(496, 72);
            lblArgos.TabIndex = 0;
            lblArgos.Text = "Argos On Demand";
            // 
            // pnlSuperior
            // 
            pnlSuperior.BackColor = Color.Transparent;
            pnlSuperior.Controls.Add(pnlDivisor);
            pnlSuperior.Controls.Add(logoMultilog);
            pnlSuperior.Controls.Add(MainMenu);
            pnlSuperior.Dock = DockStyle.Top;
            pnlSuperior.Location = new Point(0, 0);
            pnlSuperior.Margin = new Padding(0);
            pnlSuperior.Name = "pnlSuperior";
            pnlSuperior.Padding = new Padding(16, 5, 5, 5);
            pnlSuperior.Size = new Size(1350, 63);
            pnlSuperior.TabIndex = 2;
            // 
            // pnlDivisor
            // 
            pnlDivisor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDivisor.BackColor = Color.FromArgb(50, 50, 50);
            pnlDivisor.Location = new Point(0, 35);
            pnlDivisor.Name = "pnlDivisor";
            pnlDivisor.Size = new Size(1351, 1);
            pnlDivisor.TabIndex = 5;
            // 
            // logoMultilog
            // 
            logoMultilog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            logoMultilog.Image = Properties.Resources.LogoMultilogBranco;
            logoMultilog.Location = new Point(1249, 5);
            logoMultilog.Name = "logoMultilog";
            logoMultilog.Size = new Size(81, 25);
            logoMultilog.SizeMode = PictureBoxSizeMode.Zoom;
            logoMultilog.TabIndex = 4;
            logoMultilog.TabStop = false;
            // 
            // MainMenu
            // 
            MainMenu.BackColor = Color.Transparent;
            MainMenu.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            MainMenu.ImageScalingSize = new Size(20, 20);
            MainMenu.Items.AddRange(new ToolStripItem[] { telasMenu, sairMenu });
            MainMenu.Location = new Point(16, 5);
            MainMenu.Name = "MainMenu";
            MainMenu.Padding = new Padding(23, 1, 0, 0);
            MainMenu.RenderMode = ToolStripRenderMode.Professional;
            MainMenu.ShowItemToolTips = true;
            MainMenu.Size = new Size(1329, 25);
            MainMenu.TabIndex = 2;
            MainMenu.Text = "menuMain";
            // 
            // telasMenu
            // 
            telasMenu.BackColor = Color.Transparent;
            telasMenu.DropDownItems.AddRange(new ToolStripItem[] { dashMenu });
            telasMenu.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            telasMenu.ForeColor = Color.Gainsboro;
            telasMenu.Name = "telasMenu";
            telasMenu.Padding = new Padding(16, 2, 5, 3);
            telasMenu.Size = new Size(58, 24);
            telasMenu.Text = "Telas";
            // 
            // dashMenu
            // 
            dashMenu.BackColor = Color.Transparent;
            dashMenu.Name = "dashMenu";
            dashMenu.Size = new Size(132, 22);
            dashMenu.Text = "Dashboard";
            dashMenu.Click += dashMenu_Click;
            // 
            // sairMenu
            // 
            sairMenu.BackColor = Color.Transparent;
            sairMenu.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            sairMenu.ForeColor = Color.Gainsboro;
            sairMenu.Name = "sairMenu";
            sairMenu.Padding = new Padding(16, 2, 5, 3);
            sairMenu.Size = new Size(121, 24);
            sairMenu.Text = "Sair da aplicação";
            sairMenu.Click += sairMenu_Click;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.Transparent;
            pictureBox5.Image = Properties.Resources.bolinhaazul_unscreen;
            pictureBox5.Location = new Point(12, 3);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(30, 30);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 5;
            pictureBox5.TabStop = false;
            // 
            // fmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(34, 34, 34);
            BackgroundImage = Properties.Resources.BackgroundGrey;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1350, 711);
            Controls.Add(pictureBox5);
            Controls.Add(pnlArgos);
            Controls.Add(pnlSuperior);
            Controls.Add(pnlInferior);
            DoubleBuffered = true;
            ForeColor = Color.Gainsboro;
            MainMenuStrip = MainMenu;
            MinimumSize = new Size(1366, 748);
            Name = "fmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Argos";
            WindowState = FormWindowState.Maximized;
            pnlInferior.ResumeLayout(false);
            pnlCards.ResumeLayout(false);
            pnlSistema.ResumeLayout(false);
            pnlSistema.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)IconSistema).EndInit();
            pnlcardRede.ResumeLayout(false);
            pnlcardRede.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)IconRede).EndInit();
            pnlCardGerenciamento.ResumeLayout(false);
            pnlCardGerenciamento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)IconGerenciamento).EndInit();
            pnlArgos.ResumeLayout(false);
            pnlArgos.PerformLayout();
            pnlSuperior.ResumeLayout(false);
            pnlSuperior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)logoMultilog).EndInit();
            MainMenu.ResumeLayout(false);
            MainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlInferior;
        private Panel pnlArgos;
        private Button btnIrTelegram;
        private Button btnDocumentacao;
        private Label lblDescricaoArgos;
        private Label lblArgos;
        private Panel pnlCards;
        private Panel pnlcardRede;
        private Button btnRede;
        private Panel pnlCardGerenciamento;
        private Button btnAbrirGerenciamento;
        private Label lblGerenciamento;
        private Panel pnlSuperior;
        private ToolStripMenuItem telasToolStripMenuItem;
        private Panel pnlSistema;
        private Button btnSistema;
        private ToolStripMenuItem dashboardToolStripMenuItem;
        private MenuStrip MainMenu;
        private ToolStripMenuItem telasMenu;
        private ToolStripMenuItem sairMenu;
        private ToolStripMenuItem dashMenu;
        private PictureBox logoMultilog;
        private Panel pnlDivisor;
        private Panel pnlbottom;
        private Label lblDescricaoRede;
        private Label lblRede;
        private Label lblDescricaoGerenciamento;
        private PictureBox IconRede;
        private PictureBox IconGerenciamento;
        private Button btnGitHub;
        private PictureBox IconSistema;
        private Label lblDescricaoSistema;
        private Label lblSistema;
        private PictureBox pictureBox5;
    }
}