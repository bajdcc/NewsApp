using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Presentation.Behaviours
{
    public class MultiClosingBehavior : ClosingBehavior
    {
        public static readonly DependencyProperty BackupStoryboardProperty =
            DependencyProperty.Register("BackupStoryboard",
                typeof(Storyboard),
                typeof(ClosingBehavior),
                new PropertyMetadata(default(Storyboard)));

        public static readonly DependencyProperty UseBackupProperty =
            DependencyProperty.Register("UseBackup",
                typeof(bool),
                typeof(ClosingBehavior),
                new PropertyMetadata(default(bool)));

        public Storyboard BackupStoryboard
        {
            get { return (Storyboard)GetValue(BackupStoryboardProperty); }
            set { SetValue(BackupStoryboardProperty, value); }
        }

        public bool UseBackup
        {
            get { return (bool)GetValue(UseBackupProperty); }
            set { SetValue(UseBackupProperty, value); }
        }

        protected override void beginStoryBoard()
        {
            if (UseBackup)
            {
                BackupStoryboard.Completed += (o, a) => AssociatedObject.Close();
                BackupStoryboard.Begin(AssociatedObject);
            }
            else
            {
                base.beginStoryBoard();
            }
        }
    }
}
