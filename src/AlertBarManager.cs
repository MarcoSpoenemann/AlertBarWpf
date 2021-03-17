using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace AlertBarWpf
{
    public static class AlertBarManager
    {
        public static Dictionary<string, AlertBar> AlertBars { get; set; } = new Dictionary<string, AlertBar>();


        internal static void AddAlertBar(string key, AlertBar alertBar)
        {
            if (!AlertBars.ContainsKey(key))
            {
                AlertBars.Add(key, alertBar);
            }
        }

        public static void Alert(string barKey, AlertInfo info)
        {
            if (AlertBars.TryGetValue(barKey, out var alertBar))
            {
                alertBar.Title = info.Title;
                alertBar.Message = info.Message;
                alertBar.IconKind = info.PackIconMaterialKind ?? PackIconMaterialKind.Information;
                alertBar.ActionButtonContent = info.ActionButtonContent;
                alertBar.Action = info.Action;
                alertBar.CanClose = info.CanClose;
                alertBar.IsClosedByAction = info.IsClosedByAction;
                var brightness = info.Background.GetBrightness();
                if (brightness > 0.3)
                {
                    alertBar.Foreground = Brushes.Black;
                }

                alertBar.BackgroundBrush = new SolidColorBrush(info.Background);
                alertBar.TransformStage(info.Duration.TotalSeconds);
            }
        }

        public static float GetBrightness(this System.Windows.Media.Color c) =>
            System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B).GetBrightness();

    }


    public static class AlertBuilder
    {
        public static AlertInfo Build()
        {
            return new AlertInfo();
        }

        public static AlertInfo WithTitle(this AlertInfo info, string title)
        {
            return info with {Title = title};
        }

        public static AlertInfo WithMessage(this AlertInfo info, string message)
        {
            return info with {Message = message};
        }


        public static AlertInfo WithIcon(this AlertInfo info, PackIconMaterialKind iconKind)
        {
            return info with {PackIconMaterialKind = iconKind};
        }

        public static AlertInfo WithAction(this AlertInfo info, string actionButtonContent, Action action, bool closesBar = false)
        {
            return info with {ActionButtonContent = actionButtonContent, Action = action, IsClosedByAction = closesBar};
        }


        public static AlertInfo WithBackground(this AlertInfo info, Color background)
        {
            return info with {Background = background};
        }

        public static AlertInfo WithDuration(this AlertInfo info, TimeSpan timeSpan)
        {
            return info with {Duration = timeSpan};
        }

        public static AlertInfo WithDurationInSeconds(this AlertInfo info, int seconds)
        {
            return info with {Duration = TimeSpan.FromSeconds(seconds)};
        }

        public static AlertInfo AsStickyInfo(this AlertInfo info)
        {
            return info with {CanClose = false};
        }


    }


    public record AlertInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public PackIconMaterialKind? PackIconMaterialKind { get; set; }

        public string ActionButtonContent { get; set; }

        public Action Action { get; set; }

        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        public bool CanClose { get; set; } = true;

        public Color Background { get; set; } = Colors.DarkBlue;
        public bool IsClosedByAction { get; set; } = false;
    }
}
