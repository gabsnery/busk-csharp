using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.DAO;

namespace WindowsFormsApplication1
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logar con = new logar();
             
            if (con.veLogin(textBox1.Text.ToString()))
             {
                logar con2 = new logar();
                if (con2.veSenha(textBox1.Text.ToString(), textBox2.Text.ToString()))
                {
                    home frm = new home();

                    frm.Show();
                }
                else MessageBox.Show("Senha invalida");

            }
            else
            { MessageBox.Show("Login inexistente"); }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                logar con = new logar();

                if (con.veLogin(textBox1.Text.ToString()))
                {
                    logar con2 = new logar();
                    if (con2.veSenha(textBox1.Text.ToString(), textBox2.Text.ToString()))
                    {
                        home frm = new home();

                        frm.Show();
                    }
                    else MessageBox.Show("Senha invalida");

                }
                else
                { MessageBox.Show("Login inexistente"); }
            }
        }
    }
}
