using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.NetworkEmulation
{
    [ToolboxItem(false)]
    public partial class NestConfigurationControl : UserControl, IPluginConfigurationControl
    {
        private NestActivityData _activityData;
        private string _xmlString;
        private Dictionary<string, string> _networkProfiles = new Dictionary<string, string>();
        private const string NoEmulationCaptionString = "No Emulation";

        public NestConfigurationControl()
        {
            InitializeComponent();

            radioButtonLatencyFixed.Checked = radioButtonLatencyNormal.Checked = radioButtonLatencyUniform.Checked = false;
            radioButtonLatencyNo.Checked = true;
            tabPage1.Enabled = tabPage2.Enabled = tabPage3.Enabled = tabPage4.Enabled = false;
            if (ConfigurationChanged != null)
            {
                comboBox_networkprofiles.SelectedIndexChanged += (s, e) => ConfigurationChanged(s, e);
            }
        }



        #region IpluginConfigurationControl

        public event EventHandler ConfigurationChanged;

        public void Initialize(PluginEnvironment environment)
        {
            _activityData = new NestActivityData();
            InitializeUi();
        }

        public void Initialize(PluginConfigurationData configuration, PluginEnvironment environment)
        {
            _activityData = configuration.GetMetadata<NestActivityData>();
            _xmlString = _activityData.EmulationString;
            InitializeUi();
        }

        public PluginConfigurationData GetConfiguration()
        {
            CreateEmulation();
            _activityData.EmulationProfileName = comboBox_networkprofiles.Text;
            _activityData.EmulationString = _xmlString;
            return new PluginConfigurationData(_activityData, "1.0");
        }

        public PluginValidationResult ValidateConfiguration()
        {
            return new PluginValidationResult(true);
        }

        #endregion IpluginConfigurationControl

        #region UIOperations

        private void radioButtonLatencyFixed_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLatencyFixed.Enabled = radioButtonLatencyFixed.Checked;
        }

        private void radioButtonLatencyUniform_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLatencyUniformMax.Enabled = textBoxLatencyUniformMin.Enabled = radioButtonLatencyUniform.Checked;
        }

        private void radioButtonLatencyNormal_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLatencyNormalAvg.Enabled = textBoxLatencyNormalDev.Enabled = radioButtonLatencyNormal.Checked;
        }

        private void radioButtonLossPeriodic_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLossPeriodic.Enabled = radioButtonLossPeriodic.Checked;
        }

        private void radioButtonLossRandom_CheckedChanged(object sender, EventArgs e)
        {
            textBoxLossRate.Enabled = radioButtonLossRandom.Checked;
        }

        private void radioButtonErrorRandom_CheckedChanged(object sender, EventArgs e)
        {
            textBoxErrorRate.Enabled = radioButtonErrorRandom.Checked;
        }

        private void InitializeUi()
        {
            _networkProfiles.Clear();
            _networkProfiles.Add(NoEmulationCaptionString, string.Empty);
            var resources = Profiles.ResourceProfiles.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry profile in resources)
            {
                _networkProfiles.Add(profile.Key.ToString(), System.Text.Encoding.UTF8.GetString((byte[])profile.Value));
            }
            _networkProfiles.Add("Custom", string.Empty);
            comboBox_networkprofiles.DataSource = new BindingSource(_networkProfiles, null);
            comboBox_networkprofiles.DisplayMember = "Key";
            comboBox_networkprofiles.ValueMember = "Value";
            this.comboBox_networkprofiles.SelectedIndexChanged += new System.EventHandler(this.comboBox_networkprofiles_SelectedIndexChanged);
            if (string.IsNullOrEmpty(_activityData.EmulationString))
            {
                comboBox_networkprofiles.SelectedIndex = 0;
            }
            else
            {
                UpdateProfile();
            }
        }

        private void UpdateUi(string emulationString)
        {
            var xDoc = XDocument.Parse(emulationString);
            XNamespace ns = xDoc.Root.Name.Namespace;
            var emulationProfileElement = xDoc.Element(ns + "NetworkEmulationProfile");
            var emulationElement = xDoc.Element("Emulation");
            if (emulationProfileElement != null)
            {
                emulationElement = emulationProfileElement.Element(ns + "Emulation");
            }

            var linkRulesElements = emulationElement.Element(ns + "VirtualChannel").Element(ns + "VirtualLink").Elements(ns + "LinkRule");

            //first is always upstream
            var upstreamElement = linkRulesElements.ElementAt(0);

            var upBandwidthElement = upstreamElement.Element(ns + "Bandwidth");
            if (upBandwidthElement != null)
            {
                textBoxBWUpStream.Text = upBandwidthElement.Element(ns + "Speed").Value;
                comboBoxBWUpStream.SelectedIndex = (upBandwidthElement.Element(ns + "Speed").FirstAttribute.Value == "kbps") ? 1 : 0;
            }

            //get latency element
            var upLatencyElement = upstreamElement.Element(ns + "Latency");
            if (upLatencyElement != null)
            {
                var fixedLatency = upLatencyElement.Element(ns + "Fixed");
                if (fixedLatency != null)
                {
                    textBoxLatencyFixed.Text = fixedLatency.Element(ns + "Time").Value;
                    radioButtonLatencyFixed.Checked = true;
                }

                var uniformLatency = upLatencyElement.Element(ns + "Uniform");
                if (uniformLatency != null)
                {
                    textBoxLatencyUniformMin.Text = uniformLatency.Element(ns + "Min").Value;
                    textBoxLatencyUniformMax.Text = uniformLatency.Element(ns + "Max").Value;
                    radioButtonLatencyUniform.Checked = true;
                }

                var normalLatency = upLatencyElement.Element(ns + "Normal");
                if (normalLatency != null)
                {
                    textBoxLatencyNormalAvg.Text = normalLatency.Element(ns + "Average").Value;
                    textBoxLatencyNormalDev.Text = normalLatency.Element(ns + "Deviation").Value;
                    radioButtonLatencyNormal.Checked = true;
                }
            }

            // get the loss element
            var upLossElement = upstreamElement.Element(ns + "Loss");
            if (upLossElement != null)
            {
                var periodicLoss = upLossElement.Element(ns + "Periodic");
                if (periodicLoss != null)
                {
                    textBoxLossPeriodic.Text = periodicLoss.Element(ns + "PerPackets").Value;
                    radioButtonLossPeriodic.Checked = true;
                }

                var randomLoss = upLossElement.Element(ns + "Random");
                if (randomLoss != null)
                {
                    textBoxLossRate.Text = (Convert.ToDouble(randomLoss.Element(ns + "Rate").Value, CultureInfo.InvariantCulture) * 100).ToString(CultureInfo.InvariantCulture);
                    radioButtonLossRandom.Checked = true;
                }
            }

            var downstreamElement = linkRulesElements.ElementAt(1);

            //get bandwidth
            var downBandwidthElement = downstreamElement.Element(ns + "Bandwidth");
            if (downBandwidthElement != null)
            {
                textBoxBWDownStream.Text = downBandwidthElement.Element(ns + "Speed").Value;
                comboBoxBWDownStream.SelectedIndex = (downBandwidthElement.Element(ns + "Speed").FirstAttribute.Value == "kbps") ? 1 : 0;
            }
        }

        private void UpdateProfile()
        {
            if (!string.IsNullOrEmpty(_activityData.EmulationProfileName) && _activityData.EmulationProfileName != "Custom")
            {
                string profilevalue;
                _networkProfiles.TryGetValue(_activityData.EmulationProfileName, out profilevalue);
                var profileEntry = new Dictionary<string, string> { { _activityData.EmulationProfileName, profilevalue } };
                tabPage1.Enabled = tabPage2.Enabled = tabPage3.Enabled = tabPage4.Enabled = false;
                comboBox_networkprofiles.SelectedIndex = comboBox_networkprofiles.Items.IndexOf(profileEntry.ElementAt(0));
            }
            else
            {
                _networkProfiles.Remove("Custom");
                _networkProfiles.Add("Custom", _activityData.EmulationString);
                tabPage1.Enabled = tabPage2.Enabled = tabPage3.Enabled = tabPage4.Enabled = true;
                comboBox_networkprofiles.SelectedIndex = _networkProfiles.Count - 1;
            }
        }

        private void comboBox_networkprofiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string emulationData;
            _networkProfiles.TryGetValue(comboBox_networkprofiles.Text, out emulationData);

            if (comboBox_networkprofiles.Text != "Custom")
            {
                tabPage1.Enabled = tabPage2.Enabled = tabPage3.Enabled = tabPage4.Enabled = false;
                if (comboBox_networkprofiles.Text == NoEmulationCaptionString)
                {
                    textBoxBWDownStream.Text = textBoxBWUpStream.Text = string.Empty;
                    comboBoxBWUpStream.SelectedIndex = comboBoxBWDownStream.SelectedIndex = -1;
                    radioButtonLatencyNo.Checked = radioButtonErrorNo.Checked = radioButtonLossNo.Checked = true;
                }
            }
            else
            {
                tabPage1.Enabled = tabPage2.Enabled = tabPage3.Enabled = tabPage4.Enabled = true;
            }

            if (!string.IsNullOrEmpty(emulationData))
            {
                UpdateUi(emulationData);
            }
        }

        #endregion UIOperations

        private void CreateEmulation()
        {
            if (comboBox_networkprofiles.Text == NoEmulationCaptionString)
            {
                _xmlString = string.Empty;
                return;
            }

            XmlDocument emulationTree = new XmlDocument();

            var emulationNode = emulationTree.CreateElement("Emulation");

            var virtualChannelNode = emulationTree.CreateElement("VirtualChannel");
            var vchannelAttrib1 = emulationTree.CreateAttribute("name");
            vchannelAttrib1.Value = "VirtualChannel 1";
            var vchannelAttrib2 = emulationTree.CreateAttribute("DispatchType");
            vchannelAttrib2.Value = "packet";
            virtualChannelNode.Attributes.Append(vchannelAttrib1);
            virtualChannelNode.Attributes.Append(vchannelAttrib2);

            var filterElement = emulationTree.CreateElement("FilterList");
            virtualChannelNode.AppendChild(filterElement);

            var virtualLink1 = emulationTree.CreateElement("VirtualLink");
            var vLink1Attrib1 = emulationTree.CreateAttribute("name");
            vLink1Attrib1.Value = "LINK_1";
            var vLink1Attrib2 = emulationTree.CreateAttribute("instances");
            vLink1Attrib2.Value = "1";
            virtualLink1.Attributes.Append(vLink1Attrib1);
            virtualLink1.Attributes.Append(vLink1Attrib2);

            XmlElement bandwidthQueueManagement;
            XmlElement latency;
            XmlElement loss;

            var linkRuleUp = CreateLinkRule(emulationTree, out bandwidthQueueManagement, out latency, out loss);

            virtualLink1.AppendChild(linkRuleUp);

            var linkRuleDown = emulationTree.CreateElement("LinkRule");
            var dirAttrib2 = emulationTree.CreateAttribute("dir");
            dirAttrib2.Value = "downstream";
            linkRuleDown.Attributes.Append(dirAttrib2);

            if (!string.IsNullOrEmpty(textBoxBWDownStream.Text))
            {
                var bandwidthDown = emulationTree.CreateElement("Bandwidth");
                var bandwidthSpeedDown = emulationTree.CreateElement("Speed");
                var bandwidthAttrib2 = emulationTree.CreateAttribute("unit");
                bandwidthAttrib2.Value = (comboBoxBWDownStream.SelectedIndex == 0) ? "mbps" : "kbps";
                bandwidthSpeedDown.Attributes.Append(bandwidthAttrib2);
                bandwidthSpeedDown.InnerText = textBoxBWDownStream.Text;
                bandwidthDown.AppendChild(bandwidthSpeedDown);
                bandwidthDown.AppendChild(bandwidthQueueManagement.Clone());
                linkRuleDown.AppendChild(bandwidthDown);
            }

            if (latency.ChildNodes.Count > 0)
            {
                linkRuleDown.AppendChild(latency.Clone());
            }

            if (loss.ChildNodes.Count > 0)
            {
                linkRuleDown.AppendChild(loss.Clone());
            }

            virtualLink1.AppendChild(linkRuleDown);

            virtualChannelNode.AppendChild(virtualLink1);

            emulationNode.AppendChild(virtualChannelNode);

            emulationTree.CreateXmlDeclaration("1.0", "UTF-8", string.Empty);
            emulationTree.AppendChild(emulationNode);

            _xmlString = emulationTree.OuterXml;
        }

        private XmlElement CreateLinkRule(XmlDocument emulationTree, out XmlElement bandwidthQueueManagement, out XmlElement latency,
            out XmlElement loss)
        {
            var linkRuleUp = emulationTree.CreateElement("LinkRule");
            var dirAttrib1 = emulationTree.CreateAttribute("dir");
            dirAttrib1.Value = "upstream";
            linkRuleUp.Attributes.Append(dirAttrib1);

            bandwidthQueueManagement = emulationTree.CreateElement("QueueManagement");
            var bandwidthNormalQueue = emulationTree.CreateElement("NormalQueue");
            var bandwidthQueueSize = emulationTree.CreateElement("Size");
            bandwidthQueueSize.InnerText = "100";

            var bandwidthQueueMode = emulationTree.CreateElement("QueueMode");
            bandwidthQueueMode.InnerText = "packet";

            var bandwidthDropType = emulationTree.CreateElement("DropType");
            bandwidthDropType.InnerText = "Droptail";

            bandwidthNormalQueue.AppendChild(bandwidthQueueSize);
            bandwidthNormalQueue.AppendChild(bandwidthQueueMode);
            bandwidthNormalQueue.AppendChild(bandwidthDropType);

            bandwidthQueueManagement.AppendChild(bandwidthNormalQueue);

            var bandwidthAttrib = emulationTree.CreateAttribute("unit");
            bandwidthAttrib.Value = (comboBoxBWUpStream.SelectedIndex == 0) ? "mbps" : "kbps";

            if (!string.IsNullOrEmpty(textBoxBWUpStream.Text))
            {
                var bandwidthUp = emulationTree.CreateElement("Bandwidth");
                var bandwidthSpeed = emulationTree.CreateElement("Speed");

                bandwidthSpeed.Attributes.Append(bandwidthAttrib);
                bandwidthSpeed.InnerText = textBoxBWUpStream.Text;
                bandwidthUp.AppendChild(bandwidthSpeed);

                bandwidthUp.AppendChild(bandwidthQueueManagement);
                linkRuleUp.AppendChild(bandwidthUp);
            }

            latency = CreateLatencyElement(emulationTree);

            loss = CreateLossElement(emulationTree);

            if (latency.ChildNodes.Count > 0)
            {
                linkRuleUp.AppendChild(latency);
            }

            if (loss.ChildNodes.Count > 0)
            {
                linkRuleUp.AppendChild(loss);
            }
            return linkRuleUp;
        }

        private XmlElement CreateLossElement(XmlDocument emulationTree)
        {
            var loss = emulationTree.CreateElement("Loss");
            if (radioButtonLossPeriodic.Checked == true)
            {
                var periodic = emulationTree.CreateElement("Periodic");
                var perPeriod = emulationTree.CreateElement("PerPackets");
                perPeriod.InnerText = textBoxLossPeriodic.Text;
                periodic.AppendChild(perPeriod);
                loss.AppendChild(periodic);
            }
            else if (radioButtonLossRandom.Checked == true)
            {
                var random = emulationTree.CreateElement("Random");
                var rate = emulationTree.CreateElement("Rate");
                rate.InnerText =
                    (Convert.ToDouble(textBoxLossRate.Text, CultureInfo.InvariantCulture) / 100.0).ToString(
                        CultureInfo.InvariantCulture);
                random.AppendChild(rate);
                loss.AppendChild(random);
            }

            return loss;
        }

        private XmlElement CreateLatencyElement(XmlDocument emulationTree)
        {
            var latency = emulationTree.CreateElement("Latency");
            if (radioButtonLatencyNormal.Checked == true)
            {
                var normal = emulationTree.CreateElement("Normal");

                var xAttrib = emulationTree.CreateAttribute("unit");
                xAttrib.Value = "msec";

                var average = emulationTree.CreateElement("Average");
                average.Attributes.Append(xAttrib);
                average.InnerText = textBoxLatencyNormalAvg.Text;

                normal.AppendChild(average);

                var xAttrib2 = emulationTree.CreateAttribute("unit");
                xAttrib2.Value = "msec";
                var deviation = emulationTree.CreateElement("Deviation");
                deviation.Attributes.Append(xAttrib2);
                deviation.InnerText = textBoxLatencyNormalDev.Text;

                normal.AppendChild(deviation);
                latency.AppendChild(normal);
            }
            else if (radioButtonLatencyFixed.Checked == true)
            {
                var fix = emulationTree.CreateElement("Fixed");
                var time = emulationTree.CreateElement("Time");

                var xAttrib = emulationTree.CreateAttribute("unit");
                xAttrib.Value = "msec";

                time.Attributes.Append(xAttrib);

                time.InnerText = textBoxLatencyFixed.Text;
                fix.AppendChild(time);
                latency.AppendChild(fix);
            }
            else if (radioButtonLatencyUniform.Checked == true)
            {
                var uniform = emulationTree.CreateElement("Uniform");
                var min = emulationTree.CreateElement("Min");

                var xAttrib = emulationTree.CreateAttribute("unit");
                xAttrib.Value = "msec";

                min.Attributes.Append(xAttrib);
                min.InnerText = textBoxLatencyUniformMin.Text;

                uniform.AppendChild(min);
                var xAttrib2 = emulationTree.CreateAttribute("unit");
                xAttrib2.Value = "msec";
                var max = emulationTree.CreateElement("Max");
                max.Attributes.Append(xAttrib2);
                max.InnerText = textBoxLatencyUniformMax.Text;

                uniform.AppendChild(max);
                latency.AppendChild(uniform);
            }

            return latency;
        }
    }
}