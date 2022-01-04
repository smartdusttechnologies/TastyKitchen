using System.Collections.Generic;

namespace Neubel.Wow.Win.Authentication.Common
{
	public class RequestResult<T> : ResultBase
	{
		#region Public Properties
		/// <summary>
		/// Gets or sets the requested object.
		/// </summary>
		/// <value>The requested object.</value>
		public T RequestedObject { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is successful.
		/// </summary>
		public bool IsSuccessful
		{
			get
			{
				bool successful = !HasFailureMessages();

				if (successful && RequestedObject == null)
				{
					successful = false;
				}

				return successful;
			}
        }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestResult&lt;T&gt;"/> class.
		/// </summary>
		public RequestResult()
        {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestResult&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="requestedObject">The requested object.</param>
		public RequestResult(T requestedObject) : this()
		{
			RequestedObject = requestedObject;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestResult&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="validationMessages">The validation messages.</param>
		public RequestResult(IList<ValidationMessage> validationMessages)
			: this()
		{
			ValidationMessages = validationMessages;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestResult&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="requestedObject">The requested object.</param>
		/// <param name="validationMessages">The validation messages.</param>
		public RequestResult(T requestedObject, IList<ValidationMessage> validationMessages)
			: this()
		{
			RequestedObject = requestedObject;
			ValidationMessages = validationMessages;
		}

		#endregion
	}
}
