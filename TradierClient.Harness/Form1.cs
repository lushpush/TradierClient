﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradierClient;


namespace TradierClient.Harness
{
    public partial class Form1 : Form
    {
        private Gateway _apiGateway = null;

        public Form1()
        {
            InitializeComponent();
        }

        void cmbApiCall_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmbApiCall.SelectedIndex > -1)
            {
                if (cmbMessageFormat.SelectedItem == null)
                {
                    MessageBox.Show("Please select desired Message Format.");
                    cmbApiCall.SelectedIndex = -1;
                    return;
                }

                if (_apiGateway == null)
                {
                    _apiGateway = InitializeGateway();
                    SetSelectedMessageFormat();
                }

                FindControlAndAddToContainer(_apiGateway, cmbApiCall.SelectedItem.ToString());
            }
        }


        private Gateway InitializeGateway()
        {
            string accessToken = ConfigurationManager.AppSettings["AccessToken"];
            return new Gateway(accessToken);
        }

        private void FindControlAndAddToContainer(Gateway gateway, string apiCall)
        {
            pnlControlContainer.Controls.Clear();

            switch (apiCall)
            {
                case "Market/Get Option Chain":
                case "Market/Get Option Strikes":
                    pnlControlContainer.Controls.Add(new Controls.MarketData.GetOptionChain(gateway, apiCall));
                    break;
                case "Market/Get Option Expirations":
                case "Market/Get Quotes":
                    pnlControlContainer.Controls.Add(new Controls.MarketData.GetQuotes(gateway, apiCall));
                    break;
                case "Market/Get Time And Sales":
                case "Market/Get Historical Pricing":
                    pnlControlContainer.Controls.Add(new Controls.MarketData.GetTimeAndSales(gateway, apiCall));
                    break;
                case "Market/Get Intraday Status":
                case "Market/Get Market Calendar":
                    pnlControlContainer.Controls.Add(new Controls.MarketData.GetIntradayStatus(gateway, apiCall));
                    break;
                case "Market/Company Search":
                    pnlControlContainer.Controls.Add(new Controls.MarketData.CompanySearch(gateway, apiCall));
                    break;
                case "Market/Symbol Lookup":
                    pnlControlContainer.Controls.Add(new Controls.MarketData.SymbolLookup(gateway, apiCall));
                    break;
                case "User Data/Get Profile":
                case "User Data/Get Balances":
                case "User Data/Get Positions":
                case "User Data/Get History":
                case "User Data/Get Cost Basis":
                case "User Data/Get Orders":
                    pnlControlContainer.Controls.Add(new Controls.UserData.CommandPanel(gateway, apiCall));
                    break;
                case "Account/Get Balances":
                case "Account/Get Positions":
                case "Account/Get History":
                case "Account/Get Cost Basis":
                case "Account/Get Orders":
                case "Account/Get Order Status":
                    pnlControlContainer.Controls.Add(new Controls.AccountData.CommandPanel(gateway, apiCall));
                    break;
            }
        }

        private void cmbMessageFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelectedMessageFormat();
        }

        private void SetSelectedMessageFormat()
        {
            if (_apiGateway != null)
            {
                switch (cmbMessageFormat.SelectedItem.ToString())
                {
                    case "JSON":
                        _apiGateway.MessageFormat = MessageFormatEnum.JSON;
                        break;
                    case "XML":
                        _apiGateway.MessageFormat = MessageFormatEnum.XML;
                        break;
                }
            }
        }
    }
}
