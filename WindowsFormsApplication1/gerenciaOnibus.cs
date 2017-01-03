using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.ObjectModel;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using WindowsFormsApplication1.DAO;
using WindowsFormsApplication1.POJO;

namespace WindowsFormsApplication1
{
    public partial class gerenciaOnibus : Form
    {
        int linhaSegunda = 0;
        int linhaDomingo = 0;
        int linhaFeriado = 0;

        List<HorarioObj> listaHorarios = new List<HorarioObj>();
        List<onibusPontoObj> listaPontosOnibus = new List<onibusPontoObj>();
        List<PontoObj> listaPontos = new List<PontoObj>();
        List<areaObj> listaAreas = new List<areaObj>();

        List<HorarioObj> listaHorarios2 = new List<HorarioObj>();
        List<onibusPontoObj> listaPontosOnibus2 = new List<onibusPontoObj>();
        int id_onibus;

        PontoDAO pontoDAO = new PontoDAO();
        areaDAO areaDAO = new areaDAO();
        HorarioDAO horaDAO = new HorarioDAO();

        HorarioObj horaObj;
        onibusPontoObj onibuspontoObj = new onibusPontoObj();
        onibusPontoDAO onibuspontoDAO = new onibusPontoDAO();

        GMarkerGoogle marker;
        List<GMarkerGoogle> listMarks;

        public gerenciaOnibus(int id, string nome,string linha,string letra)
        {
            
            InitializeComponent();
            maskedTextBox2.Text = linha;
            textBoxNome.Text = nome;
            textBoxLetra.Text = letra;
            id_onibus = id;
        }
        public void atualizaMarkers()
        {
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            gMapControl1.Overlays.Clear();
            listMarks.Clear();
            if (gMapControl1.Zoom > 14)
            {
                foreach (PontoObj ponto in listaPontos)
                {
                    //
                    if (ponto.Terminal == 1)
                    {
                        marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.blue);
                        marker.ToolTipText = "Terminal";
                    }

                    else if (ponto.Selecionado)
                    {

                        marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.red_small);

                        marker.ToolTipText = ponto.Ordem.ToString() + "    " + ponto.Id;
                        marker.Tag = ponto.Id;
                        markersOverlay.Markers.Add(marker);

                        gMapControl1.Overlays.Add(markersOverlay);
                        listMarks.Add(marker);
                    }
                    else if (!ponto.Selecionado)
                    {
                        marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(ponto.Latitude), Convert.ToDouble(ponto.Longitude)), GMarkerGoogleType.blue_small);
                        marker.ToolTipText = "Latitude:" + ponto.Latitude + "\n" + "Longitude:" + ponto.Longitude + "    " + ponto.Id;
                        markersOverlay.Markers.Add(marker);

                        gMapControl1.Overlays.Add(markersOverlay);
                        listMarks.Add(marker);
                    }

                }
            }
            else
            {
                foreach (areaObj area in listaAreas)
                {
                    marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(area.Latitude), Convert.ToDouble(area.Longitude)), GMarkerGoogleType.blue);
                    marker.Tag = area.Id;
                    listMarks.Add(marker);
                    markersOverlay.Markers.Add(marker);
                    gMapControl1.Overlays.Add(markersOverlay);
                }
            }


        }

        public void atualizaHorarios()
        {
            linhaSegunda = 0;
            linhaDomingo = 0;
            linhaFeriado = 0;
            dataGridView1.Rows.Clear();
            listaHorarios = listaHorarios.OrderBy(o => o.Horario).ToList();
            dataGridView1.Rows.Add();
            dataGridView1.Rows.Add();


            foreach (HorarioObj h in listaHorarios)
            {
                if (h.Dia == 0)
                {

                    dataGridView1.Rows[linhaSegunda].Cells[0].Value = h.Horario;
                    linhaSegunda++;


                }
                if (h.Dia == 1)
                {

                    dataGridView1.Rows[linhaDomingo].Cells[1].Value = h.Horario;
                    linhaDomingo++;

                }
                if (h.Dia == 2)
                {
                    dataGridView1.Rows[linhaFeriado].Cells[2].Value = h.Horario;
                    linhaFeriado++;
                }
                if (linhaSegunda - dataGridView1.Rows.Count == -1 || linhaDomingo - dataGridView1.Rows.Count == -1 || linhaFeriado - dataGridView1.Rows.Count == -1)
                {
                    dataGridView1.Rows.Add();
                }
            }
        }
        public bool testaInternet()
        {
            Conexao c = new Conexao();
            if (!c.OpenConnection())
            {
                MessageBox.Show("Parece que você esta sem internet!");
                return false;
            }
            return true;
        }
        private void gerenciaOnibus_Load(object sender, EventArgs e)
        {
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Text = textBoxNome.Text;
            if (testaInternet())
            {
                listaPontos = pontoDAO.listaPontoDAO();
                listaAreas = areaDAO.listaPontoDAO();
                listaPontosOnibus = onibuspontoDAO.listaPontoDAO(id_onibus);
                listaPontosOnibus2 = onibuspontoDAO.listaPontoDAO(id_onibus);

                listaHorarios = horaDAO.listaPontoDAO(id_onibus);
                listaHorarios2 = horaDAO.listaPontoDAO(id_onibus);


                atualizaHorarios();
                foreach (PontoObj p2 in listaPontos)
                {
                    foreach (onibusPontoObj p in listaPontosOnibus)
                    {

                        if (p.Id_ponto == p2.Id)
                        {
                            p2.Selecionado = true;
                            p2.Ordem = p.Ordem;
                        }
                    }
                }

                listMarks = new List<GMarkerGoogle>();
                gMapControl1.DragButton = MouseButtons.Left;
                gMapControl1.CanDragMap = true;
                gMapControl1.Position = new PointLatLng(-22.725, -47.6476);
                gMapControl1.MinZoom = 0;
                gMapControl1.MaxZoom = 24;
                gMapControl1.Zoom = 12;
                gMapControl1.AutoScroll = true;
                gMapControl1.MapProvider = GMapProviders.OviHybridMap;
                atualizaMarkers();
            }
        }

        private void gMapControl1_OnMapZoomChanged()
        {
            atualizaMarkers();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void selecaoPonto_Click(object sender, EventArgs e)
        {
            if (selecaoPonto.Enabled)
            {
                MessageBox.Show("Selecione o(os) pontos!");
                selecaoPonto.Enabled = false;
                selecaoPonto.BackColor = Color.Red;
                deselecaoPonto.Enabled = true;
                deselecaoPonto.BackColor = Color.Transparent;
            }
        }

        private void deselecaoPonto_Click(object sender, EventArgs e)
        {
            if (deselecaoPonto.Enabled)
            {
                MessageBox.Show("Selecione o(os) pontos para tirar!");
                deselecaoPonto.Enabled = false;
                deselecaoPonto.BackColor = Color.Red;
                selecaoPonto.BackColor = Color.Transparent;
                selecaoPonto.Enabled = true;
            }
            else
                deselecaoPonto.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            atualizaMarkers();
        }

        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (GMarkerGoogle g in listMarks)
            {
                if (g.IsMouseOver)
                {
                    if (gMapControl1.Zoom > 15)
                    {
                        // Console.WriteLine("Marker hit? " + gmark.Position.ToString());
                        if (!selecaoPonto.Enabled)
                        {
                            onibuspontoObj = new onibusPontoObj();
                            onibuspontoObj.Latitude = g.Position.Lat.ToString();
                            onibuspontoObj.Longitude = g.Position.Lng.ToString();
                            onibuspontoObj.Id_ponto = Convert.ToInt16(g.Tag);
                            if (radioButton1.Checked)
                                onibuspontoObj.Ida = 0;
                            else
                                onibuspontoObj.Ida = 1;
                            onibuspontoObj.Ordem = Convert.ToInt16(maskedTextBox3.Text);
                            listaPontosOnibus.Add(onibuspontoObj);
                            foreach (PontoObj ponto in listaPontos)
                            {
                                if ((Convert.ToDouble(ponto.Latitude) == g.Position.Lat) && (Convert.ToDouble(ponto.Longitude) == g.Position.Lng))
                                {
                                    ponto.Selecionado = true;
                                }
                            }
                            atualizaMarkers();
                            atualizaMarkers();
                            break;


                        }


                        if (!deselecaoPonto.Enabled)
                        {
                            foreach (onibusPontoObj pontoni in listaPontosOnibus)
                            {
                                Console.WriteLine(pontoni.Latitude.ToString() + pontoni.Longitude.ToString());
                                if ((Convert.ToDouble(pontoni.Latitude) == g.Position.Lat) && Convert.ToDouble(pontoni.Longitude) == g.Position.Lng)
                                {
                                    listaPontosOnibus.Remove(pontoni);
                                    foreach (PontoObj ponto in listaPontos)
                                    {
                                        if ((Convert.ToDouble(ponto.Latitude) == g.Position.Lat) && (Convert.ToDouble(ponto.Longitude) == g.Position.Lng))
                                        {
                                            ponto.Selecionado = false;
                                        }
                                    }
                                    atualizaMarkers();
                                    atualizaMarkers();
                                    break;
                                }
                            }
                        }
                        break;
                    }

                }

            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
                if (testaInternet())
                {
                if (e.KeyChar == Convert.ToChar(Keys.Enter))
                {

                    string text = "";
                    int hour = 0;
                    int minute = 0;
                    try
                    {
                        text = maskedTextBox1.Text.ToString();
                        string[] parts = text.Split(':');
                        hour = int.Parse(parts[0]);
                        minute = int.Parse(parts[1]);
                    }
                    catch (Exception )
                    {
                        MessageBox.Show("Complete o campo!");
                    }

                    if (maskedTextBox1.Text.Count() == 5 && (hour <= 24 && hour >= 0) && (minute >= 00 && minute <= 59))
                    {
                        horaObj = new HorarioObj();
                        horaObj.Horario = text;
                        if (checkBoxSeg.Checked || (checkBoxDom.Checked) || checkBoxFer.Checked)
                        {
                            if (linhaSegunda - dataGridView1.Rows.Count == -1 || linhaDomingo - dataGridView1.Rows.Count == -1 || linhaFeriado - dataGridView1.Rows.Count == -1)
                            {
                                dataGridView1.Rows.Add();
                            }

                        }

                        else
                            MessageBox.Show("Selecione o(os) dia!");
                        if (checkBoxSeg.Checked)
                        {
                            horaObj.Dia = 0;
                            dataGridView1.Rows[linhaSegunda].Cells[0].Value = text;
                            listaHorarios.Add(horaObj);
                            linhaSegunda++;


                        }
                        if (checkBoxDom.Checked)
                        {
                            horaObj.Dia = 1;
                            dataGridView1.Rows[linhaDomingo].Cells[1].Value = text;
                            listaHorarios.Add(horaObj);
                            linhaDomingo++;

                        }
                        if (checkBoxFer.Checked)
                        {
                            horaObj.Dia = 2;
                            dataGridView1.Rows[linhaFeriado].Cells[2].Value = text;
                            listaHorarios.Add(horaObj);
                            linhaFeriado++;
                        }
                    }
                    else { MessageBox.Show("Hora invalida!"); }
                    maskedTextBox1.Clear();
                }
            }
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            gMapControl1.Position = new PointLatLng(gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat, gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng);
            gMapControl1.Zoom++;
        }

        private void buttonApagaHora_Click(object sender, EventArgs e)
        {

            try
            {
                dataGridView1.CurrentCell.Value.ToString();
            }
            catch
            {
                MessageBox.Show("Seleciona uma celula para Excluir!");
            }
            foreach (HorarioObj h in listaHorarios)
            {
                if (dataGridView1.CurrentCell.Value.ToString().Equals(h.Horario))
                {
                    if (dataGridView1.CurrentCell.ColumnIndex == h.Dia)
                    {
                        listaHorarios.Remove(h);
                        if (dataGridView1.CurrentCell.ColumnIndex == 0)
                        {
                            dataGridView1.CurrentCell.Value = "";
                            linhaSegunda--;
                        }
                        if (dataGridView1.CurrentCell.ColumnIndex == 1)
                        {
                            dataGridView1.CurrentCell.Value = "";
                            linhaDomingo--;
                        }
                        if (dataGridView1.CurrentCell.ColumnIndex == 2)
                        {
                            dataGridView1.CurrentCell.Value = "";
                            linhaFeriado--;
                        }
                        MessageBox.Show("Excluido!");
                        break;
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            atualizaHorarios();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (testaInternet())
            {
                pictureBox1.Visible = true;
                OnibusDAO oni = new OnibusDAO();
                PontoDAO pontoDAO = new PontoDAO();
                HorarioDAO horDAO = new HorarioDAO();
                onibusPontoDAO onibusPonto = new onibusPontoDAO();
                oni.atualizaOnibus(id_onibus, textBoxNome.Text, maskedTextBox2.Text, textBoxLetra.Text);
                foreach (HorarioObj l in listaHorarios2)
                {
                    horaDAO.deletaHorario(l.Id_horario);
                }
                foreach (onibusPontoObj onHor in listaPontosOnibus2)
                {
                    onibusPonto.deletaPontoOnibus(onHor.Id_pontoOnibus);
                }

                foreach (HorarioObj l in listaHorarios)
                {

                    horDAO.adicionaHorarioDAO(l.Horario.ToString(), id_onibus, l.Dia);
                }

                foreach (onibusPontoObj onHor in listaPontosOnibus)
                {
                    onibusPonto.adicionaPontoOnibus(onHor.Id_ponto, id_onibus, onHor.Ida,onHor.Ordem);
                }
                MessageBox.Show("Atualizado cadastrado com sucesso!");
                this.Close();
               
            }
            pictureBox1.Visible = false;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom++;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom--;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var height = 30;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                height += dr.Height;
            }

            dataGridView1.Height = height;
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            var height = 30;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                height += dr.Height;
            }

            dataGridView1.Height = height;
        }

        private void dataGridView1_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            var height = 30;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                height += dr.Height;
            }

            dataGridView1.Height = height;
        }
    }
}
















