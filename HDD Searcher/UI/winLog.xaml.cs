namespace HDD_Searcher
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for winLog.xaml
    /// </summary>
    public partial class winLog : Window
    {
        /// <summary>
        /// The update timer
        /// </summary>
        private System.Timers.Timer updateTimer = new System.Timers.Timer();

        /// <summary>
        /// The logs collection
        /// </summary>
        private ObservableCollection<Log> logCollection = new ObservableCollection<Log>();

        /// <summary>
        /// Initializes a new instance of the <see cref="winLog"/> class.
        /// </summary>
        public winLog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the logs source.
        /// </summary>
        /// <value>
        /// The logs source.
        /// </value>
        public IEnumerable<Log> LogsSource
        {
            get;
            set;
        }

        /// <summary>
        /// Handles the Loaded event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lstLog.ItemsSource = this.logCollection;
            this.updateTimer.Interval = 1000;
            this.updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.Update);
            this.updateTimer.Start();
        }

        /// <summary>
        /// Updates the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void Update(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var item in LogsSource)
                {
                    if (!logCollection.Contains(item))
                    {
                        logCollection.Add(item);
                    }
                }

                lstLog.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Id", System.ComponentModel.ListSortDirection.Ascending));
                lstLog.ScrollIntoView(lstLog.Items[lstLog.Items.Count - 1]);
            });
        }
    }
}
