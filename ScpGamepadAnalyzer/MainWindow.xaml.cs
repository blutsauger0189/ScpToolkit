﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using Ookii.Dialogs.Wpf;
using ScpControl.Driver;

namespace ScpGamepadAnalyzer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Guid TempDeviceGuid = Guid.Parse("433FA0C6-2BF1-4675-98C6-7F4FC99796FC");
        private IntPtr _hwnd;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _hwnd = new WindowInteropHelper(this).Handle;
        }

        private void InstallButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedDevice = DevicesComboBox.SelectedItem as WdiUsbDevice;

            if (selectedDevice == null)
                return;

            var msgBox = new TaskDialog
            {
                Buttons = {new TaskDialogButton(ButtonType.Yes), new TaskDialogButton(ButtonType.No)},
                WindowTitle = "I'm about to change the device driver!",
                Content = string.Format("You selected the device {1}{0}{0}Want me to change the driver now?",
                    Environment.NewLine,
                    selectedDevice)
            };

            var msgResult = msgBox.ShowDialog(this);

            var tmpPath = Path.Combine(Path.GetTempPath(), "ScpGamepadAnalyzer");

            if (msgResult.ButtonType == ButtonType.Yes)
            {
                WdiWrapper.Instance.InstallLibusbKDriver(selectedDevice.HardwareId, TempDeviceGuid, tmpPath,
                    string.Format("{0}.inf", selectedDevice.Description),
                    _hwnd, true);
            }
        }
    }
}