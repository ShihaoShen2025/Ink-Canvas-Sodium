﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace Ink_Canvas {
    public partial class MainWindow : Window {
        private void SaveScreenShot(bool isHideNotification, string fileName = null) {
            var rc = System.Windows.Forms.SystemInformation.VirtualScreen;
            var bitmap = new System.Drawing.Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);

            using (var memoryGrahics = System.Drawing.Graphics.FromImage(bitmap)) {
                memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, System.Drawing.CopyPixelOperation.SourceCopy);
            }

            if (Settings.Automation.IsSaveScreenshotsInDateFolders) {
                if (string.IsNullOrWhiteSpace(fileName)) fileName = DateTime.Now.ToString("HH-mm-ss");
                var savePath = Settings.Automation.AutoSavedStrokesLocation +
                               @"\Auto Saved - Screenshots\{DateTime.Now.Date:yyyyMMdd}\{fileName}.png";
                if (!Directory.Exists(Path.GetDirectoryName(savePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                bitmap.Save(savePath, ImageFormat.Png);
                if (!isHideNotification) ShowNotification("截图成功保存至 " + savePath);
            }
            else {
                var savePath = Settings.Automation.AutoSavedStrokesLocation + @"\Auto Saved - Screenshots";
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                bitmap.Save(savePath + @"\" + DateTime.Now.ToString("u").Replace(':', '-') + ".png", ImageFormat.Png);
                if (!isHideNotification)
                    ShowNotification("截图成功保存至 " + savePath + @"\" + DateTime.Now.ToString("u").Replace(':', '-') +
                                     ".png");
            }

            if (Settings.Automation.IsAutoSaveStrokesAtScreenshot) SaveInkCanvasStrokes(false, false);
        }

        private void SaveScreenShotToDesktop() {
            var rc = System.Windows.Forms.SystemInformation.VirtualScreen;
            var bitmap = new System.Drawing.Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);

            using (var memoryGrahics = System.Drawing.Graphics.FromImage(bitmap)) {
                memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, System.Drawing.CopyPixelOperation.SourceCopy);
            }

            var savePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            bitmap.Save(savePath + @"\" + DateTime.Now.ToString("u").Replace(':', '-') + ".png", ImageFormat.Png);
            ShowNotification("截图成功保存至【桌面" + @"\" + DateTime.Now.ToString("u").Replace(':', '-') + ".png】");
            if (Settings.Automation.IsAutoSaveStrokesAtScreenshot) SaveInkCanvasStrokes(false, false);
        }
    }
}