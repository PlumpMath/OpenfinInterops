using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fin = Openfin.Desktop;
using Openfin.WinForm;
using Newtonsoft.Json.Linq;
namespace OpenFinInteroperatability
{
    public partial class Form1 : Form
    {
        Fin.Runtime _runtime;
        public EmbeddedView embeddedview;
        public Form1()
        {
            InitializeComponent();

            // Connect to the runtime
            var runtimeOptions = new Fin.RuntimeOptions
            {
                Version = "beta",
                EnableRemoteDevTools = true,
                RemoteDevToolsPort = 9090
            };
            _runtime = Fin.Runtime.GetRuntimeInstance(runtimeOptions);
            _runtime.Connect(RuntimeConnectedCallback);


            var appOptions = new Fin.ApplicationOptions("webproj", "webproj", "file:///C:/OpenfinPOC/OpenFinInteroperatability/OpenFinInteroperatability/webproj/public/index.html");

            embeddedview = new EmbeddedView();
            panel1.Controls.Add(embeddedview);
            embeddedview.Initialize(runtimeOptions, appOptions);

            embeddedview.OnReady += embeddedView_OnReady;
        }

        private void embeddedView_OnReady(object sender, EventArgs e)
        {
            //Dispatcher.Invoke(new Action(() =>
            //{
            //    showDeveloperToolsItem.IsEnabled = true;
            //}));


            //Any Interactions with the UI must be done in the right thread.
            Openfin.WinForm.Utils.InvokeOnUiThreadIfRequired(this, () =>
                label1.Text = "EmbeddedView is ready");

            //subscribe to chart-click messages from the chartEmbeddedView
            _runtime.InterApplicationBus.subscribe(embeddedview.OpenfinApplication.getUuid(), "button-click", (senderUuid, topic, data) =>
            {
                Utils.InvokeOnUiThreadIfRequired(this, () =>
                {
                    label2.Text = data.ToString();
                });
            });
        }

        private void RuntimeConnectedCallback()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _runtime.InterApplicationBus.publish("winform-click", new JArray("This is winform interacting with HTML"));
        }

    }
}
