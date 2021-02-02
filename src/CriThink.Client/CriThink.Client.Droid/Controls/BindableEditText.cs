using System.Windows.Input;
using Android.Content;
using Android.Util;
using Google.Android.Material.TextField;

namespace CriThink.Client.Droid.Controls
{
    public class BindableEditText : TextInputEditText
    {
        public BindableEditText(Context c, IAttributeSet a)
            : base(c, a)
        {
            EditorAction += EditorAction_EventHandler;
        }

        public ICommand KeyCommand { get; set; }

        public void EditorAction_EventHandler(object sender, EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == Android.Views.InputMethods.ImeAction.Next ||
                e.ActionId == Android.Views.InputMethods.ImeAction.Send ||
                e.ActionId == Android.Views.InputMethods.ImeAction.Done ||
                e.ActionId == Android.Views.InputMethods.ImeAction.Go)
            {
                if (KeyCommand != null)
                {
                    KeyCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }
    }
}