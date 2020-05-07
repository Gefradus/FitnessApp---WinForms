﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FitApp
{
    public partial class FormDodawania : Form
    {
        private readonly ModelXML _context = new ModelXML();
        private readonly StyleOfFormDodawania _style = new StyleOfFormDodawania();

        public int WKtorym { get; set; }

        public FormDodawania()
        {
            InitializeComponent();
        }

        private void FormDodawania_Load(object sender, EventArgs e)
        {
            _style.CreatePlaceholderText(txtBoxSearch, panelGlowny, panelGorny,
                panelSearch, NazwaPosilku, btnHidden);

            NazwaPosilkuNapisz();
        }

        public void NazwaPosilkuNapisz()
        {
            if (WKtorym == 1) { NazwaPosilku.Text = "Śniadanie"; }
            if (WKtorym == 2) { NazwaPosilku.Text = "II śniadanie"; }
            if (WKtorym == 3) { NazwaPosilku.Text = "Obiad"; }
            if (WKtorym == 4) { NazwaPosilku.Text = "Deser"; }
            if (WKtorym == 5) { NazwaPosilku.Text = "Przekąska"; }
            if (WKtorym == 6) { NazwaPosilku.Text = "Kolacja"; }

            NazwaPosilku.Margin = new Padding((panelGorny.Width - NazwaPosilku.Width)/2,5,0,0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Posilek> posilki = _context.Posilki();

            posilki.Add(new Posilek() { 
                PosilekId = _context.AutoIncrementPosilki(posilki), 
                Gramatura = 200, 
                WKtorym = 1, 
                ProduktId = 2, 
                DzienId = 1 
            });
            posilki.Add(new Posilek()
            {
                PosilekId = _context.AutoIncrementPosilki(posilki),
                Gramatura = 200,
                WKtorym = 1,
                ProduktId = 1,
                DzienId = 1
            });
            posilki.Add(new Posilek()
            {
                PosilekId = _context.AutoIncrementPosilki(posilki),
                Gramatura = 200,
                WKtorym = 2,
                ProduktId = 1,
                DzienId = 1
            });

            _context.ZapiszPosilki(posilki);
        }

        private void txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string wyszukiwanaFraza = txtBoxSearch.Text;

            List<Produkt> wyszukaneProdukty = new List<Produkt>();
            foreach (var produkt in _context.Produkty()){
                if (produkt.NazwaProduktu.ToLower().StartsWith(wyszukiwanaFraza.ToLower()) && !string.IsNullOrWhiteSpace(wyszukiwanaFraza))
                {
                    wyszukaneProdukty.Add(produkt);
                }
            }

            panelGlowny.Controls.Clear();
            foreach (var item in wyszukaneProdukty)
            {
                UtworzPanelProduktu(item.ProduktId);
            }
        }

        private void UtworzPanelProduktu(int produktID)
        {
            Panel panel = new Panel()
            {
                Size = new Size(369, 50),
                BackColor = Color.White,
                Margin = new Padding(40, 0, 0, 0),
                Visible = true
            };
            Label label = new Label
            {
                Text = _context.DajProdukt(produktID).NazwaProduktu,
                Font = new Font("Microsoft Sans Serif", 9.5F, FontStyle.Bold, GraphicsUnit.Point)
            };
            Button button = new Button()
            {
                Text = "Dodaj",
                Size = new Size(30, 30),
                Location = new Point(320, 10),
                BackColor = Color.Green,
                ForeColor = Color.White

            };

            button.Click += new EventHandler((sender, e) => BtnAddClick(produktID));
            panel.Controls.Add(label);
            panel.Controls.Add(button);
            panelGlowny.Controls.Add(panel);
        }

        private void BtnAddClick(int produktID)
        {
            int dodawanyProduktID = 0;

            foreach (var produkt in _context.Produkty())
            {
                if (produkt.ProduktId == produktID)
                {
                    dodawanyProduktID = produktID;
                }
            }

            List<Posilek> posilki = _context.Posilki();
            posilki.Add(new Posilek()
            {
                DzienId = 1,
                Gramatura = 100,
                PosilekId = _context.AutoIncrementPosilki(_context.Posilki()),
                ProduktId = dodawanyProduktID,
                WKtorym = WKtorym
            });

            _context.ZapiszPosilki(posilki);
        }
    }
}
