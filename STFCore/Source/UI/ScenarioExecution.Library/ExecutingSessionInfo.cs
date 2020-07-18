using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Dispatcher;

namespace HP.ScalableTest.UI.SessionExecution
{
    public class ExecutingSessionInfo : INotifyPropertyChanged
    {
        // Register a default handler to avoid having to test for null
        // http://www.jaylee.org/post/2010/01/02/WinForms-DataBinding-and-Updates-from-multiple-Threads.aspx
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Class that holds information about an executing session
        /// </summary>
        public ExecutingSessionInfo()
        {
            State = SessionState.Available;
            MapElement = null;
            StartupTransition = SessionStartupTransition.None;
        }

        public ExecutingSessionInfo(SessionTicket ticket)
            : this()
        {
            SessionId = ticket.SessionId;
            UpdateFromTicket(ticket);
        }

        public void UpdateFromTicket(SessionTicket ticket)
        {
            Name = ticket.SessionName;
            if (ticket.SessionOwner != null)
            {
                Owner = ticket.SessionOwner.UserName;
            }
        }

        private string _sessionId;
        public string SessionId
        {
            get
            {
                return _sessionId;
            }
            set
            {
                _sessionId = (value ?? string.Empty).ToUpper();
                OnPropertyChanged(() => SessionId);
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        private string _owner;
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                OnPropertyChanged(() => Owner);
            }
        }

        private SessionState _state;
        public SessionState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged(() => State);
            }
        }

        private SessionMapElement _mapElement;
        public SessionMapElement MapElement
        {
            get
            {
                return _mapElement;
            }
            set
            {
                _mapElement = value;
                OnPropertyChanged(() => MapElement);
            }
        }

        private SessionStartupTransition _startupTransition;
        public SessionStartupTransition StartupTransition
        {
            get
            {
                return _startupTransition;
            }
            set
            {
                _startupTransition = value;
                OnPropertyChanged(() => StartupTransition);
            }
        }

        public string Dispatcher { get; set; }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged(() => StartDate);
            }
        }

        private DateTime? _estimatedEndDate;
        public DateTime? EstimatedEndDate
        {
            get
            {
                return _estimatedEndDate;
            }
            set
            {
                _estimatedEndDate = value;
                OnPropertyChanged(() => EstimatedEndDate);
            }
        }

        private DateTime? _shutdownDate;
        public DateTime? ShutDownDate
        {
            get
            {
                return _shutdownDate;
            }
            set
            {
                _shutdownDate = value;
                OnPropertyChanged(() => ShutDownDate);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ExecutingSessionInfo p = (ExecutingSessionInfo)obj;
            return (SessionId == p.SessionId);
        }

        /// <summary>
        /// Raise the PropertyChanged event for the property specified in the expression
        /// Example - below will raise a PropertyChanged event on the Foo property
        ///     OnPropertyChanged(() => Foo);
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="expr">The expression to get the property from</param>
        private void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            var body = expr.Body as MemberExpression;

            if (body != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(body.Member.Name));
            }
        }

        /// <summary>
        /// Determines whether this session can be ended.
        /// </summary>
        /// <returns><c>true</c> if this session can end; otherwise, <c>false</c>.</returns>
        public bool CanEnd()
        {
            bool result = true;

            // Check if session has already been shut down
            if (State == SessionState.RunComplete && this.ShutDownDate.HasValue)
            {
                result = false;
            }
            else
            {
                result =
                    StartupTransition != SessionStartupTransition.ReadyToStart
                    && !IsInState(
                                SessionState.Unavailable
                                , SessionState.Canceled
                                , SessionState.ShuttingDown
                                , SessionState.ShutdownComplete
                                , SessionState.Available
                            );
            }
            return result;
        }

        public bool IsInState(params SessionState[] states) => states.Contains(State);
    }
}