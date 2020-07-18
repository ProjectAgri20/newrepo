using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using HP.ScalableTest.Framework.Runtime;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines an element contained in the session map which is a hierachical 
    /// structure of elements that map what is being deployed to run a test session.
    /// </summary>
    [DataContract]
    public class SessionMapElement
    {
        /// <summary>
        /// Occurs when a request to publish this status occurs.
        /// </summary>
        public event EventHandler OnUpdateStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionMapElement"/> class.
        /// </summary>
        /// <param name="name">The name of the status object.</param>
        /// <param name="type">The type.</param>
        public SessionMapElement(string name, ElementType type)
        {
            Name = name;
            ElementType = type;

            Id = Guid.NewGuid();
            ParentId = Guid.Empty;
            ResourceId = Guid.Empty;
            ElementSubtype = string.Empty;
            State = RuntimeState.Available;
            Message = "Available";
            ErrorDetail = string.Empty;
            Enabled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionMapElement"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="subType">The subtype.</param>
        public SessionMapElement(string name, ElementType type, string subType)
            : this(name, type)
        {
            ElementSubtype = subType;
        }

        /// <summary>
        /// Updates this instance from a source copy.
        /// </summary>
        /// <param name="source">The source.</param>
        public void UpdateFrom(SessionMapElement source)
        {
            Name = source.Name;
            State = source.State;
            Message = source.Message;
            ErrorDetail = source.ErrorDetail;
        }

        /// <summary>
        /// Determines if all elements are set to a given state or states
        /// </summary>
        /// <typeparam name="T">The type of element collection to be queried</typeparam>
        /// <param name="elements">The elements that are queried for the specified state.</param>
        /// <param name="states">The state or list of states that this elements must be set to.</param>
        /// <returns><c>true</c> if all of the elements are set to one of the provided states, <c>false</c> otherwise.</returns>
        public static bool AllElementsSetTo<T>(Collection<T> elements, params RuntimeState[] states) where T : ISessionMapElement
        {
            return elements.All(x => states.Contains(x.MapElement.State));
        }

        /// <summary>
        /// Determines if any elements are set to a given state or states
        /// </summary>
        /// <typeparam name="T">The type of element collection to be queried</typeparam>
        /// <param name="elements">The elements that are queried for the specified state.</param>
        /// <param name="states">The state or list of states that this elements must be set to.</param>
        /// <returns><c>true</c> if any of the elements are set to one of the provided states, <c>false</c> otherwise.</returns>
        public static bool AnyElementsSetTo<T>(Collection<T> elements, params RuntimeState[] states) where T : ISessionMapElement
        {
            return elements.Any(x => states.Contains(x.MapElement.State));
        }

        /// <summary>
        /// Gets all the elements that are set to the defined state.
        /// </summary>
        /// <typeparam name="T">The type of element collection to be queried</typeparam>
        /// <param name="state">The state value to look for within the elements.</param>
        /// <param name="elements">The elements that are queried for the specified state.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> GetElements<T>(RuntimeState state, Collection<T> elements) where T : ISessionMapElement
        {
            return elements.Where(x => x.MapElement.State == state);
        }

        /// <summary>
        /// Gets all the elements that are NOT set to the defined state.
        /// </summary>
        /// <typeparam name="T">The type of element collection to be queried</typeparam>
        /// <param name="state">The state value to look for within the elements.</param>
        /// <param name="elements">The elements that are queried for the specified state.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetElementsNotSetTo<T>(RuntimeState state, Collection<T> elements) where T : ISessionMapElement
        {
            return elements.Where(x => x.MapElement.State != state);
        }

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        [DataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets the element id for this status.
        /// </summary>
        /// <value>
        /// The <see cref="System.Guid"/> value for the element id.
        /// </value>
        [DataMember]
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>
        /// The <see cref="System.Guid"/> value for the parent id.
        /// </value>
        [DataMember]
        public Guid ParentId { get; set; }

        /// <summary>
        /// Gets or sets the resource id if applicable.
        /// </summary>
        /// <value>
        /// The <see cref="System.Guid"/> value for the resource id, empty by default.
        /// </value>
        [DataMember]
        public Guid ResourceId { get; set; }

        /// <summary>
        /// Gets the type of the element that is managed with this status object.
        /// </summary>
        /// <value>
        /// The type of the status.
        /// </value>
        [DataMember]
        public ElementType ElementType { get; private set; }

        /// <summary>
        /// Gets or sets the element subtype.
        /// </summary>
        /// <value>
        /// The element subtype.
        /// </value>
        [DataMember]
        public string ElementSubtype { get; set; }

        /// <summary>
        /// Gets or sets the state for this element.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [DataMember]
        public RuntimeState State { get; set; }

        /// <summary>
        /// Gets or sets the name of this element.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the status message for this element.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception detail if an error occurred.
        /// </summary>
        [DataMember]
        public string ErrorDetail { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this element is enabled and will publish status information.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Updates listening clients with the error level and specified exception.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        public void UpdateStatus(RuntimeState state, string message, Exception ex)
        {
            Message = message;
            ErrorDetail = ex.ToString();
            State = state;
            UpdateStatus();
        }

        /// <summary>
        /// Updates listening clients with the specified message.
        /// </summary>
        /// <param name="message">The message to send in the update.</param>
        public void UpdateStatus(string message)
        {
            Message = message;
            UpdateStatus();
        }

        /// <summary>
        /// Updates listening clients with the specified message and <see cref="RuntimeState"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="state">The state.</param>
        public void UpdateStatus(string message, RuntimeState state)
        {
            State = state;
            Message = message;
            UpdateStatus();
        }

        /// <summary>
        /// Updates listening clients with the specified <see cref="RuntimeState"/>.
        /// </summary>
        /// <param name="state">The state.</param>
        public void UpdateStatus(RuntimeState state)
        {
            State = state;
            UpdateStatus();
        }

        /// <summary>
        /// Updates listening clients with this element data.
        /// </summary>
        public void UpdateStatus()
        {
            //TraceFactory.Logger.Debug(" {0}: {1} -> {2}".FormatWith(ElementType, Name, State));

            if (Enabled)
            {
                if (OnUpdateStatus != null)
                {
                    OnUpdateStatus(this, null);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Id:       {0}".FormatWith(Id));
            builder.AppendLine("Parent:   {0}".FormatWith(ParentId));
            builder.AppendLine("Resource: {0}".FormatWith(ResourceId));
            builder.AppendLine("Name:     {0}".FormatWith(Name));
            builder.AppendLine("Message:  {0}".FormatWith(Message));
            builder.AppendLine("State:    {0}".FormatWith(State));
            builder.AppendLine("Type:     {0}".FormatWith(ElementType));
            builder.AppendLine("Subtype:  {0}".FormatWith(ElementSubtype));

            return builder.ToString();
        }
    }
}