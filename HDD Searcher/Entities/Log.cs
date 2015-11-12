namespace HDD_Searcher
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// class for logs
    /// </summary>
    public class Log
    {
        #region static fields
        /// <summary>
        /// The log collections
        /// </summary>
        public static ConcurrentBag<Log> LogCollections = new ConcurrentBag<Log>();

        /// <summary>
        /// The log number
        /// </summary>
        private static int logNumbers = 0;
        #endregion

        #region fields
        /// <summary>
        /// The identifier
        /// </summary>
        private int id;

        /// <summary>
        /// The message
        /// </summary>
        private string message;

        /// <summary>
        /// The log time
        /// </summary>
        private DateTime logTime;

        /// <summary>
        /// The stopwatch
        /// </summary>
        private System.Diagnostics.Stopwatch stopwatch;

        /// <summary>
        /// The have tail
        /// </summary>
        private bool haveTail;

        /// <summary>
        /// The label
        /// </summary>
        private string label;
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="haveTail">if set to <c>true</c> [have tail].</param>
        /// <param name="label">The label.</param>
        public Log(string message, bool haveTail = false, string label = null)
        {
            this.id = ++logNumbers;
            this.message = message;
            this.logTime = DateTime.Now;
            this.haveTail = haveTail;

            if (label == null)
            {
                this.label = this.id.ToString();
            }
            else
            {
                this.label = label;
            }

            LogCollections.Add(this);

            if (haveTail)
            {
                this.stopwatch = new System.Diagnostics.Stopwatch();
                this.stopwatch.Start();
            }
        }
        #endregion

        #region properties
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
            }
        }

        /// <summary>
        /// Gets or sets the log time.
        /// </summary>
        /// <value>
        /// The log time.
        /// </value>
        public DateTime LogTime
        {
            get
            {
                return this.logTime;
            }

            set
            {
                this.logTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label
        {
            get
            {
                return this.label;
            }

            set
            {
                this.label = value;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Ends the of log.
        /// </summary>
        /// <param name="tailID">The tail identifier.</param>
        /// <returns>time between two logs</returns>
        public TimeSpan EndOfLog(int tailID)
        {
            Log tail = LogCollections.Where(x => x.Id == tailID).First();
            return this.EndOfLog(tail);
        }

        /// <summary>
        /// Ends the of log.
        /// </summary>
        /// <param name="tailLabel">The tail label.</param>
        /// <returns>time between two logs</returns>
        public TimeSpan EndOfLog(string tailLabel)
        {
            Log tail = LogCollections.Where(x => x.label == tailLabel).FirstOrDefault();
            tail.label = tail.id.ToString();

            return this.EndOfLog(tail);
        }

        /// <summary>
        /// Ends the of log.
        /// </summary>
        /// <param name="tail">The tail.</param>
        /// <returns>time between two logs</returns>
        public TimeSpan EndOfLog(Log tail)
        {
            System.Diagnostics.Stopwatch st;

            if (tail != null)
            {
                if (tail.haveTail)
                {
                    st = tail.stopwatch;
                    st.Stop();
                    string message = string.Format("time between {0} and {1} is {2} ms", tail.id, this.id, st.ElapsedMilliseconds.ToString());
                    Log endLog = new Log(message);
                    return st.Elapsed;
                }
            }

            return new TimeSpan();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}- {1} ({2})", this.id.ToString(), this.message, this.logTime.ToShortTimeString());
        }
        #endregion
    }
}
