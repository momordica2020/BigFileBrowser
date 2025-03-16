namespace BigFileBrowser
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            textBox2 = new MaterialSkin.Controls.MaterialTextBox();
            button3 = new MaterialSkin.Controls.MaterialButton();
            label4 = new MaterialSkin.Controls.MaterialLabel();
            listView1 = new MaterialSkin.Controls.MaterialListView();
            text = new System.Windows.Forms.ColumnHeader();
            num = new System.Windows.Forms.ColumnHeader();
            textBox3 = new MaterialSkin.Controls.MaterialTextBox();
            tabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            ButtonSearchInPage = new MaterialSkin.Controls.MaterialButton();
            NumericPage = new System.Windows.Forms.NumericUpDown();
            TextBoxSearchInPage = new MaterialSkin.Controls.MaterialTextBox();
            ComboBoxEncodings = new MaterialSkin.Controls.MaterialComboBox();
            SwitchAutoWordwarp = new MaterialSkin.Controls.MaterialSwitch();
            LabelFilePath = new MaterialSkin.Controls.MaterialLabel();
            SliderPage = new MaterialSkin.Controls.MaterialSlider();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            ComboBoxPageSize = new MaterialSkin.Controls.MaterialComboBox();
            TextBoxPageContent = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            tabPage2 = new System.Windows.Forms.TabPage();
            tabPage3 = new System.Windows.Forms.TabPage();
            label5 = new MaterialSkin.Controls.MaterialLabel();
            label2 = new MaterialSkin.Controls.MaterialLabel();
            textBox4 = new MaterialSkin.Controls.MaterialTextBox();
            textBox5 = new MaterialSkin.Controls.MaterialTextBox();
            button6 = new MaterialSkin.Controls.MaterialButton();
            listView2 = new MaterialSkin.Controls.MaterialListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            测试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericPage).BeginInit();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.FileOk += openFileDialog1_FileOk;
            // 
            // textBox2
            // 
            textBox2.AnimateReadOnly = false;
            textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox2.Depth = 0;
            textBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox2.LeadingIcon = null;
            textBox2.Location = new System.Drawing.Point(41, 62);
            textBox2.Margin = new System.Windows.Forms.Padding(4);
            textBox2.MaxLength = 50;
            textBox2.MouseState = MaterialSkin.MouseState.OUT;
            textBox2.Multiline = false;
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(220, 50);
            textBox2.TabIndex = 9;
            textBox2.Text = "";
            textBox2.TrailingIcon = null;
            // 
            // button3
            // 
            button3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            button3.Depth = 0;
            button3.HighEmphasis = true;
            button3.Icon = null;
            button3.Location = new System.Drawing.Point(269, 71);
            button3.Margin = new System.Windows.Forms.Padding(4);
            button3.MouseState = MaterialSkin.MouseState.HOVER;
            button3.Name = "button3";
            button3.NoAccentTextColor = System.Drawing.Color.Empty;
            button3.Size = new System.Drawing.Size(101, 36);
            button3.TabIndex = 10;
            button3.Text = "搜当前文件";
            button3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            button3.UseAccentColor = false;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Depth = 0;
            label4.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label4.Location = new System.Drawing.Point(392, 81);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.MouseState = MaterialSkin.MouseState.HOVER;
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(81, 19);
            label4.TabIndex = 11;
            label4.Text = "全文搜索：";
            // 
            // listView1
            // 
            listView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listView1.AutoSizeTable = false;
            listView1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { text, num });
            listView1.Depth = 0;
            listView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            listView1.FullRowSelect = true;
            listView1.Location = new System.Drawing.Point(21, 117);
            listView1.Margin = new System.Windows.Forms.Padding(4);
            listView1.MinimumSize = new System.Drawing.Size(200, 100);
            listView1.MouseLocation = new System.Drawing.Point(-1, -1);
            listView1.MouseState = MaterialSkin.MouseState.OUT;
            listView1.Name = "listView1";
            listView1.OwnerDraw = true;
            listView1.Size = new System.Drawing.Size(1073, 498);
            listView1.TabIndex = 12;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // text
            // 
            text.Text = "文本";
            text.Width = 300;
            // 
            // num
            // 
            num.Text = "页数";
            // 
            // textBox3
            // 
            textBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox3.AnimateReadOnly = false;
            textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox3.Depth = 0;
            textBox3.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox3.LeadingIcon = null;
            textBox3.Location = new System.Drawing.Point(41, 13);
            textBox3.Margin = new System.Windows.Forms.Padding(4);
            textBox3.MaxLength = 50;
            textBox3.MouseState = MaterialSkin.MouseState.OUT;
            textBox3.Multiline = false;
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(862, 50);
            textBox3.TabIndex = 15;
            textBox3.Text = "";
            textBox3.TrailingIcon = null;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Depth = 0;
            tabControl1.HotTrack = true;
            tabControl1.ItemSize = new System.Drawing.Size(284, 29);
            tabControl1.Location = new System.Drawing.Point(3, 124);
            tabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new System.Drawing.Point(56, 5);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1121, 682);
            tabControl1.TabIndex = 16;
            tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(ButtonSearchInPage);
            tabPage1.Controls.Add(NumericPage);
            tabPage1.Controls.Add(TextBoxSearchInPage);
            tabPage1.Controls.Add(ComboBoxEncodings);
            tabPage1.Controls.Add(SwitchAutoWordwarp);
            tabPage1.Controls.Add(LabelFilePath);
            tabPage1.Controls.Add(SliderPage);
            tabPage1.Controls.Add(materialLabel1);
            tabPage1.Controls.Add(ComboBoxPageSize);
            tabPage1.Controls.Add(TextBoxPageContent);
            tabPage1.Location = new System.Drawing.Point(4, 33);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(1113, 645);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "文件内容";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // ButtonSearchInPage
            // 
            ButtonSearchInPage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ButtonSearchInPage.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            ButtonSearchInPage.Depth = 0;
            ButtonSearchInPage.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            ButtonSearchInPage.HighEmphasis = true;
            ButtonSearchInPage.Icon = null;
            ButtonSearchInPage.Location = new System.Drawing.Point(933, 52);
            ButtonSearchInPage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            ButtonSearchInPage.MouseState = MaterialSkin.MouseState.HOVER;
            ButtonSearchInPage.Name = "ButtonSearchInPage";
            ButtonSearchInPage.NoAccentTextColor = System.Drawing.Color.Empty;
            ButtonSearchInPage.Size = new System.Drawing.Size(64, 36);
            ButtonSearchInPage.TabIndex = 26;
            ButtonSearchInPage.Text = "🔍";
            ButtonSearchInPage.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            ButtonSearchInPage.UseAccentColor = false;
            ButtonSearchInPage.UseVisualStyleBackColor = true;
            ButtonSearchInPage.Click += ButtonSearchInPage_Click;
            // 
            // NumericPage
            // 
            NumericPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            NumericPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            NumericPage.Location = new System.Drawing.Point(1023, 90);
            NumericPage.Name = "NumericPage";
            NumericPage.Size = new System.Drawing.Size(81, 27);
            NumericPage.TabIndex = 25;
            NumericPage.ValueChanged += NumericPage_ValueChanged;
            // 
            // TextBoxSearchInPage
            // 
            TextBoxSearchInPage.AnimateReadOnly = false;
            TextBoxSearchInPage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TextBoxSearchInPage.Depth = 0;
            TextBoxSearchInPage.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            TextBoxSearchInPage.LeadingIcon = null;
            TextBoxSearchInPage.Location = new System.Drawing.Point(569, 44);
            TextBoxSearchInPage.MaxLength = 50;
            TextBoxSearchInPage.MouseState = MaterialSkin.MouseState.OUT;
            TextBoxSearchInPage.Multiline = false;
            TextBoxSearchInPage.Name = "TextBoxSearchInPage";
            TextBoxSearchInPage.Size = new System.Drawing.Size(357, 50);
            TextBoxSearchInPage.TabIndex = 24;
            TextBoxSearchInPage.Text = "";
            TextBoxSearchInPage.TrailingIcon = null;
            TextBoxSearchInPage.KeyDown += TextBoxSearchInPage_KeyDown;
            // 
            // ComboBoxEncodings
            // 
            ComboBoxEncodings.AutoResize = false;
            ComboBoxEncodings.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            ComboBoxEncodings.Depth = 0;
            ComboBoxEncodings.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            ComboBoxEncodings.DropDownHeight = 174;
            ComboBoxEncodings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxEncodings.DropDownWidth = 121;
            ComboBoxEncodings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            ComboBoxEncodings.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            ComboBoxEncodings.FormattingEnabled = true;
            ComboBoxEncodings.IntegralHeight = false;
            ComboBoxEncodings.ItemHeight = 43;
            ComboBoxEncodings.Location = new System.Drawing.Point(17, 42);
            ComboBoxEncodings.MaxDropDownItems = 4;
            ComboBoxEncodings.MouseState = MaterialSkin.MouseState.OUT;
            ComboBoxEncodings.Name = "ComboBoxEncodings";
            ComboBoxEncodings.Size = new System.Drawing.Size(121, 49);
            ComboBoxEncodings.StartIndex = 0;
            ComboBoxEncodings.TabIndex = 23;
            ComboBoxEncodings.SelectedIndexChanged += ComboBoxEncodings_SelectedIndexChanged;
            // 
            // SwitchAutoWordwarp
            // 
            SwitchAutoWordwarp.AutoSize = true;
            SwitchAutoWordwarp.Checked = true;
            SwitchAutoWordwarp.CheckState = System.Windows.Forms.CheckState.Checked;
            SwitchAutoWordwarp.Depth = 0;
            SwitchAutoWordwarp.Location = new System.Drawing.Point(435, 52);
            SwitchAutoWordwarp.Margin = new System.Windows.Forms.Padding(0);
            SwitchAutoWordwarp.MouseLocation = new System.Drawing.Point(-1, -1);
            SwitchAutoWordwarp.MouseState = MaterialSkin.MouseState.HOVER;
            SwitchAutoWordwarp.Name = "SwitchAutoWordwarp";
            SwitchAutoWordwarp.Ripple = true;
            SwitchAutoWordwarp.Size = new System.Drawing.Size(122, 37);
            SwitchAutoWordwarp.TabIndex = 22;
            SwitchAutoWordwarp.Text = "自动换行";
            SwitchAutoWordwarp.UseVisualStyleBackColor = true;
            SwitchAutoWordwarp.CheckedChanged += SwitchAutoWordwarp_CheckedChanged;
            // 
            // LabelFilePath
            // 
            LabelFilePath.AutoSize = true;
            LabelFilePath.Depth = 0;
            LabelFilePath.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            LabelFilePath.Location = new System.Drawing.Point(17, 15);
            LabelFilePath.MouseState = MaterialSkin.MouseState.HOVER;
            LabelFilePath.Name = "LabelFilePath";
            LabelFilePath.Size = new System.Drawing.Size(81, 19);
            LabelFilePath.TabIndex = 21;
            LabelFilePath.Text = "文件路径：";
            // 
            // SliderPage
            // 
            SliderPage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            SliderPage.Depth = 0;
            SliderPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            SliderPage.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            SliderPage.Location = new System.Drawing.Point(17, 123);
            SliderPage.MouseState = MaterialSkin.MouseState.HOVER;
            SliderPage.Name = "SliderPage";
            SliderPage.Size = new System.Drawing.Size(1087, 40);
            SliderPage.TabIndex = 20;
            SliderPage.Text = "";
            SliderPage.UseAccentColor = true;
            SliderPage.Value = 0;
            SliderPage.ValueSuffix = "页";
            SliderPage.onValueChanged += FIleSlider_onValueChanged;
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialLabel1.Location = new System.Drawing.Point(170, 62);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new System.Drawing.Size(113, 19);
            materialLabel1.TabIndex = 19;
            materialLabel1.Text = "每次显示字符：";
            // 
            // ComboBoxPageSize
            // 
            ComboBoxPageSize.AutoResize = false;
            ComboBoxPageSize.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            ComboBoxPageSize.Depth = 0;
            ComboBoxPageSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            ComboBoxPageSize.DropDownHeight = 174;
            ComboBoxPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ComboBoxPageSize.DropDownWidth = 121;
            ComboBoxPageSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ComboBoxPageSize.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            ComboBoxPageSize.FormattingEnabled = true;
            ComboBoxPageSize.IntegralHeight = false;
            ComboBoxPageSize.ItemHeight = 43;
            ComboBoxPageSize.Items.AddRange(new object[] { "500", "1000", "5000", "10000", "50000", "100000" });
            ComboBoxPageSize.Location = new System.Drawing.Point(298, 43);
            ComboBoxPageSize.MaxDropDownItems = 4;
            ComboBoxPageSize.MouseState = MaterialSkin.MouseState.OUT;
            ComboBoxPageSize.Name = "ComboBoxPageSize";
            ComboBoxPageSize.Size = new System.Drawing.Size(121, 49);
            ComboBoxPageSize.StartIndex = 0;
            ComboBoxPageSize.TabIndex = 18;
            ComboBoxPageSize.SelectedIndexChanged += materialComboBox1_SelectedIndexChanged;
            // 
            // TextBoxPageContent
            // 
            TextBoxPageContent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TextBoxPageContent.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            TextBoxPageContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            TextBoxPageContent.Depth = 0;
            TextBoxPageContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TextBoxPageContent.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            TextBoxPageContent.Location = new System.Drawing.Point(17, 169);
            TextBoxPageContent.MouseState = MaterialSkin.MouseState.HOVER;
            TextBoxPageContent.Name = "TextBoxPageContent";
            TextBoxPageContent.ReadOnly = true;
            TextBoxPageContent.Size = new System.Drawing.Size(1087, 455);
            TextBoxPageContent.TabIndex = 17;
            TextBoxPageContent.Text = "";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBox3);
            tabPage2.Controls.Add(textBox2);
            tabPage2.Controls.Add(button3);
            tabPage2.Controls.Add(listView1);
            tabPage2.Controls.Add(label4);
            tabPage2.Location = new System.Drawing.Point(4, 33);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(1113, 645);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "文件内检索";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label5);
            tabPage3.Controls.Add(label2);
            tabPage3.Controls.Add(textBox4);
            tabPage3.Controls.Add(textBox5);
            tabPage3.Controls.Add(button6);
            tabPage3.Controls.Add(listView2);
            tabPage3.Location = new System.Drawing.Point(4, 33);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(1113, 645);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "文件夹搜索";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Depth = 0;
            label5.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label5.Location = new System.Drawing.Point(32, 80);
            label5.MouseState = MaterialSkin.MouseState.HOVER;
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(49, 19);
            label5.TabIndex = 21;
            label5.Text = "关键字";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Depth = 0;
            label2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            label2.Location = new System.Drawing.Point(32, 16);
            label2.MouseState = MaterialSkin.MouseState.HOVER;
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(33, 19);
            label2.TabIndex = 20;
            label2.Text = "路径";
            // 
            // textBox4
            // 
            textBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox4.AnimateReadOnly = false;
            textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox4.Depth = 0;
            textBox4.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox4.LeadingIcon = null;
            textBox4.Location = new System.Drawing.Point(121, 17);
            textBox4.Margin = new System.Windows.Forms.Padding(4);
            textBox4.MaxLength = 50;
            textBox4.MouseState = MaterialSkin.MouseState.OUT;
            textBox4.Multiline = false;
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(862, 50);
            textBox4.TabIndex = 19;
            textBox4.Text = "";
            textBox4.TrailingIcon = null;
            // 
            // textBox5
            // 
            textBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBox5.AnimateReadOnly = false;
            textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox5.Depth = 0;
            textBox5.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            textBox5.LeadingIcon = null;
            textBox5.Location = new System.Drawing.Point(121, 66);
            textBox5.Margin = new System.Windows.Forms.Padding(4);
            textBox5.MaxLength = 50;
            textBox5.MouseState = MaterialSkin.MouseState.OUT;
            textBox5.Multiline = false;
            textBox5.Name = "textBox5";
            textBox5.Size = new System.Drawing.Size(723, 50);
            textBox5.TabIndex = 16;
            textBox5.Text = "";
            textBox5.TrailingIcon = null;
            textBox5.KeyDown += textBox5_KeyDown;
            // 
            // button6
            // 
            button6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            button6.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            button6.Depth = 0;
            button6.HighEmphasis = true;
            button6.Icon = null;
            button6.Location = new System.Drawing.Point(862, 79);
            button6.Margin = new System.Windows.Forms.Padding(4);
            button6.MouseState = MaterialSkin.MouseState.HOVER;
            button6.Name = "button6";
            button6.NoAccentTextColor = System.Drawing.Color.Empty;
            button6.Size = new System.Drawing.Size(85, 36);
            button6.TabIndex = 18;
            button6.Text = "搜文件夹";
            button6.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            button6.UseAccentColor = false;
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // listView2
            // 
            listView2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            listView2.AutoSizeTable = false;
            listView2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            listView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            listView2.Depth = 0;
            listView2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            listView2.FullRowSelect = true;
            listView2.Location = new System.Drawing.Point(4, 117);
            listView2.Margin = new System.Windows.Forms.Padding(4);
            listView2.MinimumSize = new System.Drawing.Size(200, 100);
            listView2.MouseLocation = new System.Drawing.Point(-1, -1);
            listView2.MouseState = MaterialSkin.MouseState.OUT;
            listView2.Name = "listView2";
            listView2.OwnerDraw = true;
            listView2.Size = new System.Drawing.Size(1105, 506);
            listView2.TabIndex = 17;
            listView2.UseCompatibleStateImageBehavior = false;
            listView2.View = System.Windows.Forms.View.Details;
            listView2.SelectedIndexChanged += listView2_SelectedIndexChanged;
            listView2.SizeChanged += listView2_SizeChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "文件";
            columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "内容";
            columnHeader2.Width = 300;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "位置";
            columnHeader3.Width = 100;
            // 
            // materialTabSelector1
            // 
            materialTabSelector1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialTabSelector1.BaseTabControl = tabControl1;
            materialTabSelector1.CharacterCasing = MaterialSkin.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            materialTabSelector1.Depth = 0;
            materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialTabSelector1.Location = new System.Drawing.Point(7, 92);
            materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            materialTabSelector1.Name = "materialTabSelector1";
            materialTabSelector1.Size = new System.Drawing.Size(1104, 30);
            materialTabSelector1.TabIndex = 25;
            materialTabSelector1.Text = "materialTabSelector1";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 打开ToolStripMenuItem, 测试ToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(3, 64);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1121, 25);
            menuStrip1.TabIndex = 17;
            menuStrip1.Text = "menuStrip1";
            // 
            // 打开ToolStripMenuItem
            // 
            打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            打开ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            打开ToolStripMenuItem.Text = "打开";
            打开ToolStripMenuItem.Click += 打开ToolStripMenuItem_Click;
            // 
            // 测试ToolStripMenuItem
            // 
            测试ToolStripMenuItem.Name = "测试ToolStripMenuItem";
            测试ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            测试ToolStripMenuItem.Text = "测试";
            测试ToolStripMenuItem.Click += 测试ToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(3, 784);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1121, 22);
            statusStrip1.TabIndex = 18;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            toolStripStatusLabel1.Text = "状态";
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            ClientSize = new System.Drawing.Size(1127, 809);
            Controls.Add(materialTabSelector1);
            Controls.Add(statusStrip1);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "MainForm";
            Text = "大文件查看";
            FormClosed += Form1_FormClosed;
            Shown += Form1_Shown;
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumericPage).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private MaterialSkin.Controls.MaterialTextBox textBox2;
        private MaterialSkin.Controls.MaterialButton button3;
        private MaterialSkin.Controls.MaterialLabel label4;
        private MaterialSkin.Controls.MaterialListView listView1;
        private System.Windows.Forms.ColumnHeader text;
        private System.Windows.Forms.ColumnHeader num;
        private MaterialSkin.Controls.MaterialTextBox textBox3;
        private MaterialSkin.Controls.MaterialTabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测试ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialLabel label5;
        private MaterialSkin.Controls.MaterialLabel label2;
        private MaterialSkin.Controls.MaterialTextBox textBox4;
        private MaterialSkin.Controls.MaterialTextBox textBox5;
        private MaterialSkin.Controls.MaterialButton button6;
        private MaterialSkin.Controls.MaterialListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private MaterialSkin.Controls.MaterialMultiLineTextBox TextBoxPageContent;
        private MaterialSkin.Controls.MaterialComboBox ComboBoxPageSize;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialSlider SliderPage;
        private MaterialSkin.Controls.MaterialSwitch SwitchAutoWordwarp;
        private MaterialSkin.Controls.MaterialLabel LabelFilePath;
        private MaterialSkin.Controls.MaterialComboBox ComboBoxEncodings;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialTextBox TextBoxSearchInPage;
        private System.Windows.Forms.NumericUpDown NumericPage;
        private MaterialSkin.Controls.MaterialButton ButtonSearchInPage;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

