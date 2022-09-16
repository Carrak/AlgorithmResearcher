using Kursach.Model.Algorithms;
using Kursach.Model.Steps;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kursach
{
    public partial class ViewForm : Form
    {
        public AlgorithmResearcherController Controller { get; }
        public AlgorithmResearcherModel Model { get; }

        private const int MAX_BOX_HEIGHT = 522;

        private readonly Timer _autoPlayTimer = new Timer() { Interval = 120 };
        private readonly List<SwapAnimation> _activeAnimations = new List<SwapAnimation>();

        private int currentStep = 0;

        public ViewForm()
        {
            InitializeComponent();
            Model = new AlgorithmResearcherModel();
            Controller = new AlgorithmResearcherController();
            Controller.Model = Model;
            Model.ModelUpdated += ModelUpdated;
            Controller.Model.RandomizeArray(false);
            DoubleBuffered = true;
            (Controls["algorithmComboBox"] as ComboBox).SelectedIndex = 0;
            _autoPlayTimer.Tick += (sndr, args) => TimerTick();
        }

        private void ModelUpdated(AlgorithmResearcherModel model)
        {
            var array = model.DisplayedArray;
            if (model.Algorithm != null)
            {
                Controls["logBox"].Text = string.Join("\n", model.Algorithm.Steps.Select((x, i) => $"{i + 1}. {x.Description}"));
                Controls["stepsLabel"].Text = $"1 / {model.Algorithm.Steps.Count}";
                Highlight(0);
            }

            UpdateArray(array);
        }

        private void UpdateArray(int[] array)
        {
            int dh = Height - MinimumSize.Height;
            for (int i = 0; i < array.Length; i++)
            {
                int value = Map(array[i], 0, 100, 0, MAX_BOX_HEIGHT);
                var box = Controls[$"pictureBox{i}"];
                var label = Controls[$"indexLabel{i}"];

                int dy = MAX_BOX_HEIGHT - value;

                box.Height = value;
                box.Location = new Point(box.Location.X, 36 + dh + dy);

                label.Text = array[i].ToString();
                label.Location = new Point(label.Location.X, 16 + dh + dy);
            }
        }

        private int Map(float value, float start, float end, float newStart, float newEnd) =>
            (int)((value - start) / (end - start) * (newEnd - newStart) + newStart);

        private void ResetBoxColours()
        {
            for (int i = 0; i < AlgorithmResearcherModel.ARRAY_LENGTH; i++)
                Controls[$"pictureBox{i}"].BackColor = Color.White;
        }

        private void FullReset()
        {
            ResetBoxColours();
            _autoPlayTimer.Stop();
            currentStep = 0;
            Controls["logBox"].Text = "Steps will be displayed here";
            Controls["stepsLabel"].Text = "0 / 0";
        }

        private void TogglePaging(bool enable)
        {
            Controls["previousStep"].Enabled = enable;
            Controls["nextStep"].Enabled = enable;
        }

        private void ToggleAutoPlay(bool enable)
        {
            Controls["autoplayButton"].Enabled = enable;
        }

        private void Highlight(int index)
        {
            ResetBoxColours();

            foreach (var highlight in Model.Algorithm.Steps[index].Highlights)
                Controls[$"pictureBox{highlight.Index}"].BackColor = highlight.Color;

            var rtb = Controls["logBox"] as RichTextBox;
            int sum = 0;
            for (int i = 0; i < index; i++)
                sum += rtb.Lines[i].Length + 1;
            var text = rtb.Lines[index];
            rtb.Select(sum, text.Length);
        }

        private void ApplyStep(int index, int fromIndex = -1)
        {
            while (_activeAnimations.Count > 0)
                _activeAnimations[0].FinalizeAnimation();

            Controls["stepsLabel"].Text = $"{index + 1} / {Model.Algorithm.Steps.Count}";

            if (fromIndex > index && Model.Algorithm.Steps[fromIndex] is SwapAlgorithmStep currentSas)
                PlaySwapAnimation(currentSas, true);

            if (Model.Algorithm.Steps[index] is SwapAlgorithmStep sas)
            {
                if (fromIndex > index)
                    PlaySwapAnimation(sas, true);
                PlaySwapAnimation(sas, false);
            }

            Highlight(index);
        }

        private void PlaySwapAnimation(SwapAlgorithmStep sas, bool instant)
        {
            var box1 = Controls[$"pictureBox{sas.SwapIndex1}"];
            var box2 = Controls[$"pictureBox{sas.SwapIndex2}"];
            var animation1 = new SwapAnimation(box1, box2);

            var label1 = Controls[$"indexLabel{sas.SwapIndex1}"];
            var label2 = Controls[$"indexLabel{sas.SwapIndex2}"];
            var animation2 = new SwapAnimation(label1, label2);

            if (instant)
            {
                animation1.FinalizeAnimation();
                animation2.FinalizeAnimation();
            }
            else
            {
                _activeAnimations.Add(animation1);
                animation1.AnimationFinished += AnimationFinished;
                animation1.Start();
                _activeAnimations.Add(animation2);
                animation2.AnimationFinished += AnimationFinished;
                animation2.Start();
            }
        }

        private void AnimationFinished(SwapAnimation sa)
        {
            sa.AnimationFinished -= AnimationFinished;
            _activeAnimations.Remove(sa);
        }


        private void TimerTick()
        {
            currentStep++;
            ApplyStep(currentStep, currentStep - 1);

            if (currentStep == Model.Algorithm.Steps.Count - 1)
            {
                _autoPlayTimer.Stop();
                TogglePaging(true);
                return;
            }
        }

        // Events

        private void randomizeButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Sort the randomized array?", "Randomize", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
                Controller.RandomizeArray(true);
            else if (result == DialogResult.No)
                Controller.RandomizeArray(false);
            else
                return;

            currentStep = 0;
            FullReset();
            TogglePaging(false);
            ToggleAutoPlay(false);
        }

        private void generateSteps_Click(object sender, EventArgs e)
        {
            FullReset();
            TogglePaging(true);
            ToggleAutoPlay(true);
            var comboBox = Controls["algorithmComboBox"] as ComboBox;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    {
                        var searchForRaw = (Controls["searchForTextBox"] as TextBox).Text;
                        if (!int.TryParse(searchForRaw, out int searchFor))
                        {
                            MessageBox.Show("Please specify a valid number to search for.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        Controller.ChangeAlgorithm(new LinearSearchAlgorithm(Model.DisplayedArray, searchFor));
                        break;
                    }
                case 1:
                    {
                        var searchForRaw = (Controls["searchForTextBox"] as TextBox).Text;
                        if (!int.TryParse(searchForRaw, out int searchFor))
                        {
                            MessageBox.Show("Please specify a valid number to search for.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        Controller.ChangeAlgorithm(new BinarySearchAlgorithm(Model.DisplayedArray, searchFor));
                        break;
                    }
                case 2:
                    Controller.ChangeAlgorithm(new BubbleSortAlgorithm(Model.DisplayedArray));
                    break;
                case 3:
                    Controller.ChangeAlgorithm(new QuickSortAlgorithm(Model.DisplayedArray));
                    break;
                default: break;
            }
        }

        private void previousStep_Click(object sender, EventArgs e)
        {
            if (currentStep == 0)
                return;
            currentStep--;

            ApplyStep(currentStep, currentStep + 1);
        }

        private void nextStep_Click(object sender, EventArgs e)
        {
            if (currentStep == Model.Algorithm.Steps.Count - 1)
                return;
            currentStep++;

            ApplyStep(currentStep, currentStep - 1);
        }

        private void autoplayButton_Click(object sender, EventArgs e)
        {
            if (_autoPlayTimer.Enabled)
            {
                TogglePaging(true);
                _autoPlayTimer.Stop();
                return;
            }

            if (currentStep == Model.Algorithm.Steps.Count - 1)
            {
                UpdateArray(Model.Algorithm.OriginalArray);
                currentStep = 0;
            }

            TogglePaging(false);
            _autoPlayTimer.Start();
        }

        private void algorithmComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Controls["algorithmComboBox"] as ComboBox).SelectedIndex < 2)
                Controls["searchForTextBox"].Enabled = true;
            else
                Controls["searchForTextBox"].Enabled = false;
        }
    }
}
