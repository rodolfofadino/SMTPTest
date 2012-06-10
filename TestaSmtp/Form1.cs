using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using System.Text;

namespace TestaSmtp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Exception ex;
        string sucesso;
        private void btnTestar_Click(object sender, EventArgs e)
        {
            sucesso = "";
            ex = new Exception();
            txtMess.Text = string.Empty;
            try
            {
                SmtpClient cliente = new SmtpClient();

                if (txtHost.Text != "")
                    cliente.Host = txtHost.Text;

                if (txtPort.Text != "")
                    cliente.Port = Convert.ToInt32(txtPort.Text);

                cliente.EnableSsl = chbSSL.Checked;

                if (txtUser.Text != "" && txtPassword.Text != "")
                {
                    cliente.UseDefaultCredentials = false;
                    System.Net.NetworkCredential credenciais =
                        new System.Net.NetworkCredential(txtUser.Text, txtPassword.Text);
                    cliente.Credentials = credenciais;
                }
                else
                    cliente.UseDefaultCredentials = true;

                MailMessage mensagem = new MailMessage();

                if (txtFrom.Text != "")
                    mensagem.From = new MailAddress(txtFrom.Text);

                if (txtTo.Text != "")
                    mensagem.To.Add(txtTo.Text);

                if (txtSubj.Text != "")
                    mensagem.Subject = txtSubj.Text;

                if (txtBody.Text != "")
                    mensagem.Body = txtBody.Text;
                else
                    if (!string.IsNullOrEmpty(htmlEnvio))
                    {
                        mensagem.Body = htmlEnvio;
                        mensagem.IsBodyHtml = true;
                    }

                cliente.Send(mensagem);

                txtMess.Text = "Email enviado com sucesso";
                sucesso = "Email enviado com sucesso";
            }
            catch (Exception ex2)
            {
                sucesso = "Falha";
                ex = ex2;
                txtMess.Text = "Erro Mensagem: " + ex.Message + " || Erro Stack Trace: " + ex.StackTrace;
            }

        }

        private void btnGravarLog_Click(object sender, EventArgs e)
        {
            if (txtMess.Text != "")
            {
                //grava log

                try
                {
                    string nome = "dt" + DateTime.Now.ToString().Replace("/", "-").Replace(".", "-").Replace(":", "-").Replace(" ", "hr") + ".txt";
                    StreamWriter log = new StreamWriter(nome);
                    log.WriteLine("-------Log SMTP Tester -------------");
                    log.WriteLine("Data: " + DateTime.Now.ToString());
                    log.WriteLine("Status: " + sucesso);
                    try
                    {
                        log.WriteLine("Erro Message: " + ex.Message);
                        log.WriteLine("Stack Trace: " + ex.StackTrace);
                        log.WriteLine("Target Site (metodo): " + ex.TargetSite.ToString());
                    }
                    catch { }
                    log.WriteLine("");
                    log.WriteLine("SMTP");
                    log.WriteLine("Host: " + txtHost.Text);
                    log.WriteLine("Port: " + txtPort.Text);
                    log.WriteLine("SSL: " + chbSSL.Checked.ToString());
                    log.WriteLine("User: " + txtUser.Text);
                    log.WriteLine("Password: " + txtPassword.Text);
                    log.WriteLine("");
                    log.WriteLine("Email");
                    log.WriteLine("From: " + txtHost.Text);
                    log.WriteLine("To: " + txtTo.Text);
                    log.WriteLine("Subj: " + txtSubj.Text);
                    log.WriteLine("Body: " + txtBody.Text);

                    log.WriteLine("");
                    log.WriteLine("-------End Log SMTP Tester ------");
                    log.Close();

                    MessageBox.Show("Log record as: " + nome);
                }
                catch (Exception a)
                {
                    MessageBox.Show("Erro on record log: " + a.Message);
                }



            }
            else
                MessageBox.Show("No log found, make a test.");



        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtBody.Text = "";
            txtFrom.Text = "";
            txtHost.Text = "";
            txtMess.Text = "";
            txtPassword.Text = "";
            txtPort.Text = "";
            txtSubj.Text = "";
            txtTo.Text = "";
            txtUser.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        public string htmlEnvio;


        private void button1_Click_2(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        txtFileName.Text = openFileDialog1.FileName;
                        TextReader reader = File.OpenText(openFileDialog1.FileName);

                        htmlEnvio = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            htmlEnvio = null;
            txtFileName.Text = string.Empty;

        }





    }
}
