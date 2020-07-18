using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace HP.RDL.RdlHPMibTranslator
{
	/// <summary>
	/// Utilizes a Telerik stack layout in order to present a textbox and button on the 
	/// same horizontal for a menu item. There are two public events, One for the button
	/// and the other for the return key in the textbox. 
	/// 
	/// Using the return key in the textbox has some funny issues when the outer panels of the 
	/// menu item has focus, using the enter key will cause the panel to receive the event instead 
	/// of the textbox. Thus, the password will NOT be changed. Be very careful about implementing the
	/// textbox event. I'm leaving it just incase the desire to have out ways the issue.
	/// </summary>
	public class ApplyValueMenuItem:RadMenuItem
	{
		public event EventHandler ButtonClick;
		public event EventHandler TextboxKeyDown;
		
		private string _password = "admin";
		protected override Type ThemeEffectiveType
		{
			get
			{
				return typeof(RadMenuItem);
			}
		}
		/// <summary>
		/// Creates the stack layouts for the textbox and the button on the same
		/// horizontal.
		/// </summary>
		protected override void CreateChildElements()
		{
			base.CreateChildElements();
			StackLayoutElement stack = new StackLayoutElement();
			
			stack.Orientation = Orientation.Vertical;
			stack.StretchVertically = true;
			stack.StretchHorizontally = true;

			StackLayoutElement first = new StackLayoutElement();
			first.Orientation = Orientation.Horizontal;
			first.StretchHorizontally = true;
			first.StretchVertically = true;

			RadTextBoxElement textBox = new RadTextBoxElement();
			textBox.Text = TextboxText;
			textBox.MinSize = new Size(75, 0);
			textBox.KeyDown += textBox_KeyDown;

			RadButtonElement button = new RadButtonElement();
			button.Text = "OK";
			button.MaxSize = new Size(50, 20);
			button.Click += button_Click;

			first.Children.Add(textBox);
			first.Children.Add(button);

			stack.Children.Add(first);
			stack.Margin = new Padding(5, 0, 0, 0);
			stack.MinSize = new Size(120, 0);

			this.Children.Add(stack);
		}

		/// <summary>
		/// Handles the KeyDown event of the textBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if(TextboxKeyDown != null)
			{
				if (e.KeyCode.Equals(Keys.Return))
				{
					TextboxKeyDown(sender, e);
				}
			}
		}

		/// <summary>
		/// Gets or sets the textbox text.
		/// </summary>
		/// <value>
		/// The textbox text.
		/// </value>
		public string TextboxText 
		{ 
			get
			{				
				if (this.Children.Count== 4)
				{
					var item = this.Children.LastOrDefault();
					var stack = item.Children.FirstOrDefault();

					RadTextBoxElement txtbox = stack.Children.First() as RadTextBoxElement;
				
					_password = txtbox.Text;
				}

				return _password;
			}
			set
			{
				_password = value;
			}
		}
		/// <summary>
		/// Handles the Click event of the button control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		void button_Click(object sender, EventArgs e)
		{
			if (ButtonClick != null)
			{
				ButtonClick(sender, e);
			}

		}


	}
}
