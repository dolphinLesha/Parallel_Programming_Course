namespace lab3
{
    partial class main
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cancel_par = new System.Windows.Forms.Button();
            this.cancel_p = new System.Windows.Forms.Button();
            this.am_data_t = new System.Windows.Forms.Label();
            this.am_data_sl = new System.Windows.Forms.TrackBar();
            this.infol2 = new System.Windows.Forms.Label();
            this.am_data_input = new System.Windows.Forms.TextBox();
            this.infol1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.am_cl_input = new System.Windows.Forms.TextBox();
            this.par_b = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.posl_b = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.am_data_sl)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.75869F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.51724F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.48276F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1007, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1001, 403);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(494, 397);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(503, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(495, 397);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.main_ResizeEnd);
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cancel_par);
            this.groupBox1.Controls.Add(this.cancel_p);
            this.groupBox1.Controls.Add(this.am_data_t);
            this.groupBox1.Controls.Add(this.am_data_sl);
            this.groupBox1.Controls.Add(this.infol2);
            this.groupBox1.Controls.Add(this.am_data_input);
            this.groupBox1.Controls.Add(this.infol1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.am_cl_input);
            this.groupBox1.Controls.Add(this.par_b);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.posl_b);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 412);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1001, 165);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "настройки";
            // 
            // cancel_par
            // 
            this.cancel_par.FlatAppearance.BorderSize = 0;
            this.cancel_par.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.cancel_par.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cancel_par.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancel_par.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cancel_par.Location = new System.Drawing.Point(206, 70);
            this.cancel_par.Name = "cancel_par";
            this.cancel_par.Size = new System.Drawing.Size(136, 33);
            this.cancel_par.TabIndex = 11;
            this.cancel_par.Text = "отменить";
            this.cancel_par.UseVisualStyleBackColor = true;
            this.cancel_par.Visible = false;
            this.cancel_par.Click += new System.EventHandler(this.cancel_par_Click);
            // 
            // cancel_p
            // 
            this.cancel_p.FlatAppearance.BorderSize = 0;
            this.cancel_p.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.cancel_p.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cancel_p.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancel_p.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cancel_p.Location = new System.Drawing.Point(12, 70);
            this.cancel_p.Name = "cancel_p";
            this.cancel_p.Size = new System.Drawing.Size(190, 33);
            this.cancel_p.TabIndex = 10;
            this.cancel_p.Text = "отменить";
            this.cancel_p.UseVisualStyleBackColor = true;
            this.cancel_p.Visible = false;
            this.cancel_p.Click += new System.EventHandler(this.cancel_p_Click);
            // 
            // am_data_t
            // 
            this.am_data_t.AutoSize = true;
            this.am_data_t.Location = new System.Drawing.Point(673, 70);
            this.am_data_t.Name = "am_data_t";
            this.am_data_t.Size = new System.Drawing.Size(39, 15);
            this.am_data_t.TabIndex = 9;
            this.am_data_t.Text = "100%";
            // 
            // am_data_sl
            // 
            this.am_data_sl.Location = new System.Drawing.Point(549, 70);
            this.am_data_sl.Maximum = 100;
            this.am_data_sl.Minimum = 10;
            this.am_data_sl.Name = "am_data_sl";
            this.am_data_sl.Size = new System.Drawing.Size(118, 50);
            this.am_data_sl.TabIndex = 8;
            this.am_data_sl.Value = 100;
            this.am_data_sl.Scroll += new System.EventHandler(this.am_data_sl_Scroll);
            // 
            // infol2
            // 
            this.infol2.AutoSize = true;
            this.infol2.Location = new System.Drawing.Point(348, 70);
            this.infol2.Name = "infol2";
            this.infol2.Size = new System.Drawing.Size(139, 15);
            this.infol2.TabIndex = 7;
            this.infol2.Text = "данных для обработки";
            // 
            // am_data_input
            // 
            this.am_data_input.Location = new System.Drawing.Point(492, 70);
            this.am_data_input.MaxLength = 2;
            this.am_data_input.Name = "am_data_input";
            this.am_data_input.Size = new System.Drawing.Size(50, 20);
            this.am_data_input.TabIndex = 6;
            this.am_data_input.TextChanged += new System.EventHandler(this.am_data_input_TextChanged);
            // 
            // infol1
            // 
            this.infol1.AutoSize = true;
            this.infol1.Location = new System.Drawing.Point(348, 39);
            this.infol1.Name = "infol1";
            this.infol1.Size = new System.Drawing.Size(138, 15);
            this.infol1.TabIndex = 5;
            this.infol1.Text = "количество кластеров";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "время";
            // 
            // am_cl_input
            // 
            this.am_cl_input.Location = new System.Drawing.Point(492, 39);
            this.am_cl_input.MaxLength = 2;
            this.am_cl_input.Name = "am_cl_input";
            this.am_cl_input.Size = new System.Drawing.Size(50, 20);
            this.am_cl_input.TabIndex = 3;
            this.am_cl_input.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // par_b
            // 
            this.par_b.FlatAppearance.BorderSize = 0;
            this.par_b.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.par_b.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.par_b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.par_b.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.par_b.Location = new System.Drawing.Point(199, 31);
            this.par_b.Name = "par_b";
            this.par_b.Size = new System.Drawing.Size(143, 33);
            this.par_b.TabIndex = 2;
            this.par_b.Text = "параллельно";
            this.par_b.UseVisualStyleBackColor = true;
            this.par_b.Click += new System.EventHandler(this.par_b_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(9, 139);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(983, 15);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 1;
            // 
            // posl_b
            // 
            this.posl_b.FlatAppearance.BorderSize = 0;
            this.posl_b.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.posl_b.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.posl_b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.posl_b.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.posl_b.Location = new System.Drawing.Point(12, 31);
            this.posl_b.Name = "posl_b";
            this.posl_b.Size = new System.Drawing.Size(190, 33);
            this.posl_b.TabIndex = 0;
            this.posl_b.Text = "последовательное";
            this.posl_b.UseVisualStyleBackColor = true;
            this.posl_b.Click += new System.EventHandler(this.button1_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1007, 580);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "main";
            this.Text = "main";
            this.ResizeEnd += new System.EventHandler(this.main_ResizeEnd);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.am_data_sl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button posl_b;
        private System.Windows.Forms.Button par_b;
        private System.Windows.Forms.TextBox am_cl_input;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label infol1;
        private System.Windows.Forms.Label am_data_t;
        private System.Windows.Forms.TrackBar am_data_sl;
        private System.Windows.Forms.Label infol2;
        private System.Windows.Forms.TextBox am_data_input;
        private System.Windows.Forms.Button cancel_par;
        private System.Windows.Forms.Button cancel_p;
    }
}