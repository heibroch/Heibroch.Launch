using Heibroch.Common;
using Heibroch.Infrastructure.Interfaces.MessageBus;
using Heibroch.Launch.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Heibroch.Launch
{
    public class TrayIcon : IDisposable
    {
        private readonly NotifyIcon notifyIcon;
        private readonly Action<string> contextMenuItemClicked;

        public TrayIcon(IInternalMessageBus internalMessageBus, Action<string> contextMenuItemClicked, List<string> contextMenuItems)
        {
            try
            {
                this.contextMenuItemClicked = contextMenuItemClicked;
                //Tray icon
                notifyIcon = new NotifyIcon();
                notifyIcon.Click += new EventHandler(OnTrayIconClick);
                notifyIcon.DoubleClick += new EventHandler(OnTrayIconDoubleClick);
                notifyIcon.Icon = new Icon(Path.Combine(Constants.RootPath, $"LaunchLogo.ico"));
                notifyIcon.Visible = true;

                notifyIcon.ContextMenuStrip = new ContextMenuStrip();

                foreach (var contextMenuItem in contextMenuItems)
                {
                    var toolStripMenuItem = new ToolStripMenuItem(contextMenuItem);
                    toolStripMenuItem.Click += ToolStripMenuItem_Click;
                    notifyIcon.ContextMenuStrip.Items.Add(toolStripMenuItem);
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Could not launch tray icon.\r\nApplication will continue without");

                internalMessageBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"TrayIcon creation failed!\r\n{e}", EventLogEntryType.Information));
                internalMessageBus.Publish(new LogEntryPublished(Constants.ApplicationName, $"", EventLogEntryType.Information));
            }
        }

        private void ToolStripMenuItem_Click(object? sender, EventArgs e) => contextMenuItemClicked(((ToolStripMenuItem)sender).Text);

        private void OnTrayIconDoubleClick(object sender, EventArgs e) { }

        private void OnTrayIconClick(object sender, EventArgs e) { }

        public void Dispose()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}
