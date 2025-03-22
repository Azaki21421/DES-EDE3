using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace usbwork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
        }

        private static string PromptForPassword(string message)
        {
            string password = "";
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = "Enter password",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Text = message, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240, UseSystemPasswordChar = true };
            Button confirmation = new Button() { Text = "OK", Left = 90, Width = 100, Top = 80, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                password = textBox.Text;
            }

            return password;
        }

        private static byte[] GetTripleDESKey(string promptMessage)
        {
            string password = PromptForPassword(promptMessage);
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password)).Take(24).ToArray();
            }
        }

        public static void EncryptFile(string inputFile, string outputFile)
        {
            byte[] key1 = GetTripleDESKey("Enter password 1:");
            byte[] key2 = GetTripleDESKey("Enter password 2:");
            byte[] key3 = GetTripleDESKey("Enter password 3:");

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                byte[] data = File.ReadAllBytes(inputFile);

                using (ICryptoTransform enc1 = tdes.CreateEncryptor(key1, null))
                using (ICryptoTransform dec2 = tdes.CreateDecryptor(key2, null))
                using (ICryptoTransform enc3 = tdes.CreateEncryptor(key3, null))
                {
                    byte[] step1 = enc1.TransformFinalBlock(data, 0, data.Length);
                    byte[] step2 = dec2.TransformFinalBlock(step1, 0, step1.Length);
                    byte[] finalData = enc3.TransformFinalBlock(step2, 0, step2.Length);
                    File.WriteAllBytes(outputFile, finalData);
                }
            }
        }

        public static void DecryptFile(string inputFile, string outputFile)
        {
            byte[] key1 = GetTripleDESKey("Enter password 1:");
            byte[] key2 = GetTripleDESKey("Enter password 2:");
            byte[] key3 = GetTripleDESKey("Enter password 3:");

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                byte[] data = File.ReadAllBytes(inputFile);

                using (ICryptoTransform dec3 = tdes.CreateDecryptor(key3, null))
                using (ICryptoTransform enc2 = tdes.CreateEncryptor(key2, null))
                using (ICryptoTransform dec1 = tdes.CreateDecryptor(key1, null))
                {
                    byte[] step1 = dec3.TransformFinalBlock(data, 0, data.Length);
                    byte[] step2 = enc2.TransformFinalBlock(step1, 0, step1.Length);
                    byte[] finalData = dec1.TransformFinalBlock(step2, 0, step2.Length);
                    File.WriteAllBytes(outputFile, finalData);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputFile = label1.Text;
            string outputFile = inputFile + ".enc";
            EncryptFile(inputFile, outputFile);
            MessageBox.Show("The file is encrypted!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string inputFile = label1.Text;
            string outputFile = inputFile.Replace(".enc", ".dec");
            DecryptFile(inputFile, outputFile);
            MessageBox.Show("The file is decrypted!");
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string fullPath = files[0];
                    string fileName = Path.GetFileName(fullPath);

                    label1.Text = fileName;

                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(label1, fullPath);

                    pictureBox1.Visible = true;
                }
            }
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
        }
    }
}
