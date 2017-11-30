namespace ConsoleControlLibrary.Controls
{
    public class Button : Control
    {
        private string _text;
        public Button(Form parentForm, int x, int y, int width, int height, string text) : base(parentForm, x, y, width, height)
        {
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }
    }
}
