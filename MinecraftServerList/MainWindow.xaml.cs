﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinecraftServerList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ServerPingResult Result { get; set; }

        private List<Server> _servers;

        public MainWindow()
        {
            InitializeComponent();
            _servers = ConfigHelper.LoadConfig();
            serverListBox.ItemsSource = _servers;
            DataContext = Result;

            serverListBox.Items.SortDescriptions.Add(new SortDescription(nameof(Server.Description), ListSortDirection.Ascending));
            playersListBox.Items.SortDescriptions.Add(new SortDescription(nameof(Player.Name), ListSortDirection.Ascending));
        }

        private void addServerButton_Click(object sender, RoutedEventArgs e)
        {
            ServerWindow serverWindow = ShowServerWindow(new Server());
            if (serverWindow.ShowDialog() == true)
            {
                _servers.Add(serverWindow.Server);
                serverListBox.Items.Refresh();
                UpdateList(false);
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Server server = (Server)serverListBox.SelectedItem;
            ServerWindow serverWindow = ShowServerWindow(new Server(server.Description, server.Address, server.Port));
            if (serverWindow.ShowDialog() == true)
            {
                _servers.Insert(_servers.IndexOf(server), serverWindow.Server);
                _servers.Remove(server);
                UpdateList(true);
            }
        }

        private void deleteServerButton_Click(object sender, RoutedEventArgs e)
        {
            Server server = (Server)serverListBox.SelectedItem;
            _servers.Remove(server);
            UpdateList(true);
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshCurrent();
        }

        private void serverListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshCurrent();
        }

        private void UpdateList(bool refresh)
        {
            serverListBox.Items.Refresh();
            ConfigHelper.SaveConfig(_servers);

            if (refresh)
                RefreshCurrent();
        }

        private void RefreshCurrent()
        {
            if (serverListBox.SelectedItem == null)
            {
                Result = new ServerPingResult();
                UpdateContextAndButtonState();
            }
            else
            {
                Cursor = Cursors.Wait;
                Server server = (Server)serverListBox.SelectedItem;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += delegate (object sender, DoWorkEventArgs args)
                {
                    MinecraftPingRequester pingRequester = new MinecraftPingRequester(server.Address, server.Port);
                    args.Result = pingRequester.GetServerPingData();
                };
                worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs args)
                {
                    Result = (ServerPingResult)args.Result;
                    DataContext = Result;
                    UpdateContextAndButtonState();
                    Cursor = Cursors.Arrow;
                };
                worker.RunWorkerAsync();
            }
        }

        private void UpdateContextAndButtonState()
        {
            DataContext = Result;

            bool editEnabled = serverListBox.SelectedItem != null;
            editButton.IsEnabled = editEnabled;
            deleteServerButton.IsEnabled = editEnabled;
            refreshButton.IsEnabled = editEnabled;
        }

        private ServerWindow ShowServerWindow(Server server)
        {
            ServerWindow serverWindow = new ServerWindow(server) { Owner = this };
            return serverWindow;
        }
    }
}
