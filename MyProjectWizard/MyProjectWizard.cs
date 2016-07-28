using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;

namespace MyProjectWizard
{
	public class WizardImplementation : IWizard
	{
		private UserInputForm _inputForm;

		public void Execute(object application,
						   int hwndOwner, ref object[] contextParams,
						   ref object[] customParams,
						   ref EnvDTE.wizardResult retval)
		{
		}

		
		// This method is called before opening any item that 
		// has the OpenInEditor attribute.
		public void BeforeOpeningFile(ProjectItem projectItem)
		{
		}

		public void ProjectFinishedGenerating(Project project)
		{
		}

		// This method is only called for item templates,
		// not for project templates.
		public void ProjectItemFinishedGenerating(ProjectItem
			projectItem)
		{
		}

		// This method is called after the project is created.
		public void RunFinished()
		{
		}

		public void RunStarted(object automationObject,
			Dictionary<string, string> replacementsDictionary,
			WizardRunKind runKind, object[] customParams)
		{
			try
			{
			// Display a form to the user. The form collects 
			// input for the custom message.
			_inputForm = new UserInputForm();
			_inputForm.ShowDialog();

				// Add custom parameters.
				replacementsDictionary.Add("$tenant$", UserInputForm.Tenant);
				replacementsDictionary.Add("$clientId$", UserInputForm.ClientId);
				replacementsDictionary.Add("$clientSecret$", UserInputForm.ClientSecret);
				replacementsDictionary.Add("$genericPolicy$", UserInputForm.GenericPolicy);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		// This method is only called for item templates,
		// not for project templates.
		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}
	}

	
	public partial class UserInputForm : Form
	{
		// B2C fields to be configured through the wizard
		public static string Tenant = "";
		public static string ClientId = "";
		public static string ClientSecret = "";
		public static string GenericPolicy = "";

		// Default B2C fields
		public static string DefaultTenant = "phpb2c";
		public static string DefaultClientId = "fd1ae53c-cf6c-4427-812e-c065109f6501";
		public static string DefaultClientSecret = "7J0)G!3QKfSc9}Wy";
		public static string DefaultGenericPolicy = "B2C_1_signin";

		// Text boxes where the B2C settings are entered
		private TextBox _tenantTextBox;
		private TextBox _clientIdTextBox;
		private TextBox _clientSecretTextBox;
		private TextBox _genericPolicyTextBox;
		
		private Button _defaultButton;
		private Button _submitButton;

		public UserInputForm()
		{
			
			this.Text = "B2C Authentication Settings";
			this.Size = new System.Drawing.Size(1260, 800);

			Label instructionsLabel = new Label();
			instructionsLabel.Location = new System.Drawing.Point(75, 75);
			instructionsLabel.Size = new System.Drawing.Size(1000, 150);
			instructionsLabel.Text = "Please fill out your Azure Active Directory B2C Settings ." +
			                         "For more information, please see " + 
									 "aka.ms/aadb2c.";
			this.Controls.Add(instructionsLabel);

			Label tenantLabel = new Label();
			tenantLabel.Name = "tenantLabel";
			tenantLabel.Location = new System.Drawing.Point(100, 240);
			tenantLabel.Size = new System.Drawing.Size(200, 80);
			tenantLabel.Text = "Tenant Name: ";
			this.Controls.Add(tenantLabel);

			_tenantTextBox = new TextBox();
			_tenantTextBox.Name = "tenantTextBox";
			_tenantTextBox.Location = new System.Drawing.Point(400, 240);
			_tenantTextBox.Size = new System.Drawing.Size(450, 35);
			this.Controls.Add(_tenantTextBox);

			Label clientIdLabel = new Label();
			clientIdLabel.Name = "clientIdLabel";
			clientIdLabel.Location = new System.Drawing.Point(100, 320);
			clientIdLabel.Size = new System.Drawing.Size(200, 80);
			clientIdLabel.Text = "Client ID: ";
			this.Controls.Add(clientIdLabel);

			_clientIdTextBox = new TextBox();
			_clientIdTextBox.Name = "clientIdTextBox";
			_clientIdTextBox.Location = new System.Drawing.Point(400, 320);
			_clientIdTextBox.Size = new System.Drawing.Size(450, 35);
			this.Controls.Add(_clientIdTextBox);

			Label clientSecretLabel = new Label();
			clientSecretLabel.Name = "clientSecretLabel";
			clientSecretLabel.Location = new System.Drawing.Point(100, 400);
			clientSecretLabel.Size = new System.Drawing.Size(200, 80);
			clientSecretLabel.Text = "Client Secret: ";
			this.Controls.Add(clientSecretLabel);

			_clientSecretTextBox = new TextBox();
			_clientSecretTextBox.Name = "clientSecretTextBox";
			_clientSecretTextBox.Location = new System.Drawing.Point(400, 400);
			_clientSecretTextBox.Size = new System.Drawing.Size(450, 35);
			this.Controls.Add(_clientSecretTextBox);
			
			Label genericPolicyLabel = new Label();
			genericPolicyLabel.Name = "genericPolicyLabelLabel";
			genericPolicyLabel.Location = new System.Drawing.Point(100, 480);
			genericPolicyLabel.Size = new System.Drawing.Size(200, 80);
			genericPolicyLabel.Text = "Login Policy: ";
			this.Controls.Add(genericPolicyLabel);

			_genericPolicyTextBox = new TextBox();
			_genericPolicyTextBox.Name = "genericPolicyTextBox";
			_genericPolicyTextBox.Location = new System.Drawing.Point(400, 480);
			_genericPolicyTextBox.Size = new System.Drawing.Size(450, 35);
			this.Controls.Add(_genericPolicyTextBox);

			_defaultButton = new Button();
			_defaultButton.Text = "Use Default Settings";
			_defaultButton.Location = new System.Drawing.Point(500, 610);
			_defaultButton.Size = new System.Drawing.Size(300, 40);
			_defaultButton.Click += defaultButton_Click;
			this.Controls.Add(_defaultButton);

			_submitButton = new Button();
			_submitButton.Text = "Submit";
			_submitButton.Location = new System.Drawing.Point(850, 610);
			_submitButton.Size = new System.Drawing.Size(300, 40);
			_submitButton.Click += submitButton_Click;
			this.Controls.Add(_submitButton);
		}

		private void submitButton_Click(object sender, EventArgs e)
		{
			Tenant = _tenantTextBox.Text;
			ClientId = _clientIdTextBox.Text;
			ClientSecret = _clientSecretTextBox.Text;
			GenericPolicy = _genericPolicyTextBox.Text;

			this.Close();
		}
		
		private void defaultButton_Click(object sender, EventArgs e)
		{
			_tenantTextBox.Text = DefaultTenant;
			_clientIdTextBox.Text = DefaultClientId;
			_clientSecretTextBox.Text = DefaultClientSecret;
			_genericPolicyTextBox.Text = DefaultGenericPolicy;
		}
	}
}