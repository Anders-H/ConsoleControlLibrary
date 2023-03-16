namespace ConsoleControlLibrary.Controls.BaseTypes;

public abstract class ListBase : ControlBase, IControl, IControlFormOperations
{
    private int _selectedIndex;
    protected int ViewOffset { get; set; }

    protected ListBase(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        CanGetFocus = true;
        Enabled = true;
        Visible = true;
        ViewOffset = 0;
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            EnsureVisible();
            Invalidate();
        }
    }

    protected abstract void EnsureVisible();

    public override void CharacterInput(char c)
    {
    }
}