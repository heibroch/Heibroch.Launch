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

        public TrayIcon(Action<string> contextMenuItemClicked, List<string> contextMenuItems)
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

                var contextMenu = new ContextMenu();
                foreach (var contextMenuItem in contextMenuItems)
                {
                    var menuItem = new MenuItem(contextMenuItem);
                    menuItem.Click += MenuItem_Click;
                    contextMenu.MenuItems.Add(menuItem);
                }

                notifyIcon.ContextMenu = contextMenu;
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not launch tray icon.\r\nApplication will continue without");

                EventLog.WriteEntry(Constants.ApplicationName, $"TrayIcon creation failed!\r\n{e}");
                EventLog.WriteEntry(Constants.ApplicationName, $"");
            }
        }

        private void MenuItem_Click(object sender, EventArgs e) => contextMenuItemClicked(((MenuItem)sender).Text);

        private void OnTrayIconDoubleClick(object sender, EventArgs e) { }

        private void OnTrayIconClick(object sender, EventArgs e) { }

        public void Dispose()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}
